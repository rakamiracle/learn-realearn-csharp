using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== KALKULATOR SEDERHANA ===");
        Console.WriteLine("Selamat datang di program C# pertamamu!");

        bool lanjut = true;

        while (lanjut)
        {
            Console.WriteLine("\nPilih operasi:");
            Console.WriteLine("1. Penjumlahan (+)");
            Console.WriteLine("2. Pengurangan (-)");
            Console.WriteLine("3. Perkalian (*)");
            Console.WriteLine("4. Pembagian (/)");
            Console.WriteLine("5. Keluar");

            Console.Write("\nMasukkan pilihan (1-5): ");
            string pilihan = Console.ReadLine();

            if (pilihan == "5")
            {
                lanjut = false;
                Console.WriteLine("\nTerima kasih! Program selesai.");
                break;
            }

            if (pilihan != "1" && pilihan != "2" && pilihan != "3" && pilihan != "4")
            {
                Console.WriteLine("Pilihan tidak valid! Coba lagi.");
                continue;
            }

            // Input angka
            Console.Write("Masukkan angka pertama: ");
            double angka1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Masukkan angka kedua: ");
            double angka2 = Convert.ToDouble(Console.ReadLine());

            // Proses perhitungan
            double hasil = 0;
            string operasi = "";

            switch (pilihan)
            {
                case "1":
                    hasil = angka1 + angka2;
                    operasi = "+";
                    break;
                case "2":
                    hasil = angka1 - angka2;
                    operasi = "-";
                    break;
                case "3":
                    hasil = angka1 * angka2;
                    operasi = "*";
                    break;
                case "4":
                    if (angka2 == 0)
                    {
                        Console.WriteLine("Error: Tidak bisa dibagi dengan nol!");
                        continue;
                    }
                    hasil = angka1 / angka2;
                    operasi = "/";
                    break;
            }

            // Tampilkan hasil
            Console.WriteLine($"\nHasil: {angka1} {operasi} {angka2} = {hasil}");
            Console.WriteLine("Tekan Enter untuk melanjutkan...");
            Console.ReadLine();
        }

        Console.WriteLine("\nTekan Enter untuk keluar...");
        Console.ReadLine();
    }
}