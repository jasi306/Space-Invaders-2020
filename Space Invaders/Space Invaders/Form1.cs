using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Space_Invaders
{
    public partial class Form1 : Form
    {
        InvadersEngine invadersEngine = new InvadersEngine(640, 480, 7, 7);
        PictureBox player;
        List<PictureBox> enemies;

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)new Bitmap(imgToResize, size);
        }

        void correctHitbox(Control control, int objX, int objY)
        {
            //funckja, dzieki ktorej x i y znajduja sie na srodku sprite'u a nie w jego lewym gornym rogu
            control.Left = objX - control.Size.Width / 2;
            control.Top = Height - objY - control.Size.Height / 2; //odejmowanie od wartosci wysokosci, aby zachowac os y skierowana w gore
        }
        void SpawnPlayer(FO gracz)
        {
            //nadanie odpowiednich atrybutow sprite'owi z graczem
            player = new PictureBox();
            var size = new Size((int)gracz.Width, (int)gracz.Hight);
            var img = Image.FromFile("..\\Images\\cannon.png", true);
            player.Image=resizeImage(img, size);
            player.Size = size;
            player.Name = "player";
            correctHitbox(player, (int)gracz.x, (int)gracz.y);
            player.BringToFront();
            Controls.Add(player);
        }

        void SpawnInvaders(FO[,] invaders)
        {
            //nadanie odpowiednich atrybutow sprite'om przeciwnikow
            enemies = new List<PictureBox>();
            var size = new Size((int)invaders[0, 0].Width, (int)invaders[0, 0].Hight);
            var img = Image.FromFile("..\\Images\\enemy.png", true);
            img = resizeImage(img, size);
            int i = 0;
            foreach(FO invader in invaders)
            {
                PictureBox enemy = new PictureBox();
                enemy.Image = img;
                enemy.Size=size;
                enemy.Name = "invader" + i.ToString();
                correctHitbox(enemy, (int)invader.x, (int)invader.y);
                enemies.Add(enemy);
                i++;
            }
            foreach(PictureBox enemy in enemies)
            {
                Controls.Add(enemy);
            }
        }

        public Form1()
        {

            InitializeComponent();

            //okno przyjmuje wielkosc planszy
            Tuple<int, int> windowSize = invadersEngine.GetSize();
            Width = windowSize.Item1;
            Height = windowSize.Item2;

            //sprite'y są tworzone i pokazywane na ekranie
            SpawnPlayer(invadersEngine.gracz);
            SpawnInvaders(invadersEngine.Invaders);


            
        }

    }
}
