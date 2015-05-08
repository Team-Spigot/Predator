using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VoidEngine.VGame;

namespace VoidEngine.VGUI
{
	/// <summary>
	/// The button class for the VoidEngine
	/// </summary>
	public class Checkbox : Sprite
	{
		/// <summary>
		/// The types of Interactiable UI states.
		/// </summary>
		protected enum ButtonStates
		{
			Hover,
			Up,
			Down,
			Released,
			Disabled
		}
		/// <summary>
		/// The types of Checkbox states.
		/// </summary>
		protected enum CheckboxStates
		{
			Checked,
			Unchecked,
			Disabled
		}
		/// <summary>
		/// The types of buttons on the mouse.
		/// </summary>
		protected enum ButtonTypes
		{
			Left,
			Right,
			Middle
		}
		
		/// <summary>
		/// Struct for what the positions of the intercatible UI states are on the spreadsheet.
		/// </summary>
		public struct SetButtonPositions
		{
			/// <summary>
			/// The position of the hover state.
			/// </summary>
			public int HoverPosition;
			/// <summary>
			/// The position of the up state.
			/// </summary>
			public int UpPosition;
			/// <summary>
			/// The position of the down state.
			/// </summary>
			public int DownPosition;
			/// <summary>
			/// The position of the released state.
			/// </summary>
			public int ReleasedPosition;
			/// <summary>
			/// The position of the disabledState.
			/// </summary>
			public int DisabledPosition;
			
			/// <summary>
			/// Creates the SetButtonPositions struct.
			/// </summary>
			/// <param name="hoverPosition">The position of the hovered state. [0-4]</param>
			/// <param name="upPositon">The position of the up state. [0-4]</param>
			/// <param name="downPosition">The position of the down state. [0-4]</param>
			/// <param name="releasedPosition">The position of the released state. [0-4]</param>
			/// <param name="disabledPosition">The position of the disabled state. [0-4]</param>
			public void SetButtonPositions(int hoverPosition, int upPosition, int downPosition, int releasedPosition, int disabledPosition)
			{
				HoverPosition = hoverPosition;
				UpPosition = upPosition;
				DownPosition = downPosition;
				ReleasedPosition = releasedPosition;
				DisabledPosition = disabledPosition;
			}
		}
		
		/// <summary>
		/// Struct for what the positions of the checkbox UI states are on the spreadsheet.
		/// </summary>
		public struct SetCheckboxPositions
		{
			/// <summary>
			/// The position of the checked state.
			/// </summary>
			public int CheckedPosition;
			/// <summary>
			/// The position of the checked hover state.
			/// </summary>
			public int HoverCheckedPosition;
			/// <summary>
			/// The position of the unchecked state.
			/// </summary>
			public int UncheckedPosition;
			/// <summary>
			/// The position of the unchecked hover state.
			/// </summary>
			public int HoverUncheckedPosition;
			/// <summary>
			/// The position of the disabled state.
			/// </summary>
			public int DisabledPosition;
			
			/// <summary>
			/// Creates the SetCheckboxPOsitions struct.
			/// </summary>
			/// <param name="checkedPosition">The position of the checked state. [0-4]</param>
			/// <param name="hoverCheckedPosition">The position of the checked hover state. [0-4]</param>
			/// <param name="uncheckedPosition">The position of the unchecked state. [0-4]</param>
			/// <param name="hoverUncheckedPosition">The position of the unchecked hover state. [0-4]</param>
			/// <param name="disabledPosition">The position of the disabled state. [0-4]</param>
			public void SetCheckboxPositions(int checkedPosition, int hoverCheckedPosition, int uncheckedPosition, int hoverUncheckedPosition, int disabledState)
			{
				CheckedPosition = checkedPosition;
				HoverCheckedPosition = hoverCheckedPosition;
				UncheckedPosition = uncheckedPosition;
				HoverUncheckedPosition = hoverUncheckedPosition;
				DisableState = disabledState;
			}
		}

