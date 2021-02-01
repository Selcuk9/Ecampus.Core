using AngleSharp.Html.Parser;
using EcampusApi.Constans;
using EcampusApi.Entity;
using EcampusApi.Helpers;
using EcampusApi.Services.Interfaces;
using JSONUtils;
using Newtonsoft.Json;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
                var jsonShedule = await GetJsonSchedule(id,DateTime.Now);
                var schedule = JsonConvert.DeserializeObject<IList<Root>>(jsonShedule);
                return schedule;
            }
            return new List<Root>();
        }

        public async Task<IList<Root>> GetScheduleOnNextWeekAsync()
        {
            var scheduleResponse = await client.GetAsync(Links.BaseLink + Links.ScheduleLink);

            if (scheduleResponse.IsSuccessStatusCode)
            {
                var content = await scheduleResponse.Content.ReadAsStringAsync();
                var doc = parser.ParseDocument(content);
                var element = doc.QuerySelector("script[type='text/javascript']");
                var id = TextWorker.GetUserId(element.TextContent);
                var jsonShedule = await GetJsonSchedule(id, DateTime.Now.AddDays(7));
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
        private async Task<string> GetJsonSchedule(string id,DateTime date)
        {
            var body = new 
            { 
                Date = date.ToString("yyyy-MM-dd") + "T00:00:00.000Z",
                Id = Convert.ToInt32(id),
                TargetType = 4
            };
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Links.BaseLink + Links.SchedulePostLink, content);
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }

        public async Task<Student> GetStudentIdentify()
        {
            var student = new Student();
            var reponsePass= await client.GetAsync("https://ecampus.ncfu.ru/home/pass");
            if (reponsePass.IsSuccessStatusCode)
            {
                var person = await reponsePass.Content.ReadAsStringAsync();
                var parser = new HtmlParser();
                var doc = parser.ParseDocument(person);
                var scriptEl = doc.QuerySelector("script:nth-child(5)");
                if (scriptEl != null)
                {
                    var content = scriptEl.TextContent;
                    student = await ParseStudentData(content);
                }

            }
            if (reponsePass.ReasonPhrase == "Forbidden")
            {
                return new Student() { ValidTo = "БАН"};
            }
            return student;
        }
        public async Task<Image> StudentPass()
        {
            var studentIdentify = await GetStudentIdentify();
            var pass = await DrawUserPass(studentIdentify);
            return pass;
        }
        private async Task<Image> DrawUserPass(Student pass)
        {
            if (pass.ValidTo == "БАН")
            {
                var fontBan = SystemFonts.CreateFont("Arial", 40, FontStyle.Regular);
                Image imageBan = new Image<Rgba32>(1500, 500);
                var reason = "СКФУ не предоставляет иностранным студентам электронные пропуска";
                var glyphsBan = TextBuilder.GenerateGlyphs(reason, new Point(0, 200), new RendererOptions(fontBan));
                imageBan.Mutate(ctx => ctx
                .Fill(Color.Blue)
                .Fill(Color.White, glyphsBan));
                return imageBan;
            }
            Image image = Image.Load(@"Files\pass_layout.png"); // 810 x 510
            Image image1 = await Image.LoadAsync(pass.ProfilePhoto);
            image1.Mutate(im => im.Resize(250, 310));
            Image imageResult = new Image<Rgba32>(810, 510); // 810 x 510

            imageResult.Mutate(im => im
            .DrawImage(image, new Point(0, 0), 1f)
            .DrawImage(image1, new Point(40, 130), 1f)
            );
            var font = SystemFonts.CreateFont("Arial", 40, FontStyle.Regular);
            var fontLabel = SystemFonts.CreateFont("Arial", 20, FontStyle.Regular);
            string surname = pass.Surname;
            string name = pass.Name;
            string lastName = pass.LastName;
            string status = pass.ValidTo;

            string lable = $"срок действия: {status}";


            var glyphsSurname = TextBuilder.GenerateGlyphs(surname, new Point(330, 120), new RendererOptions(font));
            var glyphsName = TextBuilder.GenerateGlyphs(name, new Point(330, 180), new RendererOptions(font));
            var glyphsLastName = TextBuilder.GenerateGlyphs(lastName, new Point(330, 240), new RendererOptions(font));
            var glyphsLabel = TextBuilder.GenerateGlyphs(lable, new Point(360, 420), new RendererOptions(fontLabel));
            imageResult.Mutate(ctx => ctx
                .Fill(Color.White, glyphsSurname)
                .Fill(Color.White, glyphsName)
                .Fill(Color.White, glyphsLastName)
                .Fill(Color.White, glyphsLabel));

            return imageResult;

        }
        private async Task<Student> ParseStudentData(string content)
        {
            var student = new Student();
            content = content.Replace("\n", "");
            var regex = new Regex(@"surname:.*}\)");
            var matches = regex.Match(content);

            if (matches.Success)
            {
                var reg = new Regex("'.*?'");
                var data = matches.Value.Split(',');

                var res = reg.Match(data[0]);
                student.Surname = res.Value.Replace("\'", "");

                res = reg.Match(data[1]);
                student.Name = res.Value.Replace("\'", "");

                res = reg.Match(data[2]);
                student.LastName = res.Value.Replace("\'", "");

                res = reg.Match(data[3]);
                student.Position = res.Value.Replace("\'", "");

                res = reg.Match(data[4]);
                student.ValidTo = res.Value.Replace("\'", "");

                res = reg.Match(data[5]);
                student.Number = res.Value.Replace("\'", "");

                res = reg.Match(data[6]);
                student.Guid = Guid.Parse(res.Value.Replace("\'", ""));

                res = reg.Match(data[7]);
                student.ImgUrl = res.Value.Replace("\'", "");

                if (!string.IsNullOrEmpty(student.ImgUrl) && !string.IsNullOrWhiteSpace(student.ImgUrl))
                {
                    var responsePhoto = await client.GetAsync(Links.BaseLink + student.ImgUrl);
                    if (responsePhoto.IsSuccessStatusCode)
                    {
                        student.ProfilePhoto = await responsePhoto.Content.ReadAsStreamAsync();
                    }
                }
            }
            return student;
        }
    }
}
