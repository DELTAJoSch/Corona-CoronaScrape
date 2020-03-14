/*Webscraper purpose-built to scrape the Austrian social ministries website for current values.
    Copyright (C) 2020  Johannes Schiemer

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoronaScrape
{
    class Program
    {
        static bool exitProgram = false;
        static int reloadTimer = 21600000; //6 hours
        static long currentCases = 0;
        static long healthyCases = 0;
        static long deadCases = 0;
        static string file = "output.csv";
        static string folder;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Scrape.");
            Console.WriteLine("URL: https://www.sozialministerium.at/Informationen-zum-Coronavirus/Neuartiges-Coronavirus-(2019-nCov).html \n\r");

            for(int i = 0; i < args.Length; i++)
            {
                try
                {
                    if (args[i] == "-reload-time")
                        reloadTimer = Convert.ToInt32(args[i + 1]);

                    if (args[i] == "-output-folder")
                        folder = args[i + 1];

                    if (args[i] == "-file-name")
                        file = args[i + 1];

                    if (args[i] == "-help")
                    {
                        Console.WriteLine("Commands:");
                        Console.WriteLine("-reload-time TIME_IN_MS\tSet the reload interval in ms");
                        Console.WriteLine("-output-folder PATH/TO/FOLDER\tSet file output folder");
                        Console.WriteLine("-file-name FILE.NAME\tSet the output file name");
                    }
                }
                catch(IndexOutOfRangeException ex)
                {
                    Console.WriteLine("Arguments out of bounds. Re-check args.\n\r");
                    Console.ReadLine();
                    exitProgram = true;
                }
                catch(InvalidCastException ex)
                {
                    Console.WriteLine("Arguments are wrong format. Re-check args.\n\r");
                    Console.ReadLine();
                    exitProgram = true;
                }
            }

            GetWebsite website = new GetWebsite("https://www.sozialministerium.at/Informationen-zum-Coronavirus/Neuartiges-Coronavirus-(2019-nCov).html");

            while (!exitProgram)
            {
                string response = await website.ScrapeSite();

                int tested = await FilterNumbers.TestsDone(response);
                int dead = await FilterNumbers.Dead(response);
                int recovered = await FilterNumbers.Recovered(response);
                int cases = await FilterNumbers.AllCases(response);

                Console.WriteLine($"Tests: {tested}\n\rCases: {cases}\n\rRecovered: {recovered}\n\rDead: {dead}\n\n\r");

                var dict = new Dictionary<string, int>();
                dict.Add("dead", dead);
                dict.Add("recovered", recovered);
                dict.Add("cases", cases);
                dict.Add("tested", tested);

                if (!String.IsNullOrEmpty(folder))
                {
                    await OutputNumbers.WriteToFile(dict, $"{folder}/{file}");
                    await OutputNumbers.SendUpdateNotice(folder);
                }
                else
                {
                    await OutputNumbers.WriteToFile(dict, file);
                    await OutputNumbers.SendUpdateNotice();
                }

                Thread.Sleep(reloadTimer);
            }
        }
    }
}
