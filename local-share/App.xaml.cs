using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.Windows;
using local_share.Hubs;

namespace local_share
{
    public partial class App : Application
    {
        private WebApplication? _apiApp;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = WebApplication.CreateBuilder();

            builder.WebHost.UseUrls("http://0.0.0.0:5000");

            builder.Services.AddControllers();
            builder.Services.AddSignalR(); // 🔥 THÊM

            builder.Services.AddCors(options =>
                       {
                           options.AddPolicy("LanCors", policy =>
                           {
                               policy
                                   .SetIsOriginAllowed(_ => true)
                                   .AllowAnyHeader()
                                   .AllowAnyMethod()
                                   .AllowCredentials();
                           });
                       });

            _apiApp = builder.Build();

            _apiApp.UseDefaultFiles(); // 🔥 index.html

            _apiApp.UseStaticFiles();  // 🔥 wwwroot

            _apiApp.MapControllers();

            _apiApp.MapHub<ShareHub>("/hub/share");

            _apiApp.RunAsync(); // 🔥 chạy API nền, không block UI
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_apiApp != null)
            {
                await _apiApp.StopAsync();
            }

            base.OnExit(e);
        }
    }
}