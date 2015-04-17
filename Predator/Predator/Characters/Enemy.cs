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
	public class Enemy : Player
	{
		public enum EnemyType
		{
			BIRD,
			SLIME,
			SLIMEBALL,
			RAT,
			CHARGER,
			NONE
		}

		/// <summary>
		/// Gets the current game.
		/// </summary>
		Game1 myGame;

		/// <summary>
		///
		/// </summary>
		public EnemyType movementType;

		/// <summary>
		/// The player value.
		/// </summary>
		protected Player player;

		/// <summary>
		/// Gets the
		/// </summary>
		public Vector2 TempVelocity;

		public float JumpDelayTimer = 500;
		public bool isJumping2 = false;
		/// <summary>
		/// Gets or sets if the Enemy was hit.
		/// </summary>
		public bool isHit
		{
			get;
			set;
		}

		public Enemy(Vector2 position, EnemyType movementType, Color color, List<AnimationSet> animationSetList, Game1 myGame)
			: base(position, color, animationSetList)
		{
			Level = 1;
			MainHP = 30;
			MaxHP = 30;
			JumpbackTimer = 0;
			MaxMoveSpeed = 200;
			GroundDragFactor = 0.46f;
			AirDragFactor = 0.50f;

			SetAnimation("IDLE" + Level);

			#region Set default variables
			this.myGame = myGame;
			this.movementType = movementType;
			this.CanShoot = true;
			#endregion

			if (movementType == EnemyType.BIRD)
			{
				RotationCenter = new Vector2(animationSetList[0].frameSize.X / 2, animationSetList[0].frameSize.Y / 2);
				Offset = new Vector2(-(animationSetList[0].frameSize.X / 2), -(animationSetList[0].frameSize.Y / 2));
			}

			int width = (int)(animationSetList[0].frameSize.X);
			int left = (animationSetList[0].frameSize.X - width);
			int height = (int)(animationSetList[0].frameSize.Y);
			int top = animationSetList[0].frameSize.Y - height;
			inbounds = new Rectangle(left, top, width, height);
		}

		public virtual void Update(GameTime gameTime, Player player, List<Tile> TileList, List<Rectangle> RectangleList)
		{
			this.TileList = TileList;
			this.MapBoundries = RectangleList;
			this.player = player;

			HandleAnimations(gameTime);

			ApplyPhysics(gameTime);

			HandleHealth(gameTime);

			Center = new Vector2(Inbounds.Width / 2, Inbounds.Height / 2);
			if (!isDead && isGrounded)
			{
				if (Math.Abs(Velocity.X) - 0.02f > 0)
				{
					SetAnimation("WALK" + Level);
				}
				else
				{
					SetAnimation("IDLE" + Level);
				}
			}

			Movement = 0.0f;
			isJumping = false;
		}

		protected void HandleHealth(GameTime gameTime)
		{
			if (HP <= 1)
			{
				isDead = true;
				myGame.gameManager.bloodx = 0;
				myGame.gameManager.bloody = 360;
				if (HP <= 0)
				{
					MainHP = 0;
				}
			}
		}

		protected override void HandleEnemyCollisions(GameTime gameTime)
		{
			foreach (Projectile p in player.ProjectileList)
			{
				if (BoundingCollisions.TouchLeftOf(p.projectileRectangle) || BoundingCollisions.TouchRightOf(p.projectileRectangle))
				{
					attackCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (PositionCenter.X >= p.projectileRectangle.X + (p.projectileRectangle.Width / 2))
					{
						isJumping = true;
						Movement += 1;
						velocity.X = MaxMoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * Movement;
						velocity.Y = DoJump(velocity.Y, gameTime);
					}
					else if (PositionCenter.X < p.projectileRectangle.X + (p.projectileRectangle.Width / 2))
					{
						isJumping = true;
						Movement += -1;
						velocity.X = MaxMoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * Movement;
						velocity.Y = DoJump(velocity.Y, gameTime);
					}
				}
				if (attackCounter <= 0)
				{
					attackCounter = 1;

					if (p.projectileRectangle.TouchLeftOf(BoundingCollisions))
					{
						myGame.gameManager.bloodx = 330;
						myGame.gameManager.bloody = 350;
						isHit = true;
						MainHP -= myGame.gameManager.player.Damage;
					}
					if (p.projectileRectangle.TouchRightOf(BoundingCollisions))
					{
						myGame.gameManager.bloodx = 180;
						myGame.gameManager.bloody = 200;
						isHit = true;
						MainHP -= myGame.gameManager.player.Damage;
					}
				}
			}
		}

		protected override void HandleCollisions(GameTime gameTime)
		{
			if (movementType != EnemyType.SLIMEBALL && movementType != EnemyType.BIRD)
			{
				foreach (Tile t in TileList)
				{
					if (t.tileCollisions == Tile.TileCollisions.Impassable)
					{
						if (BoundingCollisions.TouchLeftOf(t.Collisions) || BoundingCollisions.TouchRightOf(t.Collisions))
						{
							isJumping2 = true;
						}
					}
				}
			}
		}

		public override void  ApplyPhysics(GameTime gameTime)
		{
			TempVelocity = new Vector2(player.PositionCenter.X - PositionCenter.X, player.PositionCenter.Y - PositionCenter.Y);

			if (Math.Abs(Movement) < 0.5f)
			{
				Movement = 0.0f;
			}

			if (CollisionHelper.Magnitude(TempVelocity) <= 200)
			{
				if (player.PositionCenter.X - PositionCenter.X < 0)
				{
					Movement = -1;
				}
				else if (player.PositionCenter.X - PositionCenter.X > 0)
				{
					Movement = 1;
				}
				else if (player.PositionCenter.X == PositionCenter.X)
				{
					Movement = 0;
				}

				if (movementType != EnemyType.SLIMEBALL)
				{
					SetAnimation("WALK" + Level);
				}

				if (movementType == EnemyType.BIRD)
				{
					Rotation += 0.05f;
				}
			}
			else
			{
				if (movementType != EnemyType.SLIMEBALL)
				{
					SetAnimation("IDLE" + Level);
				}
			}

			HandleCollisions(gameTime);
			HandleEnemyCollisions(gameTime);

			if (movementType == EnemyType.SLIMEBALL)
			{
				isJumping2 = true;
			}

			if (isJumping2)
			{
				JumpDelayTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

				if (JumpDelayTimer > 0)
				{
					isJumping = true;
				}
				if (JumpDelayTimer <= 0)
				{
					isJumping = false;
				}
				if (JumpDelayTimer < -200)
				{
					isJumping2 = false;
					JumpDelayTimer = 500;
				}
			}

			ApplyPhysics(gameTime);
		}
	}
}
