using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;  
using System.Collections.Generic;



namespace MERV;

public class Game1 : Game


{

    //TEXTURES
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _pixel;

    /**
    private Texture2D mervTexture;
    private Texture2D backgroundTexture;
    private Texture2D floorTexture;
    **/
    //FONT
  //  SpriteFont font;
    SegmentManager map;
    //SCREEN DIMENSIONS
    public static int screenWidth = 960;
    public static int screenHeight = 540;

    //CHARACTER DIMS
    private int mervSize = 32;

    Rectangle merv;

    //GAME STATE VARS
    private bool paused = true;
    private float gameSpeed = 200f;
    float jetpackPower = 2500f; // Force pushing up
    float gravity = 1000f;



    //VARS set for scope
   // int score;
    private Vector2 mervPosition;
    private float _velocityY = 0f;
    private Random _rng = new Random();
    private float floorX = 0f;



    float pipeSpawnTimer = 0f;
    float pipeSpawnInterval = 2f;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = screenWidth;
        _graphics.PreferredBackBufferHeight = screenHeight;
        _graphics.ApplyChanges();

    }

    protected override void Initialize()
    {
        screenWidth = GraphicsDevice.Viewport.Width;
        screenHeight = GraphicsDevice.Viewport.Height;

        mervPosition = new Vector2(screenWidth / 3, screenHeight / 2);
        Random _rng = new Random();
        map = new SegmentManager(_rng,10);
       





        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
       

   
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(
            new[] { Color.White }
        );


        // TODO: use this.Content to load your game content here
    }




    /**
    void SpawnObstacles()
    {


        int RandChoice = _rng.Next(0,4);

        
            int spawnY = _rng.Next(50, screenHeight - 150);
            int spacing = _rng.Next(20,screenWidth);

            if(RandChoice == 0){
                obstacles.Add(new Zapper(
                    new Vector2(screenWidth+spacing,spawnY),
                    ZapperType.Horizontal,
                    200)
                );
            }
            else if(RandChoice == 1){
                obstacles.Add(new Zapper(
                    new Vector2(screenWidth+spacing,spawnY),
                    ZapperType.DiagonalRight,
                    100)
                );
            }
            else if(RandChoice == 2){
                obstacles.Add(new Zapper(
                    new Vector2(screenWidth+spacing,spawnY),
                    ZapperType.DiagonalLeft,
                    100)
                );
            }
            else{
                obstacles.Add(new Zapper(
                    new Vector2(screenWidth+spacing,spawnY),
                    ZapperType.Vertical,
                    100)
                );
            }
        

                
    }



    void updateObstacles(float dt ){

        for( int i = obstacles.Count -1 ; i>=0; i --  ){
            obstacles[i].Update(dt,gameSpeed);
        

            if (obstacles[i].Hitbox.Intersects(merv))
            {
                Console.WriteLine("GAME OVER");
                paused = true;
                return; 
                //resetGame();
            }

            // Remove if off screen
            if (!obstacles[i].IsActive)
            {
                obstacles.RemoveAt(i);
            }
            
      
        }
    }
    **/
    

    

    void resetGame(){
       // obstacles = new List<Obstacle>();
        mervPosition = new Vector2(screenWidth / 3, screenHeight / 2);
       // score = 0; 

    }



    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var keyboard = Keyboard.GetState();
        merv = new Rectangle((int)mervPosition.X, (int)mervPosition.Y, mervSize, mervSize);

        if (keyboard.IsKeyDown(Keys.Up))
        {
           _velocityY -= jetpackPower * dt;
            //paused = false;
        }

        /**
        //Spawn pipes
        if(!paused){
            pipeSpawnTimer += dt;
            if (pipeSpawnTimer >= pipeSpawnInterval)
            {
                SpawnObstacles();
                pipeSpawnTimer = 0f;

            }
            updateObstacles(dt);
        **/
            _velocityY += gravity * dt;

            // Cap the speed so you don't fly infinitely fast
            _velocityY = Math.Clamp(_velocityY, -600f, 600f);

            mervPosition.Y += _velocityY * dt;

     

            floorX -= gameSpeed * dt;

        
            if (floorX <= -screenWidth)
            {
                floorX = 0;
            }
            
      //  }
        
        map.Update(dt, gameSpeed);


     



     

        //Floor and Ceiling collisions
        if(mervPosition.Y < 0){
            _velocityY =0;
            mervPosition.Y=0;
         
        }
        if(mervPosition.Y>screenHeight - mervSize){
            _velocityY =0;
            mervPosition.Y = screenHeight - mervSize;
          
        }



    
        base.Update(gameTime);
    }

   protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);


        _spriteBatch.Begin();

             //Background
       // _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

        //Draw Character
       


        /**
        _spriteBatch.DrawString(
            font,
            $"{score}",
            new Vector2(screenWidth/2, 20),
            Color.White
        );
        **/
        map.Draw(_spriteBatch, _pixel);
        _spriteBatch.Draw(_pixel, merv, Color.Blue);
        

        _spriteBatch.End();

        base.Draw(gameTime);

    }
}
