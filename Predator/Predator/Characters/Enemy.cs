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
		/// <summary>
		/// The enum to tell what type the enemy is.
		/// </summary>
		public enum EnemyTypes
		{
			BIRD,
			SLIME,
			SLIMEBALL,
			RAT,
			CHARGER,
			NONE
		}

		#region General Enemy Stuff
		/// <summary>
		/// Gets or sets if the Enemy was hit.
		/// </summary>
		public bool IsHit
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the enemy should be deleted.
		/// </summary>
		public bool DeleteMe
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the enemy has droped a pickup.
		/// </summary>
		public bool DropPickUp
		{
			get;
			set;
		}
		#endregion

		#region Movement
		/// <summary>
		/// Sets if the enemy can move.
		/// </summary>
		public bool CanMove = true;
		/// <summary>
		/// The variable to tell what type the enemy is.
		/// </summary>
		public EnemyTypes EnemyType;
		/// <summary>
		/// The velocity to tell where the player is from the enemy.
		/// </summary>
		public Vector2 TempVelocity;
		/// <summary>
		/// The delay for the enemt to jump back.
		/// </summary>
		public float JumpDelayTimer = 500;
		/// <summary>
		/// Gets or sets if the enemy is knocked back.
		/// </summary>
		public bool KnockedBack
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player is detected.
		/// </summary>
		public bool PlayerDetected
		{
			get;
			set;
		}
		#endregion

		#region Stats
		/// <summary>
		/// Gets or sets the enemy's strength stat.
		/// </summary>
		public float EStrength
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the enemy's agility stat.
		/// </summary>
		public float EAgility
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the enemy's defense stat.
		/// </summary>
		public float EDefense
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// Creates the enemy object, with the default animation set.
		/// </summary>
		/// <param name="texture">The spritesheet to use with the enemy.</param>
		/// <param name="position">The position to start the enemy at.</param>
		/// <param name="enemyType">The type of the enemy.</param>
		/// <param name="color">The color to mask the enemy with.</param>
		/// <param name="myGame">The game the enemy will run on.</param>
		public Enemy(Texture2D texture, Vector2 position, EnemyTypes enemyType, Color color, Game1 myGame)
			: base(texture, position, color)
		{
			this.myGame = myGame;

			AddAnimations(texture);
			SetAnimation("IDLE");

			#region Set default variables
			EnemyType = enemyType;
			CanShoot = true;
			Level = 1;
			MainHP = 100;
			MaxHP = 100;
			JumpbackTimer = 0;
			MaxMoveSpeed = 175;
			GroundDragFactor = 0.46f;
			AirDragFactor = 0.50f;
			#endregion

			if (enemyType == EnemyTypes.BIRD)
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

		/// <summary>
		/// Creats the enemy class, with a custom animation set.
		/// </summary>
		/// <param name="animationSetList">The custom animation set to animate the enemy with.</param>
		/// <param name="position">The position to start the enemy at.</param>
		/// <param name="enemyType">The type of the enemy.</param>
		/// <param name="color">The color to mask the enemy with.</param>
		/// <param name="myGame">The game that the enemy runs on.</param>
		public Enemy(List<AnimationSet> animationSetList, string defaultFrameName, Vector2 position, EnemyTypes enemyType, Color color, Game1 myGame)
			: base(animationSetList, defaultFrameName, position, color)
		{
			this.myGame = myGame;
			AnimationSets = animationSetList;
			SetAnimation(defaultFrameName);

			#region Set default variables
			this.EnemyType = enemyType;
			this.CanShoot = true;
			Level = 1;
			MainHP = 100;
			MaxHP = 100;
			JumpbackTimer = 0;
			MaxMoveSpeed = 175;
			GroundDragFactor = 0.46f;
			AirDragFactor = 0.50f;
			#endregion

			if (enemyType == EnemyTypes.BIRD)
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

		/// <summary>
		/// Updates the enemy class.
		/// </summary>
		/// <param name="gameTime">The game time that the game uses.</param>
		public override void Update(GameTime gameTime)
		{
			if (Math.Abs(Movement) < 0.5f)
			{
				Movement = 0.0f;
			}

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

		/// <summary>
		/// Handles the health for the enemy.
		/// </summary>
		/// <param name="gameTime">The game time that the game uses.</param>
		protected override void HandleHealth(GameTime gameTime)
		{
			if (HP <= 1)
			{
				isDead = true;
				myGame.gameManager.BloodMinRadius = 0;
				myGame.gameManager.BloodMaxRadius = 360;
				DeleteMe = true;

				if (HP <= 0)
				{
					MainHP = 0;
				}
				if (DropPickUp)
				{
					DropPickUp = false;
				}
			}
		}

		/// <summary>
		/// Handles the 
		/// </summary>
		/// <param name="gameTime">The game time that the game uses.</param>
		protected override void HandleEnemyCollisions(GameTime gameTime)
		{
			foreach (Projectile p in myGame.gameManager.Player.ProjectileList)
			{
				if (BoundingCollisions.TouchLeftOf(p.BoundingCollisions) || BoundingCollisions.TouchRightOf(p.BoundingCollisions))
				{
					AttackCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (PositionCenter.X >= p.BoundingCollisions.X + (p.BoundingCollisions.Width / 2))
					{
						IsJumping = true;
						Movement += -1;
						velocity.X = MaxMoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * Movement;
						velocity.Y = DoJump(velocity.Y, gameTime);
						myGame.gameManager.BloodMinRadius = 330;
						myGame.gameManager.BloodMaxRadius = 350;
						IsHit = true;
						MainHP -= myGame.gameManager.Player.PStrength;
					}
					else if (PositionCenter.X < p.BoundingCollisions.X + (p.BoundingCollisions.Width / 2))
					{
						IsJumping = true;
						Movement += 1;
						velocity.X = MaxMoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * Movement;
						velocity.Y = DoJump(velocity.Y, gameTime);
						myGame.gameManager.BloodMinRadius = 180;
						myGame.gameManager.BloodMaxRadius = 200;
						IsHit = true;
						MainHP -= myGame.gameManager.Player.PStrength;
					}
					if (MainHP <= 0)
					{
						isDead = true;
					}
				}
			}
		}

		/// <summary>
		/// Handles collisions with the tiles and other things.
		/// </summary>
		/// <param name="gameTime">The game time that the game uses.</param>
		protected override void HandleCollisions(GameTime gameTime)
		{
			if (EnemyType != EnemyTypes.SLIMEBALL && EnemyType != EnemyTypes.BIRD)
			{
				foreach (Tile t in myGame.gameManager.TilesList)
				{
					if (CheckInRadius(t.Position, 55))
					{
						if (t.TileType == Tile.TileCollisions.Impassable)
						{
							if (BoundingCollisions.TouchLeftOf(t.BoundingCollisions) || BoundingCollisions.TouchRightOf(t.BoundingCollisions))
							{
								IsJumping = true;
							}
						}
					}
				}
			}

			base.HandleCollisions(gameTime);
		}

		/// <summary>
		/// Applies physics to the enemy. (USE base.ApplyPhysics(gameTime); !!!)
		/// </summary>
		/// <param name="gameTime">The game time that the game uses.</param>
		public override void ApplyPhysics(GameTime gameTime)
		{
			TempVelocity = new Vector2(myGame.gameManager.Player.PositionCenter.X - PositionCenter.X, myGame.gameManager.Player.PositionCenter.Y - PositionCenter.Y);

			if (CollisionHelper.Magnitude(TempVelocity) <= 400)
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

				if (EnemyType != EnemyTypes.SLIMEBALL)
				{
					SetAnimation("WALK");
				}

				if (EnemyType == EnemyTypes.BIRD)
				{
					Rotation += 0.05f;
				}
			}
			else
			{
				if (EnemyType != EnemyTypes.SLIMEBALL)
				{
					SetAnimation("IDLE");
				}
			}

			HandleEnemyCollisions(gameTime);
			HandleCollisions(gameTime);

			if (EnemyType == EnemyTypes.SLIMEBALL)
			{
				IsJumping = true;
			}

			if (IsJumping)
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
					IsJumping = false;
					JumpDelayTimer = 500;
				}
			}

			base.ApplyPhysics(gameTime);
		}

		/// <summary>
		/// Adds animations for the enemy.
		/// </summary>
		/// <param name="texture">The spritesheet texture to process when creating the animations.</param>
		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(120, 60), new Point(1, 1), new Point(360, 000), 1600, false);
			AddAnimation("WALK", texture, new Point(120, 60), new Point(2, 2), new Point(0, 000), 100, true);


			SetAnimation("IDLE");

			base.AddAnimations(texture);
		}
	}
}
