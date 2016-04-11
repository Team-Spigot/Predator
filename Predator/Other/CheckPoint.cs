using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VoidEngine.Helpers;
using VoidEngine.VGame;

namespace Predator.Other
{
	class CheckPoint : PlaceableObject
	{
		private Game1 myGame;

		public int CheckpointIndex
		{
			get;
			protected set;
		}

		private int CheckpointSaveTimer = 0;

		public CheckPoint(Texture2D texture, Vector2 position, Color color, int index, Game1 myGame)
			: base(texture, position, color)
		{
			this.myGame = myGame;

			AddAnimations(texture);
			SetAnimation("IDLE");

			Position = position;
			Color = color;
			CheckpointIndex = index;

			Offset = new Vector2(11, 0);

			inbounds = new Rectangle(0, 0, 35, 70);
		}

		public override void Update(GameTime gameTime)
		{
			HandleAnimations(gameTime);

			if (BoundingCollisions.TouchLeftOf(myGame.gameManager.Player.BoundingCollisions) || BoundingCollisions.TouchTopOf(myGame.gameManager.Player.BoundingCollisions) || BoundingCollisions.TouchRightOf(myGame.gameManager.Player.BoundingCollisions) || BoundingCollisions.TouchBottomOf(myGame.gameManager.Player.BoundingCollisions))
			{
				if (myGame.gameManager.LastCheckpoint < CheckpointIndex)
				{
					CheckpointSaveTimer = 2500;
				}
			}

			if (CheckpointSaveTimer >= 1)
			{
				CheckpointSaveTimer -= gameTime.ElapsedGameTime.Milliseconds;

				SetAnimation("SAVING");

				if (CheckpointSaveTimer <= 1)
				{
					SetAnimation("ACTIVE");
					myGame.gameManager.LastCheckpoint = CheckpointIndex;
				}
			}

			base.Update(gameTime);
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(70, 70), new Point(0, 0), new Point(0, 0), 1600, false);
			AddAnimation("ACTIVE", texture, new Point(70, 70), new Point(0, 0), new Point(840, 0), 500, true);
			AddAnimation("SAVING", texture, new Point(70, 70), new Point(4, 0), new Point(70, 0), 500, true);
		}
	}
}