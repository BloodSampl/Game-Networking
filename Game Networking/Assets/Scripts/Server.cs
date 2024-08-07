using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class Server : MonoBehaviour
{
    private Socket socket;
    public List<Socket> clients = new List<Socket>();

    public delegate void ConnectedToServer();
    public event ConnectedToServer connectedToServer;

    public static Server instance;

    public bool acceptingNewClients = true;
    public int maxClients = 3;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(new IPEndPoint(IPAddress.Any, 3000));
        socket.Listen(10);
        socket.Blocking = false;
        Debug.Log("Server started, waiting for connections...");
    }

    private void OnDestroy()
    {
        socket.Close();
    }

    void Update()
    {
        if (acceptingNewClients) AcceptNewClients();
        ReceiveDataFromClients();
    }

    private void AcceptNewClients()
    {
        try
        {
            Socket newClient = socket.Accept();
            clients.Add(newClient);
            Debug.Log("New client connected.");
            connectedToServer?.Invoke();
        }
        catch
        {
            // No pending connections
        }
    }

    private void ReceiveDataFromClients()
    {
        foreach (Socket client in clients)
        {
            if (client.Available > 0)
            {
                byte[] buffer = new byte[client.Available];
                client.Receive(buffer);
                MessagePacket messagePacket = new MessagePacket().Deserialize(buffer);
                Debug.Log($"Server received message from {messagePacket.playerData.name}: {messagePacket.message}");
                BroadcastData(buffer, client);
            }
        }
    }

    private void BroadcastData(byte[] data, Socket exceptClient)
    {
        foreach (Socket client in clients)
        {
            if (client != exceptClient)
            {
                client.Send(data);
            }
        }
    }
}
