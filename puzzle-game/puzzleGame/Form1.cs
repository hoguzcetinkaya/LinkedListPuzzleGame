using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puzzleGame
{
    public partial class Form1 : Form
    {
        LinkedList<Image> linkedList = new LinkedList<Image>();//Image sınıfı türünden bir bağlı liste oluşturduk
        LinkedList<Image> mixedLinkedList = new LinkedList<Image>();//Image sınıfı türünden bir bağlı liste daha oluşturduk, bu listeyi karıştırılan parçalar için kullanacağız

        public Form1()
        {
            InitializeComponent();
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
            for (int i = 1; i <= 16; i++)//Form ilk açıldığında puzzle parçaları kullanılabilir değildir
            {
                var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Enabled = false;
                }
            }
        }

        private void karistir()//Sadece 16. parça doğru yerde olunca en az bir tanesi doğru yerde diye algılamıyor
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
            while (mixedLinkedListNode.Next != null)
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

        private void button1_Click(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;

            if (firstImage == null)//Eğer şu an içinde bulunulan butonu birinci olarak seçtiysen
            {
                // İlk buton seçildiğinde
                firstImage = currentButton.Image;
                firstButton = currentButton;
            }
            else//Eğer şu an içinde bulunulan butonu ikinci olarak seçtiysen
            {
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
                while (temp.Next != null)
                {
                    if (temp.Value == mixedTemp.Value)
                    {
                        for (int i = 1; i <= 16; i++)//16 buton var bunlardan düğüm değeri ile eşleşeni buluyoruz
                        {
                            var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                            if (button.Image == mixedTemp.Value)
                                button.Enabled = false;
                        }
                    }
                    temp = temp.Next;
                    mixedTemp = mixedTemp.Next;
                }

                if (button16.Image==linkedList.Last.Value)
                    button16.Enabled = false;

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
    }
}
