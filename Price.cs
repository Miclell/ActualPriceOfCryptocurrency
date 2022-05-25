using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;

namespace ActualPriceOfCryptocurrencyLibrary
{
    public class Price
    {
        private static readonly string urlAddress = $"https://api.coingecko.com/";

        //получение данных из API
        private static Tuple<string, string> GetPriceCryptocurrency(string id)
        {
            //установка подключения
            WebRequest req = WebRequest.Create(urlAddress + $"api/v3/simple/price?ids={id}&vs_currencies=usd"); //httpclient для лохов
            WebResponse resp = req.GetResponse();

            //получение JSON
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new(stream);
            string s = sr.ReadToEnd();

            //парсинг JSON, не известный формат, поэтому десериализация не используется
            string price = Regex.Matches(s, "(?<=:)[0-9.]*(?<!})").Select(x => x.Value).ToArray()[1];
            id = Regex.Matches(s, "(?<=\")[a-z]*(?<!\")").Select(x => x.Value).First();

            //возращение кортежя
            return new Tuple<string, string>(id, price);
        }

        //создание строки для вывода
        public static string GetPriceAll()
        {
            string cryptocurrencies = String.Empty;
            for (int i = 0; i < 7; i++)
            {
                var a = GetPriceCryptocurrency(Data.cryptocurrencies[i]);

                cryptocurrencies += $"  {a.Item1[0].ToString().ToUpper() + a.Item1[1..]} {a.Item2}$\n";
            }

            return cryptocurrencies + "\t\t:)";
        }
    }
}
