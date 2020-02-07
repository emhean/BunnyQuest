using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BunnyQuest.ECS.Components
{
    /// <summary>
    /// The AI component contains a velocity and a destination.
    /// The velocity is set when the component is created and can be updated later if so desired.
    /// The destination is continuously updated in System.cs, as the enemies' movement depends on where the player is.
    /// 
    /// Patrol points are now active
    /// they should hopefully work
    /// </summary>
    class CmpAI : Component
    {
        public CmpAI(Entity owner, Vector2 init_velocity, bool in_is_patrolling, bool in_is_chasing, List<Vector2> patrol_point_list) : base(owner)
        {
            velocity = init_velocity;
            is_patrolling = in_is_patrolling;
            is_chasing = in_is_chasing;
            patrol_points = patrol_point_list;
        }

        public bool is_patrolling;
        public bool is_chasing;

        public Vector2 destination;
        public Vector2 velocity;

        public float patrol_timer;

        
        public List<Vector2> patrol_points;
        public int which_point;

        public void set_patrol_timer(int time)
        {
            patrol_timer = time;
         }

        public void set_patrol_points(Vector2[] points)
        {
            patrol_points.AddRange(points);
        }

        // Puts the first element of the parol point list last
        public void cycle_patrol_points()
        {
            if (which_point < patrol_points.Count -  1)
                {
                    ++which_point;
                    Console.WriteLine(patrol_points[0]);
                }
            else
                {
                    which_point = 0;
                }
        }

        // Makes it possible to change AI type (useful to make enemies stop etc)
        public void set_ai_type(string ai_type)
        {
            if (ai_type == "chasing")
            {
                is_patrolling = false;
                is_chasing = true;
            }
            else if (ai_type == "patrolling")
            {
                is_patrolling = true;
                is_chasing = false;
            }
            else if (ai_type == "stationary")
            {
                is_patrolling = false;
                is_chasing = false;
            }
        }
        
        public void update_velocity(Vector2 new_velocity)
            {
                velocity = new_velocity;
            }

        public void set_destination(Vector2 dest)
        {
            destination = dest;
        }

        public override void Update(float delta)
        {
            var dir = Vector2.Normalize(Vector2.Subtract(destination, parent.pos));

            if (patrol_timer > 0)
            {
                patrol_timer = patrol_timer - delta;
                Console.WriteLine(patrol_timer);
            }
            parent.pos += dir * velocity;
        }
    }
}
