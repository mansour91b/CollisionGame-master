using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollisionGame
{
    enum SpriteToDraw {
        CrapFace = 1,
        Player = 2,
        Fireball = 3,
        PlayerUp =4,
        PlayerDown,
        PlayerRight,
        PlayerLeft,
    }


    // abstract class is just a structure class and it has to be implemented in the other class that is inheareted from
    // abstract class inforce the coder to implement whenever they are inheareted from this class
    // abstract class can not be instaintiated example: var object = new GameObject is illegal and can't be compiled.
    
    abstract class GameObject
    {
        public GameObject(GameObject Source)
        {
            this.Source = Source;
        }
        public abstract bool CanCollide(GameObject other);
        
        // properties of the gameobject
        public float x { get; set; }
        public GameObject Source { get; private set; }
        public float y { get; set; }
        public abstract void Tick(float currentTime, float dt, GameLogic.AddNewObjectDelegate fnAddNew);
        public abstract SpriteToDraw GetSprite();
        
    }
    class ObstacleObject : GameObject
    {

        public override bool CanCollide(GameObject other)
        {
            return true;
        }
        static Random r = new Random();
        public ObstacleObject(GameObject source) : base(source)
        {
            // Starting position on the x axiss
            x = (float)(r.NextDouble() * 500);
            // Starting position on the y axiss
            y = (float)(r.NextDouble() * 500);
        }
        public override void Tick(float currentTime, float dt, GameLogic.AddNewObjectDelegate fnAddNew)
        {

        }
        public override SpriteToDraw GetSprite()
        {
            return SpriteToDraw.CrapFace;
        }
    }
    // playerobject class inherited from gameobject
    class PlayerObject : GameObject
    {
        public PlayerObject():base(null)
        {

        }
        public override bool CanCollide(GameObject other)
        {
            if(this == other.Source)
            {
                return false;
            }

            return true;
        }

        // task 1... fireball player
        
        



        // player speed
        private float playerspeed = 75;
        // implementing the overrde Tick function from the GameObject class
        public override void Tick(float currentTime, float dt, GameLogic.AddNewObjectDelegate fnAddNew)
        {
            if (this.left)
            {
                x -= dt * playerspeed;
            }
            if (this.right)
            {
                x += dt * playerspeed;
            }
            if (this.up)
            {
                y -= dt * playerspeed;
            }
            if (this.down)
            {
                y += dt * playerspeed;
            }


           
            // tasl 1)
            if (this.space)
            {
                Fireball newfireball = new Fireball(this.x, this.y, this);
                
                fnAddNew(newfireball);
                
            }




        }
        public override SpriteToDraw GetSprite()
        {

            if(left)
            {
                return SpriteToDraw.PlayerLeft;
            }
            if(right)
            {
                return SpriteToDraw.PlayerRight;
            }
            if(up)
            {
                return SpriteToDraw.PlayerUp;

            }
            if(down)
            {
                return SpriteToDraw.PlayerDown;
            }
            return SpriteToDraw.Player;
        }
        public bool left { get; set; }
        public bool right { get; set; }
        public bool up { get; set; }
        public bool down { get; set; }



        // task 1... adding property for the space
        public bool space { get; set; }



    }
    class Enemy : ObstacleObject
    {
        private float lastfireballtime=0;

        private int enamyspeed = 50;
        private GameObject target;
        public Enemy(GameObject target, GameObject source):base(source)
        {
            this.target = target;
        }
        // overriding Tick to change the implementation from the inhireted from GameObject Class
        public override void Tick(float currentTime, float dt, GameLogic.AddNewObjectDelegate fnAddNew)
        {
            float deltaX = this.x - target.x;
            float deltaY = this.y - target.y;
            if (deltaX > 0.5)
            {
                x -= dt * enamyspeed;
            }
            else if (deltaX < -0.5)
            {
                x += dt * enamyspeed;
            }
            else if (deltaY > 0.5)
            {
                y -= dt * enamyspeed;
            }
            else if (deltaY < -0.5)
            {
                y += dt * enamyspeed;
            }
            float lasttime;
            lasttime = currentTime - lastfireballtime;
            if (lasttime>1)
            {
                fnAddNew(new Fireball(this.x, this.y, this));
                lastfireballtime = currentTime;
            }
            
        }

    }
    
    //class Fireball_player: GameObject
    //{

    //}
    class Fireball: GameObject
    {

        public override bool CanCollide(GameObject other)
        {
            return true;
        }
        
        public Fireball(float x, float y, GameObject source):base(source)
        {
            this.x = x;
            this.y = y;
        }
        public override void Tick(float currentTime, float dt, GameLogic.AddNewObjectDelegate fnAddNew)
        {
            int fireBallSpeed;
            fireBallSpeed = 100;

            x += dt * fireBallSpeed;
        }
        public override SpriteToDraw GetSprite()
        {
            return SpriteToDraw.Fireball;
        }
    }
    abstract class LevelOfGame
    {
        private float whenDidIStart;
        public LevelOfGame(float timeOfStart)
        {
            whenDidIStart = timeOfStart;
        }
        public abstract bool isLevelDone(float CurrentTime, int enemiesCount);
        public abstract PlayerObject setupLevel(GameLogic.AddNewObjectDelegate addNew);
       

        // helper functions for peeps that inherit from us:
        protected float GetTimeInLevel(float currentTime)
        {
            return currentTime - whenDidIStart;
        }
    }

    // Use LevelN to do your level 1 and level 2

    class levelN : LevelOfGame
    {
        
        //The Max Levels of N
        int numberoflevels = 10;
        
        public levelN(int howManyLevels, float timeOfStart) : base(timeOfStart) {
            this.numberoflevels = howManyLevels;
        }

        public override bool isLevelDone(float CurrentTime, int enemiesCount)
        {

            if (GetTimeInLevel(CurrentTime) > 10)
            {
                return true;
            }
            return false;
        }

        public override PlayerObject setupLevel(GameLogic.AddNewObjectDelegate addNew)
        {
            
            
                PlayerObject player = new PlayerObject();



            for (int i = 0; i < numberoflevels; i++)
            {
                addNew(new Enemy(player, null));

            }

                return player;

           
          
            
            
            
                
        }

        
    }
    /*
    class Level1 : LevelOfGame
    {
        public Level1(float timeOfStart) : base(timeOfStart) { }

        public override bool isLevelDone(float CurrentTime, int enemiesCount)
        {
            if(GetTimeInLevel(CurrentTime)>10)
            {
                return true;
            }
            return false;
        }
        public override PlayerObject setupLevel(GameLogic.AddNewObjectDelegate addNew)
        {
            PlayerObject player = new PlayerObject();

            addNew(new Enemy(player, null));

            return player;
        }

    }

    class Level2 : LevelOfGame
    {
        public Level2(float timeOfStart) : base(timeOfStart) { }

        public override bool isLevelDone(float CurrentTime, int enemiesCount)
        {
            if(GetTimeInLevel(CurrentTime) >10)
            {
                return true;
            }
            return false;
        }
        public override PlayerObject setupLevel(GameLogic.AddNewObjectDelegate addNew)
        {
            PlayerObject player = new PlayerObject();

            addNew(new Enemy(player, null));
            addNew(new Enemy(player, null));

            return player;
        }

    }
    */
    class GameLogic
    {
        int levelsCompleted = 0;
        public delegate void AddNewObjectDelegate(GameObject o);


        public List<GameObject> all { get; private set; }
        public PlayerObject player { get; private set; }
        public bool isDead { get; private set; }
        public float playerScore { get; private set; }
        private LevelOfGame currentlevel; 
        // constructor
        public GameLogic()
        {
            // score starts at zero
            playerScore = 0;
            // initilizing isDead to false at the start of the Game
            isDead = false;
            // creating all to a list for the enemy
            // intilizied to empty list and not null object
            all = new List<GameObject>();

            //currentlevel = new Level1(0);

            // initilizing the start of the level
            currentlevel = new levelN(levelsCompleted+1, 0);

            player = currentlevel.setupLevel(this.AddNewObject);
            all.Remove(player);
            all.Add(player);


        }

        private void AddNewObject(GameObject o)
        {
            // this is the implementation of the GameLogic.AddNewObjectDelegate delegate.
            // this gets called from various Tick() functions when a GameObject wants to add another GameObject to the playing field
            all.Add(o);
        }

        public void tick(float t, float dt)
        {
            playerScore = (float)Math.Pow(t, 2);
            // check for collisions between all our objects and the player
            collide();

            if(this.currentlevel.isLevelDone(t, 0))
            {

                all = new List<GameObject>();

                // currentlevel = new Level2(t);
                // not too sure!

                levelsCompleted++;
                currentlevel = new levelN((levelsCompleted + 1) * 5, t);
                



                player = currentlevel.setupLevel(this.AddNewObject);
                all.Remove(player);
                all.Add(player);

                return;
            }

            // go through every single game object and get them to update their position/logic/etc
            for (int x = 0; x < all.Count; x++)
            {
                all[x].Tick(t, dt, AddNewObject);
            }
        }
        // collide funtion to calculate the collition 
        private void collide()
        {

            for (int i = 0; i < all.Count; i++)
            {
                GameObject go = all[i];

                if(!player.CanCollide(go))
                {
                    continue;
                }
                if (go == player)
                {
                    continue;
                }
                float dx;
                float dy;
                float r;
                dx = this.player.x - go.x;
                dy = this.player.y - go.y;
                r = (float)Math.Sqrt((dx * dx) + (dy * dy));

                if (r < 10)
                {
                    isDead = true;
                    Console.WriteLine(" You lost!");

                }
            }

    
           
        }
    }
}
