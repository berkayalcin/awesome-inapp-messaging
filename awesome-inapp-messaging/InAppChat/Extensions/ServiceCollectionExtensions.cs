using System;
using awesome_inapp_messaging.InAppChat.Client;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;

namespace awesome_inapp_messaging.InAppChat.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInAppMessaging(this IServiceCollection serviceCollection,
            Func<string, string> onMessageReceived)
        {
            serviceCollection
                .AddWebSockets(options => { });

            serviceCollection.AddSingleton<ClientManager>(provider =>
            {
                var clientManager = new ClientManager
                {
                    OnMessageReceived = onMessageReceived
                };
                return clientManager;
            });
        }
    }
}