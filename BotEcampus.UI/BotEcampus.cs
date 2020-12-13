using BotEcampus.UI.DataBase;
using EcampusApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkApi.Group;
using VkApi.Group.Interfaces;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace BotEcampus.Core
{
    public class BotEcampus
    {
        private IVKMessageManager messageManager;
        private IDataBase db;
        public BotEcampus(string accessToken, IDataBase dataBase)
        {
            if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrEmpty(accessToken))
            {
                messageManager = new VKMessageManager(accessToken);
                messageManager.StartMessageHandling();
            }
            db = dataBase;
        }

       
        public void RunVkBot()
        {
            messageManager.OnNewMessage += async (message) =>
            {
                var isExist = db.GetUserById(message.FromId);
                if (message.Text.ToLower() == "начать")
                {
                    if (isExist == null)
                    {
                       await messageManager.SendMessageAsync("Здравствуйте, пожалуйста введите ваш Логин и пароль " +
                          "от \"Электронный кампус СКФУ\"\nПример: login:password",message.FromId);
                    }
                    else
                    {
                        var keyboard = new KeyboardBuilder()
                        .AddButton("Подтвердить", "btnValue", KeyboardButtonColor.Primary)
                        .SetInline(false)
                        .SetOneTime()
                        .AddLine()
                        .AddButton("Отменить", "btnValue2", KeyboardButtonColor.Default)
                        .AddButton("Отменить2", "btnValue2", KeyboardButtonColor.Default)
                        .Build();
                        await messageManager.SendMessageAsync("Функционал бота", message.FromId, keyboard);
                    }
                }
                EcampusClient clientECampus = new EcampusClient();
              
                if (isExist != null)
                {


                }
                else
                {
                   

                }
            };
        }
    }
}
