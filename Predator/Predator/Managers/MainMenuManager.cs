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


namespace Predator
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MainMenuManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Game1 myGame;
        SpriteBatch spriteBatch;

        Camera camera;

        public int levelSelect = 0; //when zoomed in the hud is always the same(same start button) so when you first click on the level it will set this value so when you click on the start it will load the level using a switch
        // level 1 = 1, level 2 = 2... etc.
        Texture2D menuBackground;

        Texture2D startTexture;
        Texture2D quitTexture;






        List<Sprite.AnimationSet> buttonAnimationSet;
        List<Sprite.AnimationSet> buttonAnimationSet2;

        Button startButton;
        Button quitButton;


        public MainMenuManager(Game1 game)
            : base(game)
        {
            myGame = game;
            spriteBatch = new SpriteBatch(myGame.GraphicsDevice);
            menuBackground = Game.Content.Load<Texture2D>(@"images\mainMenu\mainMenu");
            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            buttonAnimationSet = new List<Sprite.AnimationSet>();
            buttonAnimationSet2 = new List<Sprite.AnimationSet>();


            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            startTexture = Game.Content.Load<Texture2D>(@"images\mainMenu\startButton");
            quitTexture = Game.Content.Load<Texture2D>(@"images\mainMenu\quitButton");


            camera = new Camera(myGame.GraphicsDevice.Viewport, new Point(2048, 1536), 1f);
            camera.Position = new Vector2(512, 256);

            //start button
            buttonAnimationSet.Add(new Sprite.AnimationSet("IDLE", startTexture, new Point(100, 50), new Point(1, 1), new Point(0, 0), 16000, false));
            buttonAnimationSet.Add(new Sprite.AnimationSet("HOVER", startTexture, new Point(100, 50), new Point(1, 1), new Point(100, 0), 16000, false));
            buttonAnimationSet.Add(new Sprite.AnimationSet("PRESSED", startTexture, new Point(100, 50), new Point(1, 1), new Point(200, 0), 16000, false));
            //quit button
            buttonAnimationSet2.Add(new Sprite.AnimationSet("IDLE", quitTexture, new Point(100, 50), new Point(1, 1), new Point(0, 0), 16000, false));
            buttonAnimationSet2.Add(new Sprite.AnimationSet("HOVER", quitTexture, new Point(100, 50), new Point(1, 1), new Point(100, 0), 16000, false));
            buttonAnimationSet2.Add(new Sprite.AnimationSet("PRESSED", quitTexture, new Point(100, 50), new Point(1, 1), new Point(200, 0), 16000, false));




             startButton = new Button(new Vector2((myGame.WindowSize.X - 100) / 2, 300), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet);
             quitButton = new Button(new Vector2((myGame.WindowSize.X - 100) / 2, 550), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet2);




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





            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            //zooming, buttons on map
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransformation());
            {
               spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);
               startButton.Draw(gameTime, spriteBatch);
               quitButton.Draw(gameTime, spriteBatch);

            }
            spriteBatch.End();



        }
    }
}
