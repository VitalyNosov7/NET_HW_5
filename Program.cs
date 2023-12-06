using System.Net;
using System.Net.Sockets;
using System.Text;
using task1;

namespace task_1
{
    class Program
    {

        public static async Task Client(string name, string ip)
        {

            UdpClient udpClient = new UdpClient();
            IPEndPoint localRemouteEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            while (true)
            {
                string[] message = Console.ReadLine().Split(' ');
                var mess = new Message()
                {
                    DateMessage = DateTime.Now,
                    NickName = name,
                    TextMessage = message[1],
                    ToName = message[0]
                };

                Console.WriteLine(mess.ToName);

                await Task.Run(() =>
                {

                    try
                    {
                        var data = mess.MessageToJson();
                        byte[] bytes = Encoding.ASCII.GetBytes(data);
                        udpClient.Send(bytes, bytes.Length, localRemouteEndPoint);

                        Console.WriteLine("Сообщение отпавлено!");

                        byte[] buffer = udpClient.Receive(ref localRemouteEndPoint);
                        data = Encoding.ASCII.GetString(buffer);
                        var messageReception = Message.MessageFromJson(data);
                        Console.WriteLine($"Получено сообщение от {messageReception.NickName}," +
                        $" время получения {messageReception.DateMessage}, ");
                        Console.WriteLine(messageReception.TextMessage);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                });

                if (message[1] == "Exit")
                {
                    Console.WriteLine("Приложение закрыто пользователем!");
                    Environment.Exit(0);
                }
            }
        }

        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {

                Recipient recipient = new Recipient { Name = "Vitaly" };

                Message message1 = new Message
                {
                    Id = 1,
                    NickName = "Maria",
                    ToName = "Vitaly",
                    TextMessage = "Hi Vitaly, how are you?"
                };

                Message message2 = new Message
                {
                    Id = 2,
                    NickName = "Denis",
                    ToName = "Vitaly",
                    TextMessage = "Hello Vitaly."
                };

                Server.Instance.SendMessage(message1);
                Server.Instance.SendMessage(message2);

                Server.Instance.GetUnreadMessages(recipient);

                await Task.Run(() => Server.Instance.StartServer());
            }
            else
            {
                await Task.Run(() => Client(args[0], args[1]));
            }
        }
    }
}