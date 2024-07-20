public interface BaseSocket {
    void Listening(); // 监听连接, 监听消息
    void Receive(); // 接收到消息
    void Send(string msg); // 发送消息
}