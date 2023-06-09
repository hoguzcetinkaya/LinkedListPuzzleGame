﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puzzleGame
{
    public partial class Form1 : Form
    {
        Dosya dosya = new Dosya();
        LinkedList<Image> linkedList = new LinkedList<Image>();//Image sınıfı türünden bir bağlı liste oluşturduk
        LinkedList<Image> mixedLinkedList = new LinkedList<Image>();//Image sınıfı türünden bir bağlı liste daha oluşturduk, bu listeyi karıştırılan parçalar için kullanacağız

        private string kullaniciAdi;
        public Form1(string kullaniciAdiForm2)
        {
            InitializeComponent();
            this.kullaniciAdi = kullaniciAdiForm2;
        }

        private void original_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 16; i++)//Form ilk açıldığında puzzle parçaları kullanılabilir değildir
            {
                var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Enabled = false;
                }
            }

            original.SizeMode = PictureBoxSizeMode.StretchImage;//Orijinal resmi PicturBox boyutunda sıkıştırıyoruz
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mix.Enabled = true;
                original.Image = Image.FromFile(openFileDialog1.FileName);
                parcala();
            }

        }

        private void parcala()
        {

            if (original.Image != null)
            {
                linkedList.Clear();//Listeyi temizlemezsem her seferinde ilk seçilen görsel gelir çünkü ilk 16 düğüme bü görselin parçaları atandı
            }

            Bitmap resim = new Bitmap(original.Image, 400, 400);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Bitmap parca = resim.Clone(new Rectangle(j * 100, i * 100, 100, 100), resim.PixelFormat);
                    linkedList.AddLast(parca);
                }
            }

            LinkedListNode<Image> current = linkedList.First;//Tüm düğümleri sıra sıra gezip içideki parçaları butonlara atamak için Head değerli bir düğüm oluşturdum
            for (int i = 1; i <= 16; i++)
            {
                var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Image = current.Value;
                    current = current.Next;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mix.Enabled = false;
            for (int i = 1; i <= 16; i++)//Form ilk açıldığında puzzle parçaları kullanılabilir değildir
            {
                var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Enabled = false;
                }
            }

            if (!File.Exists(dosya.DosyaYolu)) //dosyaYolunda dosya yok ise içeriye gir
            {
                using (StreamWriter sw = File.CreateText(dosya.DosyaYolu)) //Belirtilen dosya yolunda dosya yoksa oluştur
                {
                    sw.WriteLine("Ad,Hamle,Puan"); //Oluşturulan veriye ilk satırını ekle
                }
            }

            listele();
            isimLabel.Text = kullaniciAdi;//Form2'den gelen kullanıcı adı labela aktarılıyor
        }

        private void karistir()
        {
            if (original.Image != null)
            {
                mixedLinkedList.Clear();//Listeyi temizlemezsem her seferinde ilk seçilen görsel gelir çünkü ilk 16 düğüme bü görselin parçaları atandı
            }

            Random rastgele = new Random();
            LinkedList<Image> copyLinkedList = new LinkedList<Image>(linkedList);
            for (int i = 0; i < 16; i++)//16 tane parça var 16 kere çalışacak
            {
                int rastgeleIndex = rastgele.Next(0, copyLinkedList.Count);//0 ve 16 arasında rastgele bir sayı oluşturuyoruz
                LinkedListNode<Image> rastgeleNode = copyLinkedList.First;//Image sınıfı türünden bir düğüm elde ediyoruz ve buna düzenli listenin ilk elemanını atıyoruz
                for (int j = 0; j < rastgeleIndex; j++)
                {
                    rastgeleNode = rastgeleNode.Next;//Rastgele sayının üretildiği düğümün bulunduğu yere kadar ilerliyoruz
                }
                copyLinkedList.Remove(rastgeleNode);//Karışık listeye alınan düğümü düzgün olan kopya listeden siliyoruz bu sayede aynı parça iki kez işleme alınmıyor
                mixedLinkedList.AddLast(rastgeleNode);//Karıştırılmış listeye resim parçalı düğüm ekleniyor
            }

            LinkedListNode<Image> mixedLinkedListNode = mixedLinkedList.First;//Karışık listenin ilk düğümü
            LinkedListNode<Image> linkedListNode = linkedList.First;//Düzgün listenin ilk düğümü
            int count = 0;//Her iki listede de parçalar aynıysa artacak olan sayaç
            while (mixedLinkedListNode != null)
            {
                if (mixedLinkedListNode.Value == linkedListNode.Value)
                {
                    count++;
                }
                mixedLinkedListNode = mixedLinkedListNode.Next;
                linkedListNode = linkedListNode.Next;
            }

            if (count >= 1)//En az bir parça doğru yerde mi diye kontrol ediyor
            {
                mix.Enabled = false;//Parçalardan en az bir tanesi doğru yerdeyse artık karıştırma butonu aktif olmasın yani karıştırma yapılamasın
                original.Enabled = false;//Parçalardan en az bir tanesi doğru yerdeyse artık fotoğraf seçme işlemine izin verilmesin

                for (int i = 1; i <= 16; i++)//Karıştırma işlemi düzgün şekilde bitti ve karıştırma butonu deaktif oldu puzzle parçaları aktif oldu
                {
                    var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                    if (button != null)
                    {
                        button.Enabled = true;
                    }
                }
            }

            //Her bir butona yeni bağlı listedeki sıradaki elemanı atıyoruz
            LinkedListNode<Image> current = mixedLinkedList.First;//Karışık listenin ilk düğümünü yeni bir düğüme atıyoruz
            for (int i = 1; i <= 16; i++)
            {
                var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Image = current.Value;
                    current = current.Next;
                }
            }
        }

        private void mix_Click(object sender, EventArgs e)
        {
            karistir();
        }

        private Image firstImage;
        private Button firstButton;
        private Button secondButton;
        int hamleSayac = 0;
        int skorSayac = 0;
        int eslesmeSayac = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            bool dogruHamleMi = false;
            Button currentButton = (Button)sender;

            if (firstImage == null)//Eğer şu an içinde bulunulan butonu birinci olarak seçtiysen
            {
                // İlk buton seçildiğinde
                firstImage = currentButton.Image;
                firstButton = currentButton;
            }
            else//Eğer şu an içinde bulunulan butonu ikinci olarak seçtiysen
            {
                hamleSayac++;//Her hamle yapıldığında sayaç artsın
                hamleLabel.Text = hamleSayac.ToString();
                // İkinci buton seçildiğinde
                secondButton = currentButton;

                LinkedListNode<Image> node1 = null;//Image sınıfı türünden bir düğüm elde ediyoruz
                LinkedListNode<Image> node2 = null;//Image sınıfı türünden bir düğüm elde ediyoruz

                node1 = mixedLinkedList.Find(value: firstButton.Image);//Birinci butonun resim değerine sahip olan düğümü buluyoruz
                node2 = mixedLinkedList.Find(value: secondButton.Image);//İkinci butonun resim değerine sahip olan düğümü buluyoruz
                Swap(mixedLinkedList, node1, node2);

                LinkedListNode<Image> current = mixedLinkedList.First;//Karışık listenin ilk düğümünü yeni bir düğüme atıyoruz
                for (int i = 1; i <= 16; i++)//Burada Swap fonksiyonu ile güncellenen mixedLinkedList'i yeniden butonlara atıyoruz
                {
                    var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                    if (button != null)
                    {
                        button.Image = current.Value;
                        current = current.Next;
                    }
                }

                //Doğru olan sıralı liste ile karışık listenin her bir düğümü sırasıyla karşılaştırılmalı doğru olduğu tespit edilen buton deaktif olmalı
                LinkedListNode<Image> mixedTemp = mixedLinkedList.First;//Image sınıfı türünden bir düğüm elde ediyoruz
                LinkedListNode<Image> temp = linkedList.First;//Image sınıfı türünden bir düğüm elde ediyoruz
                while (temp != null)
                {
                    if (temp.Value == mixedTemp.Value)
                    {
                        for (int i = 1; i <= 16; i++)//16 buton var bunlardan düğüm değeri ile eşleşeni buluyoruz
                        {
                            var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                            if (button.Image == mixedTemp.Value)//Bu satır her bir düğümü tekrar tekrar işleme tabi tutuyor
                            {
                                if (button.Enabled != false)//O yüzden bu satırda sadece false olmayanları işleme alıyoruz
                                {
                                    button.Enabled = false;//Doğru yerde olan buton deaktif olsun
                                    skorSayac += 5; eslesmeSayac++;
                                    skorLabel.Text = skorSayac.ToString();
                                    dogruHamleMi = true;
                                }
                            }
                        }
                    }
                    temp = temp.Next;
                    mixedTemp = mixedTemp.Next;
                }

                if (dogruHamleMi == false)
                {
                    skorSayac -= 10;
                    skorLabel.Text = skorSayac.ToString();
                }

                if (eslesmeSayac == 16)
                {
                    MessageBox.Show("Tüm parçaları doğru yere koymayı başardınız! Yeniden oynamak için bir resim seçiniz.", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    yazdir(hamleSayac, skorSayac);
                    listele();
                    original.Enabled = true;
                    hamleSayac = 0;
                    skorSayac = 0;
                    eslesmeSayac = 0;
                    skorLabel.Text = skorSayac.ToString();
                    hamleLabel.Text = hamleSayac.ToString();
                    original.Image = null;
                }

                //Depolanan resmi, ikinci butona atayın
                firstImage = null;
                firstButton = null;
                secondButton = null;
            }
        }

        //Swap fonksiyonu sayesinde seçilen butonlardaki resimlere sahip olan düğümlerin yerlerini değiştiriyoruz
        public static void Swap(LinkedList<Image> list, LinkedListNode<Image> node1, LinkedListNode<Image> node2)
        {
            if (list == null || node1 == null || node2 == null)
                return;

            if (node1 == node2)
                return;

            // Düğümlerin değerlerini geçici değişkenlerde depoluyoruz
            Image tempValue = node1.Value;
            node1.Value = node2.Value;
            node2.Value = tempValue;
        }

        void yazdir(int hamleSayac, int skorSayac)
        {
            string satir = kullaniciAdi + "," + hamleSayac.ToString() + "," + skorSayac.ToString();
            using (StreamWriter sw = File.AppendText(dosya.DosyaYolu))
            {
                sw.WriteLine(satir);
            }
        }

        void listele()
        {
            listBox1.Items.Clear();
            List<string> kayitlar = new List<string>(); // Tutulacak verilerin listelenmesi için bir List<> oluşturduk

            using (StreamReader sr = new StreamReader(dosya.DosyaYolu)) // Hangi dosyanın okunacağını belirttik
            {
                string satir;
                while ((satir = sr.ReadLine()) != null) // Eğer ki okunan dosyanın içerisinde okunma devam ediyorsa yani satır değişkeni null dönmüyorsa döngü devam etsin
                {
                    string[] sutun = satir.Split(','); // her seferinde okunan veriyi parçalıyoruz
                    if (sutun[2] != "Puan") // bu parçalamanın tek sebebi başlangıçta oluşturduğumuz sütunların eklenmemesi
                    {
                        string kayit = sutun[0] + "," + sutun[2];
                        kayitlar.Add(kayit);
                    }
                }
            }
            //Sort fonksiyonu listeyi küçükten büyüğe sıralamamıza yarıyor fakat burada bizim istediğimiz yapı ise şudur;
            //List<string> bir yapının içerisindeki int yapıya göre sıralama yapılması yani "25" ifadesini convert yapıp işleme tutuyor.
            //Bu sayede Puanlara göre bir listeleme yapıyoruz
            kayitlar.Sort((x, y) => Convert.ToInt32(x.Split(',')[1]).CompareTo(Convert.ToInt32(y.Split(',')[1])));
            kayitlar.Reverse(); //Puanlamaya göre yapılan sıralamayı büyükten küçüğe çeviriyoruz
            int sayac = 1;
            foreach (var item in kayitlar)
            {
                if (sayac <= 10)
                {
                    sayac++;
                    listBox1.Items.Add(item);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}