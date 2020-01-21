﻿using System;
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

        const int w = 800;
        const int h = 600;

        const int FPS = 60;
        InvadersEngine invadersEngine;

        Image playerImage;
        Image[,] enemyImages;
        Image playerBulletImage;
        Image enemyBulletImage;
        Image shieldPieceImage;
        Image ufoImage;

        readonly string pathToPlayerImage = "..\\Images\\cannon.png";
        readonly string[,] pathToEnemyImage = { { "..\\Images\\enemy1_frame1.png", "..\\Images\\enemy1_frame2.png" }, { "..\\Images\\enemy2_frame1.png", "..\\Images\\enemy2_frame2.png" }, { "..\\Images\\enemy3_frame1.png", "..\\Images\\enemy3_frame2.png" } };
        readonly string pathToUfoImage = "..\\Images\\ufo.png";
        const int typesCount = 3;
        const int animationFramesCount = 2;

        readonly string pathToShieldImage = "..\\Images\\shield.png";
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

        void SpawnShields(Shield[] shields, string pictureBoxName)
        {
            var size = new Size((int)(invadersEngine.shield[0].elements[0, 0].Width), (int)(invadersEngine.shield[0].elements[0, 0].Hight));
            var img = shieldPieceImage;
            foreach (Shield shield in shields)
            {
                foreach (FO shieldPiece in shield.elements)
                {
                    if (shieldPiece.alive)
                    {
                        AssignValues(shieldPiece.sprite, (int)shieldPiece.x, (int)shieldPiece.y, size, img, pictureBoxName);
                        Controls.Add(shieldPiece.sprite);
                    }
                }
            }

        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //glowna petla aplikacji
            invadersEngine.FrameCalcs(sender, e);
            Render();

            if (!invadersEngine.player1.alive)
            {
                gameTimer.Enabled = false;
                MessageBox.Show("You lose :(\nScore: " + invadersEngine.PlayerPoints.ToString() + "Points\nRemaining enemies: " + invadersEngine.AliveCount.ToString());
                //invadersEngine.reset();
                //SpawnSingleObject(invadersEngine.gracz, playerImage, "player");
                //SpawnEnemies(invadersEngine.Invaders, "aliveEnemy");
            }
            if (invadersEngine.AliveCount == 0)
            {
                gameTimer.Enabled = false;
                MessageBox.Show("You won!\nScore: " + invadersEngine.PlayerPoints.ToString() + "Points");
            }
        }

        private void Render()
        {
            RenderSprites();
            RenderBullets();
            RenderShields();
            label1.Text = "Score: " + invadersEngine.PlayerPoints.ToString();
        }

        private void RenderShields()
        {
            foreach (Shield shield in invadersEngine.shield)
            {
                if (shield.ToUpdate)
                {
                    foreach (FO shieldPiece in shield.elements)
                    {
                        if (shieldPiece.alive == false && shieldPiece.sprite.Visible)
                        {
                            shieldPiece.sprite.Hide();
                        }
                    }
                    shield.ToUpdate = false;
                }
            }
        }

        private void RenderPlayerSprites()
        {
            SetSpritePosition(invadersEngine.player1.sprite, (int)invadersEngine.player1.x, (int)invadersEngine.player1.y);
            if (invadersEngine.TwoPlayersMode)
                SetSpritePosition(invadersEngine.player2.sprite, (int)invadersEngine.player2.x, (int)invadersEngine.player2.y);
        }

        private void RenderEnemiesSprites()
        {
            foreach (Inveider invader in invadersEngine.Invaders)
            {
                if (invader.alive)
                {
                    //animowanie przeciwnikow 
                    invader.sprite.Image = enemyImages[invader.type, invader.spireteNum];

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

        private void RenderSaucerSprite()
        {
            if (invadersEngine.SaucerAlive)
            {
                switch (invadersEngine.Saucer.sprite.Name)
                {
                    case "toDraw":
                        SpawnSingleObject(invadersEngine.Saucer, ufoImage, "alive");
                        break;
                    case "alive":
                        SetSpritePosition(invadersEngine.Saucer.sprite, (int)invadersEngine.Saucer.x, (int)invadersEngine.Saucer.y);
                        break;
                }
            }
        }
        private void RenderSprites()
        {
            RenderPlayerSprites();
            RenderEnemiesSprites();
            RenderSaucerSprite();
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

        private void alignButton(Button button, int top)
        {
            button.Width = logoBox.Width;
            button.Height = OnePlayerButton.Height;
            button.Left = logoBox.Left;
            button.Top = top;
        }
        public Form1()
        {
            Self = this; //troche brzydki trick, aby Form1 byl dostepny z innych klas

            InitializeComponent();
            //okno przyjmuje wielkosc planszy
            //Tuple<int, int> windowSize = invadersEngine.GetSize();
            Width = w;
            Height = h;

            logoBox.Image = Image.FromFile("..\\Images\\logo.png");
            logoBox.Left = Width / 2 - logoBox.Width / 2;

            alignButton(OnePlayerButton, logoBox.Top + logoBox.Height + 6);
            alignButton(TwoPlayersButton, OnePlayerButton.Top + OnePlayerButton.Height + 4);
            alignButton(ScoreButton, TwoPlayersButton.Top + OnePlayerButton.Height + 4);
            ScoreButton.Enabled = false;
            alignButton(ExitButton, ScoreButton.Top + OnePlayerButton.Height + 4);

            label1.Visible = false;
            gameTimer.Enabled = false;
            enemyImages = new Image[typesCount, animationFramesCount];
        }

        private void InitializeGame(bool twoPlayersMode)
        {
            invadersEngine = new InvadersEngine(w, h, 5, 5, twoPlayersMode);

            var playerSize = new Size((int)invadersEngine.player1.Width, (int)invadersEngine.player1.Hight);
            var enemySize = new Size((int)invadersEngine.Invaders[0, 0].Width, (int)invadersEngine.Invaders[0, 0].Hight);
            var bulletSize = new Size(invadersEngine.bulletWidth, invadersEngine.bulletHeight);
            var shieldPieceSize = new Size((int)(invadersEngine.shield[0].elements[0, 0].Width), (int)(invadersEngine.shield[0].elements[0, 0].Hight));
            var ufoSize = new Size((int)invadersEngine.saucerWidth, (int)invadersEngine.saucerHeight);

            //przeskalowanie tekstur aby zgadzaly sie z faktycznymi wymiarami przeciwnikow
            playerImage = ResizeImage(Image.FromFile(pathToPlayerImage), playerSize);
            playerBulletImage = ResizeImage(Image.FromFile(pathToPlayerBulletImage), bulletSize);
            enemyBulletImage = ResizeImage(Image.FromFile(pathToPlayerBulletImage), bulletSize);
            enemyBulletImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            shieldPieceImage = ResizeImage(Image.FromFile(pathToShieldImage), shieldPieceSize);
            ufoImage = ResizeImage(Image.FromFile(pathToUfoImage), ufoSize);

            for (int i = 0; i < typesCount; i++)
            {
                for (int j = 0; j < animationFramesCount; j++)
                {
                    enemyImages[i, j] = ResizeImage(Image.FromFile(pathToEnemyImage[i, j]), enemySize);
                }
            }
            //sprite'y są tworzone i pokazywane na ekranie
            SpawnSingleObject(invadersEngine.player1, playerImage, "player");
            if (invadersEngine.TwoPlayersMode)
                SpawnSingleObject(invadersEngine.player2, playerImage, "player");
            SpawnEnemies(invadersEngine.Invaders, "aliveEnemy");
            SpawnShields(invadersEngine.shield, "shieldPiece");

            gameTimer.Interval = ConvertFPStoMsPerFrame(FPS);
            label1.Text = "Score: " + invadersEngine.PlayerPoints.ToString();
            gameTimer.Enabled = true;
        }

        private void RemoveMenuComponents()
        {
            Controls.Remove(logoBox);
            Controls.Remove(OnePlayerButton);
            Controls.Remove(TwoPlayersButton);
            Controls.Remove(ScoreButton);
            Controls.Remove(ExitButton);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //InitializeGame();
        }

        private int ConvertFPStoMsPerFrame(int fps)
        {
            int ms = (int)Math.Round(1000 / (double)fps);
            return ms;
        }
        private void OnePlayerButton_Click(object sender, EventArgs e)
        {
            RemoveMenuComponents();
            InitializeGame(false);
        }

        private void TwoPlayersButton_Click(object sender, EventArgs e)
        {
            RemoveMenuComponents();
            InitializeGame(true);
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void logoBox_Click(object sender, EventArgs e)
        {

        }

    }
}
