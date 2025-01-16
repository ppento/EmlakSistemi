using System;
using System.Collections.Generic;
using System.IO;

//UTKU MERT GEÇGEL - 241041131 - NESNEYE YÖNELİK PROGRAMLAMA PROJESİ - EMLAK SİSTEMİ

namespace EmlakSistemi
{
    // Temel emlak sınıfımız. Ortak özellikler burada tanımlandı.
    class Emlak
    {
        private int id;
        private string il;
        private string ilce;
        private string adres;
        private double fiyat;
        private int metrekare;
        private string durum; // Satılık, Kiralık, Satılmış
        private string ekAciklama;

        public int ID         // Kapsülleme kısmı: private olan değişkenleri, get set ile işleme alıyoruz.
        {
            get { return id; }
            set { id = value; }
        }

        public string Il
        {
            get { return il; }
            set { il = value; }
        }

        public string Ilce
        {
            get { return ilce; }
            set { ilce = value; }
        }

        public string Adres
        {
            get { return adres; }
            set { adres = value; }
        }

        public double Fiyat
        {
            get { return fiyat; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Fiyat negatif olamaz!");
                fiyat = value;
            }
        }

        public int Metrekare
        {
            get { return metrekare; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Metrekare sıfır veya negatif olamaz!");
                metrekare = value;
            }
        }

        public string Durum
        {
            get { return durum; }
            set
            {
                if (value != "Satılık" && value != "Kiralık" && value != "Satılmış")
                    throw new ArgumentException("Durum sadece 'Satılık', 'Kiralık' veya 'Satılmış' olabilir.");
                durum = value;
            }
        }

        public string EkAciklama
        {
            get { return ekAciklama; }
            set { ekAciklama = value; }
        }

        // Emlak bilgilerini ekrana yazdırıyoruz.
        public virtual void BilgileriGoster()
        {
            Console.WriteLine($"ID: {ID}, İl: {Il}, İlçe: {Ilce}, Adres: {Adres}, Fiyat: {Fiyat} TL, Metrekare: {Metrekare} m², Durum: {Durum}, Ek Açıklamalar: {EkAciklama}");
        }
    }

    // Ev sınıfı, Emlak sınıfından türedi.
    class Ev : Emlak
    {
        public int OdaSayisi { get; set; }
        public bool BahceVarMi { get; set; }

        // Ev bilgilerini, üst sınıfın bilgilerinin üstüne ekleyerek gösteriyoruz.
        public override void BilgileriGoster()
        {
            base.BilgileriGoster();
            Console.WriteLine($"Oda Sayısı: {OdaSayisi}, Bahçe: {(BahceVarMi ? "Var" : "Yok")}");
        }
    }

    // Villa sınıfı, Emlak sınıfından türedi.
    class Villa : Emlak
    {
        public bool HavuzVarMi { get; set; }
        public string Manzara { get; set; }

        // Villa bilgilerini, üst sınıfın bilgileriyle birleştiriyoruz.
        public override void BilgileriGoster()
        {
            base.BilgileriGoster();
            Console.WriteLine($"Havuz: {(HavuzVarMi ? "Var" : "Yok")}, Manzara: {Manzara}");
        }
    }

    // Arazi sınıfı, Emlak sınıfından türedi.
    class Arazi : Emlak
    {
        public string ImarDurumu { get; set; }  // İmarlı ya da imarsız
        public bool SuVarMi { get; set; }  // Su altyapısı var mı?

        // Arazi bilgilerini, üst sınıfın bilgileriyle birleştiriyoruz.
        public override void BilgileriGoster() //Override
        {
            base.BilgileriGoster();
            Console.WriteLine($"İmar Durumu: {ImarDurumu}, Su Altyapısı: {(SuVarMi ? "Var" : "Yok")}");
        }
    }

    // Veritabanını dosya olarak saklıyoruz.
    static class EmlakVeritabani
    {
        private static string dosyaYolu = "emlaklar.txt";

