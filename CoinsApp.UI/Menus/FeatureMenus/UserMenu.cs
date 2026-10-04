using CoinsApp.BLL.Features.Users;
using CoinsApp.BLL.Features.Users.ViewModels;
using CoinsApp.UI.Helpers;

namespace CoinsApp.UI.Menus.FeatureMenus;

internal sealed class UserMenu
{
    private readonly IUserService _userService;

    public UserMenu(IUserService userService)
    {
        _userService =
            userService
            ?? throw new ArgumentNullException(nameof(userService));
    }

    // ============================================================
    // Navigation
    // ============================================================

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            PrintHeader();

            Console.WriteLine("1. List users");
            Console.WriteLine("2. User details");
            Console.WriteLine("3. Create user");
            Console.WriteLine("4. Update user");
            Console.WriteLine("5. Set password");
            Console.WriteLine("6. Delete user");
            Console.WriteLine("0. Back");
            Console.WriteLine();

            Console.Write("Select: ");

            var input = Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out var choice))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection.");
                Pause();
                continue;
            }

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
                    Pause();
                    break;
            }
        }
    }

    // ============================================================
    // CRUD
    // ============================================================

    private async Task ListAsync()
    {
        Console.Clear();

        PrintHeader();

        try
        {
            var users = await _userService.GetAllAsync();

            PrintUsers(users);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading users.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DetailsAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("User Details");
        Console.WriteLine();

        var userId = MenuInput.ReadIdOrExit("User ID");

        if (userId is null)
        {
            return;
        }

        try
        {
            var user =
                await _userService.GetByIdAsync(userId.Value);

            if (user is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"User with ID {userId.Value} was not found.");
            }
            else
            {
                PrintDetails(user);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error loading user.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task CreateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Create User");
        Console.WriteLine();

        try
        {
            var password =
                MenuInput.ReadRequiredPassword("Password");

            var confirmPassword =
                MenuInput.ReadRequiredPassword("Confirm Password");

            var model = new CreateUserViewModel
            {
                Username = MenuInput.ReadRequiredString("Username"),
                Password = password,
                ConfirmPassword = confirmPassword,
                Email = MenuInput.ReadNullableString("Email"),
                IsActive = MenuInput.ReadRequiredBoolean("Active")
            };

            Console.WriteLine();
            Console.WriteLine("Creating user...");

            var userId =
                await _userService.CreateAsync(model);

            Console.WriteLine();
            Console.WriteLine("User created successfully.");
            Console.WriteLine($"User ID: {userId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error creating user.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Update User");
        Console.WriteLine();

        var userId = MenuInput.ReadIdOrExit("User ID");

        if (userId is null)
        {
            return;
        }

        try
        {
            var current =
                await _userService.GetByIdAsync(userId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"User with ID {userId.Value} was not found.");

                Pause();
                return;
            }

            Console.Clear();

            PrintHeader();
            Console.WriteLine("Update User");
            Console.WriteLine();

            Console.WriteLine("--- Current ---");
            Console.WriteLine();
            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("Press Enter to keep the current value.");
            Console.WriteLine();

            var model = new UpdateUserViewModel
            {
                UserId = current.UserId,
                Username = MenuInput.ReadKeepCurrentRequiredString(
                    "Username",
                    current.Username),
                Email = MenuInput.ReadKeepCurrentString(
                    "Email",
                    current.Email),
                IsActive = MenuInput.ReadKeepCurrentBoolean(
                    "Active",
                    current.IsActive)
            };

            Console.WriteLine();
            PrintUpdateSummary(model);

            Console.WriteLine();
            Console.WriteLine("Updating user...");

            var updated =
                await _userService.UpdateAsync(model);

            if (updated is null)
            {
                Console.WriteLine();
                Console.WriteLine("User could not be updated.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("User updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error updating user.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    // ============================================================
    // Domain actions
    // ============================================================

    private async Task SetPasswordAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Set User Password");
        Console.WriteLine();

        var userId = MenuInput.ReadIdOrExit("User ID");

        if (userId is null)
        {
            return;
        }

        try
        {
            var current =
                await _userService.GetByIdAsync(userId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"User with ID {userId.Value} was not found.");

                Pause();
                return;
            }

            Console.WriteLine($"User: {current.Username}");
            Console.WriteLine();

            var newPassword =
                MenuInput.ReadRequiredPassword("New Password");

            var confirmPassword =
                MenuInput.ReadRequiredPassword("Confirm Password");

            var model = new SetUserPasswordViewModel
            {
                UserId = current.UserId,
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            };

            Console.WriteLine();
            Console.WriteLine("Changing password...");

            await _userService.SetPasswordAsync(model);

            Console.WriteLine();
            Console.WriteLine(
                $"Password for user {current.UserId} changed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error changing user password.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    private async Task DeleteAsync()
    {
        Console.Clear();

        PrintHeader();
        Console.WriteLine("Delete User");
        Console.WriteLine();

        var userId = MenuInput.ReadIdOrExit("User ID");

        if (userId is null)
        {
            return;
        }

        try
        {
            var current =
                await _userService.GetByIdAsync(userId.Value);

            if (current is null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"User with ID {userId.Value} was not found.");

                Pause();
                return;
            }

            PrintDetails(current);

            Console.WriteLine();
            Console.WriteLine("WARNING: This user will be deleted.");
            Console.WriteLine("Type DELETE to confirm.");
            Console.Write("Confirm: ");

            var confirmation = Console.ReadLine()?.Trim();

            if (!string.Equals(
                    confirmation,
                    "DELETE",
                    StringComparison.Ordinal))
            {
                Console.WriteLine();
                Console.WriteLine("Delete cancelled.");
                Pause();
                return;
            }

            var model = new DeleteUserViewModel
            {
                UserId = current.UserId
            };

            Console.WriteLine();
            Console.WriteLine("Deleting user...");

            await _userService.DeleteAsync(model);

            Console.WriteLine();
            Console.WriteLine("User deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error deleting user.");
            Console.WriteLine();
            Console.WriteLine(ex.Message);
        }

        Pause();
    }

    // ============================================================
    // Display helpers
    // ============================================================

    private static void PrintHeader()
    {
        Console.WriteLine("=== Users ===");
        Console.WriteLine();
    }

    private static void PrintUsers(
        IReadOnlyList<UserListItemViewModel> users)
    {
        if (users.Count == 0)
        {
            Console.WriteLine("No users found.");
            return;
        }

        Console.WriteLine(
            $"{"ID",4}  " +
            $"{"Username",-20} " +
            $"{"Email",-30} " +
            $"{"Active",-8} " +
            $"{"Collections",12} " +
            $"{"Created",-20}");

        Console.WriteLine(new string('-', 101));

        foreach (var user in users)
        {
            Console.WriteLine(
                $"{user.UserId,4}  " +
                $"{user.Username,-20} " +
                $"{user.Email ?? "-",-30} " +
                $"{(user.IsActive ? "Yes" : "No"),-8} " +
                $"{user.CollectionCount,12} " +
                $"{user.CreatedAt,-20:yyyy-MM-dd HH:mm:ss}");
        }
    }

    private static void PrintDetails(
        UserDetailsViewModel user)
    {
        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"User ID:    {user.UserId}");
        Console.WriteLine($"Username:   {user.Username}");
        Console.WriteLine($"Email:      {user.Email ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine(
            $"Active:     {(user.IsActive ? "Yes" : "No")}");

        Console.WriteLine();

        Console.WriteLine("--- Statistics ---");
        Console.WriteLine($"Collections: {user.CollectionCount}");

        Console.WriteLine();

        Console.WriteLine("--- Metadata ---");
        Console.WriteLine(
            $"Created:    {user.CreatedAt:yyyy-MM-dd HH:mm:ss}");
    }

    private static void PrintUpdateSummary(
        UpdateUserViewModel user)
    {
        Console.WriteLine("--- Update Preview ---");
        Console.WriteLine();

        Console.WriteLine("--- Identity ---");
        Console.WriteLine($"User ID:    {user.UserId}");
        Console.WriteLine($"Username:   {user.Username}");
        Console.WriteLine($"Email:      {user.Email ?? "-"}");

        Console.WriteLine();

        Console.WriteLine("--- Status ---");
        Console.WriteLine(
            $"Active:     {(user.IsActive ? "Yes" : "No")}");
    }

    // ============================================================
    // General helpers
    // ============================================================

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}