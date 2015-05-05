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
using VoidEngine.VGUI;
using VoidEngine.VGame;


namespace Predator.Managers
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class MainMenuManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		/// <summary>
		/// The game that the main menu runs off of.
		/// </summary>
		Game1 myGame;
		/// <summary>
		/// The sprite batch that the main menu uses.
		/// </summary>
		SpriteBatch spriteBatch;

		/// <summary>
		/// The main menu Background.
		/// </summary>
		Texture2D menuBackground;

		/// <summary>
		/// The start button texture.
		/// <summary>
		Texture2D startTexture;
		/// <summary>
		/// The quit button texture.
		/// </summary>
		Texture2D quitTexture;

		/// <summary>
		/// The start button's animation set.
		/// </summary>
		List<Sprite.AnimationSet> startButtonAnimationSet = new List<Sprite.AnimationSet>();
		/// <summary>
		/// The quit button's animation set.
		/// </summary>
		List<Sprite.AnimationSet> quitButtonAnimationSet = new List<Sprite.AnimationSet>();

		/// <summary>
		/// The start button.
		/// </summary>
		Button startButton;
		/// <summary>
		/// The quit button.
		/// </summary>
		Button quitButton;

		/// <summary>
		/// Creates the game manager.
		/// </summary>
		public MainMenuManager(Game1 game)
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
			// TODO: Add your initialization code here

			base.Initialize();
		}

		/// <summary>
		/// Loads the main menu's content.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			menuBackground = Game.Content.Load<Texture2D>(@"images\gui\mainMenu\mainMenu");
			startTexture = Game.Content.Load<Texture2D>(@"images\gui\mainMenu\startButton");
			quitTexture = Game.Content.Load<Texture2D>(@"images\gui\mainMenu\quitButton");

			#region Animation Sets
			//start button
			startButtonAnimationSet.Add(new Sprite.AnimationSet("IDLE", startTexture, new Point(100, 50), new Point(1, 1), new Point(0, 0), 16000, false));
			startButtonAnimationSet.Add(new Sprite.AnimationSet("HOVER", startTexture, new Point(100, 50), new Point(1, 1), new Point(100, 0), 16000, false));
			startButtonAnimationSet.Add(new Sprite.AnimationSet("PRESSED", startTexture, new Point(100, 50), new Point(1, 1), new Point(200, 0), 16000, false));
			//quit button
			quitButtonAnimationSet.Add(new Sprite.AnimationSet("IDLE", quitTexture, new Point(100, 50), new Point(1, 1), new Point(0, 0), 16000, false));
			quitButtonAnimationSet.Add(new Sprite.AnimationSet("HOVER", quitTexture, new Point(100, 50), new Point(1, 1), new Point(100, 0), 16000, false));
			quitButtonAnimationSet.Add(new Sprite.AnimationSet("PRESSED", quitTexture, new Point(100, 50), new Point(1, 1), new Point(200, 0), 16000, false));
			#endregion

			startButton = new Button(new Vector2((myGame.WindowSize.X - 100) / 2, 300), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, startButtonAnimationSet);
			quitButton = new Button(new Vector2((myGame.WindowSize.X - 100) / 2, 550), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, quitButtonAnimationSet);

			base.LoadContent();
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			startButton.Update(gameTime);
			quitButton.Update(gameTime);

			if (startButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.GAME);
			}

			if (quitButton.Clicked())
			{
				Game.Exit();
			}
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			{
				spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);
				startButton.Draw(gameTime, spriteBatch);
				quitButton.Draw(gameTime, spriteBatch);
                //spriteBatch.Draw(Game.Content.Load<Texture2D>(@"images\gui\mainMenu\spigot"), new Rectangle(15, 500, 128, 96), Color.White);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}