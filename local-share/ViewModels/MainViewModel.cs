using local_share.Models;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace local_share.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<OnlineUser> OnlineUsers { get; } = new ObservableCollection<OnlineUser>();

        public async Task Start(string url)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl($"{url}hub/share?name=Server")
                .WithAutomaticReconnect()
                .Build();

            connection.On<IEnumerable<OnlineUser>>("UsersUpdated", users =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnlineUsers.Clear();
                    foreach (var u in users)
                        OnlineUsers.Add(u);
                });
            });

            await connection.StartAsync();
        }
    }
}
