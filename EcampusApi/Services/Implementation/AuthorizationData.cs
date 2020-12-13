using EcampusApi.Services.Interfaces;
using System;

namespace EcampusApi.Services.Implementation
{
    public class AuthorizationData : IAuth
    {
        public string Login { get; }
        public string Password { get; }
        public string Token { get; set; }
        public AuthorizationData(string login, string password)
        {
            Login = login;
            Password = password;
            Token = String.Empty;
        }
    }
}
