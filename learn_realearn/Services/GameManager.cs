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

        public GameManager()
        {
            _settings = new GameSettings();
            _player = new Player();
            _scoreService = new ScoreService();

            ConsoleHelper.Initialize(_settings);
        }

        public void Run()
        {
            ConsoleHelper.ShowHeader(_settings.AppName, _settings.Version);

            Console.Write("Masukkan nama Anda: ");
            _player.Name = Console.ReadLine() ?? "Guest";

            ConsoleHelper.PrintSuccess($"\nSelamat datang, {_player.Name}!", _settings);

            bool continueRunning = true;

            while (continueRunning)
            {
                ConsoleHelper.ShowMainMenu();

                string choice = Console.ReadLine() ?? "";

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

                    case "4": // MENU BARU: Fungsi Memori
                        ShowMemoryMenu();
                        break;

                    case "5":
                        ShowHelp();
                        break;

                    case "6":
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
                // FITUR BARU: Opsi gunakan memori
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

                double num2 = InputHandler.GetNumber("Masukkan angka kedua: ");

                ConsoleHelper.ShowOperationMenu();
                string opChoice = Console.ReadLine() ?? "";

                var operation = InputHandler.ParseOperationChoice(opChoice);
                var symbol = CalculatorService.GetOperationSymbol(operation);

                double result = CalculatorService.Calculate(num1, num2, operation);

                string expression = $"{num1} {symbol} {num2}";
                _player.AddToHistory(expression, result);

                ConsoleHelper.ShowResult(expression, result, _settings);
                _scoreService.RecordCalculation(operation);

                // FITUR BARU: Simpan hasil ke memori
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

        // FITUR BARU: Menu memori
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
                    // Kembali
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
            Console.WriteLine("1. Kalkulator mendukung 6 operasi: +, -, ×, ÷, ^ (pangkat), % (sisa bagi)");
            Console.WriteLine("2. Anda bisa melihat riwayat perhitungan di menu 2");
            Console.WriteLine("3. Statistik penggunaan dapat dilihat di menu 3");
            Console.WriteLine("4. Fungsi memori tersedia di menu 4");
            Console.WriteLine("5. Input harus berupa angka (bisa desimal dengan titik)");
            Console.WriteLine("6. Quick commands: [angka] [command] [angka]");
            Console.WriteLine("   Contoh: 10 AVG 20, 5 PERCENT 20");
            Console.WriteLine("7. Tekan Enter setelah setiap input");
        }
    }
}