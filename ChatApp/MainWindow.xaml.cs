using System.Windows;
using ChatApp.SignalR;


namespace ChatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User client;
        private SignalRService server;
        private string textMessage = string.Empty;
        private List<User> usersOnline = new List<User>();

        public MainWindow(User client, SignalRService server)
        {
            InitializeComponent();
            
            this.client = client;
            this.server = server;
            this.server.RecieveMessageFromUser += Server_RecieveMessage;
            this.server.UpdateUserList += Server_UsersHasJoined;
            this.server.UpdateUserDisconnected += Server_UserHasDisconnected;
            GetUsersList();            
        }

        // Events
        private void Server_RecieveMessage(User client, string message)
        {
            if (client.Username != this.client.Username)
            {
                Dispatcher.Invoke(() =>
                {
                    txtConsole.AppendText(client.Username + ": " + message + "\n");
                    txtConsole.ScrollToEnd();
                });
            }

        }

        private void Server_UsersHasJoined(User user)
        {
            Dispatcher.Invoke(() =>
            {
                usersOnline.Add(user);
                dgUsers.Items.Refresh();
            });

        }

        private void Server_UserHasDisconnected(User user)
        {
            User userToRemove = usersOnline.FirstOrDefault(u => u.Username == user.Username);
            Dispatcher.Invoke(() =>
            {
                usersOnline.Remove(userToRemove);
                dgUsers.Items.Refresh();

            });
        }

        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text != string.Empty)
            {
                // Users
                textMessage = txtInput.Text;
                txtConsole.AppendText("You: " + textMessage + "\n");
                txtInput.Text = string.Empty;
                txtConsole.ScrollToEnd();

                // Send to server
                await server.SendMessage(client, textMessage);
            }
        }

        public async Task GetUsersList()
        {
            usersOnline = await server.GetUsers();
            dgUsers.ItemsSource = usersOnline;
        }

        private async void Window_Closed(object sender, EventArgs e)
        {
            await server.UserDisconnected(client.Username);
        }
    }
}