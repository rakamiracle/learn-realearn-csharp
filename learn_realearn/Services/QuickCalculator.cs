using System;
using System.Collections.Generic;

namespace learn_realearn.Services
{
    public static class QuickCalculator
    {
        private static Dictionary<string, Func<double, double, double>> _quickOperations = new()
        {
            { "AVG", (a, b) => (a + b) / 2 },
            { "MIN", (a, b) => Math.Min(a, b) },
            { "MAX", (a, b) => Math.Max(a, b) },
            { "SQRT", (a, b) => Math.Sqrt(a) },
            { "PERCENT", (a, b) => (a / b) * 100 },
            { "SUM", (a, b) => a + b },
            { "DIFF", (a, b) => a - b },
            { "PROD", (a, b) => a * b },
            { "QUOT", (a, b) => a / b }
        };

        public static bool TryQuickCalculate(string input, out double result)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 3 && _quickOperations.ContainsKey(parts[1].ToUpper()))
            {
                if (double.TryParse(parts[0], out double num1) &&
                    double.TryParse(parts[2], out double num2))
                {
                    var operation = parts[1].ToUpper();

                    // Handle division by zero
                    if ((operation == "QUOT" || operation == "PERCENT") && num2 == 0)
                    {
                        result = double.NaN;
                        return true;
                    }

                    // Handle sqrt with negative
                    if (operation == "SQRT" && num1 < 0)
                    {
                        result = double.NaN;
                        return true;
                    }

                    result = _quickOperations[operation](num1, num2);
                    return true;
                }
            }

            return false;
        }

        public static void ShowQuickCommands()
        {
            Console.WriteLine("\n⚡ QUICK COMMANDS:");
            Console.WriteLine("  Format: [angka1] [command] [angka2]");
            Console.WriteLine("  Contoh: 10 AVG 20  → Rata-rata (10 + 20) / 2");
            Console.WriteLine("          5 PERCENT 20 → 5 adalah berapa persen dari 20");
            Console.WriteLine("          16 SQRT 0 → Akar kuadrat dari 16");
            Console.WriteLine("\nAvailable: AVG, MIN, MAX, SQRT, PERCENT, SUM, DIFF, PROD, QUOT");
        }
    }
}