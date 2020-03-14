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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoronaScrape
{
    class GetWebsite
    {
        public string URL { get; private set; }
        static readonly HttpClient client = new HttpClient();

        public GetWebsite(string url)
        {
            URL = url;
        }

        public async Task<string> ScrapeSite()
        {
            string response = await client.GetStringAsync(URL);
            return response;
        }
    }
}
