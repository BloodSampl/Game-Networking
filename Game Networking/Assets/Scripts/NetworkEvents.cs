using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkEvents : MonoBehaviour
{
    public delegate void ConnectedToServer();
    public ConnectedToServer ConnectedToServerEvent;

    public delegate void ReceivePlayerPositionPacket(Vector3 position, string GameObjectID);
    public ReceivePlayerPositionPacket ReceivePlayerPositionPacketEvent;
}