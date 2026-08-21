namespace ScreenLocker.Utils;

using System.Drawing;
using System.Drawing.Imaging;

public static class CryptoWalletDisplay
{
    public static Bitmap GenerateQrPlaceholder(string walletAddress, int size = 200)
    {
        var bitmap = new Bitmap(size, size, PixelFormat.Format32bppArgb);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.Clear(Color.White);

        using var borderPen = new Pen(Color.Black, 2);
        graphics.DrawRectangle(borderPen, 1, 1, size - 3, size - 3);

        int moduleSize = size / 25;
        var random = new Random(walletAddress.GetHashCode());

        for (int y = 2; y < 23; y++)
        {
            for (int x = 2; x < 23; x++)
            {
                if (random.Next(2) == 1)
                {
                    graphics.FillRectangle(Brushes.Black,
                        x * moduleSize, y * moduleSize, moduleSize, moduleSize);
                }
            }
        }

        DrawFinderPattern(graphics, 0, 0, moduleSize);
        DrawFinderPattern(graphics, 18 * moduleSize, 0, moduleSize);
        DrawFinderPattern(graphics, 0, 18 * moduleSize, moduleSize);

        return bitmap;
    }

    public static string FormatWalletAddress(string address)
    {
        if (address.Length <= 12)
            return address;

        return $"{address[..6]}...{address[^6..]}";
    }

    public static string GetPaymentUrl(string address, decimal amount, string currency)
    {
        return currency.ToLowerInvariant() switch
        {
            "btc" or "bitcoin" => $"bitcoin:{address}?amount={amount}",
            "eth" or "ethereum" => $"ethereum:{address}?value={amount}",
            "xmr" or "monero" => $"monero:{address}?tx_amount={amount}",
            _ => address
        };
    }

    private static void DrawFinderPattern(Graphics g, int x, int y, int moduleSize)
    {
        int patternSize = 7 * moduleSize;
        g.FillRectangle(Brushes.Black, x, y, patternSize, patternSize);
        g.FillRectangle(Brushes.White, x + moduleSize, y + moduleSize,
            patternSize - 2 * moduleSize, patternSize - 2 * moduleSize);
        g.FillRectangle(Brushes.Black, x + 2 * moduleSize, y + 2 * moduleSize,
            3 * moduleSize, 3 * moduleSize);
    }
}
