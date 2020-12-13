// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Aud
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }
}

public class Teacher
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }
}

public class Group
{
    [JsonPropertyName("LessonId")]
    public int LessonId { get; set; }

    [JsonPropertyName("Subgroup")]
    public string Subgroup { get; set; }

    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }
}

public class Lesson
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("LessonDayId")]
    public int LessonDayId { get; set; }

    [JsonPropertyName("TeacherId")]
    public int TeacherId { get; set; }

    [JsonPropertyName("RoomId")]
    public int RoomId { get; set; }

    [JsonPropertyName("Discipline")]
    public string Discipline { get; set; }

    [JsonPropertyName("TimeBegin")]
    public DateTime TimeBegin { get; set; }

    [JsonPropertyName("TimeEnd")]
    public DateTime TimeEnd { get; set; }

    [JsonPropertyName("Aud")]
    public Aud Aud { get; set; }

    [JsonPropertyName("LessonType")]
    public string LessonType { get; set; }

    [JsonPropertyName("PairNumberStart")]
    public int PairNumberStart { get; set; }

    [JsonPropertyName("PairNumberEnd")]
    public int PairNumberEnd { get; set; }

    [JsonPropertyName("WeekDay")]
    public object WeekDay { get; set; }

    [JsonPropertyName("SubGroups")]
    public object SubGroups { get; set; }

    [JsonPropertyName("Teacher")]
    public Teacher Teacher { get; set; }

    [JsonPropertyName("AcademicGroup")]
    public object AcademicGroup { get; set; }

    [JsonPropertyName("Groups")]
    public List<Group> Groups { get; set; }

    [JsonPropertyName("Current")]
    public bool Current { get; set; }

    [JsonPropertyName("LessonName")]
    public string LessonName { get; set; }
}

public class Shedule
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("WeekDay")]
    public string WeekDay { get; set; }

    [JsonPropertyName("Date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("Lessons")]
    public List<Lesson> Lessons { get; set; }
}

public class Root
{
    [JsonPropertyName("Shedule")]
    public List<Shedule> Shedule { get; set; }
}

