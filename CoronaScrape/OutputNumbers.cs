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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CoronaScrape
{
    class OutputNumbers
    {
        public static async Task WriteToFile(Dictionary<string, int> values, string Path)
        {
            if (!File.Exists(Path))
            {
                File.WriteAllText(Path, "DATE;TESTED;CASES;RECOVERED;DEAD\n");
            }

            using(StreamWriter file = File.AppendText(Path))
            {
                int cases;
                int dead;
                int recovered;
                int tested;

                values.TryGetValue("cases", out cases);
                values.TryGetValue("tested", out tested);
                values.TryGetValue("recovered", out recovered);
                values.TryGetValue("dead", out dead);

                await file.WriteLineAsync($"{DateTime.Now.ToString()};{tested};{cases};{recovered};{dead}");
            }
        }

        public static async Task SendUpdateNotice()
        {
            if (!File.Exists("Update.info"))
                File.WriteAllText("Update.info","");
        }

        public static async Task SendUpdateNotice(string Folder)
        {
            if (!File.Exists($"{Folder}/Update.info"))
                File.WriteAllText($"{Folder}/Update.info","");
        }
    }
}
