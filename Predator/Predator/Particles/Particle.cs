using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VoidEngine.VGame;

namespace Predator
{
    public class Particle : Sprite
    {
        public bool deleteMe = false;
        Color washColor;
        int elapsedTime = 0;
        int lifeSpan;
        Random rng = new Random();
        Vector2 PositionCenter;
		Vector2 Direction;
		float Speed;
        public Particle(Vector2 position, Texture2D tex, int lifespan, int myspeed, Color washcolor, float angle, List<AnimationSet> animationSet)
            : base(position, Color.White, animationSet)
        {
            angle *= (float)(Math.PI / 180);
            Direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            lifeSpan = lifespan;
            washColor = washcolor;
            Speed = myspeed;
            SetAnimation("IDLE");
        }
        public override void Update(GameTime gameTime)
        {
            Position += Direction * Speed;
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            //ratio = 1- (elapsedTime / lifeSpan) ;
            //washColor = new Color(washColor.R, washColor.G, washColor.B, ratio);
            if (elapsedTime >= lifeSpan)
            {
                deleteMe = true;
            }
            PositionCenter = new Vector2((CurrentAnimation.frameSize.X / 2), (CurrentAnimation.frameSize.Y / 2));
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CurrentAnimation.texture, new Vector2(Position.X + PositionCenter.X, Position.Y + PositionCenter.Y), new Rectangle((CurrentAnimation.frameSize.X) + CurrentAnimation.startPosition.X, (CurrentAnimation.frameSize.Y) + CurrentAnimation.startPosition.Y, CurrentAnimation.frameSize.X, CurrentAnimation.frameSize.Y), washColor, Rotation, RotationCenter, 1, SpriteEffects.None, 0);

        }
    }
}
