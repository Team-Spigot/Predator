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

namespace VoidEngine
{
	public class Player : Sprite
	{
		/// <summary>
		/// The mana struct for the player class.
		/// </summary>
		public struct Mana
		{
			/// <summary>
			/// The amount of mana the player has.
			/// </summary>
			public float mana;
			/// <summary>
			/// The maxium mana the player can have.
			/// </summary>
			public float maxMana;
			/// <summary>
			/// The amount of time it takes the mana to start recharging.
			/// </summary>
			public float manaRechargeTime;
			/// <summary>
			/// The default amount of time it takes the mana to start recharging.
			/// </summary>
			public float defaultManaRechargeTime;
			/// <summary>
			/// The amount of mana it will rechage every second.
			/// </summary>
			public float manaRechargeInterval;
			/// <summary>
			/// 
			/// </summary>
			public float defaultManaRechargeInterval;
			/// <summary>
			/// The ammount of mana it will decrease by.
			/// </summary>
			public float manaDecreaseAmount;

			/// <summary>
			/// Creates the Mana struct.
			/// </summary>
			/// <param name="maxMana">The maxium ammount of mana.</param>
			/// <param name="manaRechargeTime">The amount of time it takes the mana to start recharging.</param>
			/// <param name="manaRechargeInterval">The ammount of time the mana takes to charge at.</param>
			public Mana(float maxMana, float defaultManaRechargeTime, float manaRechargeInterval, float manaDecreaseAmount)
			{
				this.mana = maxMana;
				this.maxMana = maxMana;
				this.manaRechargeTime = defaultManaRechargeTime;
				this.defaultManaRechargeTime = defaultManaRechargeTime;
				this.manaRechargeInterval = manaRechargeInterval;
				this.defaultManaRechargeInterval = manaRechargeInterval;
				this.manaDecreaseAmount = manaDecreaseAmount;
			}
		}

		protected List<Rectangle> MapBoundries
		{
			get;
			set;
		}

		public List<Tile> TileList
		{
			get;
			protected set;
		}

		public List<Tile> SetTileList
		{
			set
			{
				TileList = value;
			}
		}

		public List<Enemy> EnemyList
		{
			get;
			protected set;
		}

