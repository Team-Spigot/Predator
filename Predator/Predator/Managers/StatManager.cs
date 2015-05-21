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
		/// <summary>
		/// The game that the stat manager runs off of.
		/// </summary>
		private Game1 myGame;
		/// <summary>
		/// The sprite batch that the manager uses.
		/// </summary>
		private SpriteBatch spriteBatch;

		#region Textures.
		/// <summary>
		/// Loads the texture for the menu background.
		/// </summary>
		public Texture2D MenuBackgroundTexture;
		/// <summary>
		/// Loads the texture for the plus button.
		/// </summary>
		public Texture2D PlusButtonTexture;
		/// <summary>
		/// Loads the texture for the exit button.
		/// </summary>
		public Texture2D ExitButtonTextxure;
		#endregion

		#region UI
		/// <summary>
		/// The button to increase Agility.
		/// </summary>
		public Button PlusButtonAgility;
		/// <summary>
		/// The button to increase Strength.
		/// </summary>
		public Button PlusButtonStrength;
		/// <summary>
		/// The button to increase Defense.
		/// </summary>
		public Button PlusButtonDefense;
		/// <summary>
		/// The button to exit the stat manager.
		/// </summary>
		public Button ExitButton;
		#endregion

		/// <summary>
		/// Gets or sets if the stats have changed.
		/// </summary>
		public bool StatsChanged
		{
			get;
			set;
		}
        public int agilityAmount = 1;
        public int strengthAmount = 1;
        public int defenseAmount = 1;

		/// <summary>
		/// Creates the stat manager.
		/// </summary>
		/// <param name="game">The game that the manager runs off of.</param>
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

		/// <summary>
		/// Loads the content for the stat manager.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			LoadTextures();

			PlusButtonStrength = new Button(PlusButtonTexture, new Vector2(575, 225), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);
			PlusButtonAgility = new Button(PlusButtonTexture, new Vector2(575, 425), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);
			PlusButtonDefense = new Button(PlusButtonTexture, new Vector2(575, 325), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);
			ExitButton = new Button(ExitButtonTextxure, new Vector2(742, 158), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);

			base.LoadContent();
		}

		/// <summary>
		/// Loads the textures for the stat manager.
		/// </summary>
		public void LoadTextures()
		{
			PlusButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\plusButton");
			MenuBackgroundTexture = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\menu");
			ExitButtonTextxure = Game.Content.Load<Texture2D>(@"images\gui\global\exitButton");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			PlusButtonAgility.Update(gameTime);
			PlusButtonStrength.Update(gameTime);
			PlusButtonDefense.Update(gameTime);
			ExitButton.Update(gameTime);

            if (PlusButtonAgility.Clicked() && myGame.gameManager.Player.StatPoints >= 1)
            {
                agilityAmount += 1;
                myGame.gameManager.Player.StatPoints -= 1;
                myGame.gameManager.Player.PAgility -= .025f;
            }
            if (PlusButtonStrength.Clicked() && myGame.gameManager.Player.StatPoints >= 1)
            {
                strengthAmount += 1;
                myGame.gameManager.Player.StatPoints -= 1;
                myGame.gameManager.Player.PStrength += 0.15f;
                // myGame.gameManager.player.MaxHP *= myGame.gameManager.player.PStrength;
            }
            if (PlusButtonDefense.Clicked() && myGame.gameManager.Player.StatPoints >= 1)
            {
                defenseAmount += 1;
                myGame.gameManager.Player.StatPoints -= 1;
                myGame.gameManager.Player.PDefense += 0.1f;
            }
			if (ExitButton.Clicked())
			{
				StatsChanged = true;
				myGame.SetCurrentLevel(Game1.GameLevels.GAME);
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// Draws the content of the stat manager.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			{
                spriteBatch.Draw(MenuBackgroundTexture, new Vector2(150, 150), Color.White);
                PlusButtonAgility.Draw(gameTime, spriteBatch);
                PlusButtonStrength.Draw(gameTime, spriteBatch);
                PlusButtonDefense.Draw(gameTime, spriteBatch);
                ExitButton.Draw(gameTime, spriteBatch);
                spriteBatch.DrawString(myGame.grimGhostRegular, "Attack", new Vector2(325, 237), Color.Black);
                spriteBatch.DrawString(myGame.grimGhostRegular, "Defense", new Vector2(325, 337), Color.Black);
                spriteBatch.DrawString(myGame.grimGhostRegular, "Speed", new Vector2(325, 437), Color.Black);
                spriteBatch.DrawString(myGame.grimGhostRegular, "" + agilityAmount, new Vector2(690, 433), Color.LimeGreen);
                spriteBatch.DrawString(myGame.grimGhostRegular, "" + strengthAmount, new Vector2(690, 233), Color.LimeGreen);
                spriteBatch.DrawString(myGame.grimGhostRegular, "" + defenseAmount, new Vector2(690, 333), Color.LimeGreen);
                spriteBatch.DrawString(myGame.grimGhostRegular, "Ability Points: " + myGame.gameManager.Player.StatPoints, new Vector2(165, 590), Color.LimeGreen);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
