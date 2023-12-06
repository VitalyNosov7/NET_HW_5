using System.Net.Sockets;
using System.Net;
using System.Text;
using task_1;

namespace task1
{
    public class Server
    {
        private List<Message> messages;
        private static Server instance;
        private static readonly object lockObject = new object();

        private static CancellationTokenSource cts = new CancellationTokenSource();
        private static CancellationToken ct = cts.Token;
        private const string serverName = "Server";

        private Server()
        {
            messages = new List<Message>();
        }

        public static Server Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new Server();
                        }
                    }
                }

                return instance;
            }
        }

        public void SendMessage(Message message)
        {
            messages.Add(message);
        }

        public List<Message> GetUnreadMessages(Recipient recipient)
        {
            List<Message> unreadMessages = new List<Message>();

            Console.WriteLine("Непрочитанные сообщения:");

            foreach (Message message in messages)
            {
                if (message.ToName == recipient.Name && !message.Read)
                {
                    unreadMessages.Add(message);
                    message.Read = true;
                }
            }
            foreach (Message mess in unreadMessages)
            {
                Console.WriteLine("Message Id: " + mess.Id);
                Console.WriteLine("From: " + mess.NickName);
                Console.WriteLine("Text: " + mess.TextMessage);
                Console.WriteLine();
            }
            return unreadMessages;
        }

        public async Task StartServer()
        {
            Dictionary<string, IPEndPoint> UsersList = new Dictionary<string, IPEndPoint>();
            UdpClient udpServer = new UdpClient(12345);
            IPEndPoint localRemouteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Ожидаем сообщение от пользователя:");

            while (!ct.IsCancellationRequested)
            {
                await Task.Run(() =>
                {

                    try
                    {
                        byte[] buffer = udpServer.Receive(ref localRemouteEndPoint);
                        string data = Encoding.ASCII.GetString(buffer);
                        var message = Message.MessageFromJson(data);
                        Console.WriteLine($"Получено сообщение от {message.ToName}," +
                        $" время получения {message.DateMessage}, ");
                        Console.WriteLine(message.TextMessage);

                        StringBuilder answer = new StringBuilder("Сообщение получено");


                        if (message.ToName == serverName)
                        {
                            if (message.TextMessage == "register")
                            {
                                UsersList.Add(message.NickName, new IPEndPoint(localRemouteEndPoint.Address, localRemouteEndPoint.Port));
                                answer.Append($"Пользователь {message.NickName} добавлен.");
                            }
                            if (message.TextMessage == "delete")
                            {
                                UsersList.Remove(message.NickName);
                                answer.Append($"Пользователь {message.NickName} удалён.");
                            }
                            if (message.TextMessage == "list")
                            {
                                foreach (var i in UsersList)
                                {
                                    answer.Append($"Пользователь: {i.Key}, IP: {i.Value} \n");
                                }
                            }
                        }
                        else
                        {
                            if (UsersList.TryGetValue(message.ToName, out IPEndPoint? ep))
                            {
                                var answerMessage = new Message()
                                {
                                    DateMessage = DateTime.Now,
                                    NickName = message.NickName,
                                    TextMessage = answer.ToString()
                                };

                                var answerData = answerMessage.MessageToJson();
                                byte[] bytes = Encoding.ASCII.GetBytes(answerData);
                                udpServer.Send(bytes, bytes.Length, localRemouteEndPoint);
                                answer.Append("Сообщение переслано (клиенту)!");
                            }
                            else
                            {
                                answer.Append("Такого пользователя не существует!");
                            }
                        }

                        var replyMessageJson = new Message()
                        {
                            DateMessage = DateTime.Now,
                            NickName = serverName,
                            TextMessage = answer.ToString()
                        }.MessageToJson();

                        byte[] replyBytes = Encoding.ASCII.GetBytes(replyMessageJson);

                        udpServer.Send(replyBytes, replyBytes.Length, localRemouteEndPoint);
                        Console.WriteLine("Ответ отправлен.");

                        if (message.TextMessage == "Exit")
                        {
                            Console.WriteLine("Для завершения работы сервера нажмите Enter!");

                            Console.ReadLine();

                            cts.Cancel();

                            Console.WriteLine("Сервер отключён!");

                            Environment.Exit(0);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
            }
            ct.ThrowIfCancellationRequested();
        }
    }
}
