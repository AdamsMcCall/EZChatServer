using System;

namespace EZChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();

            server.Listen(16);
        }
    }
}
