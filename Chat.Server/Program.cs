using System.Net;
using Akka.Actor;
using Chat.Server;
using Chat.Server.Configurations;

var config = new TcpServerConfiguration(IPAddress.Parse("127.0.0.1"), 8080);
using var actorSystem = ActorSystem.Create("ChatServerSystem", config.GetConfig());

var server = actorSystem.ActorOf(Props.Create(() => new ChatServer()), "echoServer");

Console.WriteLine($"Remote Echo Server running on {config.GetLocationDescriptor()}. Press Enter to exit.");
Console.ReadLine();

await actorSystem.Terminate();