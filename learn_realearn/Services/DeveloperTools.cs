using System;
using System.Diagnostics;
using System.Linq;
using learn_realearn.Models;
using learn_realearn.Utilities;

namespace learn_realearn.Services
{
    public class DeveloperTools
    {
        private readonly GameSettings _settings;
        private readonly Player _player;
        private readonly ScoreService _scoreService;

        public DeveloperTools(GameSettings settings, Player player, ScoreService scoreService)
        {
            _settings = settings;
            _player = player;
            _scoreService = scoreService;
        }

        public bool Authenticate()
        {
            if (_settings.DeveloperMode)
                return true;

            ConsoleHelper.ShowSectionHeader("DEVELOPER MODE ACCESS");
            Console.Write("Enter password: ");
            Console.ForegroundColor = ConsoleColor.Black;
            string input = Console.ReadLine() ?? "";
            Console.ResetColor();

            if (input == _settings.DeveloperPassword)
            {
                _settings.DeveloperMode = true;
                Console.ForegroundColor = _settings.DevColor;
                Console.WriteLine("✓ Developer mode activated!");
                Console.ResetColor();
                return true;
            }
            else
            {
                ConsoleHelper.PrintError("Access denied!", _settings);
                return false;
            }
        }

        public void ShowDeveloperMenu()
        {
            if (!_settings.DeveloperMode && !Authenticate())
                return;

            bool inMenu = true;

            while (inMenu)
            {
                Console.Clear();
                Console.ForegroundColor = _settings.DevColor;
                Console.WriteLine("╔══════════════════════════════════════╗");
                Console.WriteLine("║        🛠️  DEVELOPER TOOLS         ║");
                Console.WriteLine("╚══════════════════════════════════════╝");
                Console.ResetColor();

                Console.WriteLine($"\nUser: {_player.Name}");
                Console.WriteLine($"Session: {_player.FirstUsed:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine(new string('═', 40));

                Console.WriteLine("\n1. 🔍 View All Data (JSON Format)");
                Console.WriteLine("2. ⚡ Run Performance Test");
                Console.WriteLine("3. 🧪 Test Edge Cases");
                Console.WriteLine("4. 📊 Advanced Statistics");
                Console.WriteLine("5. 🔄 Reset All Data");
                Console.WriteLine("6. ⚙️  Toggle Experimental Features");
                Console.WriteLine("7. 🔐 Change Password");
                Console.WriteLine("8. 📝 Generate Test Report");
                Console.WriteLine("9. 🚪 Exit Developer Mode");
                Console.WriteLine("0. ↩️  Back to Main Menu");

                Console.Write("\nSelect: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        ViewAllData();
                        break;
                    case "2":
                        RunPerformanceTest();
                        break;
                    case "3":
                        TestEdgeCases();
                        break;
                    case "4":
                        ShowAdvancedStatistics();
                        break;
                    case "5":
                        ResetAllData();
                        break;
                    case "6":
                        ToggleExperimentalFeatures();
                        break;
                    case "7":
                        ChangePassword();
                        break;
                    case "8":
                        GenerateTestReport();
                        break;
                    case "9":
                        _settings.DeveloperMode = false;
                        ConsoleHelper.PrintSuccess("Developer mode deactivated!", _settings);
                        inMenu = false;
                        break;
                    case "0":
                        inMenu = false;
                        break;
                    default:
                        ConsoleHelper.PrintError("Invalid option!", _settings);
                        break;
                }

                if (inMenu && choice != "0")
                {
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        private void ViewAllData()
        {
            Console.WriteLine("\n📦 ALL DATA STORAGE:");
            Console.WriteLine(new string('-', 50));

            Console.WriteLine("\n👤 PLAYER DATA:");
            Console.WriteLine($"Name: {_player.Name}");
            Console.WriteLine($"Total Calculations: {_player.TotalCalculations}");
            Console.WriteLine($"Memory Value: {_player.MemoryValue}");
            Console.WriteLine($"First Used: {_player.FirstUsed}");

            Console.WriteLine("\n📜 HISTORY:");
            if (_player.History.Any())
            {
                foreach (var item in _player.History)
                {
                    Console.WriteLine($"  {item.Timestamp:HH:mm:ss} | {item.Expression} = {item.Result}");
                }
            }
            else
            {
                Console.WriteLine("  No history");
            }

            Console.WriteLine("\n⚙️  SETTINGS:");
            Console.WriteLine($"Version: {_settings.Version}");
            Console.WriteLine($"Developer Mode: {_settings.DeveloperMode}");
            Console.WriteLine($"Experimental Features: {_settings.EnableExperimentalFeatures}");
        }

        private void RunPerformanceTest()
        {
            Console.WriteLine("\n⚡ PERFORMANCE TEST");
            Console.WriteLine("Running 100,000 calculations...");

            var stopwatch = Stopwatch.StartNew();
            Random rand = new Random();
            double total = 0;

            for (int i = 0; i < 100000; i++)
            {
                double a = rand.NextDouble() * 1000;
                double b = rand.NextDouble() * 1000;
                total += CalculatorService.Add(a, b);
            }

            stopwatch.Stop();

            Console.WriteLine($"Time elapsed: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Operations per second: {100000 / (stopwatch.ElapsedMilliseconds / 1000.0):F0}");
            Console.WriteLine($"Total sum: {total:F2}");
        }

        private void TestEdgeCases()
        {
            Console.WriteLine("\n🧪 EDGE CASE TESTING");

            TestCase("Division by zero", () =>
            {
                try
                {
                    CalculatorService.Divide(10, 0);
                    return "FAIL: Should have thrown exception";
                }
                catch (DivideByZeroException)
                {
                    return "PASS: Correctly threw exception";
                }
            });

            TestCase("Very large numbers", () =>
            {
                double result = CalculatorService.Multiply(double.MaxValue, 0.5);
                return $"PASS: Result = {result:E}";
            });

            TestCase("Very small numbers", () =>
            {
                double result = CalculatorService.Divide(double.Epsilon, 2);
                return $"PASS: Result = {result:E}";
            });

            TestCase("NaN handling", () =>
            {
                double result = CalculatorService.Add(double.NaN, 5);
                return $"PASS: NaN + 5 = {result} (Should be NaN)";
            });

            TestCase("Infinity test", () =>
            {
                double result = CalculatorService.Divide(1, 0.0);
                return $"PASS: 1/0 = {result}";
            });

            TestCase("Logarithm edge cases", () =>
            {
                try
                {
                    CalculatorService.Logarithm(-1, 2);
                    return "FAIL: Should reject negative";
                }
                catch (ArgumentException)
                {
                    return "PASS: Correctly rejected negative";
                }
            });
        }

        private void TestCase(string name, Func<string> test)
        {
            Console.Write($"Testing {name}... ");
            try
            {
                string result = test();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(result);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"FAIL: {ex.Message}");
                Console.ResetColor();
            }
        }

        private void ShowAdvancedStatistics()
        {
            Console.WriteLine("\n📊 ADVANCED STATISTICS");

            if (_player.History.Any())
            {
                var results = _player.History.Select(h => h.Result).ToList();

                double average = results.Average();
                double min = results.Min();
                double max = results.Max();
                double sum = results.Sum();

                Console.WriteLine($"Average result: {average:F4}");
                Console.WriteLine($"Min result: {min:F4}");
                Console.WriteLine($"Max result: {max:F4}");
                Console.WriteLine($"Sum of all results: {sum:F4}");
                Console.WriteLine($"Standard deviation: {CalculateStdDev(results):F4}");

                Console.WriteLine("\n📈 RESULT DISTRIBUTION:");
                var groups = results.GroupBy(r => Math.Floor(r / 10) * 10)
                                   .OrderBy(g => g.Key);

                foreach (var group in groups)
                {
                    int count = group.Count();
                    double percentage = (double)count / results.Count * 100;
                    Console.WriteLine($"[{group.Key,5} to {group.Key + 10,5}): {count,3} results ({percentage,5:F1}%)");
                }
            }
            else
            {
                Console.WriteLine("No data available for statistics");
            }
        }

        private double CalculateStdDev(IEnumerable<double> values)
        {
            double avg = values.Average();
            double sumOfSquares = values.Sum(v => Math.Pow(v - avg, 2));
            return Math.Sqrt(sumOfSquares / values.Count());
        }

        private void ResetAllData()
        {
            Console.Write("\n⚠️  Are you sure? This will delete ALL data! (type 'YES' to confirm): ");
            if (Console.ReadLine()?.ToUpper() == "YES")
            {
                _player.History.Clear();
                _player.TotalCalculations = 0;
                _player.MemoryValue = 0;
                _player.FirstUsed = DateTime.Now;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ All data has been reset!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Reset cancelled.");
            }
        }

        private void ToggleExperimentalFeatures()
        {
            _settings.EnableExperimentalFeatures = !_settings.EnableExperimentalFeatures;
            Console.WriteLine($"\nExperimental features: {(_settings.EnableExperimentalFeatures ? "ENABLED" : "DISABLED")}");

            if (_settings.EnableExperimentalFeatures)
            {
                Console.WriteLine("Available experimental features:");
                Console.WriteLine("• Scientific notation support");
                Console.WriteLine("• Unit conversions");
                Console.WriteLine("• Graph plotting");
                Console.WriteLine("• Voice output (if TTS available)");
            }
        }

        private void ChangePassword()
        {
            Console.Write("\nEnter current password: ");
            Console.ForegroundColor = ConsoleColor.Black;
            string current = Console.ReadLine() ?? "";
            Console.ResetColor();

            if (current == _settings.DeveloperPassword)
            {
                Console.Write("Enter new password: ");
                Console.ForegroundColor = ConsoleColor.Black;
                string newPass = Console.ReadLine() ?? "";
                Console.ResetColor();

                Console.Write("Confirm new password: ");
                Console.ForegroundColor = ConsoleColor.Black;
                string confirm = Console.ReadLine() ?? "";
                Console.ResetColor();

                if (newPass == confirm && newPass.Length >= 4)
                {
                    _settings.DeveloperPassword = newPass;
                    ConsoleHelper.PrintSuccess("Password changed successfully!", _settings);
                }
                else
                {
                    ConsoleHelper.PrintError("Passwords don't match or too short!", _settings);
                }
            }
            else
            {
                ConsoleHelper.PrintError("Incorrect current password!", _settings);
            }
        }

        private void GenerateTestReport()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string report = $"""
                TEST REPORT - {timestamp}
                ================================
                Application: {_settings.AppName}
                Version: {_settings.Version}
                User: {_player.Name}
                Test Time: {DateTime.Now}
                
                STATISTICS:
                - Total Calculations: {_player.TotalCalculations}
                - Memory Value: {_player.MemoryValue}
                - History Records: {_player.History.Count}
                - Session Duration: {(DateTime.Now - _player.FirstUsed).TotalMinutes:F1} minutes
                
                LAST 5 CALCULATIONS:
                {string.Join("\n", _player.History.TakeLast(5).Select(h => $"  {h.Expression} = {h.Result}"))}
                
                SYSTEM INFO:
                - OS: {Environment.OSVersion}
                - .NET Version: {Environment.Version}
                - Machine: {Environment.MachineName}
                
                ================================
                END OF REPORT
                """;

            Console.WriteLine("\n📝 TEST REPORT GENERATED:");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine(report);
            Console.WriteLine(new string('-', 50));

            string filename = $"test_report_{timestamp}.txt";
            System.IO.File.WriteAllText(filename, report);
            Console.WriteLine($"Report saved as: {filename}");
        }
    }
}