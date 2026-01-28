using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MERV;
public abstract class Obstacle
{
    public Vector2 Position;
    public bool IsActive = true; // If false, remove from list

    protected Texture2D texture;
    // Every obstacle must calculate its own hitbox
    public abstract Rectangle Hitbox { get; }

    // Every obstacle updates differently
    public abstract void Update(float dt, float gameSpeed);

    // Every obstacle draws differently
    public abstract void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture);

    public abstract bool Collides(Rectangle player);
   
}