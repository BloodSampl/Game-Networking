using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public KeyCode debugKey = KeyCode.A;
    public KeyCode debugKey2 = KeyCode.W;
    private Socket socket;
    public PlayerData playerData;
    private Thread newtworkListner;
    private List<BasePacket> packets = new();
    private object key = new();

    void Start()
    {
        
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/
        
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ConnectToServer("127.0.0.1", "Player");
        newtworkListner = new Thread(ReceiveDataFromServer);
        newtworkListner.Start();
    }

    void OnDestroy()
    {
        socket.Close();
    }

    void Update()
    {
        HandlePacket();
        if (Input.GetKeyDown(debugKey))
        {
            SendMessage("Hello, Server!");
        }

        if (Input.GetKeyDown(debugKey2))
        {
            SendPosition();
        }
    }

    private void HandlePacket()
    {
        lock (key)
        {
            if (packets.Count > 0)
            {
                try
                {

                    switch (packets[0].type)
                    {
                        case BasePacket.Type.Message:
                            var message = (MessagePacket)packets[0];
                            Debug.Log(name + " recived " + message.message + " from " + message.playerData.id);
                            break;
                        case BasePacket.Type.Position:
                            var position = (PositionPacket)packets[0];
                            //Debug.Log(name + " recived " + message.message + " from " + message.playerData.id);
                            this.transform.position = position.position;
                            break;

                    }
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
                packets.RemoveAt(0);

            }
        }
    }

    public void ConnectToServer(string ipAddress = "127.0.0.1", string playerName = "Player")
    {
        playerData = new PlayerData(Guid.NewGuid().ToString(), playerName);
        socket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 3001));
        socket.Blocking = false;
        Debug.Log("Connected to server.");
    }

    public void SendMessage(string message)
    {
        MessagePacket packet = new MessagePacket( playerData, message);
        SendDataToServer(packet);
    }

    public void SendPosition()
    {
        PositionPacket packet = new(playerData, transform.position);
        SendDataToServer(packet);
    }

    private void SendDataToServer(BasePacket packet)
    {
        byte[] data = packet.Serialize();
        socket.Send(data);
    }

    private  void ReceiveDataFromServer()
    {
        while (true)
        {
            if (socket.Available > 0)
            {
                byte[] buffer = new byte[socket.Available];
                 socket.Receive(buffer);
                 BasePacket packet = new  BasePacket().BaseDeserialize(buffer);
                 if (packet.type == BasePacket.Type.Message)
                 {
                     packet = new MessagePacket().Deserialize(buffer);

                 }
                 else if(packet.type==BasePacket.Type.Position)
                 {
                     packet = new PositionPacket().Deserialize(buffer);

                 }
                
                lock (key)
                {
                    packets.Add(packet);
                }
                //Debug.Log($"Received message from server: {messagePacket.message}");
            }

        }
    }

    private void OnApplicationQuit()
    {
        newtworkListner.Abort();
    }
}