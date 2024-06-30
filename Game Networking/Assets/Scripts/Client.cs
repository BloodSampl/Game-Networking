using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    private Socket client;
    private TMP_Text text;
    // Start is called before the first frame update
    private void Awake()
    {
        text = FindObjectOfType<TMP_Text>();
    }

    void Start()
    {
        client = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.132"), 80);
        client.Connect(ipEndPoint);
        print("Connected to server");
    }

    // Update is called once per frame
    void Update()
    {
        if (client.Connected && client.Available > 0)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = client.Receive(buffer);
            string messageRecieved = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Debug.Log("messege from server : "+ messageRecieved);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            SendToServer(text.text);
        }
    }

    void SendToServer(string message)
    {
        Byte[] messageToSend = Encoding.ASCII.GetBytes(message);
        client.Send(messageToSend);
    }
    

    private void OnApplicationQuit()
    {
        if (client != null)
        {
            client.Close();
        }
    }
}