        // Tüm emlakları dosyaya kaydediyoruz.
        public static void Kaydet(List<Emlak> emlakListesi)
        {
            using (StreamWriter sw = new StreamWriter(dosyaYolu))
            {
                foreach (var emlak in emlakListesi)
                {
                    sw.WriteLine($"{emlak.ID}|{emlak.Il}|{emlak.Ilce}|{emlak.Adres}|{emlak.Fiyat}|{emlak.Metrekare}|{emlak.Durum}|{emlak.EkAciklama}");
                }
            }
        }

        // Daha önce kaydedilen emlakları dosyadan yüklüyoruz.
        public static void Yukle(List<Emlak> emlakListesi, ref int emlakID)
        {
            if (File.Exists(dosyaYolu))
            {
                using (StreamReader sr = new StreamReader(dosyaYolu))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var parts = line.Split('|');
                        emlakListesi.Add(new Emlak
                        {
                            ID = int.Parse(parts[0]),
                            Il = parts[1],
                            Ilce = parts[2],
                            Adres = parts[3],
                            Fiyat = double.Parse(parts[4]),
                            Metrekare = int.Parse(parts[5]),
                            Durum = parts[6],
                            EkAciklama = parts.Length > 7 ? parts[7] : ""
                        });
                        emlakID = Math.Max(emlakID, int.Parse(parts[0]) + 1);
                    }
                }
            }
        }
    }

    class Program
    {
        static List<Emlak> emlakListesi = new List<Emlak>();
        static int emlakID = 1;


        static void Main(string[] args)
        {
            AdminGiris.GirisYapp();
            // Program başladığında eski verileri yüklüyoruz.
            EmlakVeritabani.Yukle(emlakListesi, ref emlakID);

            while (true)
            {
                Console.WriteLine("\nEmlak Sistemi");
                Console.WriteLine("1. Ev Ekle");
                Console.WriteLine("2. Villa Ekle");
                Console.WriteLine("3. Arazi Ekle");
                Console.WriteLine("4. Emlakları Listele");
                Console.WriteLine("5. Emlak Ara (ID ile)");
                Console.WriteLine("6. Emlak Sil (ID ile)");
                Console.WriteLine("7. Emlak Düzenle (ID ile)");
                Console.WriteLine("8. Çıkış");
                Console.Write("Seçiminiz: ");
                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        EvEkle();
                        break;
                    case "2":
                        VillaEkle();
                        break;
                    case "3":
                        AraziEkle();
                        break;
                    case "4":
                        Listele();
                        break;
                    case "5":
                        Ara();
                        break;
                    case "6":
                        Sil();
                        break;
                    case "7":
                        Duzenle();
                        break;
                    case "8":
                        EmlakVeritabani.Kaydet(emlakListesi);
                        Console.WriteLine("Veriler kaydedildi. Çıkış yapılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                        break;
                }
            }
        }

        static void EvEkle()
        {
            Ev yeniEv = new Ev();
            OrtakEmlakBilgisiAl(yeniEv);
            Console.Write("Oda Sayısı: ");
            yeniEv.OdaSayisi = int.Parse(Console.ReadLine());
            Console.Write("Bahçe Var mı? (E/H): ");
            yeniEv.BahceVarMi = Console.ReadLine().ToUpper() == "E";
            emlakListesi.Add(yeniEv);
            Console.WriteLine("Ev başarıyla eklendi!");
        }

        static void VillaEkle()
        {
            Villa yeniVilla = new Villa();
            OrtakEmlakBilgisiAl(yeniVilla);
            Console.Write("Havuz Var mı? (E/H): ");
            yeniVilla.HavuzVarMi = Console.ReadLine().ToUpper() == "E";
            Console.Write("Manzara Türü: ");
            yeniVilla.Manzara = Console.ReadLine();
            emlakListesi.Add(yeniVilla);
            Console.WriteLine("Villa başarıyla eklendi!");
        }

        static void AraziEkle()
        {
            Arazi yeniArazi = new Arazi();
            OrtakEmlakBilgisiAl(yeniArazi);
            Console.Write("İmar Durumu (İmarlı/İmarsız): ");
            yeniArazi.ImarDurumu = Console.ReadLine();
            Console.Write("Su Altyapısı Var mı? (E/H): ");
            yeniArazi.SuVarMi = Console.ReadLine().ToUpper() == "E";
            emlakListesi.Add(yeniArazi);
            Console.WriteLine("Arazi başarıyla eklendi!");
        }

        static void OrtakEmlakBilgisiAl(Emlak emlak)
        {
            emlak.ID = emlakID++;
            Console.Write("İl: ");
            emlak.Il = Console.ReadLine();
            Console.Write("İlçe: ");
            emlak.Ilce = Console.ReadLine();
            Console.Write("Adres: ");
            emlak.Adres = Console.ReadLine();
            Console.Write("Fiyat: ");
            emlak.Fiyat = double.Parse(Console.ReadLine());
            Console.Write("Metrekare: ");
            emlak.Metrekare = int.Parse(Console.ReadLine());
            Console.Write("Durum (Satılık/Kiralık/Satılmış): ");
            emlak.Durum = Console.ReadLine();
            Console.Write("Ek Açıklama: ");
            emlak.EkAciklama = Console.ReadLine();
        }

        static void Listele()
        {
            foreach (var emlak in emlakListesi)
            {
                emlak.BilgileriGoster();
            }
        }

        static void Ara()
        {
            Console.Write("Aramak istediğiniz emlak ID'si: ");
            int arananID = int.Parse(Console.ReadLine());
            var bulunanEmlak = emlakListesi.Find(e => e.ID == arananID);
            if (bulunanEmlak != null)
            {
                bulunanEmlak.BilgileriGoster();
            }
            else
            {
                Console.WriteLine("Emlak bulunamadı.");
            }
        }

        static void Sil()
        {
            Console.Write("Silmek istediğiniz emlak ID'si: ");
            int silinecekID = int.Parse(Console.ReadLine());
            var silinecekEmlak = emlakListesi.Find(e => e.ID == silinecekID);
            if (silinecekEmlak != null)
            {
                emlakListesi.Remove(silinecekEmlak);
                Console.WriteLine("Emlak başarıyla silindi.");
            }
            else
            {
                Console.WriteLine("Emlak bulunamadı.");
            }
        }

        static void Duzenle()
        {
            Console.Write("Düzenlemek istediğiniz emlak ID'si: ");
            int duzenlenecekID = int.Parse(Console.ReadLine());
            var duzenlenecekEmlak = emlakListesi.Find(e => e.ID == duzenlenecekID);
            if (duzenlenecekEmlak != null)
            {
                Console.WriteLine("Emlak bilgilerini güncelleyiniz.");
                OrtakEmlakBilgisiAl(duzenlenecekEmlak);
                Console.WriteLine("Emlak bilgileri başarıyla güncellendi.");
            }
            else
            {
                Console.WriteLine("Emlak bulunamadı.");
            }
        }
    }

    static class AdminGiriss
    {
        public static void GirisYap()
        {
            string kullaniciAdi = "admin";
            string sifre = "1234";
            Console.WriteLine("Yönetici Girişi");

            for (int i = 3; i > 0; i--)
            {
                Console.Write("Kullanıcı Adı: ");
                string girilenKullaniciAdi = Console.ReadLine();
                Console.Write("Şifre: ");
                string girilenSifre = Console.ReadLine();

                if (girilenKullaniciAdi == kullaniciAdi && girilenSifre == sifre)
                {
                    Console.WriteLine("Giriş Başarılı!");
                    return;
                }
                else
                {
                    Console.WriteLine($"Hatalı giriş! {i - 1} hakkınız kaldı.");
                }
            }

            Console.WriteLine("3 defa hatalı giriş yaptınız. Programdan çıkılıyor.");
            Environment.Exit(0);
        }
    }
}
