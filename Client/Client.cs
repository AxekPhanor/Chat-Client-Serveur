using System.Net;
using System.Net.Sockets;

namespace Client;

public class ClientSocket {
    public Guid id = Guid.NewGuid();
    public string? msg = "";
    public string? Username {get; set;} = "";
    private static IPAddress IPAddress = IPAddress.Loopback;
    public IPEndPoint EndPoint = new IPEndPoint(IPAddress, 2345);
    public Socket Socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    
}