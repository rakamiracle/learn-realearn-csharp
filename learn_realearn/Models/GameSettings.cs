using System;

namespace learn_realearn.Models
{
    public class GameSettings
    {
        public string AppName { get; set; } = "KALKULATOR REAL-EARN";
        public string Version { get; set; } = "1.2.0"; // Update version
        public ConsoleColor HeaderColor { get; set; } = ConsoleColor.Cyan;
        public ConsoleColor ResultColor { get; set; } = ConsoleColor.Green;
        public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;
        public ConsoleColor DevColor { get; set; } = ConsoleColor.Magenta; // NEW
        public bool ShowHistory { get; set; } = true;
        public int MaxHistoryRecords { get; set; } = 10;

        // NEW: Developer Settings
        public bool DeveloperMode { get; set; } = false;
        public string DeveloperPassword { get; set; } = "dev123"; // Default password
        public bool EnableExperimentalFeatures { get; set; } = false;
    }
}