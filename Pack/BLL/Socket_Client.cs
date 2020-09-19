using Make.MODEL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pack.BLL
{
    public class Socket_Client
    {
        public Socket_Client(string ip, int port)
        {
            this.iPAddress = IPAddress.Parse(ip);
            this.port = port;
        }

        /// <summary>
        /// 套接字
        /// </summary>
        private Socket client_socket;
        /// <summary>
        /// 客户端要连接的ip地址
        /// </summary>
        private IPAddress iPAddress;
        /// <summary>
        /// 客户端要连接的端口好
        /// </summary>
        private int port;
        /// <summary>
        /// 创建客户端连接的套接字
        /// </summary>
        /// <returns></returns>
        public Socket Create_Client_Socket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        /// <summary>
        /// 连接服务器
        /// </summary>
        public void Connect_Server()
        {
            client_socket = Create_Client_Socket();
            //tcp连接服务器的时候只需要连接一次，因为tcp是长链接
            client_socket.Connect(new IPEndPoint(iPAddress, port));
        }

        /// <summary>
        /// 接收来自服务器的消息
        /// </summary>
        public void Recv_Msg_By_Client()
        {   
            while (true)
            {
                try
                {
                    byte[] ser_msg = new byte[20480];
                    int count = client_socket.Receive(ser_msg);
                    string origin_msg = Encoding.UTF8.GetString(ser_msg, 0, count);
                    Console.WriteLine("总计字节:"+ count +" 内容:" + origin_msg);
                    foreach(string str_msg in origin_msg.Split('&'))
                    {
                        //GeneralControl.CQLog.Debug("服务端", str_msg);
                        if (count > 0)
                        {
                            string[] msg = str_msg.Split('$');
                            if (msg.Length >= 2)
                            {
                                if (msg[0] == "客户端") XY.Client_Recieve_Server(msg[1]);
                                else if (msg[0] == "CQ")
                                {
                                    string[] cq_msg = msg[1].Split('|');
                                    if (cq_msg.Length == 3) XY.Send_To_CQ(escape(cq_msg[2]), long.Parse(cq_msg[0]), long.Parse(cq_msg[1]));
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("离线");
                    //**离线操作
                }
            }
        }
        private string escape(string msg)
        {
            return msg.Replace("[", "&#91;").Replace("]", "&#93;");
        }
        /// <summary>
        /// 向服务器发送请求
        /// </summary>
        public void Request_Client(string send_msg)
        {
            try
            {
                byte[] by_msg = Encoding.UTF8.GetBytes(send_msg+"&");
                client_socket.Send(by_msg);
            }
            catch (Exception)
            {
                Console.WriteLine("代号为：{0}的服务端已经断开！");
                //**离线操作
            }
        }
    }
}
