﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System.Timers;

using System.Windows.Input; //obsluga klawatury 

using System.Windows.Forms;  // tylko MessageBox

namespace Space_Invaders
{
    public enum MoveDirection : int
    {
        Left = -1,
        Right = 1
    };

    class FO
    {
        

        public float x, y;  // private --------------debbug-----------------
        public bool alive;
        private float width, hight;
        public FO(float x, float y,float width,float hight)
        {
            this.width = width;
            this.hight = hight;
            this.x = x;
            this.y = y;
            alive = true;
        }

        public void move(float x, float y)
        {
            this.x += x;
            this.y += y;
        }

        public bool colisionWith(FO fo)
        {
            if (fo.x + fo.width / 2 < this.x + this.width / 2 &&
                fo.x - fo.width / 2 < this.x - this.width / 2)
            {

                if (fo.y + fo.hight / 2 < this.y + this.hight / 2 &&
                    fo.y - fo.hight / 2 < this.y - this.hight / 2)
                {
                    return true;
                }
            }
            return false;
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

        public void destroy(int x, int y,int power = 6) 
        {
            if (x < 0 || y < 0 || x > cols - 1 || y > rows - 1) return;
            if (elements[x, y].alive == false) return;


            Random r;
            r = new Random();
           // r.Next();

            elements[x, y].alive = false;
            for (int i = -1; i <= 1; ++i) for (int j = -1; j <= 1; ++j)
                    if (r.Next() % 6+power> 4)  destroy(x+i, y+j, power - 3 + j ); //Do zabawy
        }
    };

    class InvadersEngine
    {
        //private static System.Timers.Timer MainLoop;
        private static Timer MainLoop;

        Shield[] shield;

        FO gracz;
        List<FO> enamyBullets;
        List<FO> playerBullets;

        //consts
        private const float UFOsRenderBottom = 0.4f;
        private const float UFOsStartXOffset = 0.1f; //how far UFO's will be from right side in start.

        private const float ShildsRenderLine = 0.2f;
        private const float PlayerRenderLine = 0.1f;

        private const float shieldScale = 0.01f;

        private const float moveConst = 0.02f;

        private const int cooldown = 300;
        //
        private const float BulletSpeed=0.5f;

        private float moveConstInPxX;
        private float moveConstInPxY;
        //

        private int aliveCount;
        private int lastMoved;

        //int moveDirection; //1 right, -1 left
        MoveDirection moveDirection;
        
        private int width, hight, UFOcols, UFOrows;
        private FO[,] Invaders;

        private long timeOfGame;
        private long timeOfLastShot;

       

        public InvadersEngine(int width, int hight, int UFOcols, int UFOrows)
        {

           

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
            gracz = new FO(width / 2, hight * PlayerRenderLine,1,5);
            //Tarcze
            //-------------
            shield = new Shield[4];
            for (int i = 0; i < 4; ++i)
                shield[i] = new Shield(i * width / 4 + width / 2, ShildsRenderLine, shieldScale * width, shieldScale * width); //zostaje przy kwadaratach
            //UFO
            //-------------
            Invaders = new FO[UFOcols, UFOrows];

            float tempXstep = (hight - (UFOsRenderBottom * hight)) / UFOrows;
            float tempYstep = (width - (UFOsStartXOffset * width)) / UFOcols;

            for (int x=0; x < UFOcols; ++x)
                for (int y=0; y < UFOrows; ++y)
                    Invaders[x, y] = new FO((x + 0.5f) * tempXstep, (y + 0.5f) * tempYstep,1,1);//<>><<>><<>><<>><<>><<>><<>><
            //-------------
            //Pociski
            enamyBullets = new List<FO>();
            playerBullets = new List<FO>();

            MainLoop = new Timer();
            MainLoop.Tick += new EventHandler(FrameCalcs);
            MainLoop.Interval = 1000 / 60;
            MainLoop.Start();
            
            /*
            MainLoop = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            MainLoop.Elapsed += OnTimedEvent;
            MainLoop.AutoReset = true;
            MainLoop.Enabled = true;*/
        }

        void FrameCalcs(Object myObject, EventArgs myEventArgs) //arg wymagane dla EventHandler
        {
            timeOfGame++;
            MoveNextUfo();
            keyboard();
            animateBullets();
        }

        private void keyboard()
        {
            if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))
            {
                gracz.x += -1;
            }
            if (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right))
            {
                gracz.x += 1;
            }
            if ((Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.Space)) && timeOfGame - timeOfLastShot > cooldown)
            {
                FirePlayer();
            }
        }

        public void FirePlayer()
        {
            playerBullets.Add(new FO(gracz.x, gracz.y + 10, 10, 10)); //<><><><><><><>< temp values
            timeOfLastShot = timeOfGame;
        }

        private void animateBullets()
        {
            //////////////////////////////////////////////
            playerBullets.ForEach(delegate (FO Bullet)
            {
                Bullet.move(0, BulletSpeed);

                for (int i = 0; i < UFOcols; ++i) for (int j = 0; j < UFOcols; ++j)
                        if (Bullet.colisionWith(Invaders[i, j]))
                        {
                            Invaders[i, j].alive = false;
                            Bullet.alive = false;
                        }
            });
            playerBullets.RemoveAll(notAlive);
            //////////////////////////////////////////////
            enamyBullets.ForEach(delegate (FO Bullet)
            {
                Bullet.move(0, BulletSpeed);

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
                if (lastMoved > UFOrows * UFOcols)
                {
                    if (ifUfoMustTurn())
                    {
                        moveDirection = (MoveDirection.Left == moveDirection) ? MoveDirection.Right : MoveDirection.Left;
                        MoveUfoDown();
                    }
                }

                //policz wspolrzedne
                x = lastMoved / UFOcols;//sprawdz <<<<<<<<<<<<
                y = lastMoved % UFOrows;
                
            } while (Invaders[x,y].alive);
            Invaders[x, y].move((moveDirection == MoveDirection.Right) ? moveConstInPxX : -moveConstInPxX , 0);
        }

        private bool notAlive(FO fo)
        {
            return !fo.alive;
        }


        private void MoveUfoDown()
        {
            for (int x = 0; x < UFOcols; ++x) //kolumna
                for (int y = 0; y < UFOrows; ++y) //wiersz
                    Invaders[x, y].move(0, moveConstInPxY);
        }

        private bool ifUfoMustTurn()
        {
            if (moveDirection == MoveDirection.Left)
                for (int x = 0; x < UFOcols; ++x) //kolumna
                    for (int y = 0; y < UFOrows; ++y) //wiersz
                    {
                        if (Invaders[x, y].alive) break;
                    }
            else
                for (int x = UFOcols; x >= 0 ; --x) //kolumna
                    for (int y = 0; y < UFOrows; ++y) //wiersz
                    {
                        if (Invaders[x, y].alive) break;
                    }
            //<><><><> TODO
            return false;
        }
    }
}