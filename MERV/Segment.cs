using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using System;

namespace MERV;


public class Segment
{


    public float StartX;
    public int LengthInScreens;
    public Color Background;
    public List<Obstacle> Obstacles = new();
   
    public float WidthPixels => LengthInScreens * Game1.screenWidth;//default Width 
    public float EndX => StartX + WidthPixels;



    public Segment(float startX, int len, Color bg)
    {
        StartX = startX;
   
        LengthInScreens = len;
        Background = bg;

       
    }

   

    public void Update(float dt, float gameSpeed)
    {
        // Moves at exactly the same speed as the floor
        StartX -= gameSpeed * dt;


        foreach( var o in Obstacles ){
            o.Update(dt,gameSpeed);
        }
   
    }

   public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {   
       
    // background slice
        spriteBatch.Draw(pixel,
            new Rectangle((int)StartX, 0, (int)WidthPixels, Game1.screenHeight),
            Background);

        foreach (var o in Obstacles)
            o.Draw(spriteBatch, pixel);
    }

    public bool IsOffScreen() => EndX < 0;
}
