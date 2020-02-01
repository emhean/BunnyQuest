using Microsoft.Xna.Framework;

namespace BunnyQuest.ECS.Components
{
    class CmpAI_Follower : Component
    {
        public enum STATE_CmpAI_Follower
        {
            NoneToFollow,
            //Separated,
            Following,
        }

        STATE_CmpAI_Follower state;
        public STATE_CmpAI_Follower State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public CmpAI_Follower(Entity owner, Vector2 speed) : base(owner)
        {
            this.speed = speed;
        }

        public void SetLeader(Entity entity)
        {
            entity_toFollow = entity;
            //this.state = STATE_CmpAI_Follower.Following;
        }

        public void RemoveLeader()
        {
            //this.state = STATE_CmpAI_Follower.NoneToFollow;
        }

        public Entity entity_toFollow;
        public Vector2 direction;
        public Vector2 speed;
        public float distance_whenToFollow = 48f; // if tile is 32, one and a half tile distance
        public float distance_whenToSeparate = 192f; // distance when this follower is too far away to follow

        public Vector2 GetVelocity()
        {
            return direction * speed;
        }

        /// <summary>
        /// Will crash if entity to follow is null.
        /// </summary>
        public float GetDistance()
        {
            return Vector2.Distance(entity.GetCenterPosition(), entity_toFollow.GetCenterPosition());
        }

        public override void Update(float delta)
        {
            // Fast exit if entity to follow is null
            if(entity_toFollow == null)
            {
                state = STATE_CmpAI_Follower.NoneToFollow;
                return;
            }

            float dist = GetDistance();

            if(dist > distance_whenToSeparate) // if distance is big enough to separate
            {
                //state = STATE_CmpAI_Follower.Separated;
                state = STATE_CmpAI_Follower.NoneToFollow;
            }
            else if (dist > distance_whenToFollow) // if distance is big enough to follow but not separate
            {
                direction = (Vector2.Normalize(Vector2.Subtract(entity_toFollow.GetCenterPosition(), entity.GetCenterPosition())));
                entity.pos += (direction * speed);

                state = STATE_CmpAI_Follower.Following;
            }
        }
    }
}
