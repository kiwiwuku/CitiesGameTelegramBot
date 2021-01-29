using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesBot
{
    class Program
    {
        static private string key = "1574797346:AAH5zYFinhCmrKtrbUuX8b2YdvS0LR5bdmc";
        static private Telega bot;
        static private DataManager manager = new DataManager();

        static void Main(string[] args)
        {
            Initialize();
            Console.WriteLine("Бот запущен");
        }
        static private void Initialize()
        {
            bot = new Telega(key);
            bot.GotMessage += Bot_GotMessage;
            
        }
        static async private void Bot_GotMessage(Telega bot, string msg, long chatid)
        {
            await Task.Run(() =>
                bot.TextMessage("Ваше сообщение: " + msg, chatid));
        }
    }
}
