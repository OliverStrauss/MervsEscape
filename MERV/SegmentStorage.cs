using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MERV;

public static class SegmentStorage
{
    public static Segment Easy(float startX, Random rng)
    {
        var seg = new Segment(startX, 1, Color.LightSkyBlue);

        int L = 200;
        int y = Game1.screenHeight/2;

        seg.Obstacles.Add(new Zapper(
            new Vector2(startX + Game1.screenWidth / 3, y),
            ZapperType.DiagonalRight,
            L));
       
        seg.Obstacles.Add(new Zapper(
            new Vector2(
                startX + (Game1.screenWidth / 3) * 2 ,
                y),
            ZapperType.DiagonalLeft,
            L));
       
        return seg;
    }
}