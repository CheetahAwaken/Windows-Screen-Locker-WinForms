namespace ScreenLocker.Config;

using System.Text.Json;

public sealed class LockConfig
{
    public string UnlockCode { get; init; } = "UNLOCK2024";
    public TimeSpan LockDuration { get; init; } = TimeSpan.FromHours(72);
    public bool BlockTaskManager { get; init; } = true;
    public bool BlockAltTab { get; init; } = true;
    public bool KillDangerousProcesses { get; init; } = true;
    public bool PreventSafeBoot { get; init; }
    public bool AutoStartOnBoot { get; init; } = true;

    public static LockConfig Load()
    {
        string configPath = Path.Combine(AppContext.BaseDirectory, "lock_config.json");
        if (!File.Exists(configPath))
            return new LockConfig();

        string json = File.ReadAllText(configPath);
        return JsonSerializer.Deserialize<LockConfig>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new LockConfig();
    }

    public void Save()
    {
        string configPath = Path.Combine(AppContext.BaseDirectory, "lock_config.json");
        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(configPath, json);
    }
}
