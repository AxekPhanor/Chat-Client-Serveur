using System.Net;
using System.Net.Sockets;

namespace Serveur;

public class ServeurSocket()
{
    public List<Client> Users = [];
    private static IPAddress IPAddress = IPAddress.Loopback;
    public IPEndPoint EndPoint = new IPEndPoint(IPAddress, 2345);
    public Socket Socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
}