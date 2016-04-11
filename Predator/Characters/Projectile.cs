using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VoidEngine.VGame;

namespace Predator.Characters
{
	public class Projectile : Sprite
	{
		/// <summary>
		/// The game that the projectile uses.
		/// </summary>
		Game1 myGame;

		/// <summary>
		/// The starting position of the projectile.
		/// </summary>
		public Vector2 StartingPosition;

		/// <summary>
		/// Gets or sets the max distance the projectile can travel.
		/// </summary>
		public float maxDistance
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets or sets if the projectile should be deleted.
		/// </summary>
		public bool DeleteMe
		{
			get;
			set;
		}
		public float MaxSpeed;
		public Vector2 Velocity;

		/// <summary>
		/// Creates the projectile class, with default animation set.
		/// </summary>
		/// <param name="texture">The texture to use with the projectile.</param>
		/// <param name="position">The starting position of the projectile.</param>
		/// <param name="color">The color to mask the projectile with.</param>
		/// <param name="myGame">The game that the projectile runs on.</param>
		public Projectile(Texture2D texture, Vector2 position, Vector2 currentVelocity, Color color, Game1 myGame)
			: base(position, color, texture)
		{
			AddAnimations(texture);
			SetAnimation("IDLE");
			this.myGame = myGame;

			StartingPosition = Position = position;
			Velocity = currentVelocity;
			Color = color;
			DeleteMe = false;
		}

		/// <summary>
		/// Updates the projectile class.
		/// </summary>
		/// <param name="gameTime">The game time that the game uses.</param>
		public override void Update(GameTime gameTime)
		{
			HandleAnimations(gameTime);

			if (Vector2.Distance(StartingPosition, Position) > maxDistance)
			{
				//DeleteMe = true;
			}
			if (!DeleteMe)
			{
				if (Velocity.X < 0)
				{
					FlipSprite(Axis.Y);
				}
				else if (Velocity.X > 0)
				{
					FlipSprite(Axis.NONE);
				}

				Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}

			inbounds = new Rectangle(0, 0, CurrentAnimation.frameSize.X, CurrentAnimation.frameSize.Y);

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (!DeleteMe)
			{
				base.Draw(gameTime, spriteBatch);
			}
		}

		public void Fire()
		{
			MaxSpeed = 5;
			maxDistance = 125;

			DeleteMe = false;
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(40, 50), new Point(3, 1), new Point(0, 0), 50, false);
			SetAnimation("IDLE");

			base.AddAnimations(texture);
		}
	}
}
