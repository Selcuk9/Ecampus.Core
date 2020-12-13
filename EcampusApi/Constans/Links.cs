using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcampusApi.Constans
{
    public static class Links
    {
        public static string LoginLink { get; }
        public static string BaseLink { get; }
        public static string ScheduleLink { get; }
        static Links()
        {
            BaseLink = "https://ecampus.ncfu.ru/";
            LoginLink = "account/login/";
            ScheduleLink = "schedule/my/student/";
        }
    }
}
