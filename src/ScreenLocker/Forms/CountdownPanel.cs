namespace ScreenLocker.Forms;

using System.Drawing;
using System.Windows.Forms;
using ScreenLocker.Utils;

public sealed class CountdownPanel : Panel
{
    private readonly TimerManager _timerManager;
    private Label _timerLabel = null!;
    private Label _statusLabel = null!;
    private System.Windows.Forms.Timer _uiTimer = null!;

    public CountdownPanel(TimeSpan duration)
    {
        _timerManager = new TimerManager(duration);
        InitializeControls();
    }

    private void InitializeControls()
    {
        BackColor = Color.FromArgb(40, 40, 55);
        BorderStyle = BorderStyle.FixedSingle;

        _timerLabel = new Label
        {
            Font = new Font("Consolas", 28, FontStyle.Bold),
            ForeColor = Color.OrangeRed,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 50,
            Text = "00:00:00:00"
        };

        _statusLabel = new Label
        {
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.Gray,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Bottom,
            Height = 25,
            Text = "Time remaining until price increases"
        };

        Controls.Add(_timerLabel);
        Controls.Add(_statusLabel);

        _uiTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        _uiTimer.Tick += OnTimerTick;
    }

    public void StartCountdown()
    {
        _timerManager.Start();
        _uiTimer.Start();
    }

    public void StopCountdown()
    {
        _timerManager.Stop();
        _uiTimer.Stop();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        var remaining = _timerManager.GetRemainingTime();

        if (remaining <= TimeSpan.Zero)
        {
            _timerLabel.Text = "EXPIRED";
            _timerLabel.ForeColor = Color.Red;
            _statusLabel.Text = "Deadline passed - price doubled";
            _uiTimer.Stop();
            return;
        }

        _timerLabel.Text = $"{remaining.Days:D2}:{remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";

        if (remaining.TotalHours < 12)
        {
            _timerLabel.ForeColor = Color.Red;
        }
        else if (remaining.TotalHours < 24)
        {
            _timerLabel.ForeColor = Color.OrangeRed;
        }
    }
}
