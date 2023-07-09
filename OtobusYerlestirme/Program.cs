using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{

    class Yolcu
    {
        public int id;
        public String adSoyad;
        public double toplamUzaklik;
        public Yolcu(int id, String adSoyad)
        {
            this.id = id;
            this.adSoyad = adSoyad;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(150, 25);
            double[,] uzaklikMatrisi = new double[40, 40];
            Random r = new Random();
            for (int i = 0; i < uzaklikMatrisi.GetLength(0); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    uzaklikMatrisi[i, j] = uzaklikMatrisi[j, i];
                }
                uzaklikMatrisi[i, i] = 0;
                for (int j = i+1; j < uzaklikMatrisi.GetLength(1); j++)
                {
                    double num = 0;
                    while (num == 0) num = r.NextDouble();
                    uzaklikMatrisi[i, j] = num * 10;
                }
            }
          
            String[] yolcuIsimleri = isimListesi(40);
            Yolcu[] yolcular = new Yolcu[40];
            Yolcu[,] otobus = new Yolcu[10, 4];
            for (int i = 0; i < yolcular.Length; i++)
            {
                yolcular[i] = new Yolcu(i, yolcuIsimleri[i]);
            }
            bool[] yerlesenYolcular = new bool[40];
            otobus[0,0] = yolcular[r.Next(0, 40)];
            yerlesenYolcular[otobus[0, 0].id] = true;
            for (int i = 1; i < 40; i++)
            {
                int rowIndex = i / 4;
                int colIndex = i % 4;
                List<int> IndexesToCheck = new List<int>();
                //Oturacak yolcu en solda değilse (Sol için)
                if (colIndex - 1 >= 0) IndexesToCheck.Add(otobus[rowIndex, colIndex - 1].id);

                //Oturacak yolcu en önde değilse
                if (rowIndex - 1 >= 0)
                {
                    IndexesToCheck.Add(otobus[rowIndex - 1, colIndex].id);
                    //Oturacak yolcu en solda değilse (Sol çapraz için)
                    if (colIndex - 1 >= 0) IndexesToCheck.Add(otobus[rowIndex - 1, colIndex - 1].id);
                    //Oturacak yolcu en sağda değilse (Sağ çapraz için)
                    if (colIndex + 1 < 4) IndexesToCheck.Add(otobus[rowIndex - 1, colIndex + 1].id);
                }
                Yolcu oturacakYolcu = minToplamUzaklikYolcu(IndexesToCheck, uzaklikMatrisi, yolcular, yerlesenYolcular);               
                otobus[rowIndex, colIndex] = oturacakYolcu;
                yerlesenYolcular[oturacakYolcu.id] = true;
            }

            otobusYazdır(otobus);
            Console.Read();            

        }

        static String[] adListesi = { "JALE", "ALİ", "MAHMUT", "MANSUR KÜRŞAD", "GAMZE", "MİRAÇ", "YÜCEL", "KUBİLAY", "HAYATİ", "BEDRİYE MÜGE", "BİRSEN", "SERDAL", "BÜNYAMİN", "ÖZGÜR", "FERDİ", "REYHAN", "İLHAN", "GÜLŞAH", "NALAN", "SEMİH", "ERGÜN", "FATİH", "ŞENAY", "SERKAN", "EMRE", "BAHATTİN", "IRAZCA", "HATİCE", "BARIŞ", "REZAN", "FATİH", "FUAT", "GÖKHAN", "ORHAN", "MEHMET", "EVREN", "OKTAY", "HARUN", "YAVUZ", "PINAR", "MEHMET", "UMUT", "MESUDE", "HÜSEYİN CAHİT", "HAŞİM ONUR", "EYYUP SABRİ", "MUSTAFA", "MUSTAFA", "UFUK", "AHMET ALİ", "MEDİHA", "HASAN", "KAMİL", "NEBİ", "ÖZCAN", "NAGİHAN", "CEREN", "SERKAN", "HASAN", "YUSUF KENAN", "ÇETİN", "TARKAN", "MERAL LEMAN", "ERGÜN", "KENAN AHMET", "URAL", "YAHYA", "BENGÜ", "FATİH NAZMİ", "DİLEK", "MEHMET", "TUFAN AKIN", "MEHMET", "TURGAY YILMAZ", "GÜLDEHEN", "GÖKMEN", "BÜLENT", "EROL", "BAHRİ", "ÖZEN ÖZLEM", "SELMA", "TUĞSEM", "TESLİME NAZLI", "GÜLÇİN", "İSMAİL", "MURAT", "EBRU", "TÜMAY", "AHMET", "EBRU", "HÜSEYİN YAVUZ", "BAŞAK", "AYŞEGÜL", "EVRİM", "YASER", "ÜLKÜ", "ÖZHAN", "UFUK", "AKSEL", "FULYA", "BURCU", "TAYLAN", "YILMAZ", "ZEYNEP", "BAYRAM", "GÜLAY", "RABİA", "SEVDA", "SERHAT", "ENGİN", "ASLI", "TUBA", "BARIŞ", "SEVGİ", "KALENDER", "HALİL", "BİLGE", "FERDA", "EZGİ", "AYSUN", "SEDA", "ÖZLEM", "ÖZDEN", "KORAY", "SENEM", "ZEYNEP", "EMEL", "BATURAY KANSU", "NURAY", "AYDOĞAN", "ÖZLEM", "DENİZ", "İLKNUR", "TEVFİK ÖZGÜN", "HASAN SERKAN", "KÜRŞAT", "SEYFİ", "ŞEYMA", "ÖZLEM", "ERSAGUN", "DİLBER", "MESUT", "ELİF", "MUHAMMET FATİH", "ÖZGÜR SİNAN", "MEHMET ÖZGÜR", "MAHPERİ", "ONUR", "İBRAHİM", "FATİH", "SEVİL", "SÜHEYLA", "VOLKAN", "İLKAY", "İLKNUR", "ZÜMRÜT ELA", "HALE", "YENER", "SEDEF", "FADIL", "SERPİL", "ZÜLFİYE", "SULTAN", "MUAMMER HAYRİ", "DERVİŞ", "YAŞAR GÖKHAN", "TUBA HANIM", "MEHRİ", "MUSTAFA FERHAT", "SERDAR", "MUSTAFA ERSAGUN", "ONAT", "ŞÜKRÜ", "OLCAY BAŞAK", "SERDAR", "YILDIZ", "AYDIN", "ALİ HALUK", "NİHAT BERKAY", "İSMAİL", "AYKAN", "SELÇUK", "MEHMET", "NEZİH", "MUSTAFA", "TİMUR", "ERHAN", "MUSTAFA", "MUTLU", "MEHMET HÜSEYİN", "İSMAİL EVREN", "OSMAN ERSEGUN", "MEHMET", "ELİF", "SERKAN", "MESUT", "MEHMET HİLMİ", "ASUDAN TUĞÇE", "AHMET GÖKHAN", "BAŞAK", "CEYHAN", "MUHAMMET TAYYİP", "ESİN", "ZEYNEP GÖKÇE", "EVRİM", "YASİN", "SALİHA", "DENİZ", "BELGİN", "ÖZLEM", "GONCA", "ESRA", "SEÇKİN", "ESRA", "FATİH", "MUSTAFA", "FEVZİYE", "MUSTAFA ARİF", "BİRGÜL", "ÖZLEM", "ÖZLEM", "FUNDA", "BERFİN", "DEMET", "SONAY", "SERÇİN", "ALMALA PINAR", "ÜMİT", "SENEM", "DENİZ", "MÜNEVER", "HATİCE", "ÖZLEM", "ÖZLEM", "ALİ SEÇKİN", "COŞKUN", "ÖZGE", "ZELİHA", "PINAR", "AYBÜKE", "HASİBE", "GÜRKAN", "ZÜHAL", "NAZIM", "ZEYNEP", "OSMAN", "AYLA", "BEYZA", "ELİF", "ERAY", "DİANA", "TUBA", "SEMRA", "VELAT", "BELGİN EMİNE", "SİBEL", "GÖKMEN ALPASLAN", "BENHUR ŞİRVAN", "DİLEK", "HANDE", "ŞAHABETTİN", "MİRAY", "ZERRİN", "İLKNUR", "ELİF", "MÜMTAZ", "TUĞBA", "DİLEK", "MEHMET BURHAN", "FUAT", "NİHAL", "AYŞEGÜL", "SEMA", "ZAFER", "NURSEL", "GÜLPERİ", "BİLGE", "FATİH", "CENGİZ", "SİMGE", "SEMA NİLAY", "EMİNE", "RİFAT CAN", "SİNAN", "LATİFE", "MEHMET", "NURDAN", "MELTEM", "ÜLKÜHAN", "HASAN", "GÜLDEN", "SAMET", "BERNA", "ÖZLEM", "NAFİYE", "KENAN", "SERKAN FAZLI", "NURSEL", "ABDULLAH", "ERGÜL", "HASAN", "MUSTAFA", "SEBAHAT", "EMİNE", "ERDAL", "LEZİZ", "BİRSEN", "TUBA", "AYŞEN", "EBRU", "TAYFUR", "MELTEM", "SERHAT", "AYCAN ÖZDEN", "ELİF", "SEVGÜL", "SELDA", "IŞIL", "SİBEL", "JÜLİDE ZEHRA", "BERİL GÜLÜŞ", "İNCİ", "ENGİN", "GÜLBAHAR", "MÜBECCEL", "NURDAN", "HANDE", "ÖZNUR", "HANDAN", "OSMAN TURGUT", "EMİN TONYUKUK", "NEJDET", "MUSTAFA", "GÜLİZ", "İPEK", "NİHAL", "MELDA", "DERYA", "DEMET", "MAHMUT", "EMEL", "ÖZNUR", "SONGÜL", "RESA", "GAMZE", "ÜMİT", "DENİZ", "MUAMMER MÜSLİM", "ÖMER FARUK", "TUĞÇE", "VELİ ENES", "ZAHİDE", "NURETTİN İREM", "SEDAT", "REMZİYE", "SİBEL", "İLKNUR", "YASEMİN", "AYLİN", "EMEL", "EMEL CENNET", "ŞAFAK", "METİN", "SÜLEYMAN", "MUKADDES", "BARIŞ", "MEHMET ALİ", "TEVFİK", "SERDAR", "EMİNE", "MÜRŞİT", "MUTLU", "FEZA", "İBRAHİM TAYFUN", "SERKAN", "AHMET SERKAN" };
        static String[] soyadListesi = { "YILMAZ", "ER", "KAYA", "YILDIRIM", "ORMANCI", "EFE", "ÇÖZER", "GİRGİN" };
        static String[] isimListesi(int adet)
        {
            Random r = new Random();
            String[] isimListesi = new String[adet];
            int sayac = 0;
            while (sayac < adet)
            {
                int adIndex = r.Next(0, adListesi.Length);
                int soyadIndex = r.Next(0, soyadListesi.Length);
                String isim = adListesi[adIndex] + " " + soyadListesi[soyadIndex];
                if (!isimListesi.Contains(isim))
                {
                    isimListesi[sayac] = isim;
                    sayac++;
                }
            }
            return isimListesi;
        }

        static Yolcu minToplamUzaklikYolcu(List<int> indexesToCheck, double[,] uzaklikMatrisi, Yolcu[] yolcular,bool[] yerlesenYolcular)
        {
            double minToplamUzaklik = indexesToCheck.Count * 10 + 1;
            int minToplamUzaklikYolcuIndex = -1;
            for (int i = 0; i < uzaklikMatrisi.GetLength(0); i++)
            {
                if (yerlesenYolcular[i]) continue;
                double toplamUzaklik = 0;
                foreach (int index in indexesToCheck)
                {      
                    toplamUzaklik += uzaklikMatrisi[i, index];
                }
                if(toplamUzaklik < minToplamUzaklik)
                {
                    minToplamUzaklik = toplamUzaklik;
                    minToplamUzaklikYolcuIndex = i;
                }
            }
            Yolcu minUzaklikYolcu = yolcular[minToplamUzaklikYolcuIndex];
            minUzaklikYolcu.toplamUzaklik = minToplamUzaklik;           
            return minUzaklikYolcu;
        }

        static void otobusYazdır(Yolcu[,]otobus)
        {
            double toplamUzaklik = 0;
            for (int i = 0; i < otobus.GetLength(0); i++)
            {
                for (int j = 0; j < otobus.GetLength(1); j++)
                {                   
                    Yolcu y = otobus[i, j];
                    string str = y.adSoyad + "[" + y.id + "][" + Math.Round(y.toplamUzaklik, 2) + "]";
                    Console.Write($"{str,-35}");
                    toplamUzaklik += y.toplamUzaklik;
                }
                Console.Write("\n");
            }
            Console.Write("\n");
            Console.Write("Toplam Uzaklık:"+ toplamUzaklik);
        }
    }
}
