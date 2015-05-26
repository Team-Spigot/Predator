using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VoidEngine.VGame;
using VoidEngine.Helpers;

namespace Predator.Characters
{
	public class Player : Sprite
	{
		protected Game1 myGame;

		#region Player Stats
		/// <summary>
		/// Gets or sets the player's level.
		/// </summary>
		public int Level
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player is dead.
		/// </summary>
		public bool isDead
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the player's HP in float.
		/// </summary>
		public float MainHP
		{
			get;
			set;
		}
		/// <summary>
		/// Gets the player's HP rounded down.
		/// </summary>
		public int HP
		{
			get
			{
				return (int)MainHP;
			}
		}
		/// <summary>
		/// Gets or sets the Max HP of the player.
		/// </summary>
		public float MaxHP
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player fell out the map.
		/// </summary>
		public bool FellFromBottom
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the player's damage stat.
		/// </summary>
		public float Damage
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the player's defense stat.
		/// </summary>
		public float Defense
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the player's stat points.
		/// </summary>
		public int StatPoints
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the player's points to strength.
		/// </summary>
		public float PStrength
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the player's points to agility.
		/// </summary>
		public float PAgility
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the player's points to defense.
		/// </summary>
		public float PDefense
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the player's points to experiance.
		/// </summary>
		public int PExp
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the player's previous points to experiance.
		/// </summary>
		public int PreviousPExp
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the player's stat level.
		/// </summary>
		public int Lvl
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player is allowed to level up.
		/// </summary>
		public bool LvlUp
		{
			get;
			set;
		}
		#endregion

		#region Movement Stats
		/// <summary>
		/// Gets the players center based off of it's posititon.
		/// </summary>
		public Vector2 PositionCenter
		{
			get
			{
				return new Vector2(BoundingCollisions.Center.X, BoundingCollisions.Center.Y);
			}
		}
		/// <summary>
		/// Gets or sets the keys to move the player with.
		/// </summary>
		public Keys[,] MovementKeys
		{
			set;
			get;
		}
		/// <summary>
		/// The keyboardState that the player can detect it's keys with.
		/// </summary>
		public KeyboardState KeyboardState;
		/// <summary>
		/// Gets or sets the velocity to move the player at.
		/// </summary>
		public Vector2 Velocity
		{
			get
			{
				return velocity;
			}
			protected set
			{
				velocity = value;
			}
		}
		/// <summary>
		/// The velocity to move the player at.
		/// </summary>
		protected Vector2 velocity;

		#region Constants for controling horizontal movement
		/// <summary>
		/// The acceleration to move the player at.
		/// </summary>
		protected float MoveAcceleration = 13000.0f;
		/// <summary>
		/// The max amount of speed the player can move at.
		/// </summary>
		protected float MaxMoveSpeed = 1750.0f;
		/// <summary>
		/// The friction that the player is experiencing when on the ground.
		/// </summary>
		protected float GroundDragFactor = 0.50f;
		/// <summary>
		/// The friction that the player is experiencing when in the air.
		/// </summary>
		protected float AirDragFactor = 0.58f;
		/// <summary>
		///
		/// </summary>
		protected float JumpbackSpeed = 50000.0f;
		#endregion

		#region Constants for controlling vertical movement
		/// <summary>
		/// The max time the player can jump.
		/// </summary>
		protected float MaxJumpTime = 0.35f;
		/// <summary>
		/// The velocity that the player jumps at.
		/// </summary>
		protected float JumpLaunchVelocity = -3500.0f;
		/// <summary>
		/// The acceleration of gravity that the player is effected by.
		/// </summary>
		protected float GravityAcceleration = 3400f;
		/// <summary>
		/// The terminal velocity of the player's fall speed.
		/// </summary>
		protected float MaxFallSpeed = 550.0f;
		/// <summary>
		/// The control power of the jump.
		/// </summary>
		protected float JumpControlPower = 0.14f;
		#endregion

		/// <summary>
		/// Gets or sets if the player is grounded.
		/// </summary>
		public bool IsGrounded
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets the movement direction of the player on the X axis.
		/// </summary>
		public float Movement
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the player is jumping.
		/// </summary>
		public bool IsJumping
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player was jumping.
		/// </summary>
		public bool WasJumping
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets the jumpTime.
		/// </summary>
		public float JumpTime
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets the Jumpback Timer.
		/// </summary>
		public int JumpbackTimer
		{
			get;
			protected set;
		}
		#endregion

