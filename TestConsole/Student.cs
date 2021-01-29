using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
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
    }
}
