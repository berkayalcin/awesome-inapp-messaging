using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using awesome_inapp_messaging.InAppChat.Client;
using Microsoft.AspNetCore.Http;

namespace awesome_inapp_messaging.InAppChat.WebSocket
{
    public class WebsocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ClientManager _clientManager;
        private readonly Func<ChatClient, bool> _authenticationFunction;

        public WebsocketMiddleware(RequestDelegate next, ClientManager clientManager,
            Func<ChatClient, bool> authenticationFunction = null)
        {
            _next = next;
            _clientManager = clientManager;
            _authenticationFunction = authenticationFunction;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var chatClient = new ChatClient()
            {
                Id = Guid.NewGuid(),
                Socket = socket,
                Query = context.Request.Query
            };

            var authResult = _authenticationFunction?.Invoke(chatClient) ?? true;
            if (!authResult)
            {
                await socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Authentication Failed",
                    CancellationToken.None);
            }

            _clientManager.Connect(chatClient);
            await _clientManager.ReceiveAsync(socket);
        }
    }
}