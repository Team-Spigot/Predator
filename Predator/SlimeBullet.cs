using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VoidEngine.Helpers;
using VoidEngine.VGame;


namespace Predator
{
	public class SlimeBullet : Sprite
	{
		Vector2 Direction;
		public int totalDistance;
		public bool deleteMe;
		Game1 myGame;


		public SlimeBullet(Texture2D tex, Vector2 pos, float newSpeed, Vector2 direction, Color color, Game1 myGame)
			: base(pos, color, tex)
		{
			AddAnimations(tex);
			speed = newSpeed;
			Direction = CollisionHelper.UnitVector(direction);
			totalDistance = 0;

			RotationCenter = new Vector2(tex.Width / 2, tex.Height / 2);

			Inbounds = new Rectangle(0, 0, tex.Width, tex.Height);

			deleteMe = false;
			this.myGame = myGame;
		}

		public override void Update(GameTime gameTime)
		{
			position += Direction * speed;

			inbounds = new Rectangle(0, 0, CurrentAnimation.frameSize.X, CurrentAnimation.frameSize.Y);

			totalDistance += (int)speed;
			Rotation -= 50;
			base.Update(gameTime);

			if (totalDistance >= 600)
			{
				deleteMe = true;
			}

			foreach (Tile t in myGame.gameManager.TilesList)
			{
				if (this.CheckInRadius(t.Position, 45))
				{
					if (BoundingCollisions.TouchLeftOf(t.BoundingCollisions) || BoundingCollisions.TouchTopOf(t.BoundingCollisions) || BoundingCollisions.TouchRightOf(t.BoundingCollisions) || BoundingCollisions.TouchBottomOf(t.BoundingCollisions))
					{
						this.deleteMe = true;
					}
				}
			}

		}

		protected override void AddAnimations(Texture2D tex)
		{
			AddAnimation("IDLE", tex, new Point(30, 30), new Point(1, 1), new Point(0, 0), 16000, false);
			SetAnimation("IDLE");
		}
	}
}
