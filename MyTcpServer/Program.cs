// See https://aka.ms/new-console-template for more information
using MyTCPServer;

Console.WriteLine("Hello, World!");

Server server = new Server("127.0.0.1", 4040);

Console.ReadLine();