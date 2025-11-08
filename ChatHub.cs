using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity;

public class ChatHub : Hub
{
    private readonly UserManager<Customer> _userManager;

    public ChatHub(UserManager<Customer> userManager)
    {
        _userManager = userManager;
    }

    public async Task SendMessage(string user, string message)
    {
        var identityUser = await _userManager.GetUserAsync(Context.User);
        string displayName;

        if (identityUser == null)
        {
            displayName = "Anonymous";
        }
        else
        {
            // customer Name > Email
            displayName = !string.IsNullOrEmpty(identityUser.Name)
                ? identityUser.Name
                : identityUser.Email ?? "User";
        }

        await Clients.All.SendAsync("ReceiveMessage", displayName, message);
    }
}