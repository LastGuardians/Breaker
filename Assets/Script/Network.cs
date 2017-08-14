using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.Threading;

public class Network : MonoBehaviour
{

    private string m_address = "";
    private const int m_port = 9000;
    private Socket m_socket = null;
    private bool m_isConnected = false;

    private const int m_packetSize = 1024;  // 패킷 최대 크기
    Byte[] recv_data = new Byte[m_packetSize];  // 수신 데이터

  

    // Use this for initialization
    void Start()
    {
        m_address = Client_IP();

    }


    public bool connect()
    {
        try
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_socket.Connect(m_address, m_port);
            m_socket.NoDelay = true;
            m_isConnected = true;
            m_socket.SendBufferSize = 0;
            // 비동기 수신
            m_socket.BeginReceive(this.recv_data, 0, recv_data.Length, SocketFlags.None,
                            new AsyncCallback(OnReceiveCallBack), m_socket);
            Debug.Log("Start client communication.");
        }
        catch (SocketException e)
        {
            m_socket = null;
            m_isConnected = false;
            Debug.Log("Socket Connect Error.");
            Debug.Log("End client communication.");
        }

        if (m_socket == null)
        {
            return false;
        }
        else return true;
    }

    public void Receive()
    {
        m_socket.BeginReceive(this.recv_data, 0, recv_data.Length, SocketFlags.None,
                             new AsyncCallback(OnReceiveCallBack), m_socket);
    }


    private void OnReceiveCallBack(IAsyncResult IAR)
    {
        try
        {
            Socket tempSock = (Socket)IAR.AsyncState;
            int nReadSize = tempSock.EndReceive(IAR);
            if (nReadSize != 0)
            {
                //ProcessPacket(recv_data);
                // Debug.Log("데이터 수신");
            }
            this.Receive();
        }
        catch (SocketException se)
        {
            if (se.SocketErrorCode == SocketError.ConnectionReset)
            {
                //this.BeginConnect();
            }
        }
    }

    public void BeginSend(byte[] buffer)
    {
        try
        {
            // 연결 성공시
            if (m_socket.Connected)
            {
                m_socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None,
                      new AsyncCallback(SendCallBack), buffer);
            }
        }
        catch (SocketException e)
        {
            Debug.Log("전송 에러 : " + e.Message);
        }
    }

    private void SendCallBack(IAsyncResult IAR)
    {
        string message = (string)IAR.AsyncState;
    }


    // 패킷 타입 분류
    //void ProcessPacket(byte[] recv_buff)
    //{
    //    switch (recv_buff[1])
    //    {
    //        case (byte)PacketID.clientID:
    //            //클라이언트 ID를 받는 부분
    //            //Debug.Log("clientID: "+recv_buff[2]);
    //            inputClientIDPacketdata(recv_buff);
    //            break;
    //        default:
    //            Debug.Log("Unknown PacketType!");
    //            break;
    //    }
    //}

    ////패킷 send 함수.
    //public int Send<T>(PacketID id, IPacket<T> packet)
    //{
    //    int sendSize = 0;

    //    if (m_socket != null)
    //    {

    //        byte[] packetData = packet.GetData();
    //        //Send(packetData, packetData.Length);
    //        this.BeginSend(packetData);

    //        //sendSize = m_socket.Send(packetData, packetData.Length, SocketFlags.None);
    //        if (sendSize < 0)
    //        {
    //            Debug.Log("Send<T> : Send Error!");
    //        }
    //    }

    //    return sendSize;
    //}

    public string Client_IP()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        string ClientIP = string.Empty;
        for (int i = 0; i < host.AddressList.Length; i++)
        {
            if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
            {
                ClientIP = host.AddressList[i].ToString();
            }
        }
        return ClientIP;
    }


    public void StartConnect()
    {       
        SceneManager.LoadScene("Loading");
        connect();
        //if (connect())
        //    SendClientData();
    }
}
