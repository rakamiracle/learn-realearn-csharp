using System;

namespace learn_realearn.Models
{
    public class GameSettings
    {
        public string AppName { get; set; } = "KALKULATOR REAL-EARN";
        public string Version { get; set; } = "1.0.0";
        public ConsoleColor HeaderColor { get; set; } = ConsoleColor.Cyan;
        public ConsoleColor ResultColor { get; set; } = ConsoleColor.Green;
        public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;
        public bool ShowHistory { get; set; } = true;
        public int MaxHistoryRecords { get; set; } = 10;
    }
}