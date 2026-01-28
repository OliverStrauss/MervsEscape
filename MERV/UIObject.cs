using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MERV;

public abstract class UIObject
{
    // Screen-space position
    public Vector2 Position;

    public bool IsVisible = true;

    // Update UI state (animations, value changes, etc.)
    public abstract void Update(float dt);

    // Draw UI element
    public abstract void Draw(SpriteBatch spriteBatch);
}