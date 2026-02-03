using System;
using System.Collections.Generic;
using learn_realearn.Models;

namespace learn_realearn.Services
{
    public class ScoreService
    {
        private Dictionary<CalculatorService.Operation, int> _operationCounts;

        public ScoreService()
        {
            _operationCounts = new Dictionary<CalculatorService.Operation, int>
            {
                { CalculatorService.Operation.Add, 0 },
                { CalculatorService.Operation.Subtract, 0 },
                { CalculatorService.Operation.Multiply, 0 },
                { CalculatorService.Operation.Divide, 0 },
                { CalculatorService.Operation.Power, 0 },
                { CalculatorService.Operation.Modulus, 0 },
                { CalculatorService.Operation.Logarithm, 0 },
                { CalculatorService.Operation.Sinus, 0 }
            };
        }

        public void RecordCalculation(CalculatorService.Operation operation)
        {
            _operationCounts[operation]++;
        }

        public void DisplayStatistics(Player player)
        {
            ConsoleHelper.ShowSectionHeader("STATISTIK PENGGUNAAN");

            Console.WriteLine($"👤 Nama Pengguna: {player.Name}");
            Console.WriteLine($"📅 Tanggal Mulai: {player.FirstUsed:dd/MM/yyyy}");
            Console.WriteLine($"🧮 Total Perhitungan: {player.TotalCalculations}");
            Console.WriteLine($"💾 Nilai Memori: {player.MemoryValue}");
            Console.WriteLine(new string('-', 40));

            Console.WriteLine("\n📊 Frekuensi Operasi:");
            bool hasOperations = false;
            foreach (var kvp in _operationCounts)
            {
                if (kvp.Value > 0)
                {
                    hasOperations = true;
                    string symbol = CalculatorService.GetOperationSymbol(kvp.Key);
                    Console.WriteLine($"  {symbol}: {kvp.Value} kali");
                }
            }

            if (!hasOperations)
            {
                Console.WriteLine("  Belum ada operasi yang dilakukan");
            }

            if (player.TotalCalculations > 0)
            {
                Console.WriteLine("\n📈 Persentase:");
                foreach (var kvp in _operationCounts)
                {
                    if (kvp.Value > 0)
                    {
                        double percentage = (double)kvp.Value / player.TotalCalculations * 100;
                        string symbol = CalculatorService.GetOperationSymbol(kvp.Key);
                        Console.WriteLine($"  {symbol}: {percentage:F1}%");
                    }
                }
            }
        }

        // New method for developer tools
        public Dictionary<CalculatorService.Operation, int> GetOperationCounts()
        {
            return new Dictionary<CalculatorService.Operation, int>(_operationCounts);
        }
    }
}