		#region Debug Stuff
		/// <summary>
		/// Used to debug the block the player is currently touching.
		/// </summary>
		public Rectangle DebugBlock;
		#endregion

		#region Combat Stuff
		/// <summary>
		/// Gets or sets the attack counter for the player.
		/// </summary>
		public float AttackCounter
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the player's attack delay.
		/// </summary>
		public int AttackDelay
		{
			get;
			set;
		}

		#region Projectile Stuff
		/// <summary>
		/// Gets or sets the list of projectiles.
		/// </summary>
		public List<Projectile> ProjectileList
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets the list of AnimationSets that the projectile has.
		/// </summary>
		public Texture2D ProjectileTexture
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player is shooting.
		/// </summary>
		public bool IsShooting
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player can shoot.
		/// </summary>
		public bool CanShoot
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player can create a new projectile.
		/// </summary>
		public bool CreateNewProjectile
		{
			get;
			set;
		}
		#endregion

		#region Mana Stuff
		/// <summary>
		/// Gets or sets the amount of mana the player has.
		/// </summary>
		protected float Mana
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the maxium mana the player can have.
		/// </summary>
		protected float MaxMana
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the amount of time it takes the mana to start recharging.
		/// </summary>
		protected float ManaRechargeTime
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the default amount of time it takes the mana to start recharging.
		/// </summary>
		protected float DefaultManaRechargeTime
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the amount of mana it will rechage every second.
		/// </summary>
		protected float ManaRechargeInterval
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the default mana recharge time.
		/// </summary>
		protected float DefaultManaRechargeInterval
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the ammount of mana it will decrease by.
		/// </summary>
		protected float ManaDecreaseAmount
		{
			get;
			set;
		}
		#endregion
		#endregion

		/// <summary>
		/// Creates the player class, with default animation set.
		/// </summary>
		/// <param name="texture">The texture to use with the player.</param>
		/// <param name="position">The starting position to put the player at.</param>
		/// <param name="movementKeys">The list of movement keys for the player.</param>
		/// <param name="color">The color to mask the player with.</param>
		/// <param name="myGame">The game that the player runs on.</param>
		public Player(Texture2D texture, Vector2 position, Keys[,] movementKeys, Color color, Game1 myGame)
			: base(position, color, texture)
		{
			this.myGame = myGame;

			AddAnimations(texture);
			SetAnimation("IDLE1");

			AttackDelay = 750;
            JumpbackTimer = 1;

            Level = 1;
            StatPoints = 0;
            PExp = 0;
            PAgility = 3.375f;
            PDefense = 2;
            PStrength = 2;
            MainHP = MaxHP = 500;

			Mana = MaxMana = 0;
			ManaRechargeTime = DefaultManaRechargeTime = 0.09f;
			ManaRechargeInterval = DefaultManaRechargeInterval = 0.1f;
			ManaDecreaseAmount = 0f;

			#region Set Projectile Factors
			ProjectileList = new List<Projectile>();
			CanShoot = true;
			CreateNewProjectile = true;
			#endregion

			#region Set Movement and Collision Factors
			MovementKeys = movementKeys;

			int width = (int)(CurrentAnimation.frameSize.X);
			int left = (CurrentAnimation.frameSize.X - width);
			int height = (int)(CurrentAnimation.frameSize.Y);
			int top = CurrentAnimation.frameSize.Y - height;
			inbounds = new Rectangle(left, top, width, height);
			#endregion
		}

		/// <summary>
		/// Creats the player class, with custom animation set.
		/// </summary>
		/// <param name="animationSetList">The custom animation set to use with the player.</param>
		/// <param name="defaultFrameName">The default frame to set the animation to.</param>
		/// <param name="position">The position to set the player at.</param>
		/// <param name="movementKeys">The list movement keys for the player.</param>
		/// <param name="color">The color to mask the player with.</param>
		/// <param name="myGame">The game that player runs on.</param>
		public Player(List<AnimationSet> animationSetList, string defaultFrameName, Vector2 position, Keys[,] movementKeys, Color color, Game1 myGame)
			: base(position, color, animationSetList)
		{
			this.myGame = myGame;

			AnimationSets = animationSetList;
			SetAnimation(defaultFrameName);

			AttackDelay = 750;
            JumpbackTimer = 1;

            Level = 1;
            StatPoints = 0;
            PExp = 0;
            PAgility = 3.375f;
            PDefense = 2;
            PStrength = 2;
            MainHP = MaxHP = 500;

			Mana = MaxMana = 0;
			ManaRechargeTime = DefaultManaRechargeTime = 0.09f;
			ManaRechargeInterval = DefaultManaRechargeInterval = 0.1f;
			ManaDecreaseAmount = 0f;

			#region Set Projectile Factors
			ProjectileList = new List<Projectile>();
			CanShoot = true;
			CreateNewProjectile = true;
			#endregion

			#region Set Movement and Collision Factors
			MovementKeys = movementKeys;

			int width = (int)(CurrentAnimation.frameSize.X);
			int left = (CurrentAnimation.frameSize.X - width);
			int height = (int)(CurrentAnimation.frameSize.Y);
			int top = CurrentAnimation.frameSize.Y - height;
			inbounds = new Rectangle(left, top, width, height);
			#endregion
		}

