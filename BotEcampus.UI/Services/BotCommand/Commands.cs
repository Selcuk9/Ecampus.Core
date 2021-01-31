using EcampusApi.Entity;
using EcampusApi.Helpers;
using EcampusApi.Services;
using SixLabors.ImageSharp;
using System;
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
                case var text when message.ToLower() == ("расписание"):
                    {
                        var schedules = await clientECampus.GetScheduleAsync();
                        if (schedules.Count <= 0)
                        {
                            return "Для данной недели расписание не предоставлено.";
                        }
                        var schedule = TextWorker.GetAllSchedule(schedules);
                        return schedule;
                    }

                case var text when message.ToLower().Contains("неделю"):
                    var schedulesNextWeek = await clientECampus.GetScheduleOnNextWeekAsync();
                    if (schedulesNextWeek.Count <= 0)
                    {
                           return "Для данной недели расписание не предоставлено.";
                    }
                    var scheduleNextWeek = TextWorker.GetAllSchedule(schedulesNextWeek);
                    return scheduleNextWeek;

                case var text when message.ToLower().Contains("пропуск"):
                    var passImage = await clientECampus.StudentPass();
                    var passName = $"ImgPasses/{Guid.NewGuid().ToString()}";
                    await passImage.SaveAsync(passName + ".jpg");
                    return passName + ".jpg";
                    
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
                case var text when message.Contains(":") && message.Split(':').Length == 2:
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
                case var text when message.ToLower().Contains("начать"):
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
                       .AddButton("Эл.пропуск", "EPass", KeyboardButtonColor.Primary)
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
