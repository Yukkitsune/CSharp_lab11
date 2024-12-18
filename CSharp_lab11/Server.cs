using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Web;
using Functions;

namespace Server
{
    public class AsyncEchoServer
    {
        private readonly int _listeningPort;
        private readonly MarketFunctions _marketFunctions;
        public AsyncEchoServer(int port, MarketFunctions marketFunctions)
        {
            _listeningPort = port;
            _marketFunctions = marketFunctions;
        }

        public async void Start()
        {

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(ipAddress, _listeningPort);
            listener.Start();
            Console.WriteLine("Server is running on port " + _listeningPort);
            while (true)
            {
                try
                {
                    var tcpClient = await listener.AcceptTcpClientAsync();
                    HandleConnectionAsync(tcpClient);
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.ToString());
                }

            }

        }

        private async void HandleConnectionAsync(TcpClient tcpClient)
        {
            string clientInfo = tcpClient.Client.RemoteEndPoint.ToString();
            Console.WriteLine("Client connected: " + clientInfo);
            try
            {

                using (var networkStream = tcpClient.GetStream())
                using (var reader = new StreamReader(networkStream, Encoding.UTF8))
                using (var writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true })
                {
                    while (true)
                    {
                        string ticker = await reader.ReadLineAsync();
                        if (string.IsNullOrEmpty(ticker))
                        {
                            Console.WriteLine("Client disconnected: " + clientInfo);
                            break;
                        }

                        Console.WriteLine($"Received ticker from {clientInfo}: {ticker}");
                        string startDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                        string endDate = DateTime.Now.ToString("yyyy-MM-dd");
                        await _marketFunctions.GetDataAndSaveAsync(ticker, startDate, endDate);
                        string response = await _marketFunctions.GetStockPriceAsync(ticker);
                        await writer.WriteLineAsync(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error handling client " + clientInfo + ": " + ex.Message);
            }
            finally
            {
                tcpClient.Close();
            }

        }
    }
}