		/// <summary>
		/// Creates the player class with the bare minimum.
		/// Used for making child class for the player.
		/// Projectile lists created already.
		/// </summary>
		/// <param name="position">The position to start the player at.</param>
		/// <param name="color">The color to mask the player with.</param>
		/// <param name="animationSetList">The animation set list for the player.</param>
		public Player(Texture2D texture, Vector2 position, Color color)
			: base(position, color, texture)
		{
			ProjectileList = new List<Projectile>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="animationSetList"></param>
		/// <param name="position"></param>
		/// <param name="color"></param>
		public Player(List<AnimationSet> animationSetList, string defaultFrameName, Vector2 position, Color color)
			: base(position, color, animationSetList)
		{
			ProjectileList = new List<Projectile>();
		}

		/// <summary>
		/// Updates the keyboard state seprately.
		/// </summary>
		/// <param name="gameTime">The GameTime that the game runs off of.</param>
		/// <param name="keyboardState">The game's KeyboardState.</param>
		public void UpdateKeyboardState(GameTime gameTime, KeyboardState keyboardState)
		{
			this.KeyboardState = keyboardState;
		}

		/// <summary>
		/// Updates the events in the player class.
		/// </summary>
		/// <param name="gameTime">The class GameTime that the game runs off of.</param>
		/// <param name="EnemyList">The Enemy list.</param>
		/// <param name="TileList">The list of Tiles.</param>
		/// <param name="MapBoundries">The list of Rectangles that make up the boundries of the world.</param>
		public override void Update(GameTime gameTime)
		{
			GetInput(gameTime);

			HandleAnimations(gameTime);

			HandleProjectile(gameTime);

			ApplyPhysics(gameTime);

			HandleHealth(gameTime);

			UpdateStats(gameTime);

			foreach (Projectile p in ProjectileList)
			{
				p.Update(gameTime);
			}

			if (!isDead && IsGrounded)
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
			IsJumping = false;
			//isShooting = false;
		}

		/// <summary>
		/// To draw the Player class.
		/// </summary>
		/// <param name="gameTime">To keep track of run time.</param>
		/// <param name="spriteBatch">The spriteBatch to draw with.</param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (Projectile p in ProjectileList)
			{
				p.Draw(gameTime, spriteBatch);
			}

			if (Velocity.X > 0)
			{
				FlipSprite(Axis.NONE);
			}
			else if (Velocity.X < 0)
			{
				FlipSprite(Axis.Y);
			}

			base.Draw(gameTime, spriteBatch);
		}

