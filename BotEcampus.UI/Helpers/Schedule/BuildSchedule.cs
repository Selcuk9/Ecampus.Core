using BotEcampus.UI.DataBase;
using JSONUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotEcampus.UI.Helpers.Schedule
{
    public class BuildSchedule
    {
        private static IDataBase db;
        public BuildSchedule(IDataBase dataBase)
        {
            db = dataBase;
        }
        /// <summary>
        /// Метод для получние готова расписание для отправки в чат
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public string GetAllSchedule(IList<Root> schedules)
        {
            string[] months = { "Января", "Февраля", "Марта", "Апреля", "Мая", "Июня", "Июля", "Авгуса", "Сентября", "Октября", "Ноября", "Декабря" };

            var text = "";
            foreach (var sch in schedules)
            {
                text += "\n";
                text += $"&#128197;{sch.Date.Day} {months[sch.Date.Month - 1]} [ {sch.WeekDay} ]\n";
                foreach (var lesson in sch.Lessons)
                {
                    var typeLessonsAndVks = Vks(lesson.Aud.Name, lesson?.Teacher?.Name);
                    var teacherType = TeacherType(lesson?.Teacher?.Name);
                    text += $"&#128212;{lesson.PairNumberStart} {lesson.LessonName}:\n" +
                        $"&#128205;{lesson.Discipline}\n" +
                        $"&#128205;{typeLessonsAndVks} " +
                        $"{null} " +
                        $"{lesson.LessonType} " +
                        $"{string.Join(',', lesson.Groups.Select(gr => gr.Name))}\n" +
                        $"{teacherType}\n" +
                        $"----------------------------------\n";
                }
                text += "\n";
            }
            return text;
        }
        private string Vks(string lessonType, string teacherName)
        {
            if (lessonType.Contains("ВКС") && !string.IsNullOrWhiteSpace(teacherName) && !string.IsNullOrEmpty(teacherName))
            {
                var result = db.GetVKS(teacherName.Trim());
                if (!string.IsNullOrWhiteSpace(result) && !string.IsNullOrEmpty(result))
                {
                    return lessonType + $"({result})";
                }
            }
            return lessonType;
        }
        public string TeacherType(string teacherName)
        {
            if (!string.IsNullOrWhiteSpace(teacherName) && !string.IsNullOrEmpty(teacherName))
            {
                if (teacherName.Split(' ').Last().Contains("вна"))
                {
                    return $"&#128105;&#8205;&#127979; {teacherName}";
                }
                else
                {
                    return $"&#128104;&#8205;&#127979; {teacherName}";
                }
            }
            return teacherName;
        }
    }
}
