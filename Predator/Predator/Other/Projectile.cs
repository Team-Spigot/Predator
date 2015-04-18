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

namespace Predator
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

		public Projectile(Vector2 startPosition, Color color, List<AnimationSet> animationSetList, Game1 myGame)
			: base(startPosition, color, animationSetList)
		{
			this.startPosition = startPosition;
			Position = startPosition;
			color = Color.White;
			AnimationSets = animationSetList;
			visible = true;

			this.myGame = myGame;

			SetAnimation("IDLE");

			if (myGame.gameManager.player.isFlipped)
			{
				Position.X = startPosition.X - animationSetList[0].frameSize.X;
			}
			else
			{
				Position.X = startPosition.X + myGame.gameManager.player.BoundingCollisions.Width;
			}
		}

		public void Update(GameTime gameTime, Player player, List<Enemy> EnemyList, List<Tile> TileList, List<Rectangle> RectangleList)
		{
			if (Vector2.Distance(startPosition, Position) > maxDistance)
			{
				//visible = false;
			}
			if (visible)
			{
				if (player.isFlipped)
				{
					FlipSprite(Axis.Y);
					Position.X = player.GetPosition.X - CurrentAnimation.frameSize.X;
				}
				else
				{
					FlipSprite(Axis.NONE);
					Position.X = player.GetPosition.X + player.BoundingCollisions.Width;
				}

				Position.Y = player.GetPosition.Y;
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

			DirectionX = myGame.gameManager.player.Velocity.X;

			visible = true;

			SetAnimation("IDLE");
		}
	}
}
