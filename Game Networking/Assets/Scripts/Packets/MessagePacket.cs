using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MessagePacket : BasePacket
{
    public string message;

    public MessagePacket() : base(Type.Message, new PlayerData())
    {
        this.message = "";
    }

    public MessagePacket( PlayerData playerData, string message) : base(Type.Message, playerData)
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



public class PositionPacket : BasePacket
{
    public Vector3 position;

    public PositionPacket():base(Type.Position, new PlayerData())
    {
        
    }

    public PositionPacket( PlayerData playerData, Vector3  possition) : base(Type.Position, playerData)
    {
        this.position = possition;
    }

    public override byte[] Serialize()
    {
        BeginSerialize();
        bw.Write(position.x);
        bw.Write(position.y);
        bw.Write(position.z);
        return EndSerialize();
    }

    public PositionPacket Deserialize(byte[] data)
    {
        PositionPacket messagePacket = new PositionPacket();
        BasePacket basePacket = messagePacket.BaseDeserialize(data);
        messagePacket.type = basePacket.type;
        messagePacket.playerData = basePacket.playerData;
        Vector3 position = new();
        position.x = basePacket.br.ReadSingle();
        position.y = basePacket.br.ReadSingle();
        position.z = basePacket.br.ReadSingle();
        messagePacket.position = position;
        return messagePacket;
    }
}