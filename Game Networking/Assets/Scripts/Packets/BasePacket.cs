using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BasePacket
{
    public enum Type
    {
        None,
        Message
    }

    public Type type;
    public PlayerData playerData;

    protected MemoryStream wms;
    protected BinaryWriter bw;

    public MemoryStream rms;
    public BinaryReader br;

    public BasePacket()
    {
        this.type = Type.None;
        this.playerData = new PlayerData();
    }

    public BasePacket(Type type, PlayerData playerData)
    {
        this.type = type;
        this.playerData = playerData;
    }

    public void BeginSerialize()
    {
        wms = new MemoryStream();
        bw = new BinaryWriter(wms);

        bw.Write((int)type);
        bw.Write(playerData.id);
        bw.Write(playerData.name);
    }

    public byte[] EndSerialize()
    {
        return wms.ToArray();
    }

    public BasePacket BaseDeserialize(byte[] data)
    {
        rms = new MemoryStream(data);
        br = new BinaryReader(rms);

        type = (Type)br.ReadInt32();
        playerData = new PlayerData(br.ReadString(), br.ReadString());

        return this;
    }

    public virtual byte[] Serialize()
    {
        BeginSerialize();
        return EndSerialize();
    }
}