		/// <summary>
		/// The button state for the interactible UI.
		/// </summary>
		protected ButtonStates ButtonState = new ButtonStates();
		/// <summary>
		/// The button to use when interacting.
		/// </summary>
		protected ButtonTypes PrimaryButton = new ButtonTypes();
		
		/// <summary>
		/// To tell if the mouse has been pressed, or previously presed.
		/// </summary>
		protected bool MousePress, PreviousMousePress;
		/// <summary>
		/// To tell where the mouse is on the screen.
		/// </summary>
		protected Point MouseCords;
		/// <summary>
		/// To set where the intercatible UI's area is.
		/// </summary>
		protected Rectangle CollisionBounds;

		/// <summary>
		/// Creates an intercatible UI element.
		/// </summary>
		/// <param name="position">The position of the UI element.</summary>
		/// <param name="color">The color to mask the UI element.</summary>
		/// <param name="primaryButton">The button to interact with the UI element.</summary>
		/// <param name="animationSetList">The animation set list to set the animation of the UI element to.</summary>
		public InercatibleUI(Vector2 position, Color color, ButtonTypes primaryButton, List<Sprite.AnimationSet> animationSetList)
			: base(position, color, animationSetList)
		{
			AnimationSets = animationSetList;
			
			Color = color;
			PrimaryButton = primaryButton;
		}
		
		/// <summary>
		/// Creates an interactible UI element.
		/// </summary>
		/// <param name="texture">The texture of the UI element.</summary>
		/// <param name="position">The position of the UI element.</summary>
		/// <param name="pr"
		public InteractibleUI(Texture2D texture, Vector2 position, ButtonTypes primaryButton, Color color) : base(position, buttonColor, texture)
		{
			AddAnimations(texture);
			
			Color = color;
			PrimaryButton = primaryButton;
		}

		/// <summary>
		/// Returns if the button is hovered over or not.
		/// </summary>
		/// <param name="tx">The texture of the button</param>
		/// <param name="ty">The texture's y</param>
		/// <param name="frameTex">the texture's width and height in Point</param>
		/// <param name="x">the x of mouse</param>
		/// <param name="y">the y of mouse</param>
		/// <returns>Boolean</returns>
		protected bool hitButtonAlpha(Vector2 position, Point frameSize, Point mouseCords)
		{
			CollisionBounds = new Rectangle((int)position.X, (int)position.Y, frameSize.X, frameSize.Y);

			if (CollisionBounds.Intersects(new Rectangle(mouseCords.X, mouseCords.Y, 1, 1)))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns weither if the button was clicked.
		/// </summary>
		/// <returns>Boolean</returns>
		public bool IsChecked
		{
			get
			{
				if (CheckboxState == CheckboxStates.Checked)
				{
					return true;
				}
				
				return false;
			}
		}

		/// <summary>
		/// Used to draw the button.
		/// </summary>
		/// <param name="gameTime">The game time that the game runs off of.</param>
		/// <param name="spriteBatch">The sprite batch used to draw with.</param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			label.Draw(gameTime, spriteBatch);
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("UNCHECKED",	  texture, new Point(texture.Width / 4, texture.Height), new Point(1, 1), new Point((texture.Width / 4) * DefaultButtonPositions.UncheckedPosition,	  0), 1600, true);
			AddAnimation("HOVERUNCHECKED", texture, new Point(texture.Width / 4, texture.Height), new Point(1, 1), new Point((texture.Width / 4) * DefaultButtonPositions.HoverUncheckedPosition, 0), 1600, true);
			AddAnimation("CHECKED",		texture, new Point(texture.Width / 4, texture.Height), new Point(1, 1), new Point((texture.Width / 4) * DefaultButtonPositions.CheckedPosition,		0), 1600, true);
			AddAnimation("HOVERCHECKED",   texture, new Point(texture.Width / 4, texture.Height), new Point(1, 1), new Point((texture.Width / 4) * DefaultButtonPositions.HoverCheckedPosition,   0), 1600, true);
			SetAnimation("UNCHECKED");

			base.AddAnimations(texture);
		}
	}
}