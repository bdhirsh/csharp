using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace practice {
    class Server {
        readonly int SERVER_PORT;
        readonly string SERVER_IP;

        public Server(int port, string ip) {
            SERVER_PORT = port;
            SERVER_IP = ip;
        }

        public void start() {
            IPAddress localIP = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localIP, SERVER_PORT);
            Console.WriteLine("Listening on port " + SERVER_PORT);
            listener.Start();

            while (true) {
                TcpClient client = listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(serviceClient), client
                );
            }
        }

        //QUESTION: better approach here than to make argument type Object?
        public void serviceClient(Object c) {
            TcpClient client = c as TcpClient;
            if (client != null) {

                //get the data from the client through a stream
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                //write the data back to the client and wait 10 seconds in total per thread
                Thread.Sleep(5000);
                int workerThreads;
                int portThreads;
                ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
                Console.WriteLine("received data from client: " + data);
                Console.WriteLine("sending back: " + workerThreads + ", " + portThreads);
                string outputStr = formatBody(
                                "<html>\r\n<body>\r\n"
                                + "<h1>Hello!</h1>\r\nunused worker threads: "
                                + workerThreads + "\r\nunused port threads: " + portThreads + "\r\n"
                                + "</body>\r\n</html>"
                );
                byte[] output = Encoding.UTF8.GetBytes(outputStr);
                nwStream.Write(output, 0, output.Length);
                Thread.Sleep(5000);
                client.Close();

            } else {
                Console.WriteLine("ERROR: serviceClient was not passed as TCP Client");
            }
        }

        private string formatBody(string body) {
            StringBuilder str = new StringBuilder();
            str.Append("HTTP/1.0 200 OK\r\n");
            str.Append("Date: " + DateTime.Now + "\r\n");
            Console.WriteLine("current date: " + DateTime.Now);
            str.Append("Content-Type: text/html\r\n");
            str.Append("Content-Length: ");
            int totalLength = str.Length + body.Length + 4; // include the \r\n\r\n preceding body
            totalLength += ("" + totalLength).Length; // include the physical length in calculation
            str.Append(totalLength + "\r\n\r\n");
            str.Append(body);
            return str.ToString();
        }
    }
}