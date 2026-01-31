using System;

namespace learn_realearn.Services
{
    public static class CalculatorService
    {
        public enum Operation
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            Power,
            Modulus
        }

        public static double Calculate(double num1, double num2, Operation operation)
        {
            return operation switch
            {
                Operation.Add => Add(num1, num2),
                Operation.Subtract => Subtract(num1, num2),
                Operation.Multiply => Multiply(num1, num2),
                Operation.Divide => Divide(num1, num2),
                Operation.Power => Power(num1, num2),
                Operation.Modulus => Modulus(num1, num2),
                _ => throw new ArgumentException("Operation not supported")
            };
        }

        public static double Add(double a, double b) => a + b;
        public static double Subtract(double a, double b) => a - b;
        public static double Multiply(double a, double b) => a * b;

        public static double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Tidak bisa dibagi dengan nol!");
            return a / b;
        }

        public static double Power(double a, double b) => Math.Pow(a, b);
        public static double Modulus(double a, double b) => a % b;

        public static string GetOperationSymbol(Operation operation)
        {
            return operation switch
            {
                Operation.Add => "+",
                Operation.Subtract => "-",
                Operation.Multiply => "×",
                Operation.Divide => "÷",
                Operation.Power => "^",
                Operation.Modulus => "%",
                _ => "?"
            };
        }
    }
}