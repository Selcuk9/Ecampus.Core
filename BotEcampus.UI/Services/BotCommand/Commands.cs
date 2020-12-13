using EcampusApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                case var text when message.Contains("расписание на сегодня"):
                    var schedule = await clientECampus.GetScheduleOn(DateTime.Today);
                    var build = $"{schedule.Date.Day} ({schedule.WeekDay}) \n " +
                    $"{LessonsBuilder(schedule.Lessons)}";
                    break;
                case var text when message.Contains("начать"):

                    break;
                case var text when message.Contains("начать"):

                    break;
                case var text when message.Contains("начать"):

                    break;
            }
            return "not command";
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
                    var login = await clientECampus.LoginAsync(message.Split(':')[0], message.Split(':')[1]);
                    if (login.IsSuccess)
                    {
                        return "Вы успешно авторизовались в систему";
                    }
                    else
                    {
                        return "Неверное имя пользователя или пароль";
                    }
                case var text when message.Contains("начать"):
                    return "Здравствуйте, пожалуйста введите ваш Логин и пароль " +
                          "от \"Электронный кампус СКФУ\"\nПример: login:password";
            }
            return "Неверная команда";


        }

        /// <summary>
        /// Метод собирает распсание в одну текстовую структуру для отправки
        /// </summary>
        /// <param name="lessons"></param>
        /// <returns></returns>
        private static string LessonsBuilder(IEnumerable<Lesson> lessons)
        {
            var text = "";
            foreach (var item in lessons)
            {
                text += $"{item.PairNumberStart } {item.LessonName}  {item.Discipline} {item.Aud.Name} {item.LessonType} {item.Groups[0].Name} {item.Teacher.Name}\n";
            }
            return text;
        }
    }
}
