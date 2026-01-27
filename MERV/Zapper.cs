using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

 
    public Zapper(Vector2 startPos, ZapperType type,int length)
    {
        Position = startPos;
        Type = type;
        Length = length;

     

        switch (Type)
        {
            case ZapperType.Vertical:
                rotation = MathHelper.PiOver2;
                scale = new Vector2(Length, Thickness);
                origin = new Vector2(0, 0.5f);
                break;

            case ZapperType.Horizontal:
                rotation = 0f;
                scale = new Vector2(Length, Thickness);
                origin = new Vector2(0, 0.5f);
             
                break;

            case ZapperType.DiagonalRight:
                rotation = MathHelper.PiOver4; // 45°
                scale = new Vector2(Length, Thickness);
                origin = new Vector2(0, 0.5f);
                break;

            case ZapperType.DiagonalLeft:
          
                rotation = 3*MathHelper.PiOver4; 
                scale = new Vector2(Length, Thickness);
                origin = new Vector2(0, 0.5f);
               
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

   public override void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        spriteBatch.Draw(
            pixel,
            Position,
            null,
            Color.Yellow,
            rotation,
            origin,
            scale,
            SpriteEffects.None,
            0f
        );
    }
}
