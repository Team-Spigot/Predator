﻿using System;
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
using VoidEngine.Helpers;
using VoidEngine.Particles;
using VoidEngine.VGame;
using VoidEngine.VGUI;

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

			Offset = new Vector2(0, 35);

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
			AddAnimation("IDLE", texture, new Point(35, 105), new Point(0, 0), new Point(0, 0), 1600, false);
			AddAnimation("ACTIVE", texture, new Point(35, 105), new Point(0, 0), new Point(35, 0), 1600, false);
			AddAnimation("SAVING", texture, new Point(35, 105), new Point(3, 0), new Point(70, 0), 500, true);
		}
	}
}