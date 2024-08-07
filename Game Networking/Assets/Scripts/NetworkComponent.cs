using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkComponent : MonoBehaviour
{
    public string ownerID;
    public string ownerName;

    public bool IsMine()
    {
        return ownerID == NetworkManager.instance.playerData.id;
    }
}
