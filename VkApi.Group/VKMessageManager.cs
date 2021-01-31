using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkApi.Group.Interfaces;
using VkNet;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace VkApi.Group
{
   public class VKMessageManager : IVKMessageManager
    {
        private IVkApi _api = new VkNet.VkApi();
        private string ts;
        private string pts;
        private string key;
        private string server;
        public event Action<Message> OnNewMessage;
        private readonly ulong groupId;

        //Событие для уведомления о новом сообщении

        public VKMessageManager(string accessToken, ulong groupId)
        {
            this.groupId = groupId;
            //Авторизуемся с учетной записью пользователя. 
            //Для обхода блокировки сообщений используем ApplicationId какого-нибудь официально зарегистрированного приложения
            //либо используем Bypass. В примере ApplicationId приложения Kate Mobile. 
            _api.Authorize(new ApiAuthParams
            {
                AccessToken = accessToken,
                Settings = Settings.Messages
            });
        }

        public void StartMessageHandling()
        {

            // Соединяемся с сервером Long Poll запросов и получаем необходимые ts и pts
            var longPoolServerResponse = _api.Groups.GetLongPollServer(groupId);

            ts = longPoolServerResponse.Ts;
            pts = longPoolServerResponse.Pts.ToString();
            key = longPoolServerResponse.Key;
            server = longPoolServerResponse.Server;


            //В отдельном потоке запускаем метод, который будет постоянно опрашивать Long Poll сервер на наличие новых сообщений
            new Thread(LongPollEventLoop).Start();
            
        }
        /// <summary>
        /// Отправляем только текстовое сообщение
        /// </summary>
        /// <param name="message">Текст для отправки</param>
        /// <param name="fromId">Id пользователя</param>
        /// <returns></returns>
        public async Task<bool> SendMessageAsync(string message, long? fromId)
        {
            var response = await _api.Messages.SendAsync(new MessagesSendParams
            { 
                UserId = fromId,
                Message = message,
                RandomId = new Random().Next(int.MaxValue)
            });
            return response == 20;
        }
        public async Task<bool> SendMessageAsync(string message, long? fromId, MessageKeyboard keyboard)
        {
            var response = await _api.Messages.SendAsync(new MessagesSendParams 
            {
                UserId = fromId,
                Message = message,
                Keyboard = keyboard,
                RandomId = 0,
                
            });
            return response == 20;
        }
        private void LongPollEventLoop()
        {

            //Запускаем бесконечный цикл опроса
            try
            {
                while (true)
                {
                    var longPollHistoryResponse = _api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
                    {
                        Ts = ts.ToString(),
                        Key = key,
                        Server = server,
                        Wait = 10
                    });
                    ts = longPollHistoryResponse.Ts;

                    foreach (var update in longPollHistoryResponse.Updates)
                    {
                       if (update.Type.ToString() == "message_new")
                        {
                            OnNewMessage.Invoke(
                                update.MessageNew.Message
                                );
                        }
                    }
                }
            }
            catch (Exception)
            {
                StartMessageHandling();
                return;
            }
        }

        public async Task<bool> SendMessagePhoto(string pathFile, long? fromId)
        {
            var uploadServer = await _api.Photo.GetMessagesUploadServerAsync((int)fromId);
            // Загрузить файл.
            var wc = new WebClient();
            var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, pathFile));
            // Сохранить загруженный файл
            var attachment = _api.Photo.SaveMessagesPhoto(responseFile);

            //Отправить сообщение с нашим вложением
            await _api.Messages.SendAsync(new MessagesSendParams
            {
                UserId = fromId, //Id получателя
                Attachments = attachment, //Вложение
                RandomId = 0, //Уникальный идентификатор
                
            });
            File.Delete(pathFile);
            return true;
        }
        
    }
}
