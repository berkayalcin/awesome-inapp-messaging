using awesome_inapp_messaging.InAppChat.Client;

namespace awesome_inapp_messaging.InAppChat.Extensions
{
    public static class ChatClientExtensions
    {
        public static string GetQueryValue(this ChatClient client, string key)
        {
            return client.Query[key];
        }
    }
}