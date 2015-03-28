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
using VoidEngine;

namespace Predator
{
	public class Projectile : Sprite
	{
		public Rectangle projectileRectangle;

		Game1 myGame;
		Player player;

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

		protected float test1;
		protected float test2;
		protected float test3;
		protected float test4;

		public Projectile(Vector2 startPosition, Color color, List<AnimationSet> animationSetList, Player player, Game1 game)
			: base(startPosition, color, animationSetList)
		{
			this.startPosition = startPosition;
			Position = startPosition;
			color = Color.White;
			AnimationSets = animationSetList;
			this.player = player;
			this.myGame = game;

			if (player.isFlipped)
			{
				Position.X = Position.X - 25;
				Direction = new Vector2(-1, 0);
			}
			else
			{
				Direction = new Vector2(1, 0);
			}
		}

		public override void Update(GameTime gameTime)
		{
			projectileRectangle = new Rectangle((int)Position.X, (int)Position.Y, 25, 3);

			if (Vector2.Distance(startPosition, Position) > maxDistance)
			{
				visible = false;
			}
			/* foreach (Rectangle r in myGame.gameManager.platformRectangles)
			{
				if (projectileRectangle.TouchLeftOf(r) || projectileRectangle.TouchTopOf(r) || projectileRectangle.TouchBottomOf(r) || projectileRectangle.TouchRightOf(r))
				{
					visible = false;
				}
			}
			foreach (Enemy er in myGame.gameManager.cEnemyList)
			{
				test1 = er.GetDirection.X * -1;
				test1 = MathHelper.Clamp(test2, -1, 1);

				if (projectileRectangle.TouchLeftOf(er.GetPlayerRectangles()) || projectileRectangle.TouchTopOf(er.GetPlayerRectangles()) || projectileRectangle.TouchBottomOf(er.GetPlayerRectangles()) || projectileRectangle.TouchRightOf(er.GetPlayerRectangles()))
				{
					visible = false;
					er.DeleteMe = true;
					myGame.gameManager.enemyhitSFX.Play(1f, 0f, test1);
				}
			}
			foreach (Enemy er in myGame.gameManager.sEnemyList)
			{
				test2 = er.GetDirection.X * -1;
				test2 = MathHelper.Clamp(test2, -1, 1);

				if (projectileRectangle.TouchLeftOf(er.GetPlayerRectangles()) || projectileRectangle.TouchTopOf(er.GetPlayerRectangles()) || projectileRectangle.TouchBottomOf(er.GetPlayerRectangles()) || projectileRectangle.TouchRightOf(er.GetPlayerRectangles()))
				{
					visible = false;
					er.DeleteMe = true;
					myGame.gameManager.enemyhitSFX.Play(1f, 0f, test2);
				}
			}
			foreach (Enemy er in myGame.gameManager.tEnemyList)
			{
				test3 = er.GetDirection.X * -1;
				test3 = MathHelper.Clamp(test2, -1, 1);

				if (projectileRectangle.TouchLeftOf(er.GetPlayerRectangles()) || projectileRectangle.TouchTopOf(er.GetPlayerRectangles()) || projectileRectangle.TouchBottomOf(er.GetPlayerRectangles()) || projectileRectangle.TouchRightOf(er.GetPlayerRectangles()))
				{
					visible = false;
					er.DeleteMe = true;
					myGame.gameManager.enemyhitSFX.Play(1f, 0f, test1);
				}
			}
			if (myGame.gameManager.BossCreated && !myGame.gameManager.bhEnemy.Dead)
			{
				test4 = Collision.UnitVector(new Vector2((myGame.gameManager.player.GetPosition.X + myGame.gameManager.player.PositionCenter.X) - (myGame.gameManager.bhEnemy.GetPosition.X + myGame.gameManager.bhEnemy.PositionCenter.X), 0)).X * -1;
				test4 = MathHelper.Clamp(test2, -1, 1);

				if (projectileRectangle.TouchLeftOf(myGame.gameManager.bhEnemy.GetPlayerRectangles()) || projectileRectangle.TouchTopOf(myGame.gameManager.bhEnemy.GetPlayerRectangles()) || projectileRectangle.TouchBottomOf(myGame.gameManager.bhEnemy.GetPlayerRectangles()) || projectileRectangle.TouchRightOf(myGame.gameManager.bhEnemy.GetPlayerRectangles()))
				{
					myGame.gameManager.bhEnemy.Lives -= 1;
					visible = false;
					myGame.gameManager.enemyhitSFX.Play(1f, 0f, test4);
				}
			} */
			if (visible)
			{
				Position.X += Direction.X * Speed + 0.5f * (DirectionX) * (Speed * Speed);
			}

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
			Speed = 3;
			maxDistance = 125;

			DirectionX = player.GetDirection.X;

			visible = true;

			SetAnimation("IDLE");
		}
	}
}
