using EcampusApi.Helpers;
using EcampusApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace BotEcampus.UI.Services.BotCommand
{
    public static class Commands
    {
        /// <summary>
        /// Метод, где возвращается ответ только для авторизованных пользователей
        /// </summary>
        /// <param name="message">Команда бота</param>
        /// <returns>Ответ которую получит пользователь</returns>
        public async static Task<string> GetAnswerForAuthorizeUser(string message, EcampusClient clientECampus)
        {

            switch (message.ToLower())
            {
                case var text when message.Contains("Расписание"):
                    var schedules = await clientECampus.GetScheduleAsync();
                    var schedule = TextWorker.GetAllSchedule(schedules);
                    return schedule;
            }

            return "Функционал";
        }
        /// <summary>
        /// Метод, где возвращается ответ только для не авторизованных пользователей
        /// </summary>
        /// <param name="message">Команда бота</param>
        /// <returns>Ответ которую получит пользователь</returns>
        public async static Task<string> GetAnswerAnonymousUser(string message, EcampusClient clientECampus)
        {
            switch (message.ToLower())
            {
                case var text when message.Contains(":"):
                    {
                        var login = await clientECampus.LoginAsync(message.Split(':')[0], message.Split(':')[1]);
                        if (login.IsSuccess)
                        {
                            return "Вы успешно авторизовались в систему";
                        }
                        else
                        {
                            return "Неверное имя пользователя или пароль";
                        }
                    }
                case var text when message.Contains("начать"):
                    {
                        return "Здравствуйте, пожалуйста введите ваш Логин и пароль " +
                        "от \"Электронный кампус СКФУ\"\nПример: login:password";
                    }

            }
            return "Неверная команда";


        }
        public static MessageKeyboard GetOptionalKeyboard()
        {
            var keyboard = new KeyboardBuilder()
                       .SetInline(false)
                       .SetOneTime()
                       .AddButton("Расписание", "btnScheduleToday", KeyboardButtonColor.Primary)
                       .AddButton("Расписание на след. неделю", "btnScheduleOnNextWeek", KeyboardButtonColor.Primary)
                       .Build();
            return keyboard;

        }

        
        /// <summary>
        /// Метод собирает распсание в одну текстовую структуру для отправки
        /// </summary>
        /// <param name="lessons"></param>
        /// <returns></returns>
        
    }
}