		public virtual void UpdateStats(GameTime gameTime)
        {
            if (LvlUp == true)
            {
                StatPoints += 3;
                LvlUp = false;
            }

            #region Level Table
            if (PExp >= 0 && Lvl == 0)
            {
                Lvl = 1;
                LvlUp = true;
            }
            if (PExp >= 1000 && Lvl == 1)
            {
                Lvl = 2;
                LvlUp = true;
            }
            if (PExp >= 2000 && Lvl == 2)
            {
                Lvl = 3;
                LvlUp = true;
            }
            if (PExp >= 4000 && Lvl == 3)
            {
                Lvl = 4;
                LvlUp = true;
            }
            if (PExp >= 5000 && Lvl == 4)
            {
                Lvl = 5;
                LvlUp = true;
            }
            if (PExp >= 6000 && Lvl == 5)
            {
                Lvl = 6;
                LvlUp = true;
            }
            if (PExp >= 7000 && Lvl == 6)
            {
                Lvl = 7;
                LvlUp = true;
            }
            if (PExp >= 8000 && Lvl == 7)
            {
                Lvl = 8;
                LvlUp = true;
            }
            if (PExp >= 9000 && Lvl == 8)
            {
                Lvl = 9;
                LvlUp = true;
            }
            if (PExp >= 10000 && Lvl == 9)
            {
                Lvl = 10;
                LvlUp = true;
            }
            if (PExp >= 11000 && Lvl == 10)
            {
                Lvl = 11;
                LvlUp = true;
            }
            if (PExp >= 12000 && Lvl == 11)
            {
                Lvl = 12;
                LvlUp = true;
            }
            if (PExp >= 13000 && Lvl == 12)
            {
                Lvl = 13;
                LvlUp = true;
            }
            if (PExp >= 14000 && Lvl == 13)
            {
                Lvl = 14;
                LvlUp = true;
            }
            if (PExp >= 15000 && Lvl == 14)
            {
                Lvl = 15;
                LvlUp = true;
            }
            if (PExp >= 16000 && Lvl == 15)
            {
                Lvl = 16;
                LvlUp = true;
            }
            if (PExp >= 17000 && Lvl == 16)
            {
                Lvl = 17;
                LvlUp = true;
            }
            if (PExp >= 18000 && Lvl == 17)
            {
                Lvl = 18;
                LvlUp = true;
            }
            if (PExp >= 19000 && Lvl == 18)
            {
                Lvl = 19;
                LvlUp = true;
            }
            if (PExp >= 20000 && Lvl == 19)
            {
                Lvl = 20;
                LvlUp = true;
            }
            #endregion

            if (KeyboardState.IsKeyDown(Keys.X) && KeyboardState.IsKeyDown(Keys.O) && KeyboardState.IsKeyDown(Keys.Multiply))
            {
                PExp += 1000;
            }

            MaxMoveSpeed *= PAgility;
            Damage = PStrength;
            Defense = PDefense;
            MaxHP = 200 + (PDefense * 150);
            GroundDragFactor = ((1.325f + (float)(PAgility * .02)) / PAgility);
            AirDragFactor = ((1.325f + (float)(PAgility * .02)) / PAgility);

            if (PAgility < 1.55)
            {
                PAgility = 1.55f;
            }
		}

		protected virtual void HandleHealth(GameTime gameTime)
		{
			if (MainHP <= 0)
			{
				isDead = true;
			}
			if (isDead == true)
			{
				ProjectileList.RemoveRange(0, ProjectileList.Count);
			}

			MainHP = MathHelper.Clamp(MainHP, 0, MaxHP);
		}

		/// <summary>
		/// Gets the keyboard input to control the player.
		/// </summary>
		protected virtual void GetInput(GameTime gameTime)
		{
			// Ignore small movements to prevent running in place.
			if (Math.Abs(Movement) < 0.5f)
			{
				Movement = 0.0f;
			}

			// If any digital horizontal movement input is found, override the analog movement.
			if (KeyboardState.IsKeyDown(MovementKeys[0, 0]) || KeyboardState.IsKeyDown(MovementKeys[1, 0]))
			{
				Movement += -1.0f;
			}
			else if (KeyboardState.IsKeyDown(MovementKeys[0, 2]) || KeyboardState.IsKeyDown(MovementKeys[1, 2]))
			{
				Movement += 1.0f;
			}
			if (KeyboardState.IsKeyDown(Keys.P))
			{
				MainHP += 1;
			}
			if (KeyboardState.IsKeyDown(Keys.L))
			{
				MainHP -= 1;
			}
            if (KeyboardState.IsKeyDown(Keys.N))
            {
                Movement = -1;
                velocity = new Vector2(MoveAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds * Movement);
                
            }

			Movement = MathHelper.Clamp(Movement, -1, 1);

			IsShooting = KeyboardState.IsKeyDown(MovementKeys[0, 4]);

			// Check if the player wants to jump.
			IsJumping = KeyboardState.IsKeyDown(MovementKeys[0, 1]) || KeyboardState.IsKeyDown(MovementKeys[1, 1]);
		}

		/// <summary>
		/// Updates the player's velocity and position based on input, gravity, etc.
		/// </summary>
		/// <param name="gameTime">The GameTime that the game runs off of.</param>
		public virtual void ApplyPhysics(GameTime gameTime)
		{
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			Vector2 previousPosition = Position;

			// Base velocity is a combination of horizontal movement control and
			// acceleration downward due to gravity.
			velocity.X += Movement * MoveAcceleration * elapsed;

			HandleEnemyCollisions(gameTime);

			if (!IsGrounded)
			{
				velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);
			}

