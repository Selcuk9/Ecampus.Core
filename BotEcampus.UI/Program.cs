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

            //тестовый
            SQLiteDb db = new SQLiteDb("Files/BotEcampus.sqlite3");
            var bot = new BotEcampus.Core.BotEcampus("e640a613d3b9f530426bb962c9fd3d94d2890" +
                "e9bd7a0d1d0f6c4f76332f99505fee9e072a9189a86c8de5", 202249381, db);
            bot.RunVkBot();
            Console.WriteLine("Bot Runned");




            ////Боевой
            //SQLiteDb db = new SQLiteDb("Files/BotEcampus.sqlite3");

            //var bot = new BotEcampus.Core.BotEcampus("172a4bff3319bac7443b4194070291f76401d" +
            //    "1b5c77d9d03b9344cded5ef71202ab8480430f2abda2dc93", 200874156, db);
            //bot.RunVkBot();


        }


    }
}
