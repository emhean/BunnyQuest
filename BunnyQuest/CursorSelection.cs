using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest
{
    class CursorSelection
    {
        #region Public fields
        public Texture2D tex;
        public Rectangle rect;
        public int border_size = 2;
        public bool selection_started;
        #endregion

        #region Private fields
        float t; // Time
        float t_updateRectBorders = 0.16f; // if 60fps, every 10th frame
        Rectangle rect_line1, rect_line2, rect_line3, rect_line4;
        Vector2 v_cursorStartPos;
        #endregion

        public CursorSelection(ContentManager contentManager)
        {
            tex = contentManager.Load<Texture2D>("csr_selection/selection");
        }

        /// <summary>
        /// This code handles the cursor selection rectangle.
        /// </summary>
        public void UpdateCursorSelection(Vector2 v_cursorPos)
        {
            if (v_cursorPos.Y > v_cursorStartPos.Y) // Selection is below start pos
            {
                if (v_cursorPos.X > v_cursorStartPos.X)// Cursor is to the right of start position
                {
                    rect.X = (int)v_cursorStartPos.X;
                    rect.Y = (int)v_cursorStartPos.Y;
                    rect.Width = (int)v_cursorPos.X - rect.X;
                    rect.Height = (int)v_cursorPos.Y - rect.Y;
                }
                else if (v_cursorPos.X < v_cursorStartPos.X)// Cursor is to the left of start position
                {
                    rect.X = (int)v_cursorPos.X;
                    rect.Y = (int)v_cursorStartPos.Y;
                    rect.Width = (int)v_cursorStartPos.X - (int)v_cursorPos.X;
                    rect.Height = (int)v_cursorPos.Y - rect.Y;
                }
            }
            else if (v_cursorPos.Y < v_cursorStartPos.Y) // Selection is above the start position
            {
                if (v_cursorPos.X > v_cursorStartPos.X) // Cursor is tto the right of starting position
                {
                    rect.X = (int)v_cursorStartPos.X;
                    rect.Y = (int)v_cursorPos.Y;
                    rect.Width = (int)v_cursorPos.X - rect.X;
                    rect.Height = (int)v_cursorStartPos.Y - (int)v_cursorPos.Y;
                }
                else if (v_cursorPos.X < v_cursorStartPos.X) // Cursor is to the left of sartin position
                {
                    rect.X = (int)v_cursorPos.X;
                    rect.Y = (int)v_cursorPos.Y;
                    rect.Width = (int)v_cursorStartPos.X - rect.X;
                    rect.Height = (int)v_cursorStartPos.Y - rect.Y;
                }
            }
        }

        /// <summary>
        /// Start the cursor selection by setting the flag to true and the start position from the parameter.
        /// </summary>
        public void Start(Vector2 v_cursorStartPos)
        {
            this.v_cursorStartPos = v_cursorStartPos;
            selection_started = true;
        }

        /// <summary>
        /// Ends the cursor selection, and sets the size to 0 and the started flag to false.
        /// </summary>
        public void End()
        {
            v_cursorStartPos = Vector2.Zero;
            rect = Rectangle.Empty;
            selection_started = false;
        }


        public void Update(float delta)
        {
            t += delta; // Usually 0.016f
            if(t > t_updateRectBorders) // if 60fps then this statement occurs every 10th frame
            {
                // Line 1
                rect_line1.X = rect.X;
                rect_line1.Y = rect.Y;
                rect_line1.Width = rect.Width;
                rect_line1.Height = border_size;

                // Line 2
                rect_line2.X = rect.X;
                rect_line2.Y = rect.Y;
                rect_line2.Width = border_size;
                rect_line2.Height = rect.Height;

                // Line 3
                rect_line3.X = rect.X + rect.Width;
                rect_line3.Y = rect.Y;
                rect_line3.Width = border_size;
                rect_line3.Height = rect.Height + border_size;

                // Line 4
                rect_line4.X = rect.X;
                rect_line4.Y = rect.Y + rect.Height;
                rect_line4.Width = rect.Width;
                rect_line4.Height = border_size;
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            if (rect != Rectangle.Empty)
            {
                spriteBatch.Draw(tex, rect_line1, Color.White);
                spriteBatch.Draw(tex, rect_line2, Color.White);
                spriteBatch.Draw(tex, rect_line3, Color.White);
                spriteBatch.Draw(tex, rect_line4, Color.White);
            }
        }
    }
}
