## �������� ������� � ����� 5. ���� ������: Entity framework, code first/db first:

���������� ��� ��������� List, ��� ������� ������ ����� �������� ��� ������������� ��������� � �������.

---
## �������:

��� ��������� List, ��� ������� ������ ����� �������� ��� ������������� ��������� � ������� ����������� ��������� �������:

1. � ������ Server �������� ��������� ���:

	1.1.  ���� (���.10) messages:
	```
	private List<Message> messages;
	```
	1.2. �����(���. 42 - 45) SendMessage(Message message):
	```
	    public void SendMessage(Message message)
        {
            messages.Add(message);
        }
	```
	1.3. ����� (���. 47 - 69) GetUnreadMessages(Recipient recipient):
	```
	    public List<Message> GetUnreadMessages(Recipient recipient)
        {
            List<Message> unreadMessages = new List<Message>();

            Console.WriteLine("������������� ���������:");

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
2. � ����� Message �������� ��������� ���:

      2.1. �������� (���. 7) Read;
      ```
      public bool Read = false;
      ```
      2.3. �������� (���. 8)  Id 
      ```
      public int Id { get; set; }
      ```
3. ��� ������������ ������ ���������� ������� ������ ��� ������ ����� Recipient � ������������ ��������� Name:
    ```
    public string Name { get; set; }

    ```
4. � ������ Main ������������������ ������ ���������� ������� ������:

    4.1. ������� ��������� ������ (���. 68) Recipient:
    ```

     Recipient recipient = new Recipient { Name = "Vitaly" };
    ```
    4.2. ������� ����������(���������) ������ Message (���. 70 - 84):
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
    4.3. ��������� � List ��������� (���. 86 - 87), ��������� ����� SendMessage(Message message):
    ```
    Server.Instance.SendMessage(message1);
    Server.Instance.SendMessage(message2);
    ```
    4.4. � ������� ������  GetUnreadMessages(recipient) (���. 89) ���������� ������������� ��������� � ������� � �������(�������� ������������):
    ```
    Server.Instance.GetUnreadMessages(recipient);
    ```