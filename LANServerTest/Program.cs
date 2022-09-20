using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

string name;
string strangerName;

int port = 5050;

BackgroundWorker worker = new BackgroundWorker();
Socket Socket;
TcpListener Server;
TcpClient Client;

Console.WriteLine("Do you want to host? (y/n)");
bool isHost = Console.ReadLine().Trim() == "y";
Console.Clear();


if (isHost)
{
    Server = new TcpListener(IPAddress.Any, port);
    Server.Start();

    Socket = Server.AcceptSocket();
}
else
{
    Console.Write("Enter ip: ");
    var ip = Console.ReadLine();

    Client = new TcpClient(ip, port);
    Socket = Client.Client;
    Console.Clear();
}

Console.Write("Type username: ");
name = Console.ReadLine().Trim();
Console.Clear();

worker.DoWork += Worker_DoWork;
worker.RunWorkerAsync();

while (true)
{
    string input = Console.ReadLine().Trim();
    SendInput(input);
}

void Worker_DoWork(object? sender, DoWorkEventArgs e)
{
    while (true)
    {
        byte[] namebuffer = new byte[1000];
        Socket.Receive(namebuffer);

        byte[] buffer = new byte[5000];
        Socket.Receive(buffer);

        strangerName = Encoding.UTF8.GetString(namebuffer);
        Console.WriteLine($"\n{strangerName}: {Encoding.UTF8.GetString(buffer)}");
    }
}

void SendInput(string input)
{
    var byteName = Encoding.UTF8.GetBytes(name);
    Socket.Send(byteName);

    var bytes = Encoding.UTF8.GetBytes(input);
    Socket.Send(bytes);
}