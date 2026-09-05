namespace CoinsApp.UI.Helpers;

internal static class MenuInput
{
    public static int ReadRequiredId(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");

            var input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid positive ID.");
        }
    }

    public static int? ReadNullableId(string label)
    {
        while (true)
        {
            Console.Write($"{label} (empty = null): ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (int.TryParse(input, out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid positive ID " +
                "or leave empty for null.");
        }
    }

    public static int ReadKeepCurrentId(
        string label,
        int current)
    {
        while (true)
        {
            Console.Write(
                $"{label} [{current}] " +
                "(Enter = keep current): ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            if (int.TryParse(input, out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid positive ID " +
                "or press Enter to keep the current value.");
        }
    }

    public static int? ReadKeepCurrentNullableId(
        string label,
        int? current)
    {
        while (true)
        {
            var currentText =
                current?.ToString() ?? "null";

            Console.Write(
                $"{label} [{currentText}] " +
                "(Enter = keep, null = clear): ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            if (input.Equals(
                    "null",
                    StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (int.TryParse(input, out var value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid positive ID, " +
                "or 'null' to clear.");
        }
    }

    public static T? ReadNullable<T>(string label)
        where T : struct, IParsable<T>
    {
        while (true)
        {
            Console.Write($"{label} (empty = null): ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (T.TryParse(input, null, out var value))
            {
                return value;
            }

            Console.WriteLine(
                $"Please enter a valid {typeof(T).Name}.");
        }
    }

    public static T? ReadKeepCurrentNullable<T>(
        string label,
        T? current)
        where T : struct, IParsable<T>
    {
        while (true)
        {
            var currentText =
                current?.ToString() ?? "null";

            Console.Write(
                $"{label} [{currentText}] " +
                "(Enter = keep, null = clear): ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            if (input.Equals(
                    "null",
                    StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (T.TryParse(input, null, out var value))
            {
                return value;
            }

            Console.WriteLine(
                $"Please enter a valid {typeof(T).Name}, " +
                "or use 'null' to clear the value.");
        }
    }

    public static decimal ReadRequiredPrice(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");

            var input = Console.ReadLine()?.Trim();

            if (decimal.TryParse(input, out var value) &&
                value >= 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid price " +
                "greater than or equal to zero.");
        }
    }

    public static decimal ReadKeepCurrentPrice(
        string label,
        decimal current)
    {
        while (true)
        {
            Console.Write(
                $"{label} [{current:0.####}] " +
                "(Enter = keep current): ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            if (decimal.TryParse(input, out var value) &&
                value >= 0)
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid price " +
                "greater than or equal to zero.");
        }
    }

    public static DateTime ReadRequiredDateTime(
        string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");

            var input = Console.ReadLine()?.Trim();

            if (DateTime.TryParse(input, out var value))
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid date and time.");
        }
    }

    public static DateTime ReadKeepCurrentDateTime(
        string label,
        DateTime current)
    {
        while (true)
        {
            Console.Write(
                $"{label} " +
                $"[{current:yyyy-MM-dd HH:mm:ss}] " +
                "(Enter = keep current): ");

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return current;
            }

            if (DateTime.TryParse(input, out var value))
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid date and time.");
        }
    }

    public static string? ReadNullableString(
        string label)
    {
        Console.Write($"{label} (empty = null): ");

        var input = Console.ReadLine();

        return string.IsNullOrWhiteSpace(input)
            ? null
            : input.Trim();
    }

    public static string? ReadKeepCurrentString(
        string label,
        string? current)
    {
        var currentText =
            current ?? "null";

        Console.Write(
            $"{label} [{currentText}] " +
            "(Enter = keep, null = clear): ");

        var input = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            return current;
        }

        if (input.Equals(
                "null",
                StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return input;
    }
}