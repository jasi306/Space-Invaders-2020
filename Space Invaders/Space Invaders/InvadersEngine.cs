using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//using System.Timers;

using System.Windows.Input; //obsluga klawatury 

using System.Windows.Forms;  // tylko MessageBox
using System.Drawing;

namespace Space_Invaders
{
    public enum MoveDirection : int
    {
        Left = -1,
        Right = 1
    };

    class FO
    {

        public string debbugMessage;

        public PictureBox sprite;

        public float x, y;  // private --------------debbug-----------------
        public bool alive;
        private float width, hight;
        public float Width
        {
            get
            {
                return width;
            }
            private set
            {
                width = value;
            }
        }
        public float Hight
        {
            get
            {
                return hight;
            }
            private set
            {
                hight = value;
            }
        }
        public FO(float x, float y,float width,float hight)
        {
            this.width = width;
            this.hight = hight;
            this.x = x;
            this.y = y;
            alive = true;
            sprite = new PictureBox();
        }

        public void move(float x, float y)
        {
            this.x += x;
            this.y += y;
        }

        public bool colisionWith(FO fo)
        {
            if (fo.x + fo.width  > x + width  &&
                fo.x - fo.width  < x - width )
            {

                if (fo.y + fo.hight  > y + hight &&
                    fo.y - fo.hight  < y - hight )
                {
                    return true;
                }
            }
            return false;
        }
    }
    class Inveider : FO
    {
        int type;
        int points;
        public Inveider(float x, float y, float width, float hight,int type) : base(x, y, width, hight)
        {
            this.type = type;
            switch (type)
            {
                case 0:
                    points = 100;
                    break;
                case 1:
                    points = 200;
                    break;
                case 2:
                    points = 300;
                    break;
                default:
                    MessageBox.Show("Error!");  //---------<<<<<<< niefachowo?
                    break;
            }
        }
    }
    class Bullet : FO
    {
        int hp;
        public Bullet(float x, float y, float width, float hight, int hp) : base(x, y, width, hight)
        {
            this.hp = hp;
        }
    }

    class Shield : FO
    {
        
        int cols = 26;
        int rows = 22;
        public FO[,] elements;
        public Shield(float x,float y,float width,float hight) : base(x,y,width,hight)
        {
           // this.width = width;
            this.x = x;
            this.y = y;
            elements = new FO[cols, rows];
            for (int i = 0; i < rows; ++i) 
            {
                for (int j = 0; j < cols; ++j)
                {
                    elements[j,i] = new FO(j + width * (j-7), i + hight * (i-7), width, hight);
                    //for 11x13
                    /* if (x + y < 2) elements[x, y].alive = false;  //lewy góry róg.
                     if ((cols - x - 1) + y < 2) elements[x, y].alive = false;  //prawy góry róg.

                     if (Math.Abs(cols/2 - x) + (rows-y-1) < 6 && (rows-y - 1) < 3 && Math.Abs(cols / 2 - x) < 5) elements[x, y].alive = false;  //srodek
                 */
                 //for 22x26
                    if (j + i < 4) elements[j, i].alive = false;  //lewy góry róg.
                    if ((cols - j - 1) + i < 4) elements[j, i].alive = false;  //prawy góry róg.

                    if (Math.Abs(cols / 2 - j) + (rows - i - 1) < 12 && (rows - i - 1) < 6 && Math.Abs(cols / 2 - j) < 10) elements[j, i].alive = false;  //srodek

                }
            }
            //destroy(10, 0);
            //print_message(); //test
        }

        public void print_message()  //<>debbug only
        {
            string outp = "";
            for (int y = 0; y < rows; ++y)
            {
                for (int x = 0; x < cols; ++x)
                {
                    if(elements[x, y].alive)
                        outp += "1";
                    else
                        outp += "0";
                }
                outp += "\n";
            }
            MessageBox.Show(outp);
        }

        public void destroy(int x, int y,int power = 6) //na pewno niszczy wskazany element. Eksplozja roznosi sie po okolicznych elementach
        {
            if (x < 0 || y < 0 || x > cols - 1 || y > rows - 1) return;  //wyjscie jesli zle wspolrzedne
            if (elements[x, y].alive == false) return;// wyjscie jesli juz martwy, martwe nie przewodza eksplozji

            Random r;
            r = new Random();

            elements[x, y].alive = false;
            for (int i = -1; i <= 1; ++i) for (int j = -1; j <= 1; ++j)
                    if (r.Next() % 6+power> 4)  destroy(x+i, y+j, power - 3 + j ); //Do zabawy z wartosciami
        }                                                                          //malo satysfakcjonujacy efekt
    };

    class InvadersEngine
    {
        //DEBBUG
        public bool IfLastWasDebbugMessage;
        //DEBBUG

