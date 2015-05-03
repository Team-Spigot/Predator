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
using VoidEngine.VGUI;
using Predator.Characters;

namespace Predator.Characters
{
	public class Projectile : Sprite
	{
		Game1 myGame;

		public Rectangle projectileRectangle;

		Vector2 startPosition;
		public Vector2 GetStartPosition
		{
			get
			{
				return startPosition;
			}
		}

		public float maxDistance
		{
			get;
			protected set;
		}

		public float DirectionX
		{
			get;
			protected set;
		}

		public bool visible
		{
			get;
			set;
		}

		public Projectile(Texture2D texture, Vector2 startPosition, Color color, Game1 myGame)
			: base(startPosition, color, texture)
		{
			this.startPosition = startPosition;
			Position = startPosition;
			color = Color.White;
			visible = true;

			this.myGame = myGame;

			AddAnimations(texture);

			SetAnimation("IDLE");
		}

		public override void Update(GameTime gameTime)
		{
			HandleAnimations(gameTime);

			if (Vector2.Distance(startPosition, Position) > maxDistance)
			{
				//visible = false;
			}
			if (visible)
			{
				if (myGame.gameManager.Player.isFlipped)
				{
					FlipSprite(Axis.Y);
					Position = new Vector2(myGame.gameManager.Player.Position.X - CurrentAnimation.frameSize.X, Position.Y + 15);
				}
				else
				{
					FlipSprite(Axis.NONE);
					Position = new Vector2(myGame.gameManager.Player.Position.X + myGame.gameManager.Player.BoundingCollisions.Width, Position.Y + 15);
				}

				Position = new Vector2(Position.X, myGame.gameManager.Player.Position.Y);
			}

			projectileRectangle = new Rectangle((int)Position.X, (int)Position.Y, CurrentAnimation.frameSize.X, CurrentAnimation.frameSize.Y);

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (visible)
			{
				base.Draw(gameTime, spriteBatch);
			}
		}

		public void Fire()
		{
			//Speed = 3;
			maxDistance = 125;

			DirectionX = myGame.gameManager.Player.Velocity.X;

			visible = true;
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(40, 24), new Point(3, 1), new Point(0, 0), 50, false);
			SetAnimation("IDLE");

			base.AddAnimations(texture);
		}
	}
}
