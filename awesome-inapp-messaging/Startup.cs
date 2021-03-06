using awesome_inapp_messaging.InAppChat.Extensions;
using awesome_inapp_messaging.InAppChat.Options;
using awesome_inapp_messaging.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace awesome_inapp_messaging
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddInAppMessaging(message => MessageReceiver.Handle(message));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
         
            
            app.UseHttpsRedirection();

            app.UseInAppMessaging(new InAppChatOptions()
            {
                InAppChatUrl = "/in-app-chat",
                AuthenticationFunc = client => AuthenticationHandler.CheckCanAuth(client)
            });
            
            app.UseRouting();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}