			velocity.Y = DoJump(velocity.Y, gameTime);

			// Apply pseudo-drag horizontally.
			if (IsGrounded)
			{
				velocity.X *= GroundDragFactor;
			}
			else
			{
				velocity.X *= AirDragFactor;
			}

			// Prevent the player from running faster than his top speed.
			velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

			// Apply velocity.
			Position += velocity * elapsed;
			Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

			// If the player is now colliding with the level, separate them.
			HandleCollisions(gameTime);

			// If the collision stopped us from moving, reset the velocity to zero.
			if (Position.X == previousPosition.X)
			{
				velocity.X = 0;
			}

			if (Position.Y == previousPosition.Y)
			{
				velocity.Y = 0;
			}
		}

		/// <summary>
		/// Calculates the Y velocity accounting for jumping and
		/// animates accordingly.
		/// </summary>
		/// <remarks>
		/// During the accent of a jump, the Y velocity is completely
		/// overridden by a power curve. During the decent, gravity takes
		/// over. The jump velocity is controlled by the jumpTime field
		/// which measures time into the accent of the current jump.
		/// </remarks>
		/// <param name="velocityY">
		/// The player's current velocity along the Y axis.
		/// </param>
		/// <returns>
		/// A new Y velocity if beginning or continuing a jump.
		/// Otherwise, the existing Y velocity.
		/// </returns>
		protected virtual float DoJump(float velocityY, GameTime gameTime)
		{
			// If the player wants to jump
			if (IsJumping)
			{
				// Begin or continue a jump
				if ((!WasJumping && IsGrounded) || JumpTime > 0.0f)
				{
					if (JumpTime == 0.0f)
					{
						//jumpSound.Play();
					}

					JumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
					//SetAnimation("JUMP" + Level);
				}

				// If we are in the ascent of the jump
				if (0.0f < JumpTime && JumpTime <= MaxJumpTime)
				{
					// Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
					velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(JumpTime / MaxJumpTime, JumpControlPower));
				}
				else
				{
					// Reached the apex of the jump
					JumpTime = 0.0f;
				}
			}
			else
			{
				// Continues not jumping or cancels a jump in progress
				JumpTime = 0.0f;
			}
			WasJumping = IsJumping;

