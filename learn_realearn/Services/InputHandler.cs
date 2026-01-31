using System;

namespace learn_realearn.Services
{
    public static class InputHandler
    {
        public static double GetNumber(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";

            // FITUR BARU: Coba quick calculation
            if (QuickCalculator.TryQuickCalculate(input, out double quickResult))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚡ Quick Result: {quickResult}");
                Console.ResetColor();
                return quickResult;
            }

            // Jika bukan quick command, parse sebagai angka biasa
            double number;
            while (!double.TryParse(input, out number))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Input tidak valid! Masukkan angka: ");
                Console.ResetColor();
                input = Console.ReadLine() ?? "";
            }

            return number;
        }

        public static CalculatorService.Operation ParseOperationChoice(string choice)
        {
            return choice switch
            {
                "1" => CalculatorService.Operation.Add,
                "2" => CalculatorService.Operation.Subtract,
                "3" => CalculatorService.Operation.Multiply,
                "4" => CalculatorService.Operation.Divide,
                "5" => CalculatorService.Operation.Power,
                "6" => CalculatorService.Operation.Modulus,
                _ => throw new ArgumentException("Pilihan operasi tidak valid")
            };
        }
    }
}