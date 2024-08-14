using Microsoft.AspNetCore.SignalR;
namespace ChatAppSignalRService.Hubs
{
    public class ChatAppHub : Hub
    {
        private static List<User> connectedUsers = new List<User>();
        public async Task AddUserToServer(User client)
        {
            Console.WriteLine($"A user {client.Username} connected to server");
            connectedUsers.Add(client);
            await Clients.All.SendAsync("UserConnectedServer", client);

        }

        public async Task<bool> IsUsernameAlreadyInUse(string username)
        {
            foreach (var user in connectedUsers)
            {
                if (user.Username == username)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task SendMessageToUsers(User user, string message)
        {
            await Clients.All.SendAsync("RecieveMessageToChat", user, message);
        }

        public async Task<List<User>> GetConnectedUsers()
        {
            return await Task.FromResult(connectedUsers);
        }

        public async Task UserDisconnected(string username)
        {
            User userToRemove = connectedUsers.FirstOrDefault(user => user.Username == username);
            if (userToRemove != null)
            {
                connectedUsers.Remove(userToRemove);
                await Clients.All.SendAsync("UserDisconnected", userToRemove);
            }

        }
    }
}
