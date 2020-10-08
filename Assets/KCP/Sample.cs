using UnityEngine;

namespace KcpProject
{
    public class Sample : MonoBehaviour
    {
        UDPSession connection;

        bool stopSend = false;
        byte[] buffer = new byte[1500];
        int counter = 0;
        int sendBytes = 0;
        int recvBytes = 0;

        void Start()
        {
            connection = new UDPSession();
            connection.AckNoDelay = true;
            connection.WriteDelay = false;

            connection.Connect("127.0.0.1", 4444);
        }

        private void Update()
        {
            connection.Update();

            if (connection.IsConnected && !stopSend)
            {
                //firstSend = false;
                // Console.WriteLine("Write Message...");
                //var text = Encoding.UTF8.GetBytes(string.Format("Hello KCP: {0}", ++counter));
                var sent = connection.Send(buffer, 0, buffer.Length);
                if (sent < 0)
                {
                    Debug.LogError("Write message failed.");
                    return;
                }

                if (sent > 0)
                {
                    counter++;
                    sendBytes += buffer.Length;
                    if (counter >= 500)
                        stopSend = true;
                }
            }

            int n = connection.Recv(buffer, 0, buffer.Length);
            if (n == 0)
            {
                return;
            }
            else if (n < 0)
            {
                Debug.LogError("Receive Message failed.");
                return;
            }
            else
            {
                recvBytes += n;
                Debug.LogError($"{recvBytes} / {sendBytes}");
            }

            //var resp = Encoding.UTF8.GetString(buffer, 0, n);
            //Console.WriteLine("Received Message: " + resp);
        }
    }
}
