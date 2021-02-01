using JSONUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EcampusApi.Helpers
{
    public static class TextWorker
    {
        /// <summary>
        /// Метод для получение Id студента
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetUserId(string content)
        {
            string pattern = @"(\d{6})"; //@"\w*(\d{6})\w*";
            Regex regex = new Regex(pattern/*, RegexOptions.Compiled | RegexOptions.IgnoreCase*/);
            MatchCollection match = regex.Matches(content);
            return match.Last().Value;
        }
    }
}
