﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CitiesBot
{
    public class DataManager
    {
        public List<string> Cities;
        public List<string> BigCities;
        public List<string> UsedCities;
        private string citiespath = @"Txts\allcities.txt";
        private string bigcitiespath = @"Txts\bigcities.txt";
        public DataManager()
        {
            Cities = new List<string>();
            BigCities = new List<string>();
            UsedCities = new List<string>();
            GetLists();
        }
        public void StartAgain()
        {
            UsedCities = new List<string>();
        }
        private void GetLists()
        {
            foreach (string city in File.ReadAllLines(citiespath))
                Cities.Add(city);
            foreach (string city in File.ReadAllLines(bigcitiespath))
                BigCities.Add(city);
        }
        private string CityNameHandler(string city)
        {
            string name = "";
            string[] words = city.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = ToUpper(words[i]);
                if (words[i].Contains('-'))
                {
                    string[] arr = words[i].Split('-');
                    foreach (var part in arr)
                    {
                        name += ToUpper(part) + "-";
                    }
                }
                else
                    name += words[i] + " ";
            }
            name = name.Remove(name.Length - 1, 1);
            return name;
        }
        private string ToUpper(string str)
        {
            if (str != "")
                str = str.Substring(0, 1).ToUpper() + str.Remove(0, 1);
            return str;
        }
        public void AddCity(string city)
        {
            string name = CityNameHandler(city);
            File.AppendAllText(citiespath, "\n" + name);
        }
        public bool IsCityExist(string name)
        {
            string city = CityNameHandler(name.ToLower());
            bool result = Cities.Contains(city);
            if (result)
                UsedCities.Add(city);
            return result;
        }
        public bool IsCityUsed(string name)
        {
            return UsedCities.Contains(CityNameHandler(name.ToLower()));
        }
        public string GetRandomCity(string name)
        {
            string symbol = GetLastLetter(name);
            Random rnd = new Random();
            string[] list = BigCities.
                Where(e => e.StartsWith(symbol.ToUpper())).ToArray();
            if (list.Length == 0)
            {
                list = Cities.
                    Where(e => e.StartsWith(symbol.ToUpper())).ToArray();
                if (list.Length == 0)
                    return "404";
            }
            string city = list[rnd.Next(list.Length)];
            if (IsCityUsed(city))
            {
                for (int i = 0; i < 15; i++)
                {
                    string tempname = list[rnd.Next(list.Length)];
                    if (!IsCityUsed(tempname))
                    {
                        city = tempname;
                        break;
                    }
                    else if (i == 14)
                        return "404";
                }
            }
            UsedCities.Add(city);
            return city;
        }
        public string GetLastLetter(string name)
        {
            string symbol = name.Last().ToString();;
            if (symbol == "ь" || symbol == "ъ" || symbol == "ы")
                symbol = name[name.Length - 2].ToString();
            return symbol;
        }
    }
}