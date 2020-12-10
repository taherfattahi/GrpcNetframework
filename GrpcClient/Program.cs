using Grpc.Core;
using GrpcLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] listofPortNumber = new int[] { 10011, 10012, 10013, 10007 };

            Channel channel = null;
            bool isConnected = false;
            for (int i = 0; i < listofPortNumber.Length; i++)
            {
                channel = new Channel("127.0.0.1:" + listofPortNumber[i], ChannelCredentials.Insecure);
                if (IsReady(channel))
                {
                    isConnected = true;
                    break;
                }
            }

            if (channel != null && isConnected)
            {
                var client = new GrpcLibrary.GrpcLibrary.GrpcLibraryClient(channel);
                while (true)
                {
                    try
                    {
                        Console.WriteLine("type something:  ");
                        var input = Console.ReadLine();
                        var reply = client.SayHello(new HelloRequest { Name = input });
                        Console.WriteLine("reply: " + reply.Message);

                        //channel.ShutdownAsync().Wait();
                        //Console.ReadKey();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
        public static bool IsReady(Channel channel)
        {
            channel.ConnectAsync();
            return channel.State == ChannelState.Ready;
        }

    }
}
