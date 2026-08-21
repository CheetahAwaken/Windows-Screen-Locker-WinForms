namespace ScreenLocker.Utils;

public sealed class TimerManager
{
    private readonly DateTime _deadline;
    private bool _isRunning;

    public TimerManager(TimeSpan duration)
    {
        _deadline = DateTime.UtcNow.Add(duration);
    }

    public void Start()
    {
        _isRunning = true;
    }

    public void Stop()
    {
        _isRunning = false;
    }

    public TimeSpan GetRemainingTime()
    {
        if (!_isRunning)
            return TimeSpan.Zero;

        var remaining = _deadline - DateTime.UtcNow;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }

    public bool IsExpired => DateTime.UtcNow >= _deadline;

    public double GetProgressPercentage(TimeSpan totalDuration)
    {
        var elapsed = totalDuration - GetRemainingTime();
        return Math.Clamp(elapsed / totalDuration * 100, 0, 100);
    }

    public string GetFormattedTime()
    {
        var remaining = GetRemainingTime();
        return $"{remaining.Days:D2}d {remaining.Hours:D2}h {remaining.Minutes:D2}m {remaining.Seconds:D2}s";
    }
}
