using System;
using awesome_inapp_messaging.InAppChat.Extensions;
using Microsoft.AspNetCore.Http;

namespace awesome_inapp_messaging.InAppChat.Client
{
    public class ChatClient
    {
        public Guid Id { get; set; }
        public System.Net.WebSockets.WebSocket Socket { get; set; }
        public string UserName => this.GetQueryValue("Username");
        public IQueryCollection Query { get; set; }
    }
}