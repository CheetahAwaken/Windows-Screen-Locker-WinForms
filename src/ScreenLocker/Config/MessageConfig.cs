namespace ScreenLocker.Config;

using System.Text.Json;

public sealed class MessageConfig
{
    public string Title { get; init; } = "YOUR COMPUTER IS LOCKED";
    public string Message { get; init; } = """
        Your computer has been locked. All files are safe but inaccessible.
        To unlock your computer, you must pay 0.1 BTC to the address below.
        After payment, you will receive the unlock code.
        DO NOT restart your computer or attempt to bypass this screen.
        """;
    public string WalletAddress { get; init; } = "bc1qxy2kgdygjrsqtzq2n0yrf2493p83kkfjhx0wlh";
    public string ContactEmail { get; init; } = "unlock@protonmail.ch";
    public string FooterText { get; init; } = "[EDUCATIONAL PROOF OF CONCEPT - NOT REAL MALWARE]";

    public static MessageConfig Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "message_config.json");
        if (!File.Exists(path))
            return new MessageConfig();

        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<MessageConfig>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new MessageConfig();
    }
}
