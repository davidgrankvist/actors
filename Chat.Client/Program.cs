using System.Net;
using Akka.Actor;
using Chat.Client;
using Chat.Client.Configurations;

var tcpConfig = new TcpClientConfiguration(IPAddress.Parse("127.0.0.1"), 8080);
using var actorSystem = ActorSystem.Create("ChatClientSystem", tcpConfig.GetConfig());

var server = await tcpConfig.CreateServerRefAsync(actorSystem);
var client = actorSystem.ActorOf(Props.Create(() => new ChatClient(server)), "client");

InputReader.RunInputLoop(client);

await actorSystem.Terminate();