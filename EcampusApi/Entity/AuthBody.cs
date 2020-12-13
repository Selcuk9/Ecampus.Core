using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcampusApi.Entity
{

    public struct AuthBody
    {

        [JsonProperty("__RequestVerificationToken")]
        public string Token { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
