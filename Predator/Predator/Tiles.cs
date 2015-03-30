using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VoidEngine;

namespace Predator
{
	public class Tiles : Sprite
	{
		Game myGame;

		public float radius = 10;
		public int type;

		public Tiles(Vector2 pos, Game mygame, int type, List<AnimationSet> animationSetLists)
			: base(pos, Color.White, animationSetLists)
		{
			myGame = mygame;
			this.type = type;
			SetAnimation(type.ToString());
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
		}
	}
}
