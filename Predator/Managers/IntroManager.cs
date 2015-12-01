using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Predator.Managers
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class IntroManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		/// <summary>
		/// The game that the splash screen manager runs off of.
		/// </summary>
		private Game1 myGame;
		/// <summary>
		/// The sprite batch that the splash screen manager uses.
		/// </summary>
		private SpriteBatch spriteBatch;

		#region Textures
		/// <summary>
		/// Loads the texture for the Spigot Logo.
		/// </summary>
		public Texture2D spigotLogoTexture;
		/// <summary>
		/// Loads the texture for the background.
		/// </summary>
		public Texture2D backgroundTexture;
		#endregion

		/// <summary>
		/// The timer for the duration of the splash screen.
		/// </summary>
		protected int Timer1 = 5000;
		protected int Timer2 = 5000;
		protected int Timer3 = 5000;
		protected int Timer4 = 5000;


		/// <summary>
		/// Creates the splash screen manager.
		/// </summary>
		/// <param name="game">The game that the manager runs off of.</param>
		public IntroManager(Game1 game)
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
		/// Loads the content for the splash screen manager.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			LoadImages();

			base.LoadContent();
		}

		/// <summary>
		/// Loads the textures for the splash screen manager.
		/// </summary>
		public void LoadImages()
		{

			backgroundTexture = Game.Content.Load<Texture2D>(@"images\game\backgrounds\sewerBackground");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			Timer1 -= (int)gameTime.ElapsedGameTime.Milliseconds;

			if (Timer1 <= 0)
			{
				myGame.SetCurrentLevel(Game1.GameLevels.MENU);
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// Draws the content of the splash screen manager.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
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
