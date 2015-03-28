using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VoidEngine;

namespace Preditor
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class LoseManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game1 myGame;
		SpriteBatch spriteBatch;
		//Texture2D background;
		//Texture2D buttonTexture;
		//Button retry;
		//Button mainMenu;
		//Button exit;
		List<Sprite.AnimationSet> animationSpriteList;
		public LoseManager(Game1 game)
			: base(game)
		{
			myGame = game;
			// TODO: Construct any child components here
			Initialize();
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public override void Initialize()
		{
			// TODO: Add your initialization code here
			animationSpriteList = new List<Sprite.AnimationSet>();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);

			//background = Game.Content.Load<Texture2D>(@"images\screens\lose");
			//buttonTexture = Game.Content.Load<Texture2D>(@"images\gui\button");

			//animationSpriteList.Add(new Sprite.AnimationSet("IDLE", buttonTexture, new Point(170, 46), new Point(1, 1), new Point(0, 0), 0));
			//animationSpriteList.Add(new Sprite.AnimationSet("HOVER", buttonTexture, new Point(170, 46), new Point(1, 1), new Point(170, 0), 0));
			//animationSpriteList.Add(new Sprite.AnimationSet("PRESSED", buttonTexture, new Point(170, 46), new Point(1, 1), new Point(340, 0), 0));

			//retry = new Button(new Vector2((myGame.WindowSize.X - 170) / 2, 366), myGame.segoeUIRegular, 1f, Color.Black, "RETRY?", Color.White, animationSpriteList);
			//mainMenu = new Button(new Vector2((myGame.WindowSize.X - 170) / 2, 190), myGame.segoeUIRegular, 1f, Color.Black, "MAIN MENU", Color.White, animationSpriteList);
			//exit = new Button(new Vector2((myGame.WindowSize.X - 170) / 2, 275), myGame.segoeUIRegular, 1f, Color.Black, "EXIT", Color.White, animationSpriteList);

			base.LoadContent();
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			//retry.Update(gameTime);
			//mainMenu.Update(gameTime);
			//exit.Update(gameTime);

			//if (exit.Clicked())
			{
				//MediaPlayer.Stop();
				//myGame.Exit();
			}
			//if (mainMenu.Clicked())
			{
				//myGame.SetCurrentLevel(Game1.GameLevels.MENU);
				//myGame.gameManager.camera.Position = new Vector2(myGame.gameManager.player.GetPosition.X, myGame.gameManager.player.GetPosition.Y);
				//myGame.gameManager.player.Lives = 3;
				//myGame.gameManager.level = 2;
				//myGame.gameManager.levelLoaded = false;
				//myGame.menuManager.title = false;
			}
			//if (retry.Clicked())
			{
				//myGame.SetCurrentLevel(Game1.GameLevels.GAME);
				//myGame.gameManager.camera.Position = new Vector2(myGame.gameManager.player.GetPosition.X, myGame.gameManager.player.GetPosition.Y);
				//myGame.gameManager.player.Lives = 3;
				//myGame.gameManager.levelLoaded = false;
				//myGame.gameManager.player._Mana.mana = myGame.gameManager.player._Mana.maxMana;
				//myGame.gameManager.player._Mana.manaRechargeTime = 5000;
				//myGame.gameManager.player.CanShootProjectile = true;
				//myGame.gameManager.player.HasShotProjectile = false;
			}



			// TODO: Add your update code here

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();

			//spriteBatch.Draw(background, new Rectangle(0, 0, (int)myGame.WindowSize.X, (int)myGame.WindowSize.Y), Color.White);

			//retry.Draw(gameTime, spriteBatch);
			//mainMenu.Draw(gameTime, spriteBatch);
			//exit.Draw(gameTime, spriteBatch);


			spriteBatch.End();



			base.Draw(gameTime);
		}
	}
}
