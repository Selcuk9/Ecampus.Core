using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var weekNumber = (int)DateTime.Now.DayOfWeek;
            if (weekNumber != 0)
            {
                var weekStart = weekNumber - 1;
                var dataRequest = DateTime.Now.AddDays(-weekStart);
            }
            else
            {
                var dataRequest = DateTime.Now.AddDays(-6);
            }

            #region MyRegion

            //var student = new Student();
            //var txt = File.ReadAllText("json.txt");
            //txt = txt.Replace("\n", "");
            //var regex = new Regex(@"surname:.*}\)");
            //var matches = regex.Match(txt);

            //if (matches.Success)
            //{
            //    var reg = new Regex("'.*?'");
            //    var data = matches.Value.Split(',');

            //    var res = reg.Match(data[0]);
            //    student.Surname = res.Value.Replace("\'", "");

            //    res = reg.Match(data[1]);
            //    student.Name = res.Value.Replace("\'", "");

            //    res = reg.Match(data[2]);
            //    student.LastName = res.Value.Replace("\'", "");

            //    res = reg.Match(data[3]);
            //    student.Position = res.Value.Replace("\'", "");

            //    res = reg.Match(data[4]);
            //    student.ValidTo = res.Value.Replace("\'", "");

            //    res = reg.Match(data[5]);
            //    student.Number = res.Value.Replace("\'", "");

            //    res = reg.Match(data[6]);
            //    student.Guid = Guid.Parse(res.Value.Replace("\'", ""));

            //    res = reg.Match(data[7]);
            //    student.ImgUrl = res.Value.Replace("\'", "");
            //}

            //// начало init({                   });
            ////var test = new Client();
            ////await test.Test();

            //// Console.ReadLine(); 
            #endregion
        }
    }
}
