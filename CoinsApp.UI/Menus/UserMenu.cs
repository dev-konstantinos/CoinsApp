using CoinsApp.BLL.Users;
using CoinsApp.BLL.Users.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus;

internal sealed class UserMenu
{
    private readonly UserService _userService;

    public UserMenu(UserService userService)
    {
        _userService =
            userService
            ?? throw new ArgumentNullException(nameof(userService));
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Users ===");
            Console.WriteLine();
            Console.WriteLine("1. List Users");
            Console.WriteLine("2. User Details");
            Console.WriteLine("3. Create User");
            Console.WriteLine("4. Update User");
            Console.WriteLine("5. Set Password");
            Console.WriteLine("6. Delete User");
            Console.WriteLine("0. Back");
            Console.WriteLine();

            Console.Write("Select: ");

            var input =
                Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out var choice))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection.");
                Console.ReadLine();
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        await ListAsync();
                        break;

                    case 2:
                        await DetailsAsync();
                        break;

                    case 3:
                        await CreateAsync();
                        break;

                    case 4:
                        await UpdateAsync();
                        break;

                    case 5:
                        await SetPasswordAsync();
                        break;

                    case 6:
                        await DeleteAsync();
                        break;

                    case 0:
                        return;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Invalid selection.");
                        Console.ReadLine();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadLine();
            }
        }
    }

    private async Task ListAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Users ===");
        Console.WriteLine();

        var users =
            await _userService.GetAllAsync();

        if (users.Count == 0)
        {
            Console.WriteLine("No users found.");
            Console.ReadLine();
            return;
        }

        foreach (var user in users)
        {
            Console.WriteLine(
                $"{user.UserId}: {user.Username}");

            Console.WriteLine(
                $"   Email: {user.Email ?? "-"}");

            Console.WriteLine(
                $"   Status: {(user.IsActive ? "Active" : "Inactive")}");

            Console.WriteLine(
                $"   Collections: {user.CollectionCount}");

            Console.WriteLine(
                $"   Created: {user.CreatedAt:yyyy-MM-dd HH:mm:ss}");

            Console.WriteLine();
        }

        Console.ReadLine();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        Console.WriteLine("=== User Details ===");
        Console.WriteLine();

        var userId =
            MenuInput.ReadRequiredId("User ID");

        var user =
            await _userService.GetByIdAsync(userId);

        if (user is null)
        {
            Console.WriteLine();
            Console.WriteLine(
                $"User with ID {userId} was not found.");

            Console.ReadLine();
            return;
        }

        Console.WriteLine(
            $"ID:           {user.UserId}");

        Console.WriteLine(
            $"Username:     {user.Username}");

        Console.WriteLine(
            $"Email:        {user.Email ?? "-"}");

        Console.WriteLine(
            $"Status:       {(user.IsActive ? "Active" : "Inactive")}");

        Console.WriteLine(
            $"Collections:  {user.CollectionCount}");

        Console.WriteLine(
            $"Created:      {user.CreatedAt:yyyy-MM-dd HH:mm:ss}");

        Console.ReadLine();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Create User ===");
        Console.WriteLine();

        var password =
            MenuInput.ReadRequiredPassword("Password");

        var confirmPassword =
            MenuInput.ReadRequiredPassword(
                "Confirm Password");

        var model = new CreateUserViewModel
        {
            Username =
                MenuInput.ReadRequiredString("Username"),

            Password = password,

            ConfirmPassword = confirmPassword,

            Email =
                MenuInput.ReadNullableString("Email"),

            IsActive =
                MenuInput.ReadRequiredBoolean("Active")
        };

        var userId =
            await _userService.CreateAsync(model);

        Console.WriteLine();
        Console.WriteLine(
            $"User created successfully. ID: {userId}");

        Console.ReadLine();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Update User ===");
        Console.WriteLine();

        var userId =
            MenuInput.ReadRequiredId("User ID");

        var current =
            await _userService.GetByIdAsync(userId);

        if (current is null)
        {
            Console.WriteLine();
            Console.WriteLine(
                $"User with ID {userId} was not found.");

            Console.ReadLine();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Enter the new values.");
        Console.WriteLine(
            "Press Enter to keep the current value.");
        Console.WriteLine();

        var username =
            MenuInput.ReadKeepCurrentRequiredString(
                "Username",
                current.Username);

        var model = new UpdateUserViewModel
        {
            UserId = userId,

            Username = username,

            Email =
                MenuInput.ReadKeepCurrentString(
                    "Email",
                    current.Email),

            IsActive =
                MenuInput.ReadKeepCurrentBoolean(
                    "Active",
                    current.IsActive)
        };

        var updated =
            await _userService.UpdateAsync(model);

        Console.WriteLine();

        if (updated is null)
        {
            Console.WriteLine(
                $"User with ID {userId} was not found.");

            Console.ReadLine();
            return;
        }

        Console.WriteLine(
            $"User {updated.UserId} updated successfully.");

        Console.ReadLine();
    }

    private async Task SetPasswordAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Set User Password ===");
        Console.WriteLine();

        var userId =
            MenuInput.ReadRequiredId("User ID");

        var current =
            await _userService.GetByIdAsync(userId);

        if (current is null)
        {
            Console.WriteLine();
            Console.WriteLine(
                $"User with ID {userId} was not found.");

            Console.ReadLine();
            return;
        }

        Console.WriteLine(
            $"User: {current.Username}");

        Console.WriteLine();

        var newPassword =
            MenuInput.ReadRequiredPassword(
                "New Password");

        var confirmPassword =
            MenuInput.ReadRequiredPassword(
                "Confirm Password");

        await _userService.SetPasswordAsync(
            new SetUserPasswordViewModel
            {
                UserId = userId,
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            });

        Console.WriteLine();
        Console.WriteLine(
            $"Password for user {userId} changed successfully.");

        Console.ReadLine();
    }

    private async Task DeleteAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Delete User ===");
        Console.WriteLine();

        var userId =
            MenuInput.ReadRequiredId("User ID");

        var current =
            await _userService.GetByIdAsync(userId);

        if (current is null)
        {
            Console.WriteLine();
            Console.WriteLine(
                $"User with ID {userId} was not found.");

            Console.ReadLine();
            return;
        }

        Console.WriteLine(
            $"User: {current.Username}");

        Console.WriteLine(
            $"Email: {current.Email ?? "-"}");

        Console.WriteLine(
            $"Collections: {current.CollectionCount}");

        Console.WriteLine();

        var confirmed =
            MenuInput.ReadRequiredBoolean(
                "Delete this user");

        if (!confirmed)
        {
            Console.WriteLine();
            Console.WriteLine("Delete cancelled.");
            Console.ReadLine();
            return;
        }

        await _userService.DeleteAsync(
            new DeleteUserViewModel
            {
                UserId = userId
            });

        Console.WriteLine();
        Console.WriteLine(
            $"User {userId} deleted successfully.");

        Console.ReadLine();
    }
}