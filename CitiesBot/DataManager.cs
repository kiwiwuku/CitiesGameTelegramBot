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
        private string citiespath = @"Txts\allcities.txt";
        private string bigcitiespath = @"Txts\allcities.txt";
        public DataManager()
        {
            Cities = new List<string>();
            BigCities = new List<string>();
            GetLists();
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
            IEnumerable<string> list = File.ReadAllLines(citiespath);
            return list.Contains(CityNameHandler(name.ToLower()));
        }
        public string GetRandomCity()
        {
            Random rnd = new Random();
            string[] list = File.ReadAllLines(bigcitiespath);
            string city = list[rnd.Next(list.Length)];
            return city;
        }
    }
}