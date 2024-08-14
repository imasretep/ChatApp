using Microsoft.AspNetCore.SignalR.Client;


namespace ChatApp.SignalR
{
    public class SignalRService
    {
        private readonly HubConnection _hubConnection;
        public string connectionId => _hubConnection.ConnectionId;

        // Events
        public event Action<User, string> RecieveMessageFromUser;
        public event Action<User> UpdateUserList;
        public event Action<User> UpdateUserDisconnected;

        public SignalRService()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7141/chatapp").WithAutomaticReconnect().Build();
            _hubConnection.On<User, string>("RecieveMessageToChat", (username, message) => RecieveMessageFromUser?.Invoke(username, message));
            _hubConnection.On<User>("UserConnectedServer", (user) => UpdateUserList?.Invoke(user));
            _hubConnection.On<User>("UserDisconnected", (user) => UpdateUserDisconnected?.Invoke(user));


        }

        public async Task ConnectToServer()
        {
            await _hubConnection.StartAsync();
        }

        public async Task AddUserToServer(User client)
        {
            await _hubConnection.InvokeAsync("AddUserToServer", client);
        }

        public async Task<bool> IsUserAvailable(string username)
        {
            return await _hubConnection.InvokeAsync<bool>("IsUsernameAlreadyInUse", username);
        }

        public async Task SendMessage(User username, string message)
        {
            await _hubConnection.InvokeAsync("SendMessageToUsers", username, message);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _hubConnection.InvokeAsync<List<User>>("GetConnectedUsers");
        }

        public async Task UserDisconnected(string username)
        {
            await _hubConnection.InvokeAsync("UserDisconnected", username);
        }

    }
}
