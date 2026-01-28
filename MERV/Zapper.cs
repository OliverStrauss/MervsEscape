using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MERV; // <--- THIS LINE IS CRITICAL
public enum ZapperType
{
        Horizontal,
        Vertical,
        DiagonalRight,
        DiagonalLeft
}


public class Zapper : Obstacle
{
    public int Width { get; set; } = 50;
    public int Height { get; set; } = 150;

    public ZapperType Type { get; private set; }
    public int Length { get; private set; }

    private float rotation;
    private Vector2 origin;
    private Vector2 scale;

    private const int Thickness = 20;
    const float HITBOX_SHRINK = 0.7f;

    public static int[] sizes = {150,175,200};


   public Zapper(Vector2 startPos, ZapperType type, int lengthIndex, Texture2D texture)
{
    Position = startPos;
    Type = type;

    // Safety check on index
    int idx = Math.Clamp(lengthIndex, 0, sizes.Length - 1);
    Length = sizes[idx];
    
    this.texture = texture ?? throw new ArgumentNullException(nameof(texture));

    // Origin: Middle-Left of the texture.
    // This allows the zapper to rotate around its starting point while being centered vertically.
    origin = new Vector2(0, texture.Height / 2.0f);

    // --- FIX IS HERE ---

    // 1. Calculate Scale X: How much to stretch the texture width to equal the target Length
    float scaleX = (float)Length / texture.Width;

    // 2. Calculate Scale Y: How much to stretch the texture height to equal the target Thickness
    float scaleY = (float)Thickness / texture.Height;

    scale = new Vector2(scaleX, scaleY);

    switch (Type)
    {
        case ZapperType.Horizontal:
            rotation = 0f;
            break;

        case ZapperType.Vertical:
            rotation = MathHelper.PiOver2; // 90 degrees
            break;

        case ZapperType.DiagonalRight:
            rotation = MathHelper.PiOver4; // 45 degrees
            break;

        case ZapperType.DiagonalLeft:
            rotation = 3 * MathHelper.PiOver4; // 135 degrees
            break;
        }
    }

    public override Rectangle Hitbox
    {
        get
        {
            return Type switch
            {
                ZapperType.Vertical =>
                    new Rectangle((int)Position.X, (int)Position.Y, Thickness, Length),

                ZapperType.Horizontal =>
                    new Rectangle((int)Position.X, (int)Position.Y, Length, Thickness),

                // Diagonals use a bounding box for collision (simple + fast)
                ZapperType.DiagonalRight =>
                    new Rectangle((int)Position.X, (int)Position.Y, Length, Length),

                ZapperType.DiagonalLeft =>
                    new Rectangle((int)Position.X, (int)Position.Y, Length, Length),

                _ => Rectangle.Empty
            };
        }
    }

    public override void Update(float dt, float gameSpeed)
    {
        // Moves at exactly the same speed as the floor
        Position.X -= gameSpeed * dt;

        // Mark for deletion if off screen
       if (Position.X + Hitbox.Width < 0)
            IsActive = false;

        
    }

    //Generates line given postion and length
    private void GetLine(out Vector2 a, out Vector2 b)
    {
            Vector2 dir = new Vector2(
                (float)Math.Cos(rotation),
                (float)Math.Sin(rotation)
            );

            a = Position;
            b = Position + dir * Length;
    }

    static float DistancePointToSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float t = Vector2.Dot(p - a, ab) / Vector2.Dot(ab, ab);
        t = Math.Clamp(t, 0f, 1f);

        Vector2 closest = a + ab * t;
        return Vector2.Distance(p, closest);
    }


    public override bool Collides(Rectangle player){
        //Find a and B 
        GetLine(out Vector2 a, out Vector2 b);

        //Find tolerance for collision , .5 for extra tolderance
        float radius = Thickness * 0.5f;

        // Check rectangle corners
        Vector2[] corners =
        {
            new(player.Left,  player.Top),
            new(player.Right, player.Top),
            new(player.Left,  player.Bottom),
            new(player.Right, player.Bottom)
        };

        foreach (var c in corners)
        {
            if (DistancePointToSegment(c, a, b) <= radius)
                return true;
        }

        return false;
    }

   public override void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {   
        Console.WriteLine(scale);

        Vector2 drawPos = new Vector2(
            (int)Math.Round(Position.X), 
            (int)Math.Round(Position.Y)
        );
        Rectangle sourceRect = new Rectangle(0, 0, Length, texture.Height);
        spriteBatch.Draw(
            texture,
            drawPos,
            sourceRect,       // Use the entire texture (Stretched by scale)
            Color.White,
            rotation,
            origin,     // Pivot point (0, Middle)
            scale,      // The corrected scale vector
            SpriteEffects.None,
            0f
        );
        /**

        GetLine(out Vector2 a, out Vector2 b);

        // draw endpoints
        spriteBatch.Draw(pixel, new Rectangle((int)a.X, (int)a.Y, 4, 4), Color.Red);
        spriteBatch.Draw(pixel, new Rectangle((int)b.X, (int)b.Y, 4, 4), Color.Red);
        **/
    }
}
