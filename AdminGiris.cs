using System;

namespace EmlakSistemi
{
    public static class AdminGiris
    {
        public static void GirisYapp()
        {
            string adminKullaniciAdi = "admin";
            string adminSifre = "0987";

            while (true)
            {
                Console.Write("Kullanıcı Adı: ");
                string girilenKullaniciAdi = Console.ReadLine();

                Console.Write("Şifre: ");
                string girilenSifre = SifreGir();

                if (girilenKullaniciAdi == adminKullaniciAdi && girilenSifre == adminSifre)
                {
                    Console.WriteLine("\nGiriş başarılı! Emlak sistemine hoş geldiniz.");
                    break;
                }
                else
                {
                    Console.WriteLine("Hatalı kullanıcı adı veya şifre! Lütfen tekrar deneyin.\n");
                }
            }
        }

        // Şifreyi gizleyerek girişi sağlayan metot
        private static string SifreGir()
        {
            string sifre = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);  // Karakteri ekrana yazmadan alır
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    sifre += key.KeyChar;
                    Console.Write("*");  // Ekranda '*' karakteri gösterir
                }
                else if (key.Key == ConsoleKey.Backspace && sifre.Length > 0)
                {
                    sifre = sifre.Substring(0, sifre.Length - 1); // Son karakteri siler
                    Console.Write("\b \b");  // Ekrandan da son '*' karakterini siler
                }
            } while (key.Key != ConsoleKey.Enter);  // Enter'a basılana kadar devam eder

            Console.WriteLine();
            return sifre;
        }
    }
}
