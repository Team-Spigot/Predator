using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using VoidEngine.Helpers;
using VoidEngine.VGame;

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
			SPITTER,
			ROLLER,
			JUMPER,
			BOSS,
			NONE
		}

		public List<SlimeBullet> EnemyProjectileList = new List<SlimeBullet>();
		public bool RollerStart;
		public bool Moveleft;
		public bool IsAttacking;
		public int EnemyAttackDelay;

		public bool bossCharge = false;
		public bool bossStuck = false;
		public bool bossNormalAttack = true;
		public int bossNormalTimer = 5000;
		public int bossStuckTimer = 2000;
		public bool lockBehavior = false;
		public int spitterSpitting = 500;
		public int spitterReposition = 1500;
		public int bossPowerUp = 1000;

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

			EStrength = 3;


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

			if (enemyType == EnemyTypes.CHARGER)
			{
				GroundDragFactor = 0.35f;
			}

			dWaterCheckTimer = WaterCheckTimer = 800;

			int width = (int)(CurrentAnimation.frameSize.X * Scale);
			int left = (int)(CurrentAnimation.frameSize.X * Scale - width);
			int height = (int)(CurrentAnimation.frameSize.Y * Scale);
			int top = (int)(CurrentAnimation.frameSize.Y * Scale - height);
			inbounds = new Rectangle(left, top, width, height);

			EStrength = 3;
		}

		/// <summary>
		/// Updates the enemy class.
		/// </summary>
		/// <param name="gameTime">The game time that the game uses.</param>
		public override void Update(GameTime gameTime)
		{
			EStrength = (EStrength * 3) * myGame.gameManager.EnemyLevel;
			KeyboardState = Keyboard.GetState();
			if (Math.Abs(Movement) < 0.5f)
			{
				Movement = 0.0f;
			}
			HandleAnimations(gameTime);

			ApplyPhysics(gameTime);

			HandleHealth(gameTime);

			HandleEnemyProjectile(gameTime);

			SpitterAttack(gameTime);

			for (int i = 0; i < EnemyProjectileList.Count; i++)
			{
				if (EnemyProjectileList[i].deleteMe)
				{
					EnemyProjectileList.RemoveAt(i);

					i--;
				}
				else
				{
					EnemyProjectileList[i].Update(gameTime);
				}
			}


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
			for (int i = 0; i < myGame.gameManager.Player.ProjectileList.Count; i++)
			{
				if (BoundingCollisions.TouchLeftOf(myGame.gameManager.Player.ProjectileList[i].BoundingCollisions) || BoundingCollisions.TouchRightOf(myGame.gameManager.Player.ProjectileList[i].BoundingCollisions))
				{
					AttackCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (BoundingCollisions.TouchRightOf(myGame.gameManager.Player.ProjectileList[i].BoundingCollisions))
					{
						IsJumping = true;
						Movement += -1;
						velocity.X = MaxMoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * Movement;
						velocity.Y = DoJump(velocity.Y, gameTime);
						myGame.gameManager.BloodMinRadius = myGame.rng.Next(145, 190);
						myGame.gameManager.BloodMaxRadius = myGame.rng.Next(205, 275);
						IsHit = true;
						MainHP -= myGame.gameManager.Player.PStrength;
						myGame.gameManager.Player.ProjectileList.RemoveAt(i);
						i--;
					}
					else if (BoundingCollisions.TouchLeftOf(myGame.gameManager.Player.ProjectileList[i].BoundingCollisions))
					{
						IsJumping = true;
						Movement += 1;
						velocity.X = MaxMoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * Movement;
						velocity.Y = DoJump(velocity.Y, gameTime);
						myGame.gameManager.BloodMinRadius = myGame.rng.Next(290, 345);
						myGame.gameManager.BloodMaxRadius = myGame.rng.Next(370, 415);
						IsHit = true;
						MainHP -= myGame.gameManager.Player.PStrength;
						myGame.gameManager.Player.ProjectileList.RemoveAt(i);
						i--;
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
			foreach (Tile t in myGame.gameManager.TilesList)
			{
				if (CheckInRadius(t.Position, 240))
				{
					if (t.TileType == Tile.TileCollisions.Impassable)
					{
						if (BoundingCollisions.TouchTopOf(t.BoundingCollisions) && t.TileType == Tile.TileCollisions.Impassable)
						{
							IsGrounded = true;
							position.Y = t.Position.Y - (CurrentAnimation.frameSize.Y * Scale);
							DebugBlock = t.BoundingCollisions;
						}
						if (BoundingCollisions.TouchLeftOf(t.BoundingCollisions) && t.TileType == Tile.TileCollisions.Impassable)
						{
							position.X = t.Position.X - BoundingCollisions.Width;
							DebugBlock = t.BoundingCollisions;
						}
						if (BoundingCollisions.TouchRightOf(t.BoundingCollisions) && t.TileType == Tile.TileCollisions.Impassable)
						{
							position.X = t.BoundingCollisions.Right;
							DebugBlock = t.BoundingCollisions;
						}
						if (BoundingCollisions.TouchBottomOf(t.BoundingCollisions) && t.TileType == Tile.TileCollisions.Impassable)
						{
							IsJumping = false;
							JumpTime = 0;
							position.Y = t.BoundingCollisions.Bottom + 2;
							DebugBlock = t.BoundingCollisions;
						}
						if (BoundingCollisions.TouchLeftOf(t.BoundingCollisions) || BoundingCollisions.TouchRightOf(t.BoundingCollisions))
						{
							//IsJumping = true;
						}
					}

					if (BoundingCollisions.TouchTopOf(t.BoundingCollisions) && t.TileType == Tile.TileCollisions.Water)
					{
						IsInWater = true;
					}

					if (IsInWater == true)
					{
						WaterCheckTimer -= gameTime.ElapsedGameTime.Milliseconds;
					}

					if (WaterCheckTimer <= 0)
					{
						IsInWater = false;
						WaterChecksTillDeath += 1;
						WaterCheckTimer = 0;
					}

					if (WaterChecksTillDeath >= 1)
					{
						WaterChecksTillDeath = 0;
						MainHP -= 10;
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
			if (CanMove == true && EnemyType != EnemyTypes.ROLLER && EnemyType != EnemyTypes.BOSS && EnemyType != EnemyTypes.SPITTER)
			{
				MainHP = 15;
				MaxHP = 15;
				if (CollisionHelper.Magnitude(TempVelocity) <= 800)
				{
					if (myGame.gameManager.Player.PositionCenter.X - PositionCenter.X < 0)
					{
						Movement = -1;
						PlayerDetected = true;
					}
					else if (myGame.gameManager.Player.PositionCenter.X - PositionCenter.X > 0)
					{
						Movement = 1;
						PlayerDetected = true;
					}
					else if (myGame.gameManager.Player.PositionCenter.X == PositionCenter.X)
					{
						Movement = 0;
						PlayerDetected = false;
					}
					if (myGame.gameManager.Player.PositionCenter.Y - PositionCenter.Y > 0)
					{
						Movement = 0;
						PlayerDetected = false;
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
			}

			HandleEnemyCollisions(gameTime);
			HandleCollisions(gameTime);
			SlimeDirection(gameTime);

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

			if (EnemyType == EnemyTypes.ROLLER)
			{
				MainHP = 10;
				MaxHP = 10;
				RotationCenter = new Vector2(CurrentAnimation.frameSize.X / 2, CurrentAnimation.frameSize.Y / 2);
				Offset = new Vector2(-(CurrentAnimation.frameSize.X / 4), -(CurrentAnimation.frameSize.Y / 4));
				//Rotation += 180;

				if (RollerStart == true)
				{
					Movement = 1;
				}
				foreach (Tile t in myGame.gameManager.TilesList)
				{
					if (t.TileType == Tile.TileCollisions.Impassable)
					{
						if (BoundingCollisions.TouchLeftOf(t.BoundingCollisions))
						{
							RollerStart = false;
							Moveleft = true;
							//Rotation += myGame.rng.Next(-180, 180) * (float)Math.PI / 180;
							//Scale = (float)myGame.rng.Next(0, 10);
							SetAnimation("WALK");
						}
						if (BoundingCollisions.TouchRightOf(t.BoundingCollisions))
						{
							RollerStart = false;
							Moveleft = false;
							//Rotation -= myGame.rng.Next(-180, 180) * (float)Math.PI / 180;
							//Scale = (float)myGame.rng.Next(0, 10);
							SetAnimation("WALK");
						}
					}
				}

				if (Moveleft == true)
				{
					Movement = -1;
				}
				else
				{
					Movement = 1;
				}
			}

			if (EnemyType == EnemyTypes.JUMPER)
			{
				MainHP = 15;
				MaxHP = 15;
				MaxMoveSpeed = 200;
				if (CollisionHelper.Magnitude(TempVelocity) <= 300)
				{
					if (PlayerDetected == true)
					{
						IsJumping = true;
						//GroundDragFactor = 0.01f;
						//AirDragFactor = 4f;
					}

					if (PlayerDetected == false)
					{
						IsJumping = false;
					}
				}
			}

			if (EnemyType == EnemyTypes.BOSS)
			{
				MainHP = 150;
				MaxHP = 150;
				if (bossStuck == true)
				{
					bossStuckTimer -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;

					Movement = 0;
					if (bossStuckTimer <= 0)
					{
						bossNormalAttack = true;
						bossStuck = false;
						position.X -= 5;
						bossStuckTimer = myGame.rng.Next(1500, 3000);
					}
				}
				if (bossCharge == true)
				{
					GroundDragFactor = 1f;
					MaxMoveSpeed = 250;
					if (CollisionHelper.Magnitude(TempVelocity) <= 1024)
					{
						foreach (Tile t in myGame.gameManager.TilesList)
						{
							if (CheckInRadius(t.Position, 120))
							{
								if (t.TileType == Tile.TileCollisions.Impassable)
								{
									if (myGame.gameManager.Player.PositionCenter.X - PositionCenter.X < 0 && lockBehavior == false && bossStuck == false)
									{
										Movement = -1;
										PlayerDetected = true;
										lockBehavior = true;
										Console.WriteLine("Stuck pre-if1");
									}
									else if (myGame.gameManager.Player.PositionCenter.X - PositionCenter.X > 0 && lockBehavior == false && bossStuck == false)
									{
										Movement = 1;
										PlayerDetected = true;
										lockBehavior = true;
										Console.WriteLine("Stuck pre-if2");
									}
									else if (lockBehavior && bossCharge)
									{
										if (BoundingCollisions.TouchLeftOf(t.BoundingCollisions) && t.TileType == Tile.TileCollisions.Impassable)
										{
											bossStuck = true;
											bossCharge = false;
											lockBehavior = false;
											Console.WriteLine("IM STUCK");
										}
										if (BoundingCollisions.TouchRightOf(t.BoundingCollisions) && t.TileType == Tile.TileCollisions.Impassable)
										{
											bossStuck = true;
											bossCharge = false;
											lockBehavior = false;
											Console.WriteLine("IM STUCK");
										}
									}
								}
							}
						}
					}
				}

				if (bossNormalAttack == true)
				{
					if (bossNormalTimer <= 0)
					{
						bossNormalAttack = false;
						bossCharge = true;
						bossNormalTimer = myGame.rng.Next(4500, 7000);
					}
					GroundDragFactor = 1f;
					MaxMoveSpeed = 120;
					bossNormalTimer -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
					if (myGame.gameManager.Player.PositionCenter.X - PositionCenter.X < 0)
					{
						Movement = -1;
						PlayerDetected = true;
					}
					else if (myGame.gameManager.Player.PositionCenter.X - PositionCenter.X > 0)
					{
						Movement = 1;
						PlayerDetected = true;
					}
				}
			}
			base.ApplyPhysics(gameTime);
		}

		public void SpitterAttack(GameTime gameTime)
		{
			if (EnemyType == EnemyTypes.SPITTER)
			{
				CanMove = false;
				IsAttacking = true;
			}
		}

		protected Vector2 SlimeDirection(GameTime gametime)
		{
			if (myGame.gameManager.Player.Position.X < position.X)
			{
				return new Vector2(-1, 0);
			}
			else
			{
				return new Vector2(1, 0);

			}
		}

		protected virtual void HandleEnemyProjectile(GameTime gameTime)
		{
			if (EnemyType == EnemyTypes.SPITTER)
			{
				MainHP = 15;
				MaxHP = 15;

				EnemyAttackDelay -= gameTime.ElapsedGameTime.Milliseconds;
				if (EnemyAttackDelay == 0)
				{
					CanShoot = true;
				}
				if (CanShoot)
				{
					spitterSpitting -= gameTime.ElapsedGameTime.Milliseconds;
					if (spitterSpitting >= 0)
					{
						SetAnimation("SHOOT");
						Movement = 0;
					}
					if (spitterSpitting <= 0)
					{
						EnemyProjectileList.Add(new SlimeBullet(myGame.gameManager.SlimeTexture, new Vector2(Position.X + (CurrentAnimation.frameSize.X / 2), Position.Y + (CurrentAnimation.frameSize.Y / 6)), 4, SlimeDirection(gameTime), Color.LimeGreen, myGame));
						EnemyAttackDelay--;
						CanShoot = false;
						IsShooting = false;
						spitterSpitting = 500;
						if (CollisionHelper.Magnitude(TempVelocity) <= 800)
						{
							if (myGame.gameManager.Player.PositionCenter.X - PositionCenter.X < 0)
							{
								Movement = -1;
								PlayerDetected = true;
							}
							else if (myGame.gameManager.Player.PositionCenter.X - PositionCenter.X > 0)
							{
								Movement = 1;
								PlayerDetected = true;
							}
						}
					}
				}
				if (EnemyAttackDelay <= 0)
				{
					EnemyAttackDelay = 1000;
					CanShoot = true;
				}
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (SlimeBullet e in EnemyProjectileList)
			{
				e.Draw(gameTime, spriteBatch);
			}
			base.Draw(gameTime, spriteBatch);
		}

		/// <summary>
		/// Adds animations for the enemy.
		/// </summary>
		/// <param name="texture">The spritesheet texture to process when creating the animations.</param>
		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(texture.Width / 7, texture.Height / 2), new Point(1, 1), new Point(0, 0), 16000, false);
			AddAnimation("WALK", texture, new Point(texture.Width / 7, texture.Height / 2), new Point(6, 1), new Point(120, 0), 80, true);

			SetAnimation("IDLE");

			base.AddAnimations(texture);
		}
	}
}