		#region Player Stats
		/// <summary>
		/// 
		/// </summary>
		public int Level
		{
			get;
			protected set;
		}
		/// <summary>
		/// 
		/// </summary>
		public bool isDead
		{
			get;
			protected set;
		}
		/// <summary>
		/// 
		/// </summary>
		public float MainHP
		{
			get;
			protected set;
		}
		/// <summary>
		/// 
		/// </summary>
		public int HP
		{
			get
			{
				return (int)MainHP;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public float MaxHP
		{
			get;
			protected set;
		}
		#endregion

		#region Movement Stats
		/// <summary>
		/// 
		/// </summary>
		public Vector2 Center
		{
			get
			{
				return center;
			}
			protected set
			{
				center = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		protected Vector2 center;
		/// <summary>
		/// 
		/// </summary>
		public Vector2 PositionCenter
		{
			get
			{
				return Position + Center;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Keys[,] MovementKeys
		{
			set;
			get;
		}
		/// <summary>
		/// 
		/// </summary>
		public KeyboardState keyboardState;
		/// <summary>
		/// 
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
		/// 
		/// </summary>
		protected Vector2 velocity;

		// Constants for controling horizontal movement
		protected float MoveAcceleration = 13000.0f;
		protected float MaxMoveSpeed = 1750.0f;
		protected float GroundDragFactor = 0.62f;
		protected float AirDragFactor = 0.66f;

		// Constants for controlling vertical movement
		protected float MaxJumpTime = 0.35f;
		protected float JumpLaunchVelocity = -3500.0f;
		protected float GravityAcceleration = 3400f;
		protected float MaxFallSpeed = 550.0f;
		protected float JumpControlPower = 0.14f;

		/// <summary>
		/// 
		/// </summary>
		public bool isGrounded
		{
			get;
			protected set;
		}
		/// <summary>
		/// 
		/// </summary>
		public float Movement
		{
			get;
			protected set;
		}
		/// <summary>
		/// 
		/// </summary>
		public bool isJumping
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		public bool wasJumping
		{
			get;
			protected set;
		}
		/// <summary>
		/// 
		/// </summary>
		public float JumpTime
		{
			get;
			protected set;
		}
		/// <summary>
		/// 
		/// </summary>
		public int JumpbackTimer
		{
			get;
			protected set;
		}
		/// <summary>
		/// 
		/// </summary>
		public bool TouchTop
		{
			get;
			protected set;
		}
		/// <summary>
		/// 
		/// </summary>
		public Rectangle BoundingCollisions
		{
			get
			{
				int left = (int)Math.Round(Position.X) + inbounds.X;
				int top = (int)Math.Round(Position.Y) + inbounds.Y;

				return new Rectangle(left, top, inbounds.Width, inbounds.Height);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Rectangle Inbounds
		{
			get
			{
				return inbounds;
			}
			set
			{
				inbounds = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		protected Rectangle inbounds;
		#endregion

		#region Projectile Stuff
		protected Mana mana;

		public Mana GetMana
		{
			get
			{
				return mana;
			}
		}

		public List<Projectile> ProjectileList
		{
			get;
			protected set;
		}

		protected List<AnimationSet> ProjectileAnimationSet
		{
			get;
			set;
		}

		public bool isShooting
		{
			get;
			protected set;
		}

		public bool CanShoot
		{
			get;
			protected set;
		}

		public bool CreateNewProjectile
		{
			get;
			protected set;
		}
		#endregion

		/// <summary>
		/// Creates the player.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="movementKeys"></param>
		/// <param name="keyboardState"></param>
		/// <param name="HP"></param>
		/// <param name="mana"></param>
		/// <param name="color"></param>
		/// <param name="animationSetList"></param>
		/// <param name="TileList"></param>
		/// <param name="MapBoundries"></param>
		public Player(Vector2 position, Keys[,] movementKeys, KeyboardState keyboardState, float HP, Mana mana, Color color, List<AnimationSet> animationSetList, List<AnimationSet> ProjectileAnimationSet)
			: base(position, color, animationSetList)
		{
			Level = 1;
			MainHP = HP;
			MaxHP = HP;
			JumpbackTimer = 1;

			this.TileList = TileList;
			this.MapBoundries = MapBoundries;

			this.ProjectileAnimationSet = ProjectileAnimationSet;

			#region Set Projectile Factors
			ProjectileList = new List<Projectile>();
			ProjectileAnimationSet = new List<AnimationSet>();
			this.mana = mana;
			CanShoot = true;
			CreateNewProjectile = true;
			#endregion

			#region Set Movement and Collision Factors
			MovementKeys = movementKeys;
			this.keyboardState = keyboardState;
			SetAnimation("IDLE" + Level);

			int width = (int)(animationSetList[0].frameSize.X);
			int left = (animationSetList[0].frameSize.X - width);
			int height = (int)(animationSetList[0].frameSize.Y);
			int top = animationSetList[0].frameSize.Y - height;
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
		public Player(Vector2 position, Color color, List<AnimationSet> animationSetList)
			: base(position, color, animationSetList)
		{
			ProjectileList = new List<Projectile>();
			ProjectileAnimationSet = new List<AnimationSet>();
		}

		public void UpdateKeyboardState(GameTime gameTime, KeyboardState keyboardState)
		{
			this.keyboardState = keyboardState;
		}

		/// <summary>
		/// Updates the Player class
		/// </summary>
		/// <param name="gameTime">To keep track of run time.</param>
		public void Update(GameTime gameTime, List<Enemy> EnemyList, List<Tile> TileList, List<Rectangle> MapBoarders)
		{
			HandleAnimations(gameTime);

			GetInput();

			HandleProjectile(gameTime);

			ApplyPhysics(gameTime);

			this.EnemyList = EnemyList;
			this.TileList = TileList;
			this.MapBoundries = MapBoarders;

			foreach (Projectile p in ProjectileList)
			{
				p.Update(gameTime, this, EnemyList, TileList, MapBoundries);
			}

			Center = new Vector2(inbounds.Width / 2, inbounds.Height / 2);

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

		/// <summary>
		/// 
		/// </summary>
		protected void GetInput()
		{
			// Ignore small movements to prevent running in place.
			if (Math.Abs(Movement) < 0.5f)
			{
				Movement = 0.0f;
			}

			// If any digital horizontal movement input is found, override the analog movement.
			if (keyboardState.IsKeyDown(MovementKeys[0, 0]) || keyboardState.IsKeyDown(MovementKeys[1, 0]))
			{
				Movement = -1.0f;
			}
			else if (keyboardState.IsKeyDown(MovementKeys[0, 2]) || keyboardState.IsKeyDown(MovementKeys[1, 2]))
			{
				Movement = 1.0f;
			}

			isShooting = keyboardState.IsKeyDown(MovementKeys[0, 4]); 

			// Check if the player wants to jump.
			isJumping = keyboardState.IsKeyDown(MovementKeys[0, 1]) || keyboardState.IsKeyDown(MovementKeys[1, 1]);
		}

		/// <summary>
		/// Updates the player's velocity and position based on input, gravity, etc.
		/// </summary>
		/// <param name="gameTime"></param>
		public void ApplyPhysics(GameTime gameTime)
		{
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			Vector2 previousPosition = Position;

			// Base velocity is a combination of horizontal movement control and
			// acceleration downward due to gravity.
			velocity.X += Movement * MoveAcceleration * elapsed;

			if (!isGrounded)
			{
				velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);
			}

			velocity.Y = DoJump(velocity.Y, gameTime);

			// Apply pseudo-drag horizontally.
			if (isGrounded)
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
			HandleCollisions();

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
		protected float DoJump(float velocityY, GameTime gameTime)
		{
			// If the player wants to jump
			if (isJumping)
			{
				// Begin or continue a jump
				if ((!wasJumping && isGrounded) || JumpTime > 0.0f)
				{
					if (JumpTime == 0.0f)
					{
						//jumpSound.Play();
					}

					JumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
					SetAnimation("JUMP" + Level);
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
			wasJumping = isJumping;

			return velocityY;
		}

		protected void HandleProjectile(GameTime gameTime)
		{
			if (isShooting && CanShoot)
			{
				ProjectileList.Add(new Projectile(Position, Color.White, ProjectileAnimationSet, this));

				CanShoot = false;
			}
			if (!CanShoot)
			{
				mana.manaRechargeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (mana.manaRechargeTime <= 0)
				{
					if (ProjectileList.Count > 0)
					{
						ProjectileList.RemoveAt(ProjectileList.Count - 1);
					}

					mana.manaRechargeInterval -= (float)gameTime.ElapsedGameTime.TotalSeconds;

					if (mana.manaRechargeInterval <= 0)
					{
						mana.manaRechargeTime = mana.defaultManaRechargeTime;
						mana.manaRechargeInterval = mana.defaultManaRechargeInterval;
						CanShoot = true;
					}
				}
			}
		}

		public Rectangle test;

		/// <summary>
		/// Detects and resolves all collisions between the player and his neighboring
		/// tiles. When a collision is detected, the player is pushed away along one
		/// axis to prevent overlapping. There is some special logic for the Y axis to
		/// handle platforms which behave differently depending on direction of movement.
		/// </summary>
		private void HandleCollisions()
		{
			// Reset flag to search for ground collision.
			isGrounded = false;

			foreach (Tile t in TileList)
			{
				if (BoundingCollisions.TouchTopOf(t.Collisions) && t.tileCollisions != Tile.TileCollisions.Passable)
				{
					isGrounded = true;
					Position.Y = t.GetPosition.Y - BoundingCollisions.Height;
					test = t.Collisions;
				}
				if (BoundingCollisions.TouchLeftOf(t.Collisions) && t.tileCollisions == Tile.TileCollisions.Impassable)
				{
					Position.X = t.GetPosition.X - BoundingCollisions.Width;
					test = t.Collisions;
				}
				if (BoundingCollisions.TouchRightOf(t.Collisions) && t.tileCollisions == Tile.TileCollisions.Impassable)
				{
					Position.X = t.GetPosition.X + t.Collisions.Width;
					test = t.Collisions;
				}
				if (BoundingCollisions.TouchBottomOf(t.Collisions) && t.tileCollisions == Tile.TileCollisions.Impassable)
				{
					Position.Y = t.Collisions.Bottom + 2;
					test = t.Collisions;
				}
			}
		}
	}
}