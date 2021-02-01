using BotEcampus.UI.DataBase;
using BotEcampus.UI.Helpers.Schedule;
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
        private static BuildSchedule buildSchedule;
        static Commands()
        {
            buildSchedule = new BuildSchedule(new SQLiteDb("Files/BotEcampus.sqlite3"));
        }
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
                        var schedule = buildSchedule.GetAllSchedule(schedules);
                        return schedule;
                    }

                case var text when message.ToLower().Contains("неделю"):
                    var schedulesNextWeek = await clientECampus.GetScheduleOnNextWeekAsync();
                    if (schedulesNextWeek.Count <= 0)
                    {
                           return "Для данной недели расписание не предоставлено.";
                    }
                    var scheduleNextWeek = buildSchedule.GetAllSchedule(schedulesNextWeek);
                    return scheduleNextWeek;

                case var text when message.ToLower().Contains("пропуск"):
                    var passImage = await clientECampus.StudentPass();
                    var passName = $"ImgPasses/{Guid.NewGuid().ToString()}";
                    await passImage.SaveAsync(passName + ".jpg");
                    return passName + ".jpg";
                    
            }

            return "Я не понимаю что вы говорите";
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
                        return "Здравствуйте, я Ваш помощник ECampusBot &#129302;\n" +
                               "Для дальнейшей работы со мной, Вам нужно авторизоваться в \"Электронный кампус СКФУ\"&#128104;&#8205;&#128187;\n" +
                               "Пример: login:password";
                    }

            }
            return "Неверная команда";


        }
        public static MessageKeyboard GetOptionalKeyboard()
        {
            var keyboard = new KeyboardBuilder()
                       .SetInline(false)
                       .AddButton("Расписание", "btnScheduleToday", KeyboardButtonColor.Positive)
                       .AddLine()
                       .AddButton("Расписание на след. неделю", "btnScheduleOnNextWeek", KeyboardButtonColor.Primary)
                       .AddLine()
                       .AddButton("Эл.пропуск", "EPass", KeyboardButtonColor.Negative)
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
