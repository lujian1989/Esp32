using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using UnityEngine.UI;
 
public class Test : MonoBehaviour
{
    private TcpClient client;
    private StreamReader reader;
    private StreamWriter writer;
    private Thread clientThread;
 
    public Button btnA;
    public Button btnB;
 
    public string serverIPAddress;
    public int serverPort;
 
    void Start()
    {
        ConnectToServer();
 
        // btnA.onClick.AddListener(() => {
        //     sendMessage("a");
        // });
        // btnB.onClick.AddListener(() => {
        //     sendMessage("b");
        // });
    }
 
    private void Update()
    {
        print(client.Connected);
    }
 
    void ConnectToServer()
    {
        try
        {
            client = new TcpClient();
            client.Connect(serverIPAddress, serverPort);
            Debug.Log("Connected to Arduino server");
 
            btnA.interactable = true;
            btnB.interactable = true;
 
            NetworkStream stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
 
            clientThread = new Thread(new ThreadStart(ReceiveMessages));
            clientThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.Log("Connection error: " + e.Message);
        }
    }
 
    /// <summary>
    /// 接收数据
    /// </summary>
    void ReceiveMessages()
    {
        try
        {
            while (client.Connected)
            {
                string message = reader.ReadLine();
                if (message != null)
                {
                    // 处理接收到的消息
                    handleMessage(message);
                }
            }
        }
        //异常断开连接
        catch (System.Exception e)
        {
            Debug.Log("Read/write error: " + e.Message);
        }
    }
 
    void sendMessage(string message)
    {
        try
        {
            writer.WriteLine(message);
            writer.Flush(); // 清空缓冲区，确保消息被发送
        }
        catch (System.Exception e)
        {
            Debug.Log("Read/write error: " + e.Message);
        }
    }
 
    void handleMessage(string message)
    {
        Debug.Log("Message received: " + message);
        // 在这里添加逻辑来处理接收到的消息
    }
 
    void OnDestroy()
    {
        if (client != null)
        {
            clientThread.Abort();
            writer.Close();
            reader.Close();
            client.Close();
        }
    }
}