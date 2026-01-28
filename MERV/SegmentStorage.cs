using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MERV;

public static class SegmentStorage
{
    public static Segment ArrowPattern(float startX, Random rng,Dictionary<string, Texture2D> textures)
    {   
        var seg = new Segment(startX, 1, Color.LightSkyBlue);

        Texture2D tex = textures["zapper_tex"];
      
        float y = Game1.screenHeight / 2f;

        float x1 = startX + Game1.screenWidth / 3f;
        float x2 = startX + Game1.screenWidth * 2f / 3f;

        bool down = rng.Next(2) == 1;

        seg.Obstacles.Add(new Zapper(
            new Vector2(x1, y),
            down ? ZapperType.DiagonalRight : ZapperType.DiagonalLeft,
            0,tex));

        seg.Obstacles.Add(new Zapper(
            new Vector2(x2, y),
            down ? ZapperType.DiagonalLeft : ZapperType.DiagonalRight,
            0,tex));

        return seg;
    }
    
     public static Segment randomZappers(float startX, Random rng, Dictionary<string, Texture2D> textures)
    {
        var seg = new Segment(startX, 1, Color.LightBlue);
       Texture2D tex = textures["zapper_tex"];

        int L1 = rng.Next(0, 3);// RECT1 , VERTICAL
        int L2 = rng.Next(0, 3);

        float y1 = rng.Next(0,Game1.screenHeight-L1);
        float y2 = rng.Next(50,Game1.screenHeight-20);

        float x1 = rng.Next((int)startX,(int)startX+Game1.screenWidth/2);
        float x2 = rng.Next((int)startX+Game1.screenWidth/2 ,(int)startX+Game1.screenWidth);

        bool flipper = rng.Next(2) == 1;
        float x = flipper ? x1 : x2;
        seg.Obstacles.Add(new Zapper(

            new Vector2(x, y1),
            ZapperType.Vertical,
            L1,tex));

        x = !flipper ? x1 : x2;
        seg.Obstacles.Add(new Zapper(
            
            new Vector2(x, y2),
             ZapperType.Horizontal,
            L2,tex));

        return seg;
    }

     

    public static Segment VertHeaven(float startX, Random rng,Dictionary<string, Texture2D> textures)
    {   
        int screens =3 ; 
        int density = 2;
        int iters = screens * density; 

      
        var seg = new Segment(startX, screens, Color.Blue);
          Texture2D tex = textures["zapper_tex"];
     
        int gap = 0;
        int bottomThird = Game1.screenHeight/3 +Game1.screenHeight/2;

        for(int i =0 ; i < iters ; i ++){
            int middlePeice = rng.Next(2);

            int L = rng.Next(1,3);
            int y1 = rng.Next(0,30);
            int y2 = rng.Next(bottomThird,bottomThird +40);
            if(middlePeice == 1)
            {
                seg.Obstacles.Add(new Zapper(
                    new Vector2(startX + Game1.screenWidth / 3 + gap, y1),
                    ZapperType.Vertical,
                    L,tex));
            
                seg.Obstacles.Add(new Zapper(
                    new Vector2(
                        startX + (Game1.screenWidth / 3)+ gap , y2),
                    ZapperType.Vertical,
                    L,tex));
            }else{
                L = 2; 
                int middle = Game1.screenWidth /2 - Zapper.sizes[2]; 
                int middleYspawn = rng.Next(middle-20, middle+20);
                seg.Obstacles.Add(new Zapper(
                    new Vector2(
                        startX + (Game1.screenWidth / 3)+ gap , middleYspawn),
                    ZapperType.Vertical,
                    L,tex));
            }
            int standardGap = ((Game1.screenWidth * screens)/iters) ;
           gap += rng.Next(standardGap-100,standardGap+100);     
        }
        return seg;
    }



    public static Segment Intro(float startX, Random rng,Dictionary<string, Texture2D> textures)
    {
        var seg = new Segment(startX, 1, Color.Gray);

        return seg;
    }

     public static Segment Debug(float startX, Random rng,Dictionary<string, Texture2D> textures)
    {   
   
            
        var seg = new Segment(startX, 1, Color.Blue);
         Texture2D tex = textures["zapper_tex"];
         seg.Obstacles.Add(new Zapper(
                    new Vector2(startX + Game1.screenWidth / 2, Game1.screenHeight/2),
                    ZapperType.Vertical,
                    0,tex));

        return seg;
    }
}