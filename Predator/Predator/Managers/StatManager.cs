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
using VoidEngine.VGame;
using VoidEngine.VGUI;


namespace Predator
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class StatManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game1 myGame;
		SpriteBatch spriteBatch;

		Texture2D menuBackground;
		Texture2D plusButtonTexture;
		Texture2D exitButtonTextxure;

		Button plusButtonAgi;
		Button plusButtonStr;
		Button plusButtonDef;
		Button exitButton;

		public StatManager(Game1 game)
			: base(game)
		{
			myGame = game;

			Initialize();
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			LoadTextures();

			plusButtonStr = new Button(plusButtonTexture, new Vector2(575, 225), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);
			plusButtonAgi = new Button(plusButtonTexture, new Vector2(575, 425), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);
			plusButtonDef = new Button(plusButtonTexture, new Vector2(575, 325), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);
			exitButton = new Button(exitButtonTextxure, new Vector2(742, 158), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);

			base.LoadContent();
		}

		public void LoadTextures()
		{
			plusButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\plusButton");
			menuBackground = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\menu");
			exitButtonTextxure = Game.Content.Load<Texture2D>(@"images\gui\global\exitButton");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			plusButtonAgi.Update(gameTime);
			plusButtonStr.Update(gameTime);
			plusButtonDef.Update(gameTime);
			exitButton.Update(gameTime);

			if (plusButtonAgi.Clicked() && myGame.gameManager.Player.statPoints >= 1)
			{
				myGame.gameManager.Player.statPoints -= 1;
				myGame.gameManager.Player.PAgility -= 0.055f * (1 - (myGame.gameManager.Player.PAgility * .1f));
			}
			if (plusButtonStr.Clicked() && myGame.gameManager.Player.statPoints >= 1)
			{
				myGame.gameManager.Player.statPoints -= 1;
				myGame.gameManager.Player.PStrength += 0.18f;
				// myGame.gameManager.player.MaxHP *= myGame.gameManager.player.PStrength;
			}
			if (plusButtonDef.Clicked() && myGame.gameManager.Player.statPoints >= 1)
			{
				myGame.gameManager.Player.statPoints -= 1;
				myGame.gameManager.Player.PDefense += 0.18f;
			}
			if (exitButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.GAME);
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			{
				spriteBatch.Draw(menuBackground, new Vector2(150, 150), Color.White);
				plusButtonAgi.Draw(gameTime, spriteBatch);
				plusButtonStr.Draw(gameTime, spriteBatch);
				plusButtonDef.Draw(gameTime, spriteBatch);
				exitButton.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
