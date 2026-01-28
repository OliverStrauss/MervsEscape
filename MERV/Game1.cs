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
    SpriteFont uiFont;

    
    private Texture2D mervTexture;
    private Texture2D backgroundTexture;
    private Texture2D floorTexture;
    Texture2D zapperTexture;

    //FONT
  //  SpriteFont font;
    SegmentManager map;
    //SCREEN DIMENSIONS
    public static int screenWidth = 960;
    public static int screenHeight = 540;

    //CHARACTER DIMS
    private int mervSize = 64;

    Rectangle merv;

    //GAME STATE VARS
    private bool paused = true;
    private float gameSpeed = 200f;
    float jetpackPower = 2500f; // Force pushing up
    float gravity = 1000f;

    List<UIObject> uiObjects = new();
    private Dictionary<string, Texture2D> obstacleTextures;


    //VARS set for scope
  
    private Vector2 mervPosition;
    private float _velocityY = 0f;
    private Random _rng = new Random();
    private float floorX = 0f;



    float pipeSpawnTimer = 0f;
    float pipeSpawnInterval = 2f;

    public static float DISTANCE =0 ;





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
        _rng = new Random();
        //map = new SegmentManager(_rng);
       
       





        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        uiFont = Content.Load<SpriteFont>("UIFont");
        uiObjects.Add(new ScoreUI(new Vector2(20, 20), uiFont));

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        mervTexture = Content.Load<Texture2D>("Merv");

        obstacleTextures = new Dictionary<string, Texture2D>();
        obstacleTextures["zapper_basic"] = Content.Load<Texture2D>("ZapperBasic");
        obstacleTextures["zapper_tex"] = Content.Load<Texture2D>("LaserText");

        map = new SegmentManager(_rng, obstacleTextures);
    }




    
    



    
    


    void resetGame()
    {
        map = new SegmentManager(_rng, obstacleTextures);

        mervPosition = new Vector2(screenWidth / 3, screenHeight / 2);
        _velocityY = 0f;
        DISTANCE = 0f;
        floorX = 0f;
    }



    protected override void Update(GameTime gameTime)
    {

        try{
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboard = Keyboard.GetState();
            merv = new Rectangle((int)mervPosition.X, (int)mervPosition.Y, mervSize, mervSize);

            if (keyboard.IsKeyDown(Keys.Up))
            {
            _velocityY -= jetpackPower * dt;
                paused = false;
            }

        

            if(!paused){
                _velocityY += gravity * dt;

                _velocityY = Math.Clamp(_velocityY, -600f, 600f);

                mervPosition.Y += _velocityY * dt;

        

                floorX -= gameSpeed * dt;
                DISTANCE += (float).2;

            
                if (floorX <= -screenWidth)
                {
                    floorX = 0;
                }
                

            
                map.Update(dt, gameSpeed);
                if(map.CheckCollisions(merv)){
                    Console.WriteLine("HEYYEYEYE HE DIED");
                    paused = true;
                    resetGame();
                }
            

                //Floor and Ceiling collisions
                if(mervPosition.Y < 0){
                    _velocityY =0;
                    mervPosition.Y=0;
                
                }
                if(mervPosition.Y>screenHeight - mervSize){
                    _velocityY =0;
                    mervPosition.Y = screenHeight - mervSize;
                
                }
            }
            foreach (var ui in uiObjects){
                ui.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine("CRASH IN UPDATE: " + e.Message);
            System.Diagnostics.Debug.WriteLine(e.StackTrace);
            throw; // Re-throw to pause debugger
        }
    }

   protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);


        _spriteBatch.Begin(
            SpriteSortMode.Deferred, 
            BlendState.AlphaBlend, 
            SamplerState.LinearWrap, // <--- THIS ALLOWS THE ZAPPER TO TILE
            null, 
            null
        );
 

        
 
        map.Draw(_spriteBatch, _pixel);
        _spriteBatch.Draw(mervTexture, merv, Color.White);
       // _spriteBatch.Draw(_pixel, merv, Color.White);

  
        foreach (var ui in uiObjects){
            ui.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);

    }
}
