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
	public class Player : Sprite
	{
		#region Lists
		/// <summary>
		/// Gets or sets the boundires of the map.
		/// </summary>
		protected List<Rectangle> MapBoundries
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the tiles of the map.
		/// </summary>
		public List<Tile> TileList
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets the enemies of the map.
		/// </summary>
		public List<Enemy> EnemyList
		{
			get;
			protected set;
		}
		#endregion

		#region Player Stats
		/// <summary>
		/// Gets or sets the player's level.
		/// </summary>
		public int Level
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the player is dead.
		/// </summary>
		public bool isDead
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets the player's HP in float.
		/// </summary>
		public float MainHP
		{
			get;
			protected set;
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
			protected set;
		}
		/// <summary>
		/// Gets or sets if the player fell out the map.
		/// </summary>
		public bool FellFromBottom
		{
			get;
			protected set;
		}
		public float Damage;
		public float Defense;
		public int statPoints;

		public float PStrength;
		public float PAgility;
		public float PDefense;
		public int PExp;
		public int Lvl;
		public bool LvlUp = false;
		#endregion

		#region Movement Stats
		/// <summary>
		/// Gets or sets center of the player's bounding boxes.
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
		/// The center of the player's bounding boxes.
		/// </summary>
		protected Vector2 center;
		/// <summary>
		/// Gets the players center based off of it's posititon.
		/// </summary>
		public Vector2 PositionCenter
		{
			get
			{
				return Position + Center;
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
		public KeyboardState keyboardState;
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
		public bool isGrounded
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
		public bool isJumping
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player was jumping.
		/// </summary>
		public bool wasJumping
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
		/// <summary>
		/// Gets the bounding Collisions.
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
		/// Gets or sets the inner bounds of the player.
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
		/// The inner bounds of the player.
		/// </summary>
		protected Rectangle inbounds;
		#endregion

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
		protected List<AnimationSet> ProjectileAnimationSet
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player is shooting.
		/// </summary>
		public bool isShooting
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
		/// <summary>
		/// The amount of mana the player has.
		/// </summary>
		protected float Mana;
		/// <summary>
		/// The maxium mana the player can have.
		/// </summary>
		protected float MaxMana;
		/// <summary>
		/// The amount of time it takes the mana to start recharging.
		/// </summary>
		protected float ManaRechargeTime;
		/// <summary>
		/// The default amount of time it takes the mana to start recharging.
		/// </summary>
		protected float DefaultManaRechargeTime;
		/// <summary>
		/// The amount of mana it will rechage every second.
		/// </summary>
		protected float ManaRechargeInterval;
		/// <summary>
		/// The default mana recharge time.
		/// </summary>
		protected float DefaultManaRechargeInterval;
		/// <summary>
		/// The ammount of mana it will decrease by.
		/// </summary>
		protected float ManaDecreaseAmount;
		#endregion

		/// <summary>
		/// Creates the Player class.
		/// </summary>
		/// <param name="position">The starting position of the player.</param>
		/// <param name="movementKeys">The Keys to move the player with.</param>
		/// <param name="HP">The hp that the player will start out with.</param>
		/// <param name="color">The Color to mask the player with.</param>
		/// <param name="animationSetList">The AnimationSet the player has.</param>
		/// <param name="ProjectileAnimationSet">The AnimationSet of the projectile.</param>
		public Player(Vector2 position, Keys[,] movementKeys, float HP, Color color, List<AnimationSet> animationSetList, List<AnimationSet> ProjectileAnimationSet)
			: base(position, color, animationSetList)
		{
			Level = 1;
			MainHP = HP;
			MaxHP = HP;
			JumpbackTimer = 1;

			PStrength = 2;
			PAgility = 2;
			PDefense = 2;
			PExp = 0;
			statPoints = 0;

			this.TileList = TileList;
			this.MapBoundries = MapBoundries;

			this.ProjectileAnimationSet = ProjectileAnimationSet;

			Mana = MaxMana = 0;
			ManaRechargeTime = DefaultManaRechargeTime = 0.09f;
			ManaRechargeInterval = DefaultManaRechargeInterval = 0.1f;
			ManaDecreaseAmount = 0f;

			#region Set Projectile Factors
			ProjectileList = new List<Projectile>();
			ProjectileAnimationSet = new List<AnimationSet>();
			CanShoot = true;
			CreateNewProjectile = true;
			#endregion

			#region Set Movement and Collision Factors
			MovementKeys = movementKeys;
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

		/// <summary>
		/// Updates the keyboard state seprately.
		/// </summary>
		/// <param name="gameTime">The GameTime that the game runs off of.</param>
		/// <param name="keyboardState">The game's KeyboardState.</param>
		public void UpdateKeyboardState(GameTime gameTime, KeyboardState keyboardState)
		{
			this.keyboardState = keyboardState;
		}

		/// <summary>
		/// Updates the events in the player class.
		/// </summary>
		/// <param name="gameTime">The class GameTime that the game runs off of.</param>
		/// <param name="EnemyList">The Enemy list.</param>
		/// <param name="TileList">The list of Tiles.</param>
		/// <param name="MapBoundries">The list of Rectangles that make up the boundries of the world.</param>
		public void Update(GameTime gameTime, List<Enemy> EnemyList, List<Tile> TileList, List<Rectangle> MapBoundries)
		{
			this.EnemyList = EnemyList;
			this.TileList = TileList;
			this.MapBoundries = MapBoundries;

			GetInput();

			HandleAnimations(gameTime);

			HandleProjectile(gameTime);

			ApplyPhysics(gameTime);

			HandleHealth();

			foreach (Projectile p in ProjectileList)
			{
				p.Update(gameTime, this, EnemyList, TileList, MapBoundries);
			}

			if (LvlUp == true)
			{
				statPoints += 3;
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
			#endregion

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

		public virtual void UpdateStats()
		{
			MaxMoveSpeed *= PAgility;
			Damage = PStrength;
			Defense = PDefense;
		}

		protected virtual void HandleHealth()
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
		protected virtual void GetInput()
		{
			// Ignore small movements to prevent running in place.
			if (Math.Abs(Movement) < 0.5f)
			{
				Movement = 0.0f;
			}

			// If any digital horizontal movement input is found, override the analog movement.
			if (keyboardState.IsKeyDown(MovementKeys[0, 0]) || keyboardState.IsKeyDown(MovementKeys[1, 0]))
			{
				Movement += -1.0f;
			}
			else if (keyboardState.IsKeyDown(MovementKeys[0, 2]) || keyboardState.IsKeyDown(MovementKeys[1, 2]))
			{
				Movement += 1.0f;
			}
			if (keyboardState.IsKeyDown(Keys.P))
			{
				MainHP += 1;
			}
			if (keyboardState.IsKeyDown(Keys.L))
			{
				MainHP -= 1;
			}

			Movement = MathHelper.Clamp(Movement, -1, 1);

			isShooting = keyboardState.IsKeyDown(MovementKeys[0, 4]);

			// Check if the player wants to jump.
			isJumping = keyboardState.IsKeyDown(MovementKeys[0, 1]) || keyboardState.IsKeyDown(MovementKeys[1, 1]);
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

		/// <summary>
		///
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void HandleProjectile(GameTime gameTime)
		{
			if (isShooting && CanShoot)
			{
				ProjectileList.Add(new Projectile(Position, Color.White, ProjectileAnimationSet, this));

				CanShoot = false;
			}
			if (!CanShoot)
			{
				ManaRechargeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (ManaRechargeTime <= 0)
				{
					if (ProjectileList.Count > 0)
					{
						ProjectileList.RemoveAt(ProjectileList.Count - 1);
					}

					ManaRechargeInterval -= (float)gameTime.ElapsedGameTime.TotalSeconds;

					if (ManaRechargeInterval <= 0)
					{
						ManaRechargeTime = DefaultManaRechargeTime;
						ManaRechargeInterval = DefaultManaRechargeInterval;
						CanShoot = true;
					}
				}
			}
		}

		public Rectangle test;
		public float attackCounter = 1;
		public float defaultSpeed;

		protected virtual void HandleEnemyCollisions(GameTime gameTime)
		{
			if (EnemyList != null)
			{
				foreach (Enemy e in EnemyList)
				{
					if (BoundingCollisions.TouchLeftOf(e.BoundingCollisions) || BoundingCollisions.TouchRightOf(e.BoundingCollisions))
					{
						attackCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
						if (PositionCenter.X >= e.PositionCenter.X)
						{
							isJumping = true;
							Movement += 1;
							velocity.X = MaxMoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * Movement;
							velocity.Y = DoJump(velocity.Y, gameTime);
						}
						else if (PositionCenter.X < e.PositionCenter.X)
						{
							isJumping = true;
							Movement += -1;
							velocity.X = MaxMoveSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * Movement;
							velocity.Y = DoJump(velocity.Y, gameTime);
						}
					}
					if (attackCounter <= 0)
					{
						MainHP -= 5;
						attackCounter = 1;
					}
				}
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
					Position.X = t.Collisions.Right;
					test = t.Collisions;
				}
				if (BoundingCollisions.TouchBottomOf(t.Collisions) && t.tileCollisions == Tile.TileCollisions.Impassable)
				{
					isJumping = false;
					JumpTime = 0;
					Position.Y = t.Collisions.Bottom + 2;
					test = t.Collisions;
				}
			}

			foreach (Rectangle r in MapBoundries)
			{
				if (r.TouchBottomOf(BoundingCollisions))
				{
					FellFromBottom = true;
					test = r;
				}
				if (r.TouchRightOf(BoundingCollisions))
				{
					Position.X = r.Left - BoundingCollisions.Width;
					test = r;
				}
				else if (r.TouchLeftOf(BoundingCollisions))
				{
					Position.X = r.Right;
					test = r;
				}
				if (r.TouchTopOf(BoundingCollisions))
				{
					isJumping = false;
					JumpTime = 0;
					Position.Y = r.Bottom + 2;
					test = r;
				}
			}
		}
	}
}