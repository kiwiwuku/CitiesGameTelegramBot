﻿using System;
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
        static char lastEndLetter = ' ';

        static void Main(string[] args)
        {
            Initialize();
            Console.WriteLine("Бот запущен");
            Console.ReadKey();
        }
        static private void Initialize()
        {
            bot = new Telega(key);
            bot.GotMessage += Bot_GotMessage;
            
        }
        static async private void Bot_GotMessage(Telega bot, string msg, long chatid)
        {
            string answer;
            if (lastEndLetter == msg.First() || lastEndLetter == ' ')
            {
                if (manager.IsCityUsed(msg))
                    answer = "Уже было";
                else if (!manager.IsCityExist(msg))
                    answer = "Не знаю такой город:(";
                else
                {
                    answer = manager.GetRandomCity(msg);
                    lastEndLetter = manager.GetLastLetter(answer).ToCharArray()[0];
                    if (answer == "404")
                    {
                        answer = "Ты выиграл! Начинаем заново";
                        manager.StartAgain();
                        lastEndLetter = ' ';
                    }
                }
            }
            else
                answer = "Что то тут не так";
            await Task.Run(() =>
                        bot.TextMessage(answer, chatid));
        }
    }
}
