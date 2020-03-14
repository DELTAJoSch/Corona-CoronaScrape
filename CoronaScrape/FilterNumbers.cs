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
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoronaScrape
{
    static class FilterNumbers
    {
        public static async Task<int> TestsDone(string html)
        {
            var res = Regex.Match(html, @"Testungen: <\/strong><strong>[0-9\.]*<\/strong><br \/>");

            string containsValue = res.Value;

            try
            {
                string[] cut = containsValue.Split("<strong>");
                cut = cut[1].Split(@"</strong>");
                cut[0] = cut[0].Replace(".", string.Empty);

                return Convert.ToInt32(cut[0]);
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<int> AllCases(string html)
        {
            var res = Regex.Match(html, @"F&auml;lle: [0-9\.]*<br \/>");

            string containsValue = res.Value;

            try
            {
                string[] cut = containsValue.Split(":");
                cut = cut[1].Split(@"<br />");

                cut[0].Trim();
                return Convert.ToInt32(cut[0], CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<int> Dead(string html)
        {
            var res = Regex.Match(html, @"Todesf&auml;lle: [0-9\.]*<\/strong><br \/>");

            string containsValue = res.Value;

            try
            {
                string[] cut = containsValue.Split(":");
                cut = cut[1].Split(@"</strong>");

                cut[0].Trim();
                return Convert.ToInt32(cut[0], CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<int> Recovered(string html)
        {
            var res = Regex.Match(html, @"Genesene Personen: [0-9\.]*<br \/>");

            string containsValue = res.Value;

            try
            {
                string[] cut = containsValue.Split(":");
                cut = cut[1].Split(@"<br />");

                cut[0].Trim();
                return Convert.ToInt32(cut[0], CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }
    }
}
