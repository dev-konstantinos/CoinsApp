namespace CoinsApp.UI.Menus;

internal sealed class MenuItem
{
    private readonly Func<Task>? _action;

    public string Key { get; }
    public string Title { get; }

    public MenuItem(
        string key,
        string title,
        Func<Task>? action = null)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Title = title ?? throw new ArgumentNullException(nameof(title));

        _action = action;
    }

    public Task ExecuteAsync()
    {
        return _action is null
            ? Task.CompletedTask
            : _action();
    }
}