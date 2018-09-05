using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;

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
            Console.WriteLine("\n\n\nNEW CLIENT\n\n\n");
            TcpClient client = c as TcpClient;
            if (client != null) {

                //get the data from the client through a stream
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                //check what url the user would like
                string responseFirstLine = data.Substring(0, data.IndexOf(" HTTP/1.1"));
                string path = responseFirstLine.Substring(responseFirstLine.IndexOf("/"));
                Console.WriteLine("PATH: " + path);

                //if main page, display the form
                if (path.Equals("/")) {
                    Console.WriteLine("received data (" + bytesRead + " bytes) from client: " + data);
                    sendForm(nwStream);
                } else if (path.StartsWith("/?url=") && !path.Equals("/?url=")) {
                    string url = path.Substring(6);
                    url = fixUnicodeChars(url);
                    Console.WriteLine("URL: " + url);
                    sendLinkData(nwStream, url);
                } else {
                    Console.WriteLine("ERROR: " + path + " is not a valid URL");
                    sendError(nwStream, path);
                }
                
            } else {
                Console.WriteLine("ERROR: serviceClient was not passed as TCP Client");
            }
        }

        private void sendForm(NetworkStream nwStream) {
            //write the data back to the client and wait 10 seconds in total per thread
                Thread.Sleep(1000);
                int workerThreads;
                int portThreads;
                ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
                Console.WriteLine("sending back: " + workerThreads + ", " + portThreads);
                string outputStr = formatBody(
                                  "<html>\r\n"
                                +     "<body>\r\n"
                                +     "<h1>Hello!</h1>\r\n"
                                +     "ID of thread servicing your request: " + Thread.CurrentThread.ManagedThreadId + "<br>\r\n"
                                +     "unused worker threads: " + workerThreads + "<br>\r\n"
                                +     "unused port threads: " + portThreads + "<br>\r\n"
                                +     "<form>\r\nEnter a URL:<br>\r\n"
                                +       "<input type=\"text\" name=\"url\"><br>\r\n"
                                +       "<input type=\"submit\" value=\"submit\">\r\n"
                                +     "</form>\r\n"
                                +   "</body>\r\n"
                                + "</html>"
                );
                byte[] output = Encoding.UTF8.GetBytes(outputStr);
                nwStream.Write(output, 0, output.Length);
                Thread.Sleep(1000);
                nwStream.Close();
        }

        // given a URL from the response, parse it and display all links from its source
        private void sendLinkData(NetworkStream nwStream, string url) {
            Thread.Sleep(1000);
            HashSet<string> links = scraper.Scraper.scrapeLinks(url);
            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append(
                 "<html>\r\n"
              +    "<body>\r\n"
              +     "ID of thread servicing your request: " + Thread.CurrentThread.ManagedThreadId + "<br>\r\n"
              +      "<h4>Links extracted from " + url + ":</h4>\r\n"
              +      "<ul>\r\n"
            );
            foreach (string link in links) {
                outputBuilder.Append(
                       "<li>" + link + "</li>\r\n"
                );
            }
            outputBuilder.Append(
                     "</ul>\r\n"
              +    "</body>\r\n"
              +  "</html>\r\n"
            );

            string outputStr = formatBody(outputBuilder.ToString());
            byte[] output = Encoding.UTF8.GetBytes(outputStr);
            nwStream.Write(output, 0, output.Length);
            Thread.Sleep(1000);

            //redirect user if they click on a resulting hyperlink
            byte[] buffer = new byte[10]; //max length url
            Console.WriteLine("about to read in response from clicking hyperlinks...");
            int bytesRead = nwStream.Read(buffer, 0, 10);
            string data = Encoding.ASCII.GetString(buffer, 0, 10);
            Console.WriteLine("read: " + data);
            nwStream.Close();
        }

        private void sendError(NetworkStream nwStream, string path) {
            string outputStr = formatBody(
                                  "<html>\r\n"
                                +     "<body>\r\n"
                                +       "Not a valid link: " + path + "\r\n"
                                +   "</body>\r\n"
                                + "</html>"
                );
                byte[] output = Encoding.UTF8.GetBytes(outputStr);
                nwStream.Write(output, 0, output.Length);
                Thread.Sleep(1000);
                nwStream.Close();
        }

        // for some reason the submit form converts ":" into "%3A" and "/" into "%2F"
        // will stay on the lookout for any other required conversions
        // there's probably a better way than this...
        private string fixUnicodeChars(string url) {
            url = url.Replace("%3A", ":");
            url = url.Replace("%2F", "/");
            return url;
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