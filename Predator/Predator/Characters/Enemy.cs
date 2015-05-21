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
            this.EnemyType = enemyType;
            this.CanShoot = true;
            Level = 1;
            JumpbackTimer = 0;
            MaxMoveSpeed = 175;
            MaxHP = MainHP = 150;
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
            JumpbackTimer = 0;
            MaxMoveSpeed = 175;
            MaxHP = MainHP = 150;
            GroundDragFactor = 0.46f;
            AirDragFactor = 0.50f;
			#endregion

            //CanMove = false;

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
					if (BoundingCollisions.TouchRightOf(p.BoundingCollisions))
					{
						IsJumping = true;
						Movement += -1;
                        velocity.X = MoveAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds * Movement;
						velocity.Y = DoJump(velocity.Y, gameTime);
						myGame.gameManager.BloodMinRadius = 330;
						myGame.gameManager.BloodMaxRadius = 400;
						IsHit = true;
						MainHP -= myGame.gameManager.Player.PStrength;
					}
					else if (BoundingCollisions.TouchLeftOf(p.BoundingCollisions))
					{
						IsJumping = true;
						Movement += 1;
                        velocity.X = MoveAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds * Movement;
						velocity.Y = DoJump(velocity.Y, gameTime);
						myGame.gameManager.BloodMinRadius = 180;
						myGame.gameManager.BloodMaxRadius = 250;
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
			foreach (Tile t in myGame.gameManager.TilesList)
			{
				if (CheckInRadius(t.Position, 120))
				{
					if (t.TileType == Tile.TileCollisions.Impassable)
					{
						if (BoundingCollisions.TouchTopOf(t.BoundingCollisions) && t.TileType != Tile.TileCollisions.Passable)
						{
							IsGrounded = true;
							position.Y = t.Position.Y - BoundingCollisions.Height;
							DebugBlock = t.BoundingCollisions;
						}
						if (BoundingCollisions.TouchLeftOf(t.BoundingCollisions) && t.TileType == Tile.TileCollisions.Impassable)
						{
							position.X = t.Position.X - BoundingCollisions.Width - 1;
							DebugBlock = t.BoundingCollisions;
						}
						if (BoundingCollisions.TouchRightOf(t.BoundingCollisions) && t.TileType == Tile.TileCollisions.Impassable)
						{
							position.X = t.BoundingCollisions.Right + 1;
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
				}
            }

            foreach (Rectangle r in myGame.gameManager.MapBoundries)
            {
                if (r.TouchBottomOf(BoundingCollisions))
                {
                    FellFromBottom = true;
                    DebugBlock = r;
                }
                if (r.TouchRightOf(BoundingCollisions))
                {
                    position.X = r.Left - BoundingCollisions.Width - 1;
                    DebugBlock = r;
                }
                else if (r.TouchLeftOf(BoundingCollisions))
                {
                    position.X = r.Right + 1;
                    DebugBlock = r;
                }
                if (r.TouchTopOf(BoundingCollisions))
                {
                    IsJumping = false;
                    JumpTime = 0;
                    position.Y = r.Bottom + 2;
                    DebugBlock = r;
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
			if (CanMove == true && EnemyType != EnemyTypes.ROLLER)
			{
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

            }

			base.ApplyPhysics(gameTime);
		}

        public void SpitterAttack(GameTime gameTime)
        {
            if (EnemyType == EnemyTypes.SPITTER)
            {
                CanMove = true;
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
                
                EnemyAttackDelay -= gameTime.ElapsedGameTime.Milliseconds;
                if (EnemyAttackDelay == 0)
                {
                    CanShoot = true;
                }
                if (CanShoot)
                {
                    EnemyProjectileList.Add(new SlimeBullet(myGame.gameManager.SlimeTexture, new Vector2 (Position.X + (CurrentAnimation.frameSize.X /2) , Position.Y + (CurrentAnimation.frameSize.Y / 6)), 4, SlimeDirection(gameTime), Color.LimeGreen, myGame));
                    EnemyAttackDelay--;
                    CanShoot = false;
                    IsShooting = false;
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
