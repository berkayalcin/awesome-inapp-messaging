using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using awesome_inapp_messaging.InAppChat.WebSocket;
using Newtonsoft.Json;

namespace awesome_inapp_messaging.InAppChat.Client
{
    public sealed class ClientManager
    {
        private readonly ConcurrentDictionary<Guid, ChatClient> _clients =
            new ConcurrentDictionary<Guid, ChatClient>();

        public Func<string, string> OnMessageReceived;

        public ChatClient GetClientById(Guid id)
        {
            return _clients
                .FirstOrDefault(client => client.Key.Equals(id))
                .Value;
        }

        public IEnumerable<ChatClient> GetAllClients()
        {
            return _clients.Select(client => client.Value);
        }


        public bool Connect(ChatClient client)
        {
            return _clients.TryAdd(client.Id, client);
        }

        public async Task DisconnectAsync(Guid id)
        {
            _clients.TryRemove(id, out var client);
            try
            {
                await client.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    "Closed by the Client Manager",
                    CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Task DisconnectAsync(System.Net.WebSockets.WebSocket socket) =>
            DisconnectAsync(_clients.FirstOrDefault(t => t.Value.Socket == socket).Key);

        public Task DisconnectAsync(ChatClient client) => DisconnectAsync(client.Id);

        public async Task SendMessageAsync(WebsocketMessage message, System.Net.WebSockets.WebSocket receiver)
        {
            if (receiver.State != WebSocketState.Open)
                return;

            var jsonString = JsonConvert.SerializeObject(message);
            await receiver.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(jsonString),
                    0,
                    jsonString.Length),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }

        public async Task SendMessageAsync(WebsocketMessage message)
        {
            foreach (var chatClient in GetAllClients())
            {
                await SendMessageAsync(message, chatClient.Socket);
            }
        }

        public async Task ReceiveAsync(System.Net.WebSockets.WebSocket socket)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                    CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                var incomingMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                OnMessageReceived.Invoke(incomingMessage);
            }

            await DisconnectAsync(socket);
        }
    }
}