        //private static System.Timers.Timer MainLoop;
        private static Timer MainLoop;

        Shield[] shield;

        public FO gracz;
        public List<FO> enamyBullets;
        public List<FO> playerBullets;

        
        //consts
        private const float UFOsRenderBottom = 0.6f;
        private const float UFOsStartXOffset = 0.2f; //how far UFO's will be from right side in start.

        private const float ShildsRenderLine = 0.2f;
        private const float PlayerRenderLine = 0.12f;

        private const float shieldScale = 0.01f;

        private const float moveConst = 0.02f;

        private const int cooldown = 20;
        //
        private const float BulletSpeed=5.5f;

        private float moveConstInPxX;
        private float moveConstInPxY;
        //

        public int PlayerPoints;
        private int aliveCount;
        private int lastMoved;

        public const int bulletWidth = 10;
        public const int bulletHeight = 10;

        //int moveDirection; //1 right, -1 left
        MoveDirection moveDirection;

        private int width, hight;
        private int uFOcols, uFOrows;
        public int UFOcols
        {
            get => uFOcols;
            private set
            {
                uFOcols = value;
            }
        }
        public int UFOrows
        {
            get => uFOrows;
            private set
            {
                uFOrows = value;
            }
        }
        public FO[,] Invaders;

        private long timeOfGame;    //liczy klatki
        private long timeOfLastShot;//zapisuje czas ostatniego strzalu do sprawdzenia cooldowna

       

        public InvadersEngine(int width, int hight, int UFOcols, int UFOrows)
        {
            //DEBBUG
            IfLastWasDebbugMessage = false;
            //DEBBUG


            PlayerPoints = 0;
            timeOfGame = 0;
            timeOfLastShot = -10000;

            this.width = width;
            this.hight = hight;
            this.UFOcols = UFOcols;
            this.UFOrows = UFOrows;

            aliveCount = UFOrows * UFOcols;

            moveConstInPxX = moveConst * width;
            moveConstInPxY = moveConst * hight;

            lastMoved = -1;
            moveDirection = MoveDirection.Right;

            //Gracz
            //-------------
            gracz = new FO(width / 2, hight * PlayerRenderLine,30,12);
            //Tarcze
            //-------------
            shield = new Shield[4];
            for (int i = 0; i < 4; ++i)
                shield[i] = new Shield(i * width / 4 + width / 2, ShildsRenderLine, shieldScale * width, shieldScale * width); //zostaje przy kwadaratach
            //UFO
            //-------------
            Invaders = new Inveider[UFOcols, UFOrows];

            float tempXstep = (width - (UFOsStartXOffset * width)) / UFOcols;
            float tempYstep = (hight - (UFOsRenderBottom * hight)) / UFOrows;

            for (int x=0; x < UFOcols; ++x)
                for (int y=0; y < UFOrows; ++y)
                {
                    Invaders[x, y] = new Inveider((x + 0.5f) * tempXstep, hight - ((y + 0.5f) * tempYstep), 30, 20, (y>1)?2:(y>2)?1:0  );   //!!!!!!!!!!poprawic warunki!!!!!!!!!!!
                    Invaders[x, y].debbugMessage = x.ToString()+" " + y.ToString();
                    //!!!!!!!!!!!!! poprawic !!!!!!!!!!!!!!
                }

            //-------------
            //Pociski
            enamyBullets = new List<FO>();
            playerBullets = new List<FO>();

            /*
            MainLoop = new Timer();
            MainLoop.Tick += new EventHandler(FrameCalcs);
            MainLoop.Interval = 1000 / 60;
            MainLoop.Start();
            */

            /*
            MainLoop = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            MainLoop.Elapsed += OnTimedEvent;
            MainLoop.AutoReset = true;
            MainLoop.Enabled = true;*/
        }

        public Tuple<int,int> GetSize() //funkcja zwracajaca wymiary planszy
        {
            var boardSize=new Tuple<int,int>(this.width,this.hight);
            return boardSize;
        }

        public void FrameCalcs(Object myObject, EventArgs myEventArgs) //arg wymagane dla EventHandler
        {
            timeOfGame++;
            //MessageBox.Show("1 "+timeOfGame.ToString());
            MoveNextUfo();
            //MessageBox.Show("2 "+timeOfGame.ToString());


            keyboard();
            //MessageBox.Show("3 "+timeOfGame.ToString());
            animateBullets();
            //MessageBox.Show("4 "+timeOfGame.ToString());
        }

