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


namespace Predator.Managers
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class SplashScreenManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game1 myGame;
		SpriteBatch spriteBatch;

		Texture2D spigotLogoTexture;
		Texture2D backgroundTexture;

		int Timer = 5000;

		public SplashScreenManager(Game1 game)
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

			LoadImages();

			base.LoadContent();
		}

		public void LoadImages()
		{
			spigotLogoTexture = Game.Content.Load<Texture2D>(@"images\gui\mainMenu\spigot");
			backgroundTexture = Game.Content.Load<Texture2D>(@"images\game\backgrounds\sewerBackground");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			Timer -= (int)gameTime.ElapsedGameTime.Milliseconds;

			if (Timer <= 0)
			{
				myGame.SetCurrentLevel(Game1.GameLevels.MENU);
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			{
				spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
				spriteBatch.Draw(spigotLogoTexture, Vector2.Zero, Color.White);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
