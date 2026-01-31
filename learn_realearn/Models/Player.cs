using System;
using System.Collections.Generic;

namespace learn_realearn.Models
{
    public class Player
    {
        public string Name { get; set; } = "Guest";
        public int TotalCalculations { get; set; } = 0;
        public DateTime FirstUsed { get; set; } = DateTime.Now;
        public List<CalculationHistory> History { get; set; } = new List<CalculationHistory>();

        // FITUR BARU: Memori
        public double MemoryValue { get; set; } = 0;

        public class CalculationHistory
        {
            public DateTime Timestamp { get; set; }
            public string Expression { get; set; }
            public double Result { get; set; }

            public CalculationHistory(string expression, double result)
            {
                Timestamp = DateTime.Now;
                Expression = expression;
                Result = result;
            }
        }

        public void AddToHistory(string expression, double result)
        {
            if (History.Count >= 10)
            {
                History.RemoveAt(0);
            }
            History.Add(new CalculationHistory(expression, result));
            TotalCalculations++;
        }

        // FITUR BARU: Method untuk memori
        public void MemoryStore(double value)
        {
            MemoryValue = value;
        }

        public double MemoryRecall()
        {
            return MemoryValue;
        }

        public void MemoryClear()
        {
            MemoryValue = 0;
        }

        public void MemoryAdd(double value)
        {
            MemoryValue += value;
        }

        public void MemorySubtract(double value)
        {
            MemoryValue -= value;
        }
    }
}