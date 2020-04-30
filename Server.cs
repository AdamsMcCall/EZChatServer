using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

namespace EZChatServer
{
    public class Server
    {
        private int _port;
        private IPAddress _address;
        private IPEndPoint _localEndPoint;
        private Socket _listener;
        private uint _maxSize = 1024;

        public Server(int port = 4242)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());

            _port = port;
            _address = ipHost.AddressList[0];
            _localEndPoint = new IPEndPoint(_address, _port);
            _listener = new Socket(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Initializing server on port " + _port.ToString());
        }

        public void Listen(int queueSize)
        {
            Console.WriteLine("Start listening");
            try
            {
                _listener.Bind(_localEndPoint);
                _listener.Listen(queueSize);
                //Temporary solution
                //Should be replaced by threads
                while (true)
                {
                    Console.WriteLine("Waiting connection...");
                    Socket clientSocket = _listener.Accept();
                    Console.WriteLine(((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString() + " connected");
                    Console.WriteLine("Message received: " + DecodeClientMessage(clientSocket));
                    clientSocket.Send(Encoding.ASCII.GetBytes("Message received!"));
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION CAUGHT: " + e.Message);
                Console.WriteLine("=============");
                Console.WriteLine("Stacktrace: " + e.StackTrace);
            }
        }

        private string DecodeClientMessage(Socket client)
        {
            byte[] data = new byte[_maxSize];
            string output = null;

            do
            {
                int numByte = client.Receive(data);

                output += Encoding.ASCII.GetString(data, 0, numByte);
            } while (output.IndexOf("<EOF>") < 0);
            return (output);
        }

    }
}
