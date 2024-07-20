using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SyncSocketClient : BaseSocket {
    private Socket clientSocket; // 客户端socket
    private Action<string> msgCallback; // 消息回调
    private byte[] readBuff; // 收到消息的缓存

    private int msgID = 1;
    public SyncSocketClient(Action<string> callback) {
        msgCallback = callback;
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        new Thread(() => {
            clientSocket.Connect("192.168.4.1", 3333); //连接服务器, 未连接上就会一直阻塞
            Listening();
        }).Start();
    }
 
    public void Listening() {
        readBuff = new byte[1024];
        while(true) {
            Receive();
        }
    }
 
    public void Receive() {
        Array.Clear(readBuff, 0, readBuff.Length); // 清空缓存
        int count = clientSocket.Receive(readBuff); // 收到消息, 并存放在缓冲区, 没有消息就会一直阻塞
        string msg = Encoding.UTF8.GetString(readBuff, 0, count);
        msgCallback("ESP32发来消息: " + msg);
        Debug.Log(msg);
    }
 
    
    //时间戳 {"msgId":1,"msgType":2,"deviceId":"ESP32-C3","cmdType":1,"data":"2024-7-20 15:20:10"}
    //单个命令
    //{ "msgId":1, "msgType":2, "deviceId":"ESP32-C3", "cmdType":2, "data":{ "a":"A:1", "c":"C:1" } }
    //多个命令
    //{"msgId":1,"msgType":2,"deviceId":"ESP32-C3","cmdType":3,"data":[{"a":"A:1","c":"C:1","t":5}, {"a":"A:1","c":"C:2","t":10}] }
    //{"msgId":1,"msgType":2,"deviceId":"ESP32-C3","cmdType":3,"data":[{"a":"A:1","c":"C:1","t":5},{"a":"A:1","c":"C:2","t":10},{"a":"A:1","c":"C:3","t":15},{"a":"A:1","c":"C:4","t":16}]}
    public void Send(string msg) {
        byte[] buffer = Encoding.UTF8.GetBytes(msg);
        clientSocket.Send(buffer);
        msgCallback("APP上传: " + msg);
    }
 
    ~SyncSocketClient() {
        clientSocket.Close();
    }
}