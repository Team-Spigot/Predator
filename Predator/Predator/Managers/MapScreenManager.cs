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
using VoidEngine.VGame;
using VoidEngine.VGUI;
using VoidEngine.Helpers;

namespace Predator.Managers
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class MapScreenManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game1 myGame;
		SpriteBatch spriteBatch;

		Camera camera;

		/// <summary>
		/// Loads the line texture.
		/// </summary>
		public Texture2D lineTexture;
		Texture2D mapBackdrop;
		Texture2D mapBackground;
		Texture2D mapHudTexture;
		Texture2D buttonTexture;
		Texture2D backButtonTexture;
		Texture2D hudBackButtonTexture;

		float Speed = 10;
		int HudScroll = 1024;
		public int levelSelect = 0; //when zoomed in the hud is always the same(same start button) so when you first click on the level it will set this value so when you click on the start it will load the level using a switch
		// level 1 = 1, level 2 = 2... etc.

		Vector2 Direction;

		public bool levelPullUp = false; //becomes true when you select a level pulling up level info hud
		public bool isZoomingIn = false;
		public bool isZoomingOut = false;
		public bool isHudScrollingIn = false;
		public bool isHudScrollingOut = false;

		List<Sprite.AnimationSet> buttonAnimationSet;

		Button sewer1Button; //Level Select 1
		Button sewer2Button;//Level Select 2
		Button City1Button;//Level Select 3
		Button City2Button;//Level Select 4
		Button BridgeButton;//Level Select 5
		Button HQ1Button;//Level Select 6
		Button BossButton;//Level Select 7

		Button StartButton;
		Button backButton;
		Button hudBackButton;

		/// <summary>
		/// Gets or sets the tranition alpha variable.
		/// </summary>
		public float TransitionAlpha
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the level is transitioning into the map screen.
		/// </summary>
		public bool isTransitioningIn
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the level is transitioning out of the map screen.
		/// </summary>
		public bool isTransitioningOut
		{
			get;
			set;
		}
		public bool SetAlpha
		{
			get;
			set;
		}

		Label debugLabel;
		string[] debugStrings = new string[10];

		public MapScreenManager(Game1 game)
			: base(game)
		{
			myGame = game;
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);
			Initialize();
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public override void Initialize()
		{
			buttonAnimationSet = new List<Sprite.AnimationSet>();

			// TODO: Add your initialization code here

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			lineTexture = Game.Content.Load<Texture2D>(@"images\other\line");
			mapBackground = Game.Content.Load<Texture2D>(@"images\gui\map\mapBackground");
			mapBackdrop = Game.Content.Load<Texture2D>(@"images\gui\map\mapBackdrop");
			buttonTexture = Game.Content.Load<Texture2D>(@"images\gui\map\mapButtons");
			hudBackButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\map\mapButtons");
			backButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\map\mapButtons");

			mapHudTexture = Game.Content.Load<Texture2D>(@"images\gui\map\mapHud");

			camera = new Camera(myGame.GraphicsDevice.Viewport, new Point(mapBackground.Width, mapBackground.Height), 1f);
			camera.Position = new Vector2(512, 256);

			camera.Zoom = 0.5f;

			// The Map Buttons
			buttonAnimationSet.Add(new Sprite.AnimationSet("IDLE", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(0, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("HOVER", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(20, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("PRESSED", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(40, 0), 16000, false));
			// Hud Back Button
			buttonAnimationSet.Add(new Sprite.AnimationSet("IDLE", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(0, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("HOVER", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(20, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("PRESSED", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(40, 0), 16000, false));
			// Back Button
			buttonAnimationSet.Add(new Sprite.AnimationSet("IDLE", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(0, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("HOVER", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(20, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("PRESSED", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(40, 0), 16000, false));

			sewer1Button = new Button(buttonTexture, new Vector2(50 + 110, 50 + 1250), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, camera);
			sewer2Button = new Button(buttonTexture, new Vector2(50 + 475, 50 + 1120), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, camera);
			City1Button = new Button(buttonTexture, new Vector2(50 + 950, 50 + 1100), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, camera);
			City2Button = new Button(buttonTexture, new Vector2(50 + 1300, 50 + 646), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, camera);
			BridgeButton = new Button(buttonTexture, new Vector2(50 + 1655, 50 + 840), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, camera);
			HQ1Button = new Button(buttonTexture, new Vector2(50 + 1886, 50 + 518), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, camera);
			BossButton = new Button(buttonTexture, new Vector2(50 + 1805, 50 + 120), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, camera);
			backButton = new Button(buttonTexture, new Vector2(50 + 50, 50 + 700), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);
			hudBackButton = new Button(buttonTexture, new Vector2(50 + 100, 50 + 700), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, camera);

			StartButton = new Button(myGame.mainMenuManager.ButtonTexture, new Vector2(HudScroll + 55, 150), myGame.segoeUIRegular, 1f, Color.White, "START", Color.White);

			backButton = new Button(new Vector2(50, 700), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet);

			hudBackButton = new Button(new Vector2(100, 700), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet);

			debugLabel = new Label(new Vector2(350, 15), myGame.segoeUIMonoDebug, 1.0f, Color.White, "");

			base.LoadContent();
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// TODO: Add your update code here
			if (isTransitioningIn)
			{
				isTransitioningOut = false;

				TransitionAlpha += 0.05f;

				if (TransitionAlpha > 1.0f)
				{
					isTransitioningIn = false;
					TransitionAlpha = 1.0f;
				}
			}

			if (isTransitioningOut)
			{
				isTransitioningIn = false;

				TransitionAlpha -= 0.05f;

				if (TransitionAlpha <= 0.0f)
				{
					isTransitioningOut = false;
					TransitionAlpha = 0.0f;

					myGame.SetCurrentLevel(Game1.GameLevels.GAME);
				}
			}

			if (isZoomingIn == true)
			{
				camera.Zoom += 0.01f;
				camera.Position += Direction * Speed;

			}
			if (isZoomingOut == true)
			{
				camera.Zoom -= 0.01f;
				camera.Position -= Direction * Speed;

			}
			if (camera.Zoom <= 0.5f)
			{
				camera.Zoom = 0.5f;
				camera.Position = new Vector2(0, 0);
				isZoomingOut = false;
			}

			if (isHudScrollingIn)
			{
				HudScroll -= 10;
				if (HudScroll <= 527)
				{
					HudScroll = 527;
					isHudScrollingIn = false;
				}
			}
			if (isHudScrollingOut)
			{
				HudScroll += 10;
				if (HudScroll >= 1024)
				{
					HudScroll = 1024;
					isHudScrollingOut = false;
				}
			}
			switch (levelSelect)
			{
				case 1:
					if (camera.Position.X - sewer1Button.Position.X >= 100 && camera.Position.Y - sewer1Button.Position.Y <= 100)
					{
						Speed = 0.0f;
					}
					break;
				case 2:
					if (camera.Position.X - sewer2Button.Position.X <= 100 && camera.Position.Y - sewer2Button.Position.Y <= 100)
					{
						Speed = 0.0f;
					}
					break;
				case 3:
					if (camera.Position.X - City1Button.Position.X >= 100 && camera.Position.Y - City1Button.Position.Y <= 100)
					{
						Speed = 0.0f;
					}
					break;
				case 4:
					if (camera.Position.X - City2Button.Position.X <= 100 && camera.Position.Y - City2Button.Position.Y <= 100)
					{
						Speed = 0.0f;
					}
					break;
				case 5:
					if (camera.Position.X - BridgeButton.Position.X >= 100 && camera.Position.Y - BridgeButton.Position.Y <= 100)
					{
						Speed = 0.0f;
					}
					break;
				case 6:
					if (camera.Position.X - HQ1Button.Position.X <= 100 && camera.Position.Y - HQ1Button.Position.Y <= 100)
					{
						Speed = 0.0f;
					}
					break;
				case 7:
					if (camera.Position.X - BossButton.Position.X <= 100 && camera.Position.Y - BossButton.Position.Y <= 100)
					{
						Speed = 0.0f;
					}
					break;
			}
			if (levelPullUp == false)
			{
				Vector2 tempDirection;

				sewer1Button.Update(gameTime);
				sewer2Button.Update(gameTime);
				City1Button.Update(gameTime);
				City2Button.Update(gameTime);
				BridgeButton.Update(gameTime);
				HQ1Button.Update(gameTime);
				BossButton.Update(gameTime);

				backButton.Update(gameTime);
				if (sewer1Button.Clicked()) // Zooming into zone 1
				{
					Speed = 10.0f;
					levelSelect = 1;
					isHudScrollingIn = true;
					levelPullUp = true;
					isZoomingIn = true;
					isZoomingOut = false;
					tempDirection = new Vector2(sewer1Button.Position.X - camera.Position.X, sewer1Button.Position.Y - camera.Position.Y);
					Direction = CollisionHelper.UnitVector(tempDirection);
				}
				if (sewer2Button.Clicked()) // Zooming into zone 2
				{
					Speed = 10.0f;
					levelSelect = 2;
					isHudScrollingIn = true;
					levelPullUp = true;
					isZoomingIn = true;
					isZoomingOut = false;
					tempDirection = new Vector2(sewer2Button.Position.X - camera.Position.X, sewer2Button.Position.Y - camera.Position.Y);
					Direction = CollisionHelper.UnitVector(tempDirection);
				}
				if (City1Button.Clicked()) // Zooming into zone 2
				{
					Speed = 10.0f;
					levelSelect = 3;
					isHudScrollingIn = true;
					levelPullUp = true;
					isZoomingIn = true;
					isZoomingOut = false;
					tempDirection = new Vector2(City1Button.Position.X - camera.Position.X, City1Button.Position.Y - camera.Position.Y);
					Direction = CollisionHelper.UnitVector(tempDirection);
				}
				if (City2Button.Clicked()) // Zooming into zone 2
				{
					Speed = 10.0f;
					levelSelect = 4;
					isHudScrollingIn = true;
					levelPullUp = true;
					isZoomingIn = true;
					isZoomingOut = false;
					tempDirection = new Vector2(City2Button.Position.X - camera.Position.X, City2Button.Position.Y - camera.Position.Y);
					Direction = CollisionHelper.UnitVector(tempDirection);
				}
				if (BridgeButton.Clicked()) // Zooming into zone 2
				{
					Speed = 10.0f;
					levelSelect = 5;
					isHudScrollingIn = true;
					levelPullUp = true;
					isZoomingIn = true;
					isZoomingOut = false;
					tempDirection = new Vector2(BridgeButton.Position.X - camera.Position.X, BridgeButton.Position.Y - camera.Position.Y);
					Direction = CollisionHelper.UnitVector(tempDirection);
				}
				if (HQ1Button.Clicked()) // Zooming into zone 2
				{
					Speed = 10.0f;
					levelSelect = 6;
					isHudScrollingIn = true;
					levelPullUp = true;
					isZoomingIn = true;
					isZoomingOut = false;
					tempDirection = new Vector2(HQ1Button.Position.X - camera.Position.X, HQ1Button.Position.Y - camera.Position.Y);
					Direction = CollisionHelper.UnitVector(tempDirection);
				}
				if (BossButton.Clicked()) // Zooming into zone 2
				{
					Speed = 10.0f;
					levelSelect = 6;
					isHudScrollingIn = true;
					levelPullUp = true;
					isZoomingIn = true;
					isZoomingOut = false;
					tempDirection = new Vector2(BossButton.Position.X - camera.Position.X, BossButton.Position.Y - camera.Position.Y);
					Direction = CollisionHelper.UnitVector(tempDirection);
				}
				if (backButton.Clicked())
				{
					isTransitioningOut = true;
				}
			}
			if (levelPullUp == true)
			{
				Vector2 tempDirection;

				hudBackButton.Update(gameTime);
				StartButton.Update(gameTime);
				StartButton.Position = new Vector2(HudScroll + 55, 150);

				if (hudBackButton.Clicked())
				{
					levelPullUp = false;
					isZoomingOut = true;
					isZoomingIn = false;
					isHudScrollingOut = true;

					switch (levelSelect) // Zooming out
					{
						case 1:
							Speed = 10.0f;
							tempDirection = new Vector2(camera.Position.X - sewer1Button.Position.X, camera.Position.Y - sewer1Button.Position.Y);
							Direction = CollisionHelper.UnitVector(tempDirection);
							break;
						case 2:
							Speed = 10.0f;
							tempDirection = new Vector2(camera.Position.X - sewer2Button.Position.X, camera.Position.Y - sewer2Button.Position.Y);
							Direction = CollisionHelper.UnitVector(tempDirection);
							break;
						case 3:
							Speed = 10.0f;
							tempDirection = new Vector2(camera.Position.X - City1Button.Position.X, camera.Position.Y - City1Button.Position.Y);
							Direction = CollisionHelper.UnitVector(tempDirection);
							break;
						case 4:
							Speed = 10.0f;
							tempDirection = new Vector2(camera.Position.X - City2Button.Position.X, camera.Position.Y - City2Button.Position.Y);
							Direction = CollisionHelper.UnitVector(tempDirection);
							break;
						case 5:
							Speed = 10.0f;
							tempDirection = new Vector2(camera.Position.X - BridgeButton.Position.X, camera.Position.Y - BridgeButton.Position.Y);
							Direction = CollisionHelper.UnitVector(tempDirection);
							break;
						case 6:
							Speed = 10.0f;
							tempDirection = new Vector2(camera.Position.X - HQ1Button.Position.X, camera.Position.Y - HQ1Button.Position.Y);
							Direction = CollisionHelper.UnitVector(tempDirection);
							break;
						case 7:
							Speed = 10.0f;
							tempDirection = new Vector2(camera.Position.X - BossButton.Position.X, camera.Position.Y - BossButton.Position.Y);
							Direction = CollisionHelper.UnitVector(tempDirection);
							break;
					}
				}

				if (StartButton.Clicked())
				{
					switch (levelSelect)
					{
						case 1:
							myGame.gameManager.CurrentLevel = 1;
							myGame.gameManager.LevelLoaded = false;
							isTransitioningOut = true;
							break;
						case 2:
							myGame.gameManager.CurrentLevel = 2;
							myGame.gameManager.LevelLoaded = false;
							isTransitioningOut = true;
							break;
						case 3:
							myGame.gameManager.CurrentLevel = 3;
							myGame.gameManager.LevelLoaded = false;
							isTransitioningOut = true;
							break;
						case 4:
							myGame.gameManager.CurrentLevel = 4;
							myGame.gameManager.LevelLoaded = false;
							isTransitioningOut = true;
							break;
						case 5:
							myGame.gameManager.CurrentLevel = 5;
							myGame.gameManager.LevelLoaded = false;
							isTransitioningOut = true;
							break;
						case 6:
							myGame.gameManager.CurrentLevel = 6;
							myGame.gameManager.LevelLoaded = false;
							isTransitioningOut = true;
							break;
						case 7:
							myGame.gameManager.CurrentLevel = 7;
							myGame.gameManager.LevelLoaded = false;
							isTransitioningOut = true;
							break;
					}
				}
			}

			base.Update(gameTime);
		}



		public override void Draw(GameTime gameTime)
		{
			//zooming, buttons on map
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransformation());
			{
				if (!isTransitioningIn && !isTransitioningOut)
				{
					spriteBatch.Draw(mapBackdrop, new Rectangle(0, 0, camera.Size.X, camera.Size.Y), Color.White);
					spriteBatch.Draw(mapBackground, new Rectangle(50, 50, (int)(mapBackground.Width - 100), (int)(mapBackground.Height - 100)), Color.White);
					sewer1Button.Draw(gameTime, spriteBatch);
					sewer2Button.Draw(gameTime, spriteBatch);
					City1Button.Draw(gameTime, spriteBatch);
					City2Button.Draw(gameTime, spriteBatch);
					HQ1Button.Draw(gameTime, spriteBatch);
					BossButton.Draw(gameTime, spriteBatch);
					BridgeButton.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			spriteBatch.Begin();
			{
				if (!isTransitioningIn && !isTransitioningOut)
				{
					spriteBatch.Draw(mapHudTexture, new Vector2(HudScroll, 0), Color.White);

					if (levelPullUp == false)
					{
						backButton.Draw(gameTime, spriteBatch);
					}
					if (levelPullUp == true)
					{
						hudBackButton.Draw(gameTime, spriteBatch);
						StartButton.Draw(gameTime, spriteBatch);
					}
				}

				if (isTransitioningIn || isTransitioningOut)
				{
					spriteBatch.Draw(lineTexture, new Rectangle(0, 0, myGame.WindowSize.X, myGame.WindowSize.X), new Color(0, 0, 0, TransitionAlpha));
				}

				if (myGame.IsGameDebug)
				{
					debugLabel.Draw(gameTime, spriteBatch);

					spriteBatch.Draw(lineTexture, new Rectangle((int)sewer1Button.Position.X, (int)sewer1Button.Position.Y, 1000, 1), Color.Red);
					spriteBatch.Draw(lineTexture, new Rectangle((int)sewer1Button.Position.X, (int)sewer1Button.Position.Y, 1, 1000), Color.Red);
					spriteBatch.Draw(lineTexture, new Rectangle((int)(sewer1Button.Position.X / camera.Zoom), (int)(sewer1Button.Position.Y / camera.Zoom), 1000, 1), Color.Blue);
					spriteBatch.Draw(lineTexture, new Rectangle((int)(sewer1Button.Position.X / camera.Zoom), (int)(sewer1Button.Position.Y / camera.Zoom), 1, 1000), Color.Blue);
				}
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
