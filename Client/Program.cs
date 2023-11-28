using System.Net.Sockets;
using System.Text;
using Client;

ClientSocket client = new();
try
{
    client.Socket.Connect(client.EndPoint);
    Console.WriteLine($"Mon id : {client.id}");
    client.Socket.Send(Encoding.UTF8.GetBytes(client.id.ToString()));
    Console.Write("Entrer votre username: ");
    client.Username = Console.ReadLine();
    if(client.Username is not null)
    {
        client.Socket.Send(Encoding.UTF8.GetBytes(client.Username));
    }
    Thread thread = new(ReceiveServeur);
    thread.Start(client.Socket);
    while (true)
    {
        client.msg = Console.ReadLine();
        if (client.msg == "q")
        {
            break;
        }
        if (client.msg is not null)
        {
            client.Socket.Send(Encoding.UTF8.GetBytes(client.msg));
        }
    }
}
catch(Exception e)
{
    Console.WriteLine(e.Message);
}
finally
{
    if(client.Socket.Connected){
        client.Socket.Shutdown(SocketShutdown.Both);
    }
    client.Socket.Close();
}
void ReceiveServeur(object? serveurSocket)
{
    if(serveurSocket is Socket socket)
    {
        while(true)
        {
            if(!socket.Connected){
                Console.WriteLine("Connexion fermé");
                if (client.Socket.Connected)
                {
                    client.Socket.Shutdown(SocketShutdown.Both);
                }
                client.Socket.Close();
                break;
            }
            byte[] buffer = new byte[256];
            socket.Receive(buffer);
            Console.WriteLine($"{Encoding.UTF8.GetString(buffer)}");
        }
    }
}
