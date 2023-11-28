using System.Data.SqlTypes;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using Serveur;

ServeurSocket serveur = new ServeurSocket();


try
{
    serveur.Socket.Bind(serveur.EndPoint);
    serveur.Socket.Listen();
    Console.WriteLine("Serveur sur écoute...");
}
catch
{
    Console.WriteLine("Impossible de démarrer le serveur");
}
try
{
    while(true)
    {
        
        var clientSocket = serveur.Socket.Accept();
        Thread threadClient = new(ListenClient);
        threadClient.Start(clientSocket);
    }
}
catch(Exception e)
{
    Console.WriteLine("Erreur: " + e.Message);
}

void ListenClient(object? clientSocket)
{
    if(clientSocket is Socket socket)
    {
        if(socket.RemoteEndPoint is not null)
        {
            Console.WriteLine("I am connected to " + IPAddress.Parse(((IPEndPoint)socket.RemoteEndPoint).Address.ToString()) + " on port number " + ((IPEndPoint)socket.RemoteEndPoint).Port.ToString());
        }

        try
        {
            byte[] buffer = new byte[64];
            Client user = new();
            user.Socket = socket;

            socket.Receive(buffer);
            user.Id = Encoding.UTF8.GetString(buffer);
            Console.WriteLine($"id: {Encoding.UTF8.GetString(buffer)}");

            buffer = new byte[16];
            socket.Receive(buffer);
            user.Username = Encoding.UTF8.GetString(buffer);
            Console.WriteLine($"username: {Encoding.UTF8.GetString(buffer)}");

            serveur.Users.Add(user);
            while (true)
            {
                buffer = new byte[256];
                int nb = socket.Receive(buffer);
                if (nb == 0)
                {
                    break;
                }
                foreach (var u in serveur.Users)
                {

                    if (user.Id != u.Id && u.Socket.Connected)
                    {
                        string msg = $"{user.Username}: {Encoding.UTF8.GetString(buffer, 0, nb)}";
                        u.Socket.Send(Encoding.UTF8.GetBytes(msg));
                    }
                }
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            socket.Close();
        }
    }
}