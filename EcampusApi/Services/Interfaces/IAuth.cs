using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcampusApi.Services.Interfaces
{
    public interface IAuth
    {
        public string Login { get; }
        public string Password { get; }
        public string Token { get; set; }
    }
}
