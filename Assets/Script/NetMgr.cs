using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
 
public class NetMgr : MonoBehaviour
{
    private static NetMgr instance;
    public static NetMgr Instance => instance;
 
    //客户端Socket
    private Socket socket;
 
    //用于发送消息队列 公共容器 主线程往里面放
    //private Queue<string> sendMsgQueue = new Queue<string>();
 
    //用于处理分包时，缓存的字节数组和字节数组长度
    private byte[] cacheBytes = new byte[1024 * 1024];
    private int cacheNum = 0;
 
    //是否连接
    private bool isConnected = false;
 
    private int SEND_HEART_MSG_TIME = 2;
 
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
 
    public void Connect(string ip, int port)
    {
        //避免多次连接
        if (isConnected) return;
        if (socket == null)
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //连接服务端
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
 
        try
        {
            print("开始服务器连接.." + ip + " " +port);
            socket.Connect(ipPoint);
            isConnected = true;
 
            //开启发送线程
            //ThreadPool.QueueUserWorkItem(SendMsg);
 
        }
        catch (SocketException s)
        {
            if (s.ErrorCode == 10061)
                print("服务器拒绝连接");
            else
                print("连接失败" + s.ErrorCode + s.Message);
        }
    }
 
    //发送消息
    public void Send(string str) 
    {
        //sendMsgQueue.Enqueue(str);
        socket.Send(Encoding.UTF8.GetBytes(str));
    }
 
    //private void SendMsg(object state)
    //{
    //    while (isConnected)
    //    {
    //        if (sendMsgQueue.Count > 0)
    //        {
    //            string str = sendMsgQueue.Dequeue();
    //            print("Send:" + str);
    //            socket.Send(Encoding.UTF8.GetBytes(str));
    //        }
    //    }
    //}
 
    public void Close()
    {
        if (socket != null)
        {
            print("客户端主动断开连接");
            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(false);
            socket.Close();
            socket = null;
            isConnected = false;
        }
    }
 
    private void OnDestroy()
    {
        Close();
    }
}