			return velocityY;
		}

		/// <summary>
		/// Handels the projectile for the player class.
		/// </summary>
		/// <param name="gameTime">The game time that the game runs.</param>
		protected virtual void HandleProjectile(GameTime gameTime)
		{
			AttackDelay -= gameTime.ElapsedGameTime.Milliseconds;
			if (AttackDelay == 0)
			{
				CanShoot = true;
			}
			if (IsShooting && CanShoot)
			{
				if (!isFlipped)
				{
					ProjectileList.Add(new Projectile(ProjectileTexture, new Vector2(Position.X + CurrentAnimation.frameSize.X, Position.Y + 15), velocity, Color.White, myGame));
				}
				else
				{
					ProjectileList.Add(new Projectile(ProjectileTexture, new Vector2(Position.X - 50, Position.Y + 15), velocity, Color.White, myGame));
				}
				AttackDelay--;
				CanShoot = false;
				IsShooting = false;
			}
			if (AttackDelay <= 0)
			{
				AttackDelay = 750;
				CanShoot = true;
				if (ProjectileList.Count > 0)
				{
					ProjectileList.RemoveAt(ProjectileList.Count - 1);
				}
			}
		}

		#region [OLD]
		//    attackDelay -= gameTime.ElapsedGameTime.Milliseconds;
		//    if (attackDelay == 0)
		//    {
		//        CanShoot = true;
		//    }
		//    if (IsShooting && CanShoot)
		//    {
		//        ProjectileList.Add(new Projectile(ProjectileTexture, Position, Color.White, myGame));
		//        attackDelay--;
		//        CanShoot = false;
		//
		//    }
		//    if (!CanShoot)
		//    {
		//        ManaRechargeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
		//
		//        if (ManaRechargeTime <= 0)
		//        {
		//            if (ProjectileList.Count > 0)
		//            {
		//                ProjectileList.RemoveAt(ProjectileList.Count - 1);
		//            }
		//
		//            ManaRechargeInterval -= (float)gameTime.ElapsedGameTime.TotalSeconds;
		//
		//            if (ManaRechargeInterval <= 0)
		//            {
		//                ManaRechargeTime = DefaultManaRechargeTime;
		//                ManaRechargeInterval = DefaultManaRechargeInterval;
		//                CanShoot = true;
		//            }
		//        }
		//    }
		//}
		#endregion

		/// <summary>
		/// Handles the enemy collisions for the player.
		/// </summary>
		/// <param name="gameTime">The game time that the game runs.</param>
		protected virtual void HandleEnemyCollisions(GameTime gameTime)
		{
			if (myGame.gameManager.EnemyList != null)
			{
				foreach (Enemy e in myGame.gameManager.EnemyList)
				{
                    if (BoundingCollisions.TouchLeftOf(e.BoundingCollisions) || BoundingCollisions.TouchTopOf(e.BoundingCollisions) || BoundingCollisions.TouchRightOf(e.BoundingCollisions) || BoundingCollisions.TouchBottomOf(e.BoundingCollisions))
					{
						AttackCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
						if (PositionCenter.X >= e.PositionCenter.X)
						{
                            Movement += 1;
                            JumpbackTimer = 100;
							MainHP -= 2;
						}
						else if (PositionCenter.X < e.PositionCenter.X)
						{
                            JumpbackTimer = 100;
							MainHP -= 2;
						}
					}

				}
			}

            if (JumpbackTimer > 0)
            {
                JumpbackTimer -= gameTime.ElapsedGameTime.Milliseconds;

                Movement = -1;
                velocity = new Vector2(MoveAcceleration * ((float)gameTime.ElapsedGameTime.TotalSeconds + 0.1f) * Movement, -(MoveAcceleration * ((float)gameTime.ElapsedGameTime.TotalSeconds * 6)));
            }
		}

		/// <summary>
		/// Detects and resolves all collisions between the player and his neighboring
		/// tiles. When a collision is detected, the player is pushed away along one
		/// axis to prevent overlapping. There is some special logic for the Y axis to
		/// handle platforms which behave differently depending on direction of movement.
		/// </summary>
		protected virtual void HandleCollisions(GameTime gameTime)
		{
			// Reset flag to search for ground collision.
			IsGrounded = false;

			foreach (Tile t in myGame.gameManager.TilesList)
			{
				if (CheckInRadius(t.Position, 55))
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
		}

		/// <summary>
		/// Creates the default animation frames for the player.
		/// </summary>
		/// <param name="texture">The texture to process.</param>
		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE1", texture, new Point(35, 50), new Point(1, 1), new Point(000, 000), 1600, true);
			AddAnimation("WALK1", texture, new Point(35, 50), new Point(1, 1), new Point(035, 000), 1600, true);
			AddAnimation("JUMP1", texture, new Point(35, 50), new Point(1, 1), new Point(070, 000), 1600, true);
			AddAnimation("FALL1", texture, new Point(35, 50), new Point(1, 1), new Point(105, 000), 1600, true);
			AddAnimation("HURT1", texture, new Point(35, 50), new Point(1, 1), new Point(140, 000), 1600, true);
			AddAnimation("ATK-1", texture, new Point(35, 50), new Point(1, 1), new Point(175, 000), 1600, true);
			AddAnimation("DIE-1", texture, new Point(35, 50), new Point(1, 1), new Point(210, 000), 1600, true);
			AddAnimation("GAIN1", texture, new Point(35, 50), new Point(1, 1), new Point(245, 000), 1600, true);
			AddAnimation("IDLE2", texture, new Point(35, 50), new Point(1, 1), new Point(000, 050), 1600, true);
			AddAnimation("WALK2", texture, new Point(35, 50), new Point(1, 1), new Point(035, 050), 1600, true);
			AddAnimation("JUMP2", texture, new Point(35, 50), new Point(1, 1), new Point(070, 050), 1600, true);
			AddAnimation("FALL2", texture, new Point(35, 50), new Point(1, 1), new Point(105, 050), 1600, true);
			AddAnimation("HURT2", texture, new Point(35, 50), new Point(1, 1), new Point(140, 050), 1600, true);
			AddAnimation("ATK-2", texture, new Point(35, 50), new Point(1, 1), new Point(175, 050), 1600, true);
			AddAnimation("DIE-2", texture, new Point(35, 50), new Point(1, 1), new Point(210, 050), 1600, true);
			AddAnimation("GAIN2", texture, new Point(35, 50), new Point(1, 1), new Point(245, 050), 1600, true);
			SetAnimation("IDLE1");

			base.AddAnimations(texture);
		}
	}
}