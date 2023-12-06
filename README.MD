## Домашнее задание к Уроку 5. Базы данных: Entity framework, code first/db first:

Реализуйте тип сообщений List, при котором клиент будет получать все непрочитанные сообщения с сервера.

---
## Решение:

Тип сообщений List, при котором клиент будет получать все непрочитанные сообщения с сервера реализовано следующим образом:

1. В классе Server добавлен следующий код:

	1.1.  Поле (стр.10) messages:
	```
	private List<Message> messages;
	```
	1.2. Метод(стр. 42 - 45) SendMessage(Message message):
	```
	    public void SendMessage(Message message)
        {
            messages.Add(message);
        }
	```
	1.3. Метод (стр. 47 - 69) GetUnreadMessages(Recipient recipient):
	```
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
	````
2. В класс Message добавлен следующий код:

      2.1. Свойство (стр. 7) Read;
      ```
      public bool Read = false;
      ```
      2.3. Свойство (стр. 8)  Id 
      ```
      public int Id { get; set; }
      ```
3. Для демонстрации работы реализации текущей задачи был создан класс Recipient с единственным свойством Name:
    ```
    public string Name { get; set; }

    ```
4. В методе Main продемонстрирована работа реализации текущей задачи:

    4.1. Создаем экцемплян класса (стр. 68) Recipient:
    ```

     Recipient recipient = new Recipient { Name = "Vitaly" };
    ```
    4.2. Создаем экземпляры(сообщений) класса Message (стр. 70 - 84):
    ```

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
    ```
    4.3. Добавляем в List сообщения (стр. 86 - 87), используя метод SendMessage(Message message):
    ```
    Server.Instance.SendMessage(message1);
    Server.Instance.SendMessage(message2);
    ```
    4.4. С помощью метода  GetUnreadMessages(recipient) (стр. 89) определяем непрочитанные сообщения и выводим в консоль(передаем пользователю):
    ```
    Server.Instance.GetUnreadMessages(recipient);
    ```