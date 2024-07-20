using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SyncSocketServer : BaseSocket{
    private Socket serverSocket; // 服务端通讯主机socket, 监听打进来的电话，并转接给客服
    private Socket kefuScoket; // 客服socket, 负责与客户一对一通讯
    private Action<string> msgCallback; // 消息回调
    private byte[] readBuff; // 收到消息的缓存
 
    public SyncSocketServer(Action<string> callback) {
        msgCallback = callback;
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 12345);
        serverSocket.Bind(endPoint); // 绑定ip和端口
        new Thread(() => {
            Listening();
        }).Start();
    }
 
    public void Listening() {
        serverSocket.Listen(1); // 监听电话连接, 设置最大客服人数, 如果是0就是无限个客服
        kefuScoket = serverSocket.Accept(); // 接电话, 分配客服和客户进行一对一通信, 没有电话打进来就会一直阻塞
        msgCallback("本地端口是: " + kefuScoket.LocalEndPoint.ToString());
        msgCallback("客户的远程端口是: " + kefuScoket.RemoteEndPoint.ToString());
        readBuff = new byte[1024];
        while(true) {
            Receive();
        }
    }
 
    public void Receive() {
        Array.Clear(readBuff, 0, readBuff.Length); // 清空缓存
        int count = kefuScoket.Receive(readBuff); // 收到消息, 并存放在缓冲区, 没有消息就会一直阻塞
        string msg = Encoding.UTF8.GetString(readBuff, 0, count);
        msgCallback("客户端发来消息: " + msg);
        Debug.Log(msg);
    }
 
    public void Send(string msg) {
        Debug.Log("Send"+msg);
        byte[] buffer = Encoding.UTF8.GetBytes(msg);
        kefuScoket.Send(buffer);
    }
 
    ~SyncSocketServer() {
        kefuScoket.Close();
        serverSocket.Close();
    }
}