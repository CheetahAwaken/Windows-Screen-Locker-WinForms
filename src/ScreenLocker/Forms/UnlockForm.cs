namespace ScreenLocker.Forms;

using System.Drawing;
using System.Windows.Forms;

public sealed class UnlockForm : Form
{
    private readonly string _correctCode;
    private TextBox _codeTextBox = null!;
    private Label _statusLabel = null!;
    private int _attempts;

    public bool IsUnlocked { get; private set; }

    public UnlockForm(string correctCode)
    {
        _correctCode = correctCode;
        InitializeForm();
        InitializeControls();
    }

    private void InitializeForm()
    {
        Text = "Enter Unlock Code";
        Size = new Size(400, 220);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        TopMost = true;
        BackColor = Color.FromArgb(30, 30, 40);
    }

    private void InitializeControls()
    {
        var promptLabel = new Label
        {
            Text = "Enter the unlock code to release this machine:",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.White,
            Location = new Point(20, 20),
            AutoSize = true
        };

        _codeTextBox = new TextBox
        {
            Font = new Font("Consolas", 14),
            Location = new Point(20, 55),
            Size = new Size(340, 30),
            UseSystemPasswordChar = true
        };
        _codeTextBox.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
                TryUnlock();
        };

        var unlockButton = new Button
        {
            Text = "Unlock",
            Font = new Font("Segoe UI", 11),
            Location = new Point(20, 100),
            Size = new Size(160, 40),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(0, 120, 60)
        };
        unlockButton.Click += (_, _) => TryUnlock();

        var cancelButton = new Button
        {
            Text = "Cancel",
            Font = new Font("Segoe UI", 11),
            Location = new Point(200, 100),
            Size = new Size(160, 40),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(120, 30, 30)
        };
        cancelButton.Click += (_, _) => Close();

        _statusLabel = new Label
        {
            Text = string.Empty,
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.Red,
            Location = new Point(20, 150),
            AutoSize = true
        };

        Controls.AddRange([promptLabel, _codeTextBox, unlockButton, cancelButton, _statusLabel]);
    }

    private void TryUnlock()
    {
        _attempts++;
        string entered = _codeTextBox.Text.Trim();

        if (string.Equals(entered, _correctCode, StringComparison.Ordinal))
        {
            IsUnlocked = true;
            Close();
        }
        else
        {
            _statusLabel.Text = $"Invalid code. Attempts: {_attempts}";
            _codeTextBox.Clear();
            _codeTextBox.Focus();

            if (_attempts >= 5)
            {
                _codeTextBox.Enabled = false;
                _statusLabel.Text = "Too many attempts. Wait 60 seconds.";
                var timer = new System.Windows.Forms.Timer { Interval = 60000 };
                timer.Tick += (_, _) =>
                {
                    _codeTextBox.Enabled = true;
                    _attempts = 0;
                    _statusLabel.Text = string.Empty;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
        }
    }
}
