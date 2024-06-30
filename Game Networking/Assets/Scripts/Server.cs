using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class Server : MonoBehaviour
{
    private Socket serverSocket;
    private Socket clientSocket;
    void Start()
    {
        serverSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.132"), 80);
        serverSocket.Bind(ipEndPoint);
        serverSocket.Listen(4);
        serverSocket.Blocking = false;
        print("Waiting for Client");
    }
    void Update()
    {
        if (clientSocket == null)
        {
            try
            {
                clientSocket = serverSocket.Accept();
                print("Accepted Connection");
                SendMessageToClient("hello Client");
            }
            catch
            {
            
            }
        }

        try
        {
            Byte[] buffer = new byte[1024];
            int byteCount = clientSocket.Receive(buffer);
            string messageRecive = Encoding.ASCII.GetString(buffer, 0, byteCount);
            Debug.Log(messageRecive);
        }
        catch
        {
            
        }
    }

    void SendMessageToClient(string message)
    {
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        clientSocket.Send(messageBytes);
    }

    private void OnApplicationQuit()
    {
        if (clientSocket != null)
        {
            clientSocket.Close();
        }
        serverSocket.Close();
    }
}
