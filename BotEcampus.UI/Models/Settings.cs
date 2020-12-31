using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotEcampus.Core.Models
{
    public class Authorization
    {
        public string Login { get; set;}
        public string Password { get; set; }
        public long? UserId { get; set; }
    }
}
