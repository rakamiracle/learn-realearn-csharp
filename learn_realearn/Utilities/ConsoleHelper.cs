using System;
using learn_realearn.Models;

namespace learn_realearn.Utilities
{
    public static class ConsoleHelper
    {
        private static GameSettings _settings;

        public static void Initialize(GameSettings settings)
        {
            _settings = settings;
            Console.Title = $"{settings.AppName} v{settings.Version}";
        }

        public static void ShowHeader(string appName, string version)
        {
            Console.ForegroundColor = _settings.HeaderColor;
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"           {appName} v{version}");
            Console.WriteLine(new string('=', 60));
            Console.ResetColor();
            Console.WriteLine();
        }

        // UPDATE: Menu dengan emoji
        public static void ShowMainMenu()
        {
            Console.WriteLine("\n📱 MENU UTAMA:");
            Console.WriteLine("1. 🧮 Lakukan Perhitungan");
            Console.WriteLine("2. 📜 Lihat Riwayat");
            Console.WriteLine("3. 📊 Lihat Statistik");
            Console.WriteLine("4. 💾 Fungsi Memori");
            Console.WriteLine("5. ❓ Bantuan");
            Console.WriteLine("6. 🚪 Keluar");
            Console.Write("\nPilih menu (1-6): ");
        }

        public static void ShowOperationMenu()
        {
            Console.WriteLine("\n🔢 PILIH OPERASI:");
            Console.WriteLine("1. Penjumlahan (+)");
            Console.WriteLine("2. Pengurangan (-)");
            Console.WriteLine("3. Perkalian (×)");
            Console.WriteLine("4. Pembagian (÷)");
            Console.WriteLine("5. Pangkat (^)");
            Console.WriteLine("6. Modulus/Sisa Bagi (%)");
            Console.Write("\nPilih operasi (1-6): ");
        }

        public static void ShowResult(string expression, double result, GameSettings settings)
        {
            Console.ForegroundColor = settings.ResultColor;
            Console.WriteLine($"\n✨ HASIL: {expression} = {result}");
            Console.ResetColor();
        }

        public static void ShowSectionHeader(string title)
        {
            Console.ForegroundColor = _settings.HeaderColor;
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('-', title.Length));
            Console.ResetColor();
        }

        public static void PrintSuccess(string message, GameSettings settings)
        {
            Console.ForegroundColor = settings.ResultColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintError(string message, GameSettings settings)
        {
            Console.ForegroundColor = settings.ErrorColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}