using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Pipelines;

namespace MyTCPServer
{
    internal class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public Server()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 9090);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
            Console.WriteLine("Server started at {0} :{1} @ {2}", IPAddress.Any, 9090, DateTime.Now.ToString());
        }

        public Server(string ip, int port)
        {
            var listenEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var listenSocket = new Socket(listenEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.NoDelay = true;
            listenSocket.Bind(listenEndpoint);
            listenSocket.Listen();

            Task.Factory.StartNew(async () =>
            {
                await KeepAccept(listenSocket);
            });
        }

        private async Task KeepAccept(Socket listenSocket)
        {
            while (true)
            {
                try
                {
                    var client = await listenSocket.AcceptAsync().ConfigureAwait(false);
                    IList<ArraySegment<byte>> array = new List<ArraySegment<byte>>();
                    array.Add(new ArraySegment<byte>(new byte[] { (byte)'O', (byte)'K' }));
                    await client.SendAsync(array, SocketFlags.None);
                }
                catch (Exception e)
                {
                    if (e is ObjectDisposedException || e is NullReferenceException)
                        break;

                    if (e is SocketException se)
                    {
                        var errorCode = se.ErrorCode;

                        //The listen socket was closed
                        if (errorCode == 125 || errorCode == 89 || errorCode == 995 || errorCode == 10004 || errorCode == 10038)
                        {
                            break;
                        }
                    }

                    continue;
                }
            }
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();

                //create a thread to handle communication
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            Console.WriteLine("Client @[{0}] connected @{1}", tcpClient.Client.LocalEndPoint, DateTime.Now.ToString());

            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead = 0;
            //bool isRight=false;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    Console.WriteLine("Error:receive msg error");
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    Console.WriteLine("Client @[{0}] disconnect @{1}", tcpClient.Client.LocalEndPoint, DateTime.Now.ToString());
                    break;
                }

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                //System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));
                string recvstr = encoder.GetString(message, 0, bytesRead);
                Console.WriteLine("Recv:[{1}]:msg:@[{0}] @{2}", recvstr, tcpClient.Client.LocalEndPoint, DateTime.Now.ToString());

                //send msg to client
                string sendstr = "Server OK";
                if (recvstr == "101")
                {
                    //isRight = true;
                    sendstr = "202";
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                byte[] buffer = encoder.GetBytes(sendstr);
                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();

                Console.WriteLine("Sent:[{1}]:msg:@[{0}] @{2}", sendstr, tcpClient.Client.LocalEndPoint, DateTime.Now.ToString());
            }

            tcpClient.Close();
        }
    }
}