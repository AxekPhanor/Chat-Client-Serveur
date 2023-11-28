using System.Net;
using System.Net.Sockets;
using Microsoft.VisualBasic;

namespace Serveur;

public class Client 
{
    public string Id { get; set; } = "";
    public string Username { get; set; } = "";
    private static IPAddress IPAddress = IPAddress.Loopback;
    public IPEndPoint EndPoint = new IPEndPoint(IPAddress, 2345);
    public Socket Socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    public string Msg { get; set; } = "";
}