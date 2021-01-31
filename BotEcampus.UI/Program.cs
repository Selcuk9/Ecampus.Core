using BotEcampus.UI.DataBase;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BotEcampus.UI
{
   public class Program
    {
       public static void Main()
        {
            SQLiteDb db = new SQLiteDb("Files/BotEcampus.sqlite3");
            //e640a613d3b9f530426bb962c9fd3d94d2890e9bd7a0d1d0f6c4f76332f99505fee9e072a9189a86c8de5
            var bot = new BotEcampus.Core.BotEcampus("e640a613d3b9f530426bb962c9fd3d94d2890" +
                "e9bd7a0d1d0f6c4f76332f99505fee9e072a9189a86c8de5", 202249381, db);
            bot.RunVkBot();
            Console.WriteLine("Bot Runned");
        }
    }
}
