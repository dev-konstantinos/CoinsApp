using CoinsApp.BLL.Users.ViewModels;
using CoinsApp.DAL.Users;
using CoinsApp.DAL.Users.Models;

namespace CoinsApp.BLL.Users;

public sealed class UserService
{
    private const int MinimumPasswordLength = 8;

    private readonly UserRepository _repository;
    private readonly PasswordHasher _passwordHasher;

    public UserService(
        UserRepository repository,
        PasswordHasher passwordHasher)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));

        _passwordHasher =
            passwordHasher
            ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public async Task<IReadOnlyList<UserListItemViewModel>> GetAllAsync()
    {
        var users =
            await _repository.GetAllAsync();

        return users
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<UserDetailsViewModel?> GetByIdAsync(
        int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(userId));
        }

        var user =
            await _repository.GetByIdAsync(userId);

        return user is null
            ? null
            : MapToDetails(user);
    }

    public async Task<int> CreateAsync(
        CreateUserViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var passwordHash =
            _passwordHasher.Hash(model.Password);

        var data = new UserCreateData
        {
            Username = model.Username.Trim(),
            PasswordHash = passwordHash,
            Email = NormalizeEmail(model.Email),
            IsActive = model.IsActive
        };

        return await _repository.CreateAsync(data);
    }

    public async Task<UserDetailsViewModel?> UpdateAsync(
        UpdateUserViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateUpdate(model);

        var data = new UserUpdateData
        {
            UserId = model.UserId,
            Username = model.Username.Trim(),
            Email = NormalizeEmail(model.Email),
            IsActive = model.IsActive
        };

        var user =
            await _repository.UpdateAsync(data);

        return user is null
            ? null
            : MapToDetails(user);
    }

    public async Task<int> SetPasswordAsync(
        SetUserPasswordViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateSetPassword(model);

        var passwordHash =
            _passwordHasher.Hash(model.NewPassword);

        var data = new UserSetPasswordData
        {
            UserId = model.UserId,
            PasswordHash = passwordHash
        };

        return await _repository.SetPasswordAsync(data);
    }

    public async Task<int> DeleteAsync(
        DeleteUserViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.UserId <= 0)
        {
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(model.UserId));
        }

        var data = new UserDeleteData
        {
            UserId = model.UserId
        };

        return await _repository.DeleteAsync(data);
    }

    private static void ValidateCreate(
        CreateUserViewModel model)
    {
        ValidateUsername(model.Username);
        ValidateEmail(model.Email);
        ValidatePassword(
            model.Password,
            model.ConfirmPassword);
    }

    private static void ValidateUpdate(
        UpdateUserViewModel model)
    {
        if (model.UserId <= 0)
        {
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(model.UserId));
        }

        ValidateUsername(model.Username);
        ValidateEmail(model.Email);
    }

    private static void ValidateSetPassword(
        SetUserPasswordViewModel model)
    {
        if (model.UserId <= 0)
        {
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(model.UserId));
        }

        ValidatePassword(
            model.NewPassword,
            model.ConfirmPassword);
    }

    private static void ValidateUsername(
        string? username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException(
                "Username is required.",
                nameof(username));
        }

        if (username.Trim().Length > 50)
        {
            throw new ArgumentException(
                "Username cannot exceed 50 characters.",
                nameof(username));
        }
    }

    private static void ValidateEmail(
        string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        if (email.Trim().Length > 255)
        {
            throw new ArgumentException(
                "Email cannot exceed 255 characters.",
                nameof(email));
        }
    }

    private static void ValidatePassword(
        string? password,
        string? confirmPassword)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException(
                "Password is required.",
                nameof(password));
        }

        if (password.Length < MinimumPasswordLength)
        {
            throw new ArgumentException(
                $"Password must contain at least {MinimumPasswordLength} characters.",
                nameof(password));
        }

        if (!string.Equals(
                password,
                confirmPassword,
                StringComparison.Ordinal))
        {
            throw new ArgumentException(
                "Password and confirmation password do not match.",
                nameof(confirmPassword));
        }
    }

    private static string? NormalizeEmail(
        string? email)
    {
        return string.IsNullOrWhiteSpace(email)
            ? null
            : email.Trim();
    }

    private static UserListItemViewModel MapToListItem(
        UserData user)
    {
        return new UserListItemViewModel
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            CollectionCount = user.CollectionCount
        };
    }

    private static UserDetailsViewModel MapToDetails(
        UserData user)
    {
        return new UserDetailsViewModel
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            CollectionCount = user.CollectionCount
        };
    }
}