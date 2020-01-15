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
        readonly InvadersEngine invadersEngine = new InvadersEngine(640, 480, 5, 6);

        Image playerImage;
        Image[,] enemyImages;
        Image playerBulletImage;
        Image enemyBulletImage;

        readonly string pathToPlayerImage = "..\\Images\\cannon.png";
        readonly string[,] pathToEnemyImage = { { "..\\Images\\enemy1_frame1.png", "..\\Images\\enemy1_frame2.png" }, { "..\\Images\\enemy2_frame1.png", "..\\Images\\enemy2_frame2.png" }, { "..\\Images\\enemy3_frame1.png", "..\\Images\\enemy3_frame2.png" } };
        const int typesCount = 3;
        const int animationFramesCount = 2;


        readonly string pathToPlayerBulletImage = "..\\Images\\cannon.png";
        readonly string pathToEnemyBulletImage = "..\\Images\\cannon.png";

        public static Image ResizeImage(Image imgToResize, Size size)
        {
            return (Image)new Bitmap(imgToResize, size);
        }

        Tuple<int, int> HitboxCenter(int x, int y, int width, int height)
        {
            //funckja, dzieki ktorej x i y znajduja sie na srodku sprite'u a nie w jego lewym gornym rogu
            var hitbox = new Tuple<int, int>(x - width / 2, Height - y - height); //odejmowanie od wartosci wysokosci, aby zachowac os y skierowana w gore
            return hitbox;
        }
        void SetSpritePosition(Control control, int objX, int objY)
        {
            Tuple<int, int> hitboxCorner = HitboxCenter(objX, objY, control.Size.Width, control.Size.Height);
            control.Left = hitboxCorner.Item1;
            control.Top = hitboxCorner.Item2;
        }

        void AssignValues(PictureBox sprite, int posX, int posY, Size size, Image imageWithCorrectMeasures, string name)
        {
            sprite.Size = size;
            sprite.Image = imageWithCorrectMeasures;
            sprite.Name = name;
            SetSpritePosition(sprite, posX, posY);
        }

        void SpawnSingleObject(FO flyingObject, Image imgForSprite, string pictureBoxName)
        {
            var size = new Size((int)flyingObject.Width, (int)flyingObject.Hight);
            AssignValues(flyingObject.sprite, (int)flyingObject.x, (int)flyingObject.y, size, imgForSprite, pictureBoxName);
            flyingObject.sprite.BringToFront();
            Controls.Add(flyingObject.sprite);
        }

        void SpawnEnemies(Inveider[,] invaders, string pictureBoxName)
        {
            var size = new Size((int)invaders[0, 0].Width, (int)invaders[0, 0].Hight);
            foreach (Inveider invader in invaders)
            {
                Image img = enemyImages[invader.type, 0];
                AssignValues(invader.sprite, (int)invader.x, (int)invader.y, size, img, pictureBoxName);
                Controls.Add(invader.sprite);
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //glowna petla aplikacji
            invadersEngine.FrameCalcs(sender, e);
            if (!invadersEngine.gracz.alive)
            {
                gameTimer.Enabled = false;
                MessageBox.Show("przegrana");
                //invadersEngine.reset();
                //SpawnSingleObject(invadersEngine.gracz, playerImage, "player");
                //SpawnEnemies(invadersEngine.Invaders, "aliveEnemy");
            }
            if (invadersEngine.AliveCount==0)
            {
                gameTimer.Enabled = false;
                MessageBox.Show("wygrana");
            }

            Render();
        }

        private void Render()
        {
            RenderSprites();
            RenderBullets();
        }

        private void RenderSprites()
        {
            SetSpritePosition(invadersEngine.gracz.sprite, (int)invadersEngine.gracz.x, (int)invadersEngine.gracz.y);
            foreach (Inveider invader in invadersEngine.Invaders)
            {
                if (invader.alive)
                {
                    //animowanie przeciwnikow (dziala ale spowalnia gre, mozna odkomentowac)
                    //invader.sprite.Image = enemyImages[invader.type, (invadersEngine.TimeOfGame / (invadersEngine.AliveCount)) % 2];
                    
                    SetSpritePosition(invader.sprite, (int)invader.x, (int)invader.y);
                }
                else
                {
                    //przeciwnicy znikaja po smierci tylko raz
                    if (invader.sprite.Visible)
                    {
                        invader.sprite.Hide();
                    }
                }
            }
        }

        private void RenderSingleBullet(FO bullet, Image bulletImage)
        {
            switch (bullet.sprite.Name)
            {
                //bullet tylko raz bedzie wolany na ekran, pozniej tylko przesuwany
                case "toDraw":
                    SpawnSingleObject(bullet, bulletImage, "alive");
                    break;
                case "alive":
                    SetSpritePosition(bullet.sprite, (int)bullet.x, (int)bullet.y);
                    break;
            }
        }

        private void RenderBullets()
        {
            foreach (FO bullet in invadersEngine.playerBullets)
            {
                RenderSingleBullet(bullet, playerBulletImage);
            }
            foreach (FO bullet in invadersEngine.enamyBullets)
            {
                RenderSingleBullet(bullet, enemyBulletImage);
            }

        }


        public Form1()
        {
            Self = this; //troche brzydki trick, aby Form1 byl dostepny z innych klas

            InitializeComponent();

            //okno przyjmuje wielkosc planszy
            Tuple<int, int> windowSize = invadersEngine.GetSize();
            Width = windowSize.Item1;
            Height = windowSize.Item2;

            enemyImages = new Image[typesCount, animationFramesCount];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var playerSize = new Size((int)invadersEngine.gracz.Width, (int)invadersEngine.gracz.Hight);
            var enemySize = new Size((int)invadersEngine.Invaders[0, 0].Width, (int)invadersEngine.Invaders[0, 0].Hight);
            var bulletSize = new Size(invadersEngine.bulletWidth, invadersEngine.bulletHeight);

            //przeskalowanie tekstur aby zgadzaly sie z faktycznymi wymiarami przeciwnikow
            playerImage = ResizeImage(Image.FromFile(pathToPlayerImage), playerSize);
            playerBulletImage = ResizeImage(Image.FromFile(pathToPlayerBulletImage), bulletSize);
            enemyBulletImage = ResizeImage(Image.FromFile(pathToPlayerBulletImage), bulletSize);
            enemyBulletImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            for (int i = 0; i < typesCount; i++)
            {
                for (int j = 0; j < animationFramesCount; j++)
                {
                    enemyImages[i, j] = ResizeImage(Image.FromFile(pathToEnemyImage[i, j]), enemySize);
                }
            }

            //sprite'y są tworzone i pokazywane na ekranie
            SpawnSingleObject(invadersEngine.gracz, playerImage, "player");
            SpawnEnemies(invadersEngine.Invaders, "aliveEnemy");
            
            gameTimer.Interval = ConvertFPStoMsPerFrame(FPS);
        }

        private int ConvertFPStoMsPerFrame(int fps)
        {
            int ms = (int)Math.Round(1000 / (double)fps );
            return ms;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }


    }
}
