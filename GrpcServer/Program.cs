using Grpc.Core;
using GrpcLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServer
{
    class GrpcImpl : GrpcLibrary.GrpcLibrary.GrpcLibraryBase
    {
        // SayHello
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
        }
    }
    class Program
    {
        static int[] listofPortNumber = new int[] { 10011, 10012, 10013, 10007 };
        static void Main(string[] args)
        {
            int Port = FreeTcpPort();

            Server server = new Server
            {
                Services = { GrpcLibrary.GrpcLibrary.BindService(new GrpcImpl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("GrpcService server listening on port " + Port);
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
        public static int FreeTcpPort()
        {
            for (int i = 0; i < listofPortNumber.Length; i++)
            {
                try
                {
                    TcpListener l = new TcpListener(IPAddress.Loopback, listofPortNumber[i]);
                    l.Start();
                    int port = ((IPEndPoint)l.LocalEndpoint).Port;
                    l.Stop();
                    return port;
                }
                catch
                {
                    continue;
                }
            }
            return 0;
        }

    }
}
