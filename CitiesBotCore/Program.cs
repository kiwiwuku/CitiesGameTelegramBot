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
        static private CommandProcessor cmdprocessor = new CommandProcessor();
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
            bot.GotCommand += Bot_GotCommand;
            cmdprocessor.NewGame += NewGameCmd;
            cmdprocessor.Add += AddCmd;
            cmdprocessor.Help += HelpCmd;
            cmdprocessor.Start += StartCmd;
        }
        static async private void Bot_GotCommand(Telega _bot, string cmd, string[] args, long chatid)
        {
            await Task.Run(async () =>
            {
                if (cmdprocessor.CanProcess(cmd))
                    cmdprocessor.ProcessCommand(cmd, args, chatid);
                else
                    await _bot.TextMessage("Не удалось найти такую команду", chatid);
            });
        }
        static async private void Bot_GotMessage(Telega bot, string msg, long chatid)
        {
            string answer;
            if (lastEndLetter == msg.ToLower().First() || lastEndLetter == ' ')
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
                answer = "Что-то тут не так";
            await Task.Run(() =>
                            bot.TextMessage(answer, chatid));
        }
        static async private void NewGameCmd(long chatid)
        {
            manager.StartAgain();
            lastEndLetter = ' ';
            await Task.Run(() =>
                            bot.TextMessage("Начинаем заново!", chatid));
        }
        static async private void AddCmd(long chatid, string[] args)
        {
            string answer = "";
            if (args[0] != "")
            {
                string city = "";
                foreach (var word in args)
                    city += word + " ";
                city.Substring(city.Length - 1);
                manager.AddCity(city);
                answer = "Город добавлен! Продолжаем";
            }
            else
                answer = "После команды /add нужно ввести название города";
            await Task.Run(() =>
                            bot.TextMessage(answer, chatid));
        }
        static async private void StartCmd(long chatid)
        {
            string welcome = "Привет! Первый ход всегда твой.";
            await Task.Run(() =>
                            bot.TextMessage(welcome, chatid));
        }
        static async private void HelpCmd(long chatid)
        {
            string help =
                "Помощь \n" +
                "Названия городов нужно вводить полные, со всеми дефисами и пробелами. Регистром можно пренебречь. \n" +
                "Если бот перестанет отвечать, перезапустите его. \n" +
                "Список команд: \n" +
                "/help - помощь, список всех команд \n" +
                "/newgame - начать новую игру \n" +
                "/add [название города] - научить бота существующему городу, который он не знает \n";
            await Task.Run(() =>
                            bot.TextMessage(help, chatid));
        }
    }
}
