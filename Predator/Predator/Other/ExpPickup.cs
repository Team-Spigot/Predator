using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VoidEngine;
using VoidEngine.VGame;
using VoidEngine.Helpers;

namespace Predator.Other
{
    public class ExpPickUp : Sprite
    {
        /// <summary>
        /// The game that the health pick up runs off of.
        /// </summary>
        private Game1 myGame;
        /// <summary>
        /// Gets or sets if the health pick up should be deleted.
        /// </summary>
        public bool DeleteMe
        {
            get;
            set;
        }

        /// <summary>
        /// Creates the health pickup object, with default animation sets.
        /// </summary>
        /// <param name="texture">The texture use with the health pick up.</param>
        /// <param name="position">The position to place the health pickup at.</param>
        /// <param name="myGame">The game that the health pick up runs off of.</param>
        public ExpPickUp(Texture2D texture, Vector2 position, Game1 myGame)
            : base(position, Color.White, texture)
        {
            this.myGame = myGame;

            inbounds = new Rectangle((int)(0), (int)(0), (int)((texture.Width / 4) / 2), texture.Height);

            AddAnimations(texture);
        }

        /// <summary>
        /// Updates the health pick up.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            HandleAnimations(gameTime);

            Rectangle playerCollisions = myGame.gameManager.Player.BoundingCollisions;

            if (BoundingCollisions.TouchLeftOf(playerCollisions) || BoundingCollisions.TouchTopOf(playerCollisions) || BoundingCollisions.TouchRightOf(playerCollisions) || BoundingCollisions.TouchBottomOf(playerCollisions))
            {
                DeleteMe = true;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the health pick up.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="spriteBatch">The sprite batch that the game uses.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Adds animations for the health pick up.
        /// </summary>
        /// <param name="texture">The texture to process.</param>
        protected override void AddAnimations(Texture2D texture)
        {
            AddAnimation("IDLE", texture, new Point(15, 15), new Point(3, 0), new Point(0, 0), 75, true);
            SetAnimation("IDLE");
        }
    }
}
