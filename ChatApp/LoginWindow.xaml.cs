
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using ChatApp.SignalR;


namespace ChatApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private User client;
        private SignalRService server;
        private bool isAvailable = false;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void btnLogin_ClickAsync(object sender, RoutedEventArgs e)
        {
            server = new SignalRService();
            await server.ConnectToServer();
            // Check if already exists.
            if (!await server.IsUserAvailable(txtUsername.Text))
            {
                // Create new user, connect to server.
                if (txtUsername.Text != string.Empty)
                {
                    client = new User(txtUsername.Text);
                    await server.AddUserToServer(client);
                    MainWindow mainWindow = new MainWindow(client, server);
                    mainWindow.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Käyttäjänimi on jo käytössä");
            }
        }
    }
}
