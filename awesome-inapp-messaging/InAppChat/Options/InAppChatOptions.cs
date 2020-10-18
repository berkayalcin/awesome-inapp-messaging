using System;
using awesome_inapp_messaging.InAppChat.Client;

namespace awesome_inapp_messaging.InAppChat.Options
{
    public class InAppChatOptions
    {
        public Func<ChatClient, bool> AuthenticationFunc { get; set; }
        public string InAppChatUrl { get; set; } = "in-app-chat";
    }
}