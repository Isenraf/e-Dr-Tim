using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Lab
{
    public void Main()
    {
        int port = 4148; // Le port configuré dans le Yumizen
        var listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"En attente de connexion sur le port {port}...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Task.Run(() => HandleClient(client));
        }
    }

    static void HandleClient(TcpClient client)
    {
        Console.WriteLine("Connexion reçue...");
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[4096];
        int bytesRead;
        StringBuilder message = new StringBuilder();

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            string chunk = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            message.Append(chunk);

            // Optionnel : afficher les données brutes
            Console.Write(chunk);
        }

        Console.WriteLine("\n--- Message complet reçu ---\n");
        Console.WriteLine(message.ToString());

        stream.Close();
        client.Close();
    }
}