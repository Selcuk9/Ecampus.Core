using BotEcampus.UI.DataBase;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BotEcampus.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLiteDb db = new SQLiteDb("Files/BotEcampus.sqlite3");

            var bot = new BotEcampus.Core.BotEcampus("172a4bff3319bac7443b4194070291f76401d" +
                "1b5c77d9d03b9344cded5ef71202ab8480430f2abda2dc93",db);
            bot.RunVkBot();
            Console.WriteLine("Bot Runned");
        }
    }
}