        private void keyboard()
        {
            if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))
            {
                gracz.x += -4;
            }
            if (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right))
            {
                gracz.x += 4;
            }
            if ((Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.Space)) && timeOfGame - timeOfLastShot > cooldown)
            {
                FirePlayer();
            }

           
            if (Keyboard.IsKeyDown(Key.Home))
            {
                bool _temp = IfLastWasDebbugMessage;
                IfLastWasDebbugMessage = true;

                if (!_temp)
                {
                    //pozycja gracza.
                    //MessageBox.Show("x=" + gracz.x.ToString() + ", y=" + gracz.y.ToString() );
                    //cooldown
                   MessageBox.Show("Czas gry: " + timeOfGame.ToString() + " Czas ostatniego wystrzalu:" + timeOfLastShot.ToString() + " ilosc pociskow: " + playerBullets.Count);
                }
            }
            else
            {
                IfLastWasDebbugMessage = false;
            }
        }

        void UfoTryToAttack()
        {
            Random r = new Random();
            if (r.Next() % 200 == 13)
            {//TRY!
                int rand = r.Next() % UFOcols;  //TO DO TO DO TO DO

            }
            
        }

        public void FirePlayer()
        {
            FO bullet = new FO(gracz.x, gracz.y + 10, bulletWidth, bulletHeight);
            bullet.sprite.Name = "toDraw";
            playerBullets.Add(bullet); //<><><><><><><>< temp values

            timeOfLastShot = timeOfGame;
        }

        private void animateBullets()
        {
            //////////////////////////////////////////////
            playerBullets.ForEach(delegate (FO Bullet)
            {
                Bullet.move(0, BulletSpeed);
                if (Bullet.y > hight + Bullet.Hight) Bullet.alive = false;
                for (int i = 0; i < UFOcols; ++i) for (int j = 0; j < UFOrows; ++j)
                        if (Invaders[i, j].alive)
                        {
                            if (Bullet.colisionWith(Invaders[i, j]))
                            {
                                //MessageBox.Show(Invaders[i, j].debbugMessage);
                                Invaders[i, j].alive = false;
                                Bullet.alive = false;
                                Form1.Self.Controls.Remove(Bullet.sprite);
                            }
                        }
            });
            playerBullets.RemoveAll(notAlive);
            //////////////////////////////////////////////
            enamyBullets.ForEach(delegate (FO Bullet)
            {
                Bullet.move(0, BulletSpeed);
                if (Bullet.y < 0- Bullet.Hight) Bullet.alive = false;
                for (int i = 0; i < UFOcols; ++i) for (int j = 0; j < UFOcols; ++j)
                        if (Bullet.colisionWith(gracz))
                        {
                            Invaders[i, j].alive = false;
                            Bullet.alive = false;
                        }
            });
            enamyBullets.RemoveAll(notAlive);
            //////////////////////////////////////////////
        }


        private void MoveNextUfo()
        {
            int x, y;
            do {
                //przesun
                lastMoved++;

                //czy juz ostatnie?
                if (lastMoved >= UFOrows * UFOcols )
                {
                    if (ifUfoMustTurn())//useless
                    {
                        moveDirection = (MoveDirection.Left == moveDirection) ? MoveDirection.Right : MoveDirection.Left;
                        MoveUfoDown();
                    }
                    lastMoved = 0;
                }


                x = lastMoved / UFOcols;
                y = ((lastMoved-x* UFOcols) % UFOrows);

               // MessageBox.Show("x = " + x.ToString() + " y = " + y.ToString() + " LM = " + lastMoved.ToString());
            } while (!Invaders[UFOcols - y-1, UFOrows -1 -x].alive);

            Invaders[UFOcols - y-1, UFOrows -1 - x].move((moveDirection == MoveDirection.Right) ? moveConstInPxX : -moveConstInPxX , 0);
        }

        private bool notAlive(FO fo)
        {
            return !fo.alive;
        }


        private void MoveUfoDown()
        {
            for (int x = 0; x < UFOcols; ++x) //kolumna
                for (int y = 0; y < UFOrows; ++y) //wiersz
                    Invaders[x, y].move(0, -moveConstInPxY);
        }

        private bool ifUfoMustTurn()
        {
            int x=0, y=0;

            if (moveDirection == MoveDirection.Left)
                for ( x = 0; x < UFOcols; ++x) //kolumna
                    for (y = 0; y < UFOrows; ++y) //wiersz
                    {
                        if (Invaders[x, y].alive) break;
                    }
            else
                for ( x = UFOcols-1; x >= 0 ; --x) //kolumna
                    for ( y = 0; y < UFOrows; ++y) //wiersz
                    {
                        if (Invaders[x, y].alive) break;
                    }
            //<><><><> TODO
            x = UFOcols-1 ;y = 0;
            if(  Invaders[x, y].x + Invaders[x, y].Width > width  || Invaders[x, y].x - Invaders[x, y].Width < 0)
                return true;
            x = 0; y = 0;
            if (Invaders[x, y].x + Invaders[x, y].Width > width || Invaders[x, y].x - Invaders[x, y].Width < 0)
                return true;

            return false;
        }
    }
}