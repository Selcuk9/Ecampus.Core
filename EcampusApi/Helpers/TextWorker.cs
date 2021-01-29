using JSONUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EcampusApi.Helpers
{
    public static class TextWorker
    {
        /// <summary>
        /// Метод для получение Id студента
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetUserId(string content)
        {
            string pattern = @"(\d{6})"; //@"\w*(\d{6})\w*";
            Regex regex = new Regex(pattern/*, RegexOptions.Compiled | RegexOptions.IgnoreCase*/);
            MatchCollection match = regex.Matches(content);
            return match.Last().Value;
        }
        /// <summary>
        /// Метод для получние готова расписание для отправки в чат
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public static string GetAllSchedule(IList<Root> schedule)
        {
            string[] months = { "Января", "Февраля", "Марта", "Апреля", "Мая", "Июня", "Июля", "Авгуса", "Сентября", "Октября", "Ноября", "Декабря" };

            var text = "";
            foreach (var sch in schedule)
            {
                text += $"&#128197;{sch.WeekDay} {sch.Date.Day} {months[sch.Date.Month - 1]}\n" +
                    $"&#128213;&#128213;&#128213;&#128213;&#128213;&#128213;&#128213;&#128213;&#128213;&#128213;&#128213;&#128213;\n";
                foreach (var lesson in sch.Lessons)
                {
                    text += $"&#128276;{lesson.PairNumberStart} {lesson.LessonName}\n" +
                        $"&#128214;{lesson.Discipline}\n" +
                        $"&#128421;{lesson.Aud.Name} " +
                        $"{null} " +
                        $"{lesson.LessonType} " +
                        $"{string.Join(',', lesson.Groups.Select(gr => gr.Name))}\n" +
                        $"&#128372;{lesson?.Teacher?.Name}\n" +
                        $"&#128216;&#128216;&#128216;&#128216;&#128216;&#128216;&#128216;&#128216;&#128216;&#128216;&#128216;&#128216;\n";
                }
            }
            return text;
        }
    }
}
