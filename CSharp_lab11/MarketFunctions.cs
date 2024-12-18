using marketContext;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Functions
{
    public class StockData
    {
        public string s { get; set; } // status
        public List<double> c { get; set; } // close
        public List<double> h { get; set; } // high
        public List<double> l { get; set; } // low
        public List<double> o { get; set; } // open
        public List<int> t { get; set; } // timeStamp UNIX
        public List<int> v { get; set; } // volume

    };

    public class MarketFunctions
    {
        private readonly MarketContext _db;
        public MarketFunctions(MarketContext db)
        {
            _db = db;
        }
        public async Task GetDataAndSaveAsync(string ticker, string startDate, string endDate)
        {
            string apiKey = "bm1LdXNyLWpEUDBjME1WRVBTR1BIZjlNeVRHWGxwblE3WHpPRUJJU0Uxcz0";
            string URL = $"https://api.marketdata.app/v1/stocks/candles/D/{ticker}/?from={startDate}&to={endDate}&token={apiKey}";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(URL);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Something went wrong: {response.StatusCode}");
                return;
            }
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<StockData>(json);
            if (data == null || data.s == null || data.c == null || data.h == null || data.l == null || data.o == null || data.t == null || data.v == null)
            {
                Console.WriteLine($"Fetching data error: Not enough data");
                return;
            }
            var dbTicker = _db.Tickers.FirstOrDefault(t => t.tickerSym == ticker);
            await _db.SaveChangesAsync();
            if (dbTicker == null)
            {
                dbTicker = new Models.Ticker { tickerSym = ticker };
                _db.Tickers.Add(dbTicker);
                await _db.SaveChangesAsync();
            }
            for (int i = 0; i < data.c.Count; i++)
            {
                var price = data.c[i];
                var time = DateTimeOffset.FromUnixTimeSeconds(data.t[i]).DateTime;
                _db.Prices.Add(new Models.Price
                {
                    price = price,
                    date = time,
                    tickerId = dbTicker.id
                });
            }
            await _db.SaveChangesAsync();

        }
        public async Task AnalyzeData()
        {
            var Tickers = _db.Tickers.ToList();
            foreach (var ticker in Tickers)
            {
                var prices = _db.Prices
                    .Where(p => p.tickerSymPrices == ticker)
                    .OrderByDescending(p => p.date)
                    .ToList();
                if (prices.Count >= 2)
                {
                    var latestPrice = prices[0];
                    var previousPrice = prices.FirstOrDefault(p => p.price != latestPrice.price);
                    if (previousPrice != null && latestPrice != null)
                    {
                        var condition = latestPrice.price > previousPrice.price
                            ? $"UP    Latest Price: {latestPrice.price}    Previous Price: {previousPrice.price}" :
                            $"Down    Latest Price: {latestPrice.price}    Previous Price: {previousPrice.price}";
                        _db.todaysConditions.Add(new TodaysCondition
                        {
                            tickerId = ticker.id,
                            state = condition
                        });
                    }

                }

            }
            await _db.SaveChangesAsync();

        }
        public async Task<string> GetStockPriceAsync(string tick)
        {
            try
            {
                var dbTicker = await _db.Tickers
                    .FirstOrDefaultAsync(t => t.tickerSym == tick);

                if (dbTicker == null)
                {
                    return $"Ticker '{tick}' not found in the database.";
                }

                var latestPrice = await _db.Prices
                    .Where(p => p.tickerId == dbTicker.id)
                    .OrderByDescending(p => p.date)
                    .FirstOrDefaultAsync();

                if (latestPrice == null)
                {
                    return $"No price data available for ticker '{tick}'.";
                }

                return $"The latest price for {tick} is {latestPrice.price}.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                return "Error retrieving stock price. Please try again later.";
            }
        }
    }
}