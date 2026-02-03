using System;
using learn_realearn.Models;
using learn_realearn.Utilities;

namespace learn_realearn.Services
{
    public class GameManager
    {
        private readonly GameSettings _settings;
        private readonly Player _player;
        private readonly ScoreService _scoreService;
        private readonly DeveloperTools _devTools;

        public GameManager()
        {
            _settings = new GameSettings();
            _player = new Player();
            _scoreService = new ScoreService();
            _devTools = new DeveloperTools(_settings, _player, _scoreService);

            ConsoleHelper.Initialize(_settings);
        }

        public void Run()
        {
            ConsoleHelper.ShowHeader(_settings.AppName, _settings.Version);

            if (_settings.DeveloperMode)
            {
                Console.ForegroundColor = _settings.DevColor;
                Console.WriteLine("🛠️  Developer Mode: ACTIVE");
                Console.ResetColor();
            }

            Console.Write("Masukkan nama Anda: ");
            _player.Name = Console.ReadLine() ?? "Guest";

            ConsoleHelper.PrintSuccess($"\nSelamat datang, {_player.Name}!", _settings);

            if (_player.Name.ToLower() == "developer")
            {
                Console.ForegroundColor = _settings.DevColor;
                Console.WriteLine("Tip: Type 'dev' in main menu for developer tools!");
                Console.ResetColor();
            }

            bool continueRunning = true;

            while (continueRunning)
            {
                ConsoleHelper.ShowMainMenu();

                string choice = Console.ReadLine() ?? "";

                if (choice.ToLower() == "dev")
                {
                    _devTools.ShowDeveloperMenu();
                    continue;
                }

                switch (choice)
                {
                    case "1":
                        PerformCalculation();
                        break;

                    case "2":
                        ShowHistory();
                        break;

                    case "3":
                        ShowStatistics();
                        break;

                    case "4":
                        ShowMemoryMenu();
                        break;

                    case "5":
                        ShowHelp();
                        break;

                    case "6":
                        _devTools.ShowDeveloperMenu();
                        break;

                    case "7":
                        continueRunning = false;
                        ConsoleHelper.PrintSuccess("\nTerima kasih telah menggunakan kalkulator!", _settings);
                        break;

                    default:
                        ConsoleHelper.PrintError("Pilihan tidak valid!", _settings);
                        break;
                }

                if (continueRunning)
                {
                    Console.WriteLine("\nTekan Enter untuk melanjutkan...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }

            Console.WriteLine("\nTekan Enter untuk keluar...");
            Console.ReadLine();
        }

        private void PerformCalculation()
        {
            try
            {
                Console.Write("Gunakan memori? (M untuk gunakan, Enter untuk skip): ");
                bool useMemory = (Console.ReadLine()?.ToUpper() == "M");

                double num1;
                if (useMemory)
                {
                    num1 = _player.MemoryRecall();
                    Console.WriteLine($"Angka pertama (dari memori): {num1}");
                }
                else
                {
                    num1 = InputHandler.GetNumber("Masukkan angka pertama: ");
                }

                double num2 = 0;
                var operation = CalculatorService.Operation.Add;

                ConsoleHelper.ShowOperationMenu(_settings.EnableExperimentalFeatures);
                string opChoice = Console.ReadLine() ?? "";

                operation = InputHandler.ParseOperationChoice(opChoice, _settings.EnableExperimentalFeatures);

                if (operation != CalculatorService.Operation.Sinus)
                {
                    num2 = InputHandler.GetNumber("Masukkan angka kedua: ");
                }

                var symbol = CalculatorService.GetOperationSymbol(operation);

                double result = CalculatorService.Calculate(num1, num2, operation);

                string expression = operation == CalculatorService.Operation.Sinus
                    ? $"{symbol}({num1}°)"
                    : $"{num1} {symbol} {num2}";

                _player.AddToHistory(expression, result);

                ConsoleHelper.ShowResult(expression, result, _settings);
                _scoreService.RecordCalculation(operation);

                Console.Write("\nSimpan hasil ke memori? (Y/N): ");
                if (Console.ReadLine()?.ToUpper() == "Y")
                {
                    _player.MemoryStore(result);
                    ConsoleHelper.PrintSuccess($"Hasil disimpan ke memori: {result}", _settings);
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Error: {ex.Message}", _settings);
            }
        }

        private void ShowMemoryMenu()
        {
            ConsoleHelper.ShowSectionHeader("FUNGSI MEMORI");
            Console.WriteLine($"Nilai memori saat ini: {_player.MemoryValue}");
            Console.WriteLine("\n1. Store (simpan ke memori)");
            Console.WriteLine("2. Recall (ambil dari memori)");
            Console.WriteLine("3. Add (tambahkan ke memori)");
            Console.WriteLine("4. Subtract (kurangkan dari memori)");
            Console.WriteLine("5. Clear (hapus memori)");
            Console.WriteLine("6. Kembali");
            Console.Write("\nPilih: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    double storeValue = InputHandler.GetNumber("Masukkan nilai untuk disimpan: ");
                    _player.MemoryStore(storeValue);
                    ConsoleHelper.PrintSuccess($"Disimpan: {storeValue}", _settings);
                    break;

                case "2":
                    double recalled = _player.MemoryRecall();
                    ConsoleHelper.PrintSuccess($"Nilai memori: {recalled}", _settings);
                    break;

                case "3":
                    double addValue = InputHandler.GetNumber("Masukkan nilai untuk ditambahkan: ");
                    _player.MemoryAdd(addValue);
                    ConsoleHelper.PrintSuccess($"Memori ditambah {addValue}. Total: {_player.MemoryValue}", _settings);
                    break;

                case "4":
                    double subValue = InputHandler.GetNumber("Masukkan nilai untuk dikurangkan: ");
                    _player.MemorySubtract(subValue);
                    ConsoleHelper.PrintSuccess($"Memori dikurangi {subValue}. Total: {_player.MemoryValue}", _settings);
                    break;

                case "5":
                    _player.MemoryClear();
                    ConsoleHelper.PrintSuccess("Memori dihapus!", _settings);
                    break;

                case "6":
                    break;

                default:
                    ConsoleHelper.PrintError("Pilihan tidak valid!", _settings);
                    break;
            }
        }

        private void ShowHistory()
        {
            ConsoleHelper.ShowSectionHeader("RIWAYAT PERHITUNGAN");

            if (_player.History.Count == 0)
            {
                Console.WriteLine("Belum ada riwayat perhitungan.");
                return;
            }

            Console.WriteLine($"Total perhitungan: {_player.TotalCalculations}");
            Console.WriteLine(new string('-', 50));

            foreach (var item in _player.History)
            {
                Console.WriteLine($"[{item.Timestamp:HH:mm:ss}] {item.Expression} = {item.Result}");
            }
        }

        private void ShowStatistics()
        {
            _scoreService.DisplayStatistics(_player);
        }

        private void ShowHelp()
        {
            ConsoleHelper.ShowSectionHeader("BANTUAN");
            Console.WriteLine("1. Kalkulator mendukung operasi: +, -, ×, ÷, ^, %");
            if (_settings.EnableExperimentalFeatures)
            {
                Console.WriteLine("   + Experimental: log, sin");
            }
            Console.WriteLine("2. Menu 2: Lihat riwayat perhitungan");
            Console.WriteLine("3. Menu 3: Lihat statistik penggunaan");
            Console.WriteLine("4. Menu 4: Fungsi memori (simpan/ambil nilai)");
            Console.WriteLine("5. Menu 6: Developer tools (password: dev123)");
            Console.WriteLine("6. Quick commands: [angka] [command] [angka]");
            Console.WriteLine("   Contoh: 10 AVG 20, 5 PERCENT 20");
            Console.WriteLine("7. Tekan 'M' saat input angka untuk gunakan memori");
            Console.WriteLine("8. Tekan 'dev' di main menu untuk akses cepat developer tools");
            Console.WriteLine("\nShortcuts:");
            Console.WriteLine("  • dev → Developer menu");
            Console.WriteLine("  • M → Use memory");
            Console.WriteLine("  • Y/N → Yes/No prompts");
        }
    }
}