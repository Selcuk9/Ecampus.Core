using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JSONUtils
{

    public class Aud
    {

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }

    public class Group
    {

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("LessonId")]
        public int LessonId { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Subgroup")]
        public string Subgroup { get; set; }
    }

    public class Teacher
    {

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }

    public class Lesson
    {

        [JsonProperty("AcademicGroup")]
        public object AcademicGroup { get; set; }

        [JsonProperty("Aud")]
        public Aud Aud { get; set; }

        [JsonProperty("Current")]
        public bool Current { get; set; }

        [JsonProperty("Discipline")]
        public string Discipline { get; set; }

        [JsonProperty("Groups")]
        public IList<Group> Groups { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("LessonDayId")]
        public int LessonDayId { get; set; }

        [JsonProperty("LessonName")]
        public string LessonName { get; set; }

        [JsonProperty("LessonType")]
        public string LessonType { get; set; }

        [JsonProperty("PairNumberEnd")]
        public int PairNumberEnd { get; set; }

        [JsonProperty("PairNumberStart")]
        public int PairNumberStart { get; set; }

        [JsonProperty("RoomId")]
        public int RoomId { get; set; }

        [JsonProperty("SubGroups")]
        public object SubGroups { get; set; }

        [JsonProperty("Teacher")]
        public Teacher Teacher { get; set; }

        [JsonProperty("TeacherId")]
        public int TeacherId { get; set; }

        [JsonProperty("TimeBegin")]
        public DateTime TimeBegin { get; set; }

        [JsonProperty("TimeEnd")]
        public DateTime TimeEnd { get; set; }

        [JsonProperty("WeekDay")]
        public object WeekDay { get; set; }
    }

    public class Root
    {

        [JsonProperty("Date")]
        public DateTime Date { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Lessons")]
        public IList<Lesson> Lessons { get; set; }

        [JsonProperty("WeekDay")]
        public string WeekDay { get; set; }
    }

}