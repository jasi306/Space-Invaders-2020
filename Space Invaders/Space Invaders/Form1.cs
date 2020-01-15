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
        public static Form1 Self;

        const int FPS = 60;
        InvadersEngine invadersEngine = new InvadersEngine(640, 480, 4,8);
        PictureBox playerSprite;
        List<PictureBox> enemiesSprites;

        List<PictureBox> playerBulletsGraphics;
        List<PictureBox> enemyBulletsGraphics;

        Image playerImage;
        Image enemyImage;
        Image playerBulletImage;
        Image enemyBulletImage;

        string pathToPlayerImage = "..\\Images\\cannon.png";
        string pathToEnemyImage = "..\\Images\\enemy.png";
        string pathToPlayerBulletImage = "..\\Images\\cannon.png";
        string pathToEnemyBulletImage = "..\\Images\\cannon.png";

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)new Bitmap(imgToResize, size);
        }

        Tuple<int, int> hitboxCenter(int x, int y, int width, int height)
        {
            //funckja, dzieki ktorej x i y znajduja sie na srodku sprite'u a nie w jego lewym gornym rogu
            var hitbox = new Tuple<int, int>(x - width / 2, Height - y - height); //odejmowanie od wartosci wysokosci, aby zachowac os y skierowana w gore
            return hitbox;
        }
        void setSpritePosition(Control control, int objX, int objY)
        {
            Tuple<int, int> hitboxCorner = hitboxCenter(objX, objY, control.Size.Width, control.Size.Height);
            control.Left = hitboxCorner.Item1;
            control.Top = hitboxCorner.Item2;
        }

        void assignValues(PictureBox sprite, int posX, int posY, Size size, Image imageWithCorrectMeasures, string name)
        {
            sprite.Size = size;
            sprite.Image = imageWithCorrectMeasures;
            sprite.Name = name;
            setSpritePosition(sprite, posX, posY);
        }

        void SpawnSingleObject(FO flyingObject, Image imgForSprite, string pictureBoxName)
        {
            var size = new Size((int)flyingObject.Width, (int)flyingObject.Hight);
            var img = resizeImage(imgForSprite, size);
            assignValues(flyingObject.sprite, (int)flyingObject.x, (int)flyingObject.y, size, img, pictureBoxName);
            flyingObject.sprite.BringToFront();
            Controls.Add(flyingObject.sprite);
        }

        void SpawnSetOfObjects(FO[,] flyingObjects, Image imgForSprite, string pictureBoxName)
        {
            var size = new Size((int)flyingObjects[0, 0].Width, (int)flyingObjects[0, 0].Hight);
            var img = resizeImage(imgForSprite, size);
            foreach (FO obj in flyingObjects)
            { 
                assignValues(obj.sprite, (int)obj.x, (int)obj.y, size, img, pictureBoxName);
                Controls.Add(obj.sprite);
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            invadersEngine.FrameCalcs(sender, e);
            Render();
        }

        private void Render()
        {
            RenderSprites();
            RenderBullets();
        }

        private void RenderSprites()
        {
            setSpritePosition(invadersEngine.gracz.sprite, (int)invadersEngine.gracz.x, (int)invadersEngine.gracz.y);
            foreach(FO invader in invadersEngine.Invaders)
            {
                if (invader.alive)
                {
                    setSpritePosition(invader.sprite, (int)invader.x, (int)invader.y);
                }
                else
                {
                    invader.sprite.Hide();
                }
            }
        }

        private void RenderBullets()
        {
            foreach(FO bullet in invadersEngine.playerBullets)
            {
                switch(bullet.sprite.Name)
                {
                    case "toDraw":
                        SpawnSingleObject(bullet, playerBulletImage, "alive");
                        break;
                    case "alive":
                        setSpritePosition(bullet.sprite, (int)bullet.x, (int)bullet.y);
                        break;
                }
            }
            
        }


        public Form1()
        {
            Self = this;

            InitializeComponent();

            //okno przyjmuje wielkosc planszy
            Tuple<int, int> windowSize = invadersEngine.GetSize();
            Width = windowSize.Item1;
            Height = windowSize.Item2;


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            playerSprite = new PictureBox();
            enemiesSprites = new List<PictureBox>();
            playerBulletsGraphics = new List<PictureBox>();
            enemyBulletsGraphics = new List<PictureBox>();

            playerImage = Image.FromFile(pathToPlayerImage);
            enemyImage = Image.FromFile(pathToEnemyImage);
            playerBulletImage = Image.FromFile(pathToPlayerBulletImage);
            enemyBulletImage = Image.FromFile(pathToEnemyBulletImage);

            //sprite'y są tworzone i pokazywane na ekranie
            SpawnSingleObject(invadersEngine.gracz, playerImage, "player");
            SpawnSetOfObjects(invadersEngine.Invaders, enemyImage, "aliveEnemy");
            gameTimer.Interval = convertFPStoMsPerFrame(FPS);
        }

        private int convertFPStoMsPerFrame(int fps)
        {
            int ms = (int)Math.Round(1000 / (double)fps);
            return ms;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }


    }
}
