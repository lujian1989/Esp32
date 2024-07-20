using UnityEngine;
using UnityEngine.SceneManagement;

public class SyncSocketTest : MonoBehaviour {
    private BaseSocket socket; // 客户端/服务端socket
    private string sendText; // 发送的消息
    private string receiveText; // 接收的消息
    private bool isSideInited = false; // 是否已初始化端测
    private string sideTag = null; // 端测标记, 服务端/客户端
 
    private void Awake() {
        Application.runInBackground = true; // 支持后台运行
        GameObject.DontDestroyOnLoad(this); 
    }

    private Rect rect3;
    private Rect rect4;
    Rect rect5;
    Rect rect6;
    private void OnGUI() {
        
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        style.wordWrap = true;
        
        rect3 = new Rect(Screen.width / 2 - 120, Screen.height / 2 - 50, 240, 50);
        
        rect4 = new Rect(Screen.width / 2 - 120, Screen.height / 2, 240, 50);
        
        rect5 = new Rect(Screen.width / 2 - 120, Screen.height / 2 + 50, 240, 50);
        rect6 = new Rect(Screen.width / 2 - 120, Screen.height / 2 + 100, 240, 50);
        
        InitSide();
        initSideView();
    }
 
    private void InitSide() { // 初始化端测
        if (!isSideInited) {
            CreateServer();
            CreateClient();
        }
    }
 
    private void CreateServer() { // 创建服务器
        if (GUI.Button(rect3,"创建服务器")) {
            socket = new SyncSocketServer((msg) => {
                receiveText += msg + "\n";
            });
            sideTag = "服务端";
            isSideInited = true;
        }
    }
 
    private void CreateClient() { // 创建客户端
        if (GUI.Button(rect4,"创建客户端")) {
            socket = new SyncSocketClient((msg) => {
                receiveText += msg + "\n";
            });
            sideTag = "客户端";
            isSideInited = true;
            
            SceneManager.LoadScene("SampleScene");
        }
    }
 
    private void initSideView() { // 初始化端测界面
        if (isSideInited) {
            GUILayout.Label(sideTag);
            sendText = GUILayout.TextField(sendText);
            if (GUILayout.Button("发送")) {
                socket.Send(sendText);
            }
            GUILayout.Label("接收到的消息: ");
            GUILayout.Label(receiveText);

           
        }
    }
}