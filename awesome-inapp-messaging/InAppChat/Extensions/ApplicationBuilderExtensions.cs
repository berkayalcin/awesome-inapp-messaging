using System;
using awesome_inapp_messaging.InAppChat.Client;
using awesome_inapp_messaging.InAppChat.Options;
using awesome_inapp_messaging.InAppChat.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace awesome_inapp_messaging.InAppChat.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseInAppMessaging(this IApplicationBuilder applicationBuilder, InAppChatOptions options)
        {
            applicationBuilder.UseWebSockets();

            var clientManager = applicationBuilder.ApplicationServices.GetRequiredService<ClientManager>();
            if (clientManager == null)
                throw new ArgumentNullException(nameof(clientManager),
                    "To Use InAppMessaging,You need to AddInAppMessaging First");

            applicationBuilder.Map(options.InAppChatUrl,
                app => app.UseMiddleware<WebsocketMiddleware>(clientManager, options.AuthenticationFunc));
        }
    }
}