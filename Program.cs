using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Please enter all arguments!");
            }
            else
            {
                string pathToFile = args[0];
                int playYrsMax = int.Parse(args[1]);
                double minRating = double.Parse(args[2]);
                string pathToCsvFile = args[3];

                DateTime now = DateTime.Today;
                int currYear = int.Parse(now.ToString("yyyy"));

                using (StreamReader r = new StreamReader(pathToFile))
                {
                    Console.WriteLine("Reading json file...");
                    var json = r.ReadToEnd();
                    JArray playersData = JArray.Parse(json);
                    JArray sortPlayersData = new JArray(playersData.OrderByDescending(obj => obj["Rating"])
                                                                   .ThenBy(obj => obj["Name"]));

                    var csv = new StringBuilder();
                    csv.AppendLine("Name, Rating");

                    foreach (var playerData in sortPlayersData)
                    {
                        double pRating = playerData["Rating"].ToObject<double>();
                        string pName = playerData["Name"].ToObject<string>();

                        if ((currYear - playerData["PlayerSince"].ToObject<int>()) < playYrsMax &
                            pRating > minRating)
                        {
                            var newLine = string.Format("{0},{1:F1}", pName, pRating);
                            csv.AppendLine(newLine);
                        }
                    }
                    if (csv.Length > 14)
                    {
                        Console.WriteLine("Writing csv file...");
                        File.WriteAllText(pathToCsvFile, csv.ToString());
                        Console.WriteLine("Writing csv file Done!");
                    }
                }
            }
        }
    }
}
