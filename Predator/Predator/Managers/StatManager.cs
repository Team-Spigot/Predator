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


namespace Predator
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class StatManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Game1 myGame;
        SpriteBatch spriteBatch;

        public int levelSelect = 0; //when zoomed in the hud is always the same(same start button) so when you first click on the level it will set this value so when you click on the start it will load the level using a switch
        // level 1 = 1, level 2 = 2... etc.
        Texture2D menuBackground;

        Texture2D plusTexture;
        Texture2D minusTexture;

        List<Sprite.AnimationSet> buttonAnimationSet;
        List<Sprite.AnimationSet> buttonAnimationSet2;

        Button plusButton;
        Button minusButton;


        public StatManager(Game1 game)
            : base(game)
        {
            myGame = game;
            menuBackground = Game.Content.Load<Texture2D>(@"images\tiles\temp");
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
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

            plusTexture = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\plusBtn");
            minusTexture = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\minusBtn");

            //plus button
            buttonAnimationSet.Add(new Sprite.AnimationSet("IDLE", plusTexture, new Point(100, 100), new Point(1, 1), new Point(0, 0), 16000, false));
            buttonAnimationSet.Add(new Sprite.AnimationSet("HOVER", plusTexture, new Point(100, 100), new Point(1, 1), new Point(0, 0), 16000, false));
            buttonAnimationSet.Add(new Sprite.AnimationSet("PRESSED", plusTexture, new Point(100, 100), new Point(1, 1), new Point(0, 0), 16000, false));
            //minus button
            buttonAnimationSet2.Add(new Sprite.AnimationSet("IDLE", minusTexture, new Point(100, 100), new Point(1, 1), new Point(0, 0), 16000, false));
            buttonAnimationSet2.Add(new Sprite.AnimationSet("HOVER", minusTexture, new Point(100, 100), new Point(1, 1), new Point(0, 0), 16000, false));
            buttonAnimationSet2.Add(new Sprite.AnimationSet("PRESSED", minusTexture, new Point(100, 100), new Point(1, 1), new Point(0, 0), 16000, false));




            plusButton = new Button(new Vector2((myGame.WindowSize.X - 100) / 2, 300), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet);
            minusButton = new Button(new Vector2((myGame.WindowSize.X - 100) / 2, 550), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet2);




            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            plusButton.Update(gameTime);
            minusButton.Update(gameTime);



            if (plusButton.Clicked() && myGame.gameManager.player.statPoints >= 1)
            {
                myGame.gameManager.player.statPoints -= 1;
                myGame.gameManager.player.PAgility += 1.5f;

            }
            if (minusButton.Clicked())
            {
                myGame.gameManager.player.PAgility -= 1.5f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                myGame.SetCurrentLevel(Game1.GameLevels.GAME);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            {
                plusButton.Draw(gameTime, spriteBatch);
                minusButton.Draw(gameTime, spriteBatch);
            }
			spriteBatch.End();

			base.Draw(gameTime);
        }
    }
}
