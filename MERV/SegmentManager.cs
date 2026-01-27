using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MERV;
public class SegmentManager
{
    private readonly List<Segment> segments = new();
    private readonly Random rng;
    private float nextSpawnX;

    private int segmentsSize;

    public SegmentManager(Random rng,int segSize)
    {
        this.rng = rng;
        nextSpawnX = 960;
        segmentsSize = segSize;
    }

    public void Update(float dt, float speed)
    {
        foreach (var s in segments)
            s.Update(dt, speed);
        //Checking if newest element in list 
        if (segments.Count < segmentsSize )
            SpawnNext();

        Console.WriteLine(segments.Count);
        segments.RemoveAll(s => s.IsOffScreen());
    }

    void SpawnNext()
    {
        /**
        Segment seg = rng.Next(0, 2) switch
        {
            0 => SegmentFactory.Easy(nextSpawnX, rng),
            _ => SegmentFactory.DiagonalRun(nextSpawnX)
        };
        **/
        
        Segment seg = SegmentStorage.Easy(nextSpawnX,rng);
        segments.Add(seg);
        nextSpawnX = seg.EndX;
    }

    public void Draw(SpriteBatch sb, Texture2D pixel)
    {
        foreach (var s in segments)
            s.Draw(sb, pixel);
    }
}