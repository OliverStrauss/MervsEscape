using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MERV;
public class SegmentManager
{
    public readonly List<Segment> segments = new();
    private readonly Random rng;
    private float nextSpawnX;
    private Dictionary<string, Texture2D> textures;



    private int segmentsPlaced = 0; 

    public SegmentManager(Random rng,Dictionary<string, Texture2D> textures)
    {
        this.rng = rng;
        nextSpawnX = 960;
        this.textures = textures;
       
    }

    public bool CheckCollisions(Rectangle player)
    {
        foreach (var s in segments)
            if (s.CheckCollisions(player))
                return true;

        return false;
    }

    public void Update(float dt, float speed)
    {
        foreach (var s in segments)
            s.Update(dt, speed);
        //if(segments[len-1].Endx < screenwidth)
        while (segments.Count == 0 || segments[^1].EndX < Game1.screenWidth)
            {
                SpawnNext();
            }

    
        segments.RemoveAll(s => s.IsOffScreen());
    }

 
    void SpawnNext()
    {
        
    

        float spawnX = 0f;

        if (segments.Count == 0)
        {
            spawnX = 0f;
        }
        else
        {
            var last = segments[^1]; // last element
            spawnX = last.EndX;
        }

        

        Segment seg; 
        bool debug = false; 
        
      

        //Intro Segment;
        if(segmentsPlaced == 0){
            seg = SegmentStorage.Intro(spawnX,rng,textures);
        }else{
            
           int choice = rng.Next(5);
            if(debug){
                seg =SegmentStorage.Debug(spawnX,rng,textures);
                
            }else{
            

            if(choice == 0){
                seg = SegmentStorage.ArrowPattern(spawnX,rng,textures);
            }
            else if ( choice == 1){
                seg = SegmentStorage.VertHeaven(spawnX,rng,textures);
            }else{
                seg = SegmentStorage.randomZappers(spawnX,rng,textures);
            }

            }

            
        }
        
        segments.Add(seg);
        segmentsPlaced ++; 
   
    }

    public void Draw(SpriteBatch sb, Texture2D pixel)
    {
        foreach (var s in segments)
            s.Draw(sb, pixel);
    }
}