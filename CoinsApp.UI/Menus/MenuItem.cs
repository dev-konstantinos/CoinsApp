namespace CoinsApp.UI.Menus;

internal sealed class MenuItem
{
    public string Key { get; }

    public string Title { get; }

    public Action? Action { get; }

    public MenuItem(string key, string title, Action? action = null)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Menu key cannot be empty!");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Menu title cannot be empty!");

        Key = key;
        Title = title;
        Action = action;
    }
}