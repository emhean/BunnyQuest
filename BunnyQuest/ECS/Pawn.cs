using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.ECS
{
    /// <summary>
    /// A Pawn is an Entity controllable by the player.
    /// </summary>
    class Pawn : Entity
    {
        KeyboardState keyboardState, prev_keyboardState;

        public Pawn(uint UUID, ContentManager content) : base(UUID)
        {
        }


    }
}
