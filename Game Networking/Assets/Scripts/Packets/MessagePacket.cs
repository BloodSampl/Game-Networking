using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePacket : BasePacket
{
    public string message;

    public MessagePacket() : base(Type.None, new PlayerData())
    {
        this.message = "";
    }

    public MessagePacket(Type type, PlayerData playerData, string message) : base(type, playerData)
    {
        this.message = message;
    }

    public override byte[] Serialize()
    {
        BeginSerialize();
        bw.Write(message);
        return EndSerialize();
    }

    public MessagePacket Deserialize(byte[] data)
    {
        MessagePacket messagePacket = new MessagePacket();
        BasePacket basePacket = messagePacket.BaseDeserialize(data);
        messagePacket.type = basePacket.type;
        messagePacket.playerData = basePacket.playerData;
        messagePacket.message = basePacket.br.ReadString();
        return messagePacket;
    }
}
