using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VoidEngine;
using VoidEngine.VGame;
using VoidEngine.Helpers;

namespace Predator.Other
{
    public class HealthPickUp : Sprite
    {
        public Game1 myGame;
        public bool deleteMe = false;

        public HealthPickUp(Texture2D texture, Vector2 Position, Game1 mygame)
            : base(Position, Color.White, texture)
        {
            myGame = mygame;

			RotationCenter = new Vector2((texture.Width / 4) / 2, texture.Height / 2);
            
            AddAnimations(texture);
        }
        public override void Update(GameTime gameTime)
        {
            HandleAnimations(gameTime);

			if (Math.Abs(CollisionHelper.Magnitude(position - myGame.gameManager.Player.PositionCenter)) <= 10f)
			{
				deleteMe = true;
			}

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
      
        protected override void AddAnimations(Texture2D tex)
        {
            AddAnimation("IDLE", tex, new Point(15, 15), new Point(4, 0), new Point(0, 0), 75, true);
            SetAnimation("IDLE");
        }
    }
}
