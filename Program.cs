using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string nazwaPliku = "plik_testowy.bin";
        int wielkoscPlikuMB = 300;
        UtworzPlik(nazwaPliku, wielkoscPlikuMB);

        WykonajTestKopiowania("Metoda1 (FileStream)", KopiujPlikMetoda1, nazwaPliku);
        WykonajTestKopiowania("Metoda2 (File.Copy)", KopiujPlikMetoda2, nazwaPliku);

        UsunPlik(nazwaPliku);

        Console.WriteLine("Testy zakończone.");
        Console.ReadKey();
    }

    static void WykonajTestKopiowania(string opis, Action<string, string> metodaKopiowania, string nazwaPliku)
    {
        Console.WriteLine($"Rozpoczęto test: {opis}");

        Stopwatch stoper = new Stopwatch();
        stoper.Start();

        string nazwaKopiowanegoPliku = $"kopia_{nazwaPliku}";

        metodaKopiowania(nazwaPliku, nazwaKopiowanegoPliku);

        stoper.Stop();
        Console.WriteLine($"Czas wykonania: {stoper.ElapsedMilliseconds} ms");

        UsunPlik(nazwaKopiowanegoPliku);

        Console.WriteLine($"Zakończono test: {opis}\n");
    }

    static void KopiujPlikMetoda1(string sciezkaPlikuWejsciowego, string sciezkaPlikuWyjsciowego)
    {
        using (FileStream wejsciowyPlik = new FileStream(sciezkaPlikuWejsciowego, FileMode.Open))
        {
            using (FileStream wyjsciowyPlik = new FileStream(sciezkaPlikuWyjsciowego, FileMode.Create))
            {
                byte[] bufor = new byte[1024];
                int odczytaneBajty;

                while ((odczytaneBajty = wejsciowyPlik.Read(bufor, 0, bufor.Length)) > 0)
                {
                    wyjsciowyPlik.Write(bufor, 0, odczytaneBajty);
                }
            }
        }
    }

    static void KopiujPlikMetoda2(string sciezkaPlikuWejsciowego, string sciezkaPlikuWyjsciowego)
    {
        File.Copy(sciezkaPlikuWejsciowego, sciezkaPlikuWyjsciowego);
    }

    static void UtworzPlik(string nazwaPliku, int wielkoscMB)
    {
        byte[] dane = new byte[wielkoscMB * 1024 * 1024];
        new Random().NextBytes(dane);
        File.WriteAllBytes(nazwaPliku, dane);
    }

    static void UsunPlik(string nazwaPliku)
    {
        if (File.Exists(nazwaPliku))
        {
            File.Delete(nazwaPliku);
        }
    }
}
