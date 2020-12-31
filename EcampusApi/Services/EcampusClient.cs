using AngleSharp.Html.Parser;
using EcampusApi.Constans;
using EcampusApi.Entity;
using EcampusApi.Helpers;
using EcampusApi.Services.Interfaces;
using JSONUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EcampusApi.Services
{
    /// <summary>
    /// Main class for work with profile
    /// </summary>
    public class EcampusClient
    {
        /// <summary>
        /// Auth data profile(password and login)
        /// </summary>
        private IAuth authUser = null;
        private HttpClient client;
        private IHtmlParser parser;
        public EcampusClient(IAuth auth)
        {
            if (auth == null)
            {
                throw new ArgumentNullException();
            }
            authUser = auth;
            client = ClientGeneration.GetClient();
            parser = new HtmlParser();
        }
        public EcampusClient()
        {
            parser = new HtmlParser();
            client = ClientGeneration.GetClient();
        }

        /// <summary>
        /// Получаем расписание на текущею неделю
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Root>> GetScheduleAsync()
        {
            var scheduleResponse = await client.GetAsync(Links.BaseLink + Links.ScheduleLink);
            
            if (scheduleResponse.IsSuccessStatusCode)
            {
                var content = await scheduleResponse.Content.ReadAsStringAsync();
                var doc = parser.ParseDocument(content);
                var element = doc.QuerySelector("script[type='text/javascript']");
                var id = TextWorker.GetUserId(element.TextContent);
                var jsonShedule = await GetJsonSchedule(id);
                var schedule = JsonConvert.DeserializeObject<IList<Root>>(jsonShedule);
                return schedule;
            }
            return new List<Root>();
        }
        /// <summary>
        /// Получаем расписание на след. неделю
        /// </summary>
        /// <returns></returns>
        public async Task<Root> GetScheduleNextWeek()
        {
            throw new Exception();
        }
        /// <summary>
        /// Method for authorization in to the system
        /// </summary>
        /// <param name="login">Ecampus User name </param>
        /// <param name="password">Ecampus passsword</param>
        /// <returns></returns>
        public async Task<Login> LoginAsync(string login, string password)
        {
            if (authUser == null && string.IsNullOrWhiteSpace(login) && string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException();
            }
            var loginPage = await client.GetAsync(Links.BaseLink + Links.LoginLink);

            if (!loginPage.IsSuccessStatusCode)
            {
                throw new Exception("Server Error");
            }

            // html code for auth
            var loginContent = await loginPage.Content.ReadAsStringAsync();
            var token = GetVerificationToken(loginContent);
            var authRequest = new AuthBody()
            {
                Login = login,
                Password = password,
                Token = token,
                RememberMe = true
            };
            var requestBody = new StringContent($"__RequestVerificationToken={authRequest.Token}" +
                $"&Login={authRequest.Login}&Password={authRequest.Password}&RememberMe={authRequest.RememberMe}", Encoding.UTF8, "application/x-www-form-urlencoded");

            var loginResponse = await client.PostAsync(string.Concat(Links.BaseLink, Links.LoginLink), requestBody);

            if (!loginResponse.IsSuccessStatusCode)
            {
                return new Login { IsSuccess = false, ReasonFail = Reason.ErrorServer };
            }
            var resultContent = await loginResponse.Content.ReadAsStringAsync();
            var success = resultContent.Contains("<span class=\"username\">");
            if (success == false)
            {
                var checkOnPassLogin = resultContent.Contains("validation-summary-errors alert alert-danger alert-block");
                if (checkOnPassLogin)
                {
                    return new Login { IsSuccess = false, ReasonFail = Reason.Pasword };
                }
                else
                {
                    return new Login { IsSuccess = false, ReasonFail = Reason.OtherProblem };
                }
            }
            return new Login { IsSuccess = true, ReasonFail = Reason.None };
        }

        /// <summary>
        /// Get token which needs for authorization
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetVerificationToken(string content)
        {
            var token = "";
            if (string.IsNullOrWhiteSpace(content) || string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("The Argument content is null or empty");
            }
            var document = parser.ParseDocument(content);
            var elementToken = document.QuerySelector("input[name ='__RequestVerificationToken']");
            token = elementToken.GetAttribute("value");
            return token.Trim();
        }

        /// <summary>
        /// Корректируем Json данные 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task<string> GetJsonSchedule(string id)
        {
            var body = new 
            { 
                Date = DateTime.Now.Date.ToString("yyyy-MM-dd") + "T00:00:00.000Z",
                Id = Convert.ToInt32(id),
                TargetType = 4
            };
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Links.BaseLink + Links.SchedulePostLink, content);
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
   
    }
}
