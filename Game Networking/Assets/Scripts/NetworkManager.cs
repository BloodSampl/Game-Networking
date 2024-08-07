using System;
using UnityEngine;
using System.Net.Sockets;
using System.Net;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    private Socket socket;
    public PlayerData playerData;

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
        ConnectToServer("127.0.0.1", "Player");
    }

    void OnDestroy()
    {
        socket.Close();
    }

    void Update()
    {
        ReceiveDataFromServer();
        if (Input.GetKeyDown(KeyCode.M))
        {
            SendMessage("Hello, Server!");
        }
    }
    
    public void ConnectToServer(string ipAddress = "127.0.0.1", string playerName = "Player")
    {
        playerData = new PlayerData(Guid.NewGuid().ToString(), playerName);
        socket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 3000));
        socket.Blocking = false;
        Debug.Log("Connected to server.");
    }

    public void SendMessage(string message)
    {
        MessagePacket packet = new MessagePacket(BasePacket.Type.Message, playerData, message);
        SendDataToServer(packet);
    }

    private void SendDataToServer(BasePacket packet)
    {
        byte[] data = packet.Serialize();
        socket.Send(data);
    }

    private void ReceiveDataFromServer()
    {
        if (socket.Available > 0)
        {
            byte[] buffer = new byte[socket.Available];
            socket.Receive(buffer);
            MessagePacket messagePacket = new MessagePacket().Deserialize(buffer);
            Debug.Log($"Received message from server: {messagePacket.message}");
        }
    }
}