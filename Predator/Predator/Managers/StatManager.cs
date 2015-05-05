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



        public int levelSelect = 0; //when zoomed in the hud is always the same(same start button) so when you first click on the level it will set this value so when you click on the start it will load the level using a switch
        // level 1 = 1, level 2 = 2... etc.
        Texture2D menuBackground;

        Texture2D plusTexture;

        Texture2D exitBtnTex;

        List<Sprite.AnimationSet> buttonAnimationSet;
        List<Sprite.AnimationSet> buttonAnimationSet2;
        List<Sprite.AnimationSet> buttonAnimationSet3;
        List<Sprite.AnimationSet> exitButtonAnimationSet;

        Button plusButtonA;
        Button plusButtonS;
        Button plusButtonD;
        Button exitBtn;



        public StatManager(Game1 game)
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
            buttonAnimationSet2 = new List<Sprite.AnimationSet>();
            buttonAnimationSet3 = new List<Sprite.AnimationSet>();
            exitButtonAnimationSet = new List<Sprite.AnimationSet>();


            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            plusTexture = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\Icon+Sprite");
            menuBackground = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\Menu#3");
            exitBtnTex = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\Exitbuttonsprite");



            //camera = new Camera(myGame.GraphicsDevice.Viewport, new Point(2048, 1536), 1f);
            //camera.Position = new Vector2(512, 256);

            //Agility
            buttonAnimationSet.Add(new Sprite.AnimationSet("IDLE", plusTexture, new Point(45, 45), new Point(1, 1), new Point(0, 0), 16000, false));
            buttonAnimationSet.Add(new Sprite.AnimationSet("HOVER", plusTexture, new Point(45, 45), new Point(1, 1), new Point(46, 0), 16000, false));
            buttonAnimationSet.Add(new Sprite.AnimationSet("CLICKED", plusTexture, new Point(45, 45), new Point(1, 1), new Point(91, 0), 16000, false));

            //Strength
            buttonAnimationSet2.Add(new Sprite.AnimationSet("IDLE", plusTexture, new Point(45, 45), new Point(1, 1), new Point(0, 0), 16000, false));
            buttonAnimationSet2.Add(new Sprite.AnimationSet("HOVER", plusTexture, new Point(45, 45), new Point(1, 1), new Point(46, 0), 16000, false));
            buttonAnimationSet2.Add(new Sprite.AnimationSet("CLICKED", plusTexture, new Point(45, 45), new Point(1, 1), new Point(91, 0), 16000, false));

            //Defense
            buttonAnimationSet3.Add(new Sprite.AnimationSet("IDLE", plusTexture, new Point(45, 45), new Point(1, 1), new Point(0, 0), 16000, false));
            buttonAnimationSet3.Add(new Sprite.AnimationSet("HOVER", plusTexture, new Point(45, 45), new Point(1, 1), new Point(46, 0), 16000, false));
            buttonAnimationSet3.Add(new Sprite.AnimationSet("CLICKED", plusTexture, new Point(45, 45), new Point(1, 1), new Point(91, 0), 16000, false));

            //Exit Button
            exitButtonAnimationSet.Add(new Sprite.AnimationSet("IDLE", exitBtnTex, new Point(40, 40), new Point(1, 1), new Point(0, 0), 160000, false));
            exitButtonAnimationSet.Add(new Sprite.AnimationSet("HOVER", exitBtnTex, new Point(40, 40), new Point(1, 1), new Point(40, 0), 160000, false));



            plusButtonS = new Button(new Vector2(575, 225), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet);
            plusButtonA = new Button(new Vector2(575, 425), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet2);
            plusButtonD = new Button(new Vector2(575, 325), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet3);
            exitBtn = new Button(new Vector2(742, 158), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, exitButtonAnimationSet);


            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            plusButtonA.Update(gameTime);
            plusButtonS.Update(gameTime);
            plusButtonD.Update(gameTime);
            exitBtn.Update(gameTime);



            if (plusButtonA.Clicked() && myGame.gameManager.Player.statPoints >= 1)
            {
                myGame.gameManager.Player.statPoints -= 1;
                myGame.gameManager.Player.PAgility -= 0.055f * (1 - (myGame.gameManager.Player.PAgility * .1f));

            }
            if (plusButtonS.Clicked() && myGame.gameManager.Player.statPoints >= 1)
            {
                myGame.gameManager.Player.statPoints -= 1;
                myGame.gameManager.Player.PStrength += 0.18f;
                // myGame.gameManager.player.MaxHP *= myGame.gameManager.player.PStrength;

            }
            if (plusButtonD.Clicked() && myGame.gameManager.Player.statPoints >= 1)
            {
                myGame.gameManager.Player.statPoints -= 1;
                myGame.gameManager.Player.PDefense += 0.18f;

            }
            if (exitBtn.Clicked())
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
                plusButtonA.Draw(gameTime, spriteBatch);
                plusButtonS.Draw(gameTime, spriteBatch);
                plusButtonD.Draw(gameTime, spriteBatch);
                exitBtn.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
