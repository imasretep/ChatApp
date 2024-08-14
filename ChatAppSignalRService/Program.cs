using ChatAppSignalRService.Hubs;

namespace ChatAppSignalRService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();

            var app = builder.Build();

            app.MapHub<ChatAppHub>("/chatapp");

            app.Run();
        }
    }
}
