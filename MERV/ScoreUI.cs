using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace MERV;

public class ScoreUI : UIObject
{
    public int Score;

    private SpriteFont font;
    private Color color = Color.White;

    public ScoreUI(Vector2 position, SpriteFont font)
    {
        Position = position;
        this.font = font;
    }

    public override void Update(float dt)
    {
       
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsVisible) return;
        float roundedValue = MathF.Round(Game1.DISTANCE, 2);
        spriteBatch.DrawString(
            font,
            $"METERS: {roundedValue}",
            Position,
            color
        );
    }
    
}