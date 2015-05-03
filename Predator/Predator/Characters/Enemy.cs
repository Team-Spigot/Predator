using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VoidEngine.VGame;
using VoidEngine.Helpers;

namespace Predator.Characters
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
		///
		/// </summary>
		public EnemyType movementType;

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

		public Enemy(Texture2D texture, Vector2 position, EnemyType movementType, Color color, Game1 myGame)
			: base(texture, position, color)
		{
			this.myGame = myGame;
			AddAnimations(texture);

			Level = 1;
			MainHP = 30;
			MaxHP = 30;
			JumpbackTimer = 0;
			MaxMoveSpeed = 175;
			GroundDragFactor = 0.46f;
			AirDragFactor = 0.50f;

			Scale = 0.5833f;

			SetAnimation("IDLE");

			#region Set default variables
			this.myGame = myGame;
			this.movementType = movementType;
			this.CanShoot = true;
			#endregion

			if (movementType == EnemyType.BIRD)
			{
				RotationCenter = new Vector2(CurrentAnimation.frameSize.X / 2, CurrentAnimation.frameSize.Y / 2);
				Offset = new Vector2(-(CurrentAnimation.frameSize.X / 2), -(CurrentAnimation.frameSize.Y / 2));
			}

			int width = (int)(CurrentAnimation.frameSize.X * Scale);
			int left = (int)(CurrentAnimation.frameSize.X * Scale - width);
			int height = (int)(CurrentAnimation.frameSize.Y * Scale);
			int top = (int)(CurrentAnimation.frameSize.Y * Scale - height);
			inbounds = new Rectangle(left, top, width, height);
		}

		public override void Update(GameTime gameTime)
		{
			HandleAnimations(gameTime);

			ApplyPhysics(gameTime);

			HandleHealth(gameTime);

			if (!isDead && IsGrounded)
			{
				if (Math.Abs(Velocity.X) - 0.02f > 0)
				{
					SetAnimation("WALK");
				}
				else
				{
					SetAnimation("IDLE");
				}
			}

			Movement = 0.0f;
			IsJumping = false;
		}

		protected void HandleHealth(GameTime gameTime)
		{
			if (HP <= 1)
			{
				isDead = true;
				myGame.gameManager.BloodMinRadius = 0;
				myGame.gameManager.BloodMaxRadius = 360;
				if (HP <= 0)
				{
					MainHP = 0;
				}
			}
		}

		protected override void HandleEnemyCollisions(GameTime gameTime)
		{
			foreach (Projectile p in myGame.gameManager.Player.ProjectileList)
			{
				if (BoundingCollisions.TouchLeftOf(p.projectileRectangle) || BoundingCollisions.TouchRightOf(p.projectileRectangle))
				{
					attackCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (PositionCenter.X >= p.projectileRectangle.X + (p.projectileRectangle.Width / 2))
					{
						IsJumping = true;
						Movement += 1;
						velocity.X = MaxMoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * Movement;
						velocity.Y = DoJump(velocity.Y, gameTime);
					}
					else if (PositionCenter.X < p.projectileRectangle.X + (p.projectileRectangle.Width / 2))
					{
						IsJumping = true;
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
						myGame.gameManager.BloodMinRadius = 330;
						myGame.gameManager.BloodMaxRadius = 350;
						isHit = true;
						MainHP -= myGame.gameManager.Player.Damage;
					}
					if (p.projectileRectangle.TouchRightOf(BoundingCollisions))
					{
						myGame.gameManager.BloodMinRadius = 180;
						myGame.gameManager.BloodMaxRadius = 200;
						isHit = true;
						MainHP -= myGame.gameManager.Player.Damage;
					}
				}
			}
		}

		protected override void HandleCollisions(GameTime gameTime)
		{
			if (movementType != EnemyType.SLIMEBALL && movementType != EnemyType.BIRD)
			{
				foreach (Tile t in myGame.gameManager.TilesList)
				{
					if (t.TileType == Tile.TileCollisions.Impassable)
					{
						if (BoundingCollisions.TouchLeftOf(t.BoundingCollisions) || BoundingCollisions.TouchRightOf(t.BoundingCollisions))
						{
							isJumping2 = true;
						}
					}
				}
			}

			base.HandleCollisions(gameTime);
		}

		public override void ApplyPhysics(GameTime gameTime)
		{
			TempVelocity = new Vector2(myGame.gameManager.Player.PositionCenter.X - PositionCenter.X, myGame.gameManager.Player.PositionCenter.Y - PositionCenter.Y);

			if (Math.Abs(Movement) < 0.5f)
			{
				Movement = 0.0f;
			}

			if (CollisionHelper.Magnitude(TempVelocity) <= 200)
			{
				if (myGame.gameManager.Player.PositionCenter.X - PositionCenter.X < 0)
				{
					Movement = -1;
				}
				else if (myGame.gameManager.Player.PositionCenter.X - PositionCenter.X > 0)
				{
					Movement = 1;
				}
				else if (myGame.gameManager.Player.PositionCenter.X == PositionCenter.X)
				{
					Movement = 0;
				}

				if (movementType != EnemyType.SLIMEBALL)
				{
					SetAnimation("WALK");
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
					SetAnimation("IDLE");
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
					IsJumping = true;
				}
				if (JumpDelayTimer <= 0)
				{
					IsJumping = false;
				}
				if (JumpDelayTimer < -200)
				{
					isJumping2 = false;
					JumpDelayTimer = 500;
				}
			}

			base.ApplyPhysics(gameTime);
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(60, 120), new Point(1, 1), new Point(0,   0), 1600, false);
			AddAnimation("WALK", texture, new Point(60, 120), new Point(8, 1), new Point(0,   0), 50, true);
			AddAnimation("JUMP", texture, new Point(60, 120), new Point(3, 1), new Point(240, 0), 50, true);
			SetAnimation("IDLE");

			base.AddAnimations(texture);
		}
	}
}
