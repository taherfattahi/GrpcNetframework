# GrpcNetframework
use <a href="https://grpc.io/docs/what-is-grpc/introduction/">Grpc</a> service in .net framework
<br>
in GrpcLibrary use <b>transfer.cm</b> for Generated source code by the protocol buffer compiler
<br>
## ChatServiceToServer.proto:
```html
syntax = "proto3";
package GrpcLibrary;
service GrpcLibrary {
  rpc RequestStringFunction (RequestString) returns (RequestString);
  rpc SendMessageFunction (SendMessage) returns (SendMessage);
  rpc RequestByteFunction (RequestByte) returns (RequestByte);
  rpc SayHello (HelloRequest) returns (HelloReply) {}
}

message Empty {}

message HelloRequest {
  string name = 1;
}
  
message HelloReply {
  string message = 1;
}

message RequestString {
    string sourceId = 1;
    repeated string destinationIDs = 2;
    string rqst = 3;
    string option = 4;
}

message SendMessage {
    string sourceID = 1;
    repeated string destinationIDs = 2;
    string msg = 3;
    string option = 4;
}

message RequestByte {
    string sourceID = 1;
    repeated string destinationIDs = 2;
    string msg = 3;
    bytes rqst = 4;
    string option = 5;
}
```
## GrpcServer:
```html
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
```

## GrpcClient:
```html
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

```
