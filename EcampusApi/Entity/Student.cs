using System;
using System.IO;

namespace EcampusApi.Entity
{
    //enum validTo {
    //    "бессрочно"
    //}
    public class Student
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string ValidTo { get; set; }
        public string Number { get; set; }
        public Guid Guid { get; set; }
        public string ImgUrl { get; set; }
        public Stream ProfilePhoto { get; set; }
    }
}
