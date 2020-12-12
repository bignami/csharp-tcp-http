using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace project
{
    class Server
    {
        private int _port = 80;
        private string _ip = "";

        public int Port
        {
            get
            {
                return _port;

            }
            set
            {
                _port = value;
            }
        }
        private string GetLoclIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    _ip = ip.ToString();
                }
            }
            return _ip;
        }



        static void Main(string[] args)
        {

           
            Server server = new Server();

            IPAddress ip = IPAddress.Parse(server.GetLoclIP());
            TcpListener listenr = new TcpListener(ip,server._port);

           
            listenr.Start();

            Console.WriteLine("{0}, {1} ", ip,server._port);

            byte[] buff = new byte[1024];

            string resMsg1= @"HTTP/1.1 200 OK
Connection: Keep-alive
Content-Type: text/html

 Hello,world";
            string resMsg2 = @"HTTP/1.1 200 OK
Connection: Keep-alive
Content-Type: text/html

 denam is good";


            byte[] res1 = Encoding.ASCII.GetBytes(resMsg1);
            byte[] res2 = Encoding.ASCII.GetBytes(resMsg2);
            while (true)
            {
                TcpClient tc = listenr.AcceptTcpClient();
                Console.WriteLine("연결됨.");
                NetworkStream stream = tc.GetStream();

              
                
                
                while ((stream.Read(buff, 0, buff.Length)) > 0)
                {
                    string req = Encoding.UTF8.GetString(buff, 0, buff.Length);

                    string[] reqMathodAndResoure = req.Substring(req.IndexOf("GET"), req.IndexOf("HTTP")).Split(" ");
                    string mathod = reqMathodAndResoure[0].ToString();
                    string resource = reqMathodAndResoure[1].ToString();


                    Console.WriteLine(resource);

                    if(resource == "/")
                    {
                        stream.Write(res1, 0, res1.Length);
                        
                    }
                    if(resource == "/deanam")
                    {
                        stream.Write(res2, 0, res2.Length);
                    }


                    Console.WriteLine(req);
                    Console.WriteLine(Encoding.UTF8.GetString(res1, 0, res1.Length));
                    
                    

                }

                stream.Close();
                tc.Close();

            }

        }
    }
}
