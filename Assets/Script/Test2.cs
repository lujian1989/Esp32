using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using UnityEngine.UI;

 
public class Test2 : MonoBehaviour
{
    public Button btnA;
    public Button btnB;
    private void Start()
    {
        if (NetMgr.Instance == null)
        {
            GameObject obj = new GameObject("Net");
            obj.AddComponent<NetMgr>();
        }
        NetMgr.Instance.Connect("192.168.4.1", 3333);
 
 
       
    }
 
}