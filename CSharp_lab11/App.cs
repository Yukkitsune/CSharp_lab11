using Functions;
using marketContext;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app
{
    internal class App
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            using (var db = new MarketContext())
            {
                var marketFuncs = new MarketFunctions(db);
                int port = 1112;
                var server = new AsyncEchoServer(port, marketFuncs);
                server.Start();
                Console.WriteLine("TCP server is running. Press Enter to shut down.");
                Console.ReadLine();
            }
        }
    }
}