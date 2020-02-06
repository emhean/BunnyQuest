using Microsoft.Xna.Framework;
using System;

namespace BunnyQuest.ECS.Components
{
    class CmpAI_Follower : Component
    {
        public enum STATE_CmpAI_Follower
        {
            NoneToFollow,
            Following,
        }

        public CmpAI_Follower(Entity owner, Vector2 speed) : base(owner)
        {
            this.speed = speed;
        }

        public STATE_CmpAI_Follower State { get; set; }
        public Entity entity_toFollow;
        public Vector2 direction;
        public Vector2 speed;
        public float distance_whenToFollow = 48f; // if tile is 32, one and a half tile distance
        public float distance_whenToSeparate = 592f; // distance when this follower is too far away to follow

        public bool destination_set;
        public Vector2 destination;
        Vector2 pos_previous;
        int count_whenIsStuck = 0;


        public event EventHandler<EntityArgs> StoppedFollowing;
        public event EventHandler<EntityArgs> DestinationReached;

        public void OnStoppedFollowing()
        {
            entity_toFollow = null;
            direction = Vector2.Zero;
            State = STATE_CmpAI_Follower.NoneToFollow;

            if (StoppedFollowing != null) // there is a hook
            {
                StoppedFollowing.Invoke(this.entity, new EntityArgs(this.entity, this));
            }
        }

        protected void OnDestinationReached()
        {
            destination_set = false;

            entity_toFollow = null;
            direction = Vector2.Zero;
            State = STATE_CmpAI_Follower.NoneToFollow;

            if (DestinationReached != null)
                DestinationReached.Invoke(this, new EntityArgs(this.entity, this));
        }

        public void SetDestination(Vector2 destination)
        {
            this.destination = destination;
            destination_set = true;
        }

        public void SetLeader(Entity entity)
        {
            entity_toFollow = entity;
            this.State = STATE_CmpAI_Follower.Following;
        }

        public void RemoveLeader()
        {
            this.State = STATE_CmpAI_Follower.NoneToFollow;
        }

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

        public float GetDistanceFromDestination()
        {
            return Vector2.Distance(entity.GetCenterPosition(), destination);
        }

        private void CheckIfStuck(Vector2 targetPosition)
        {
            // maybe < ??????????
            if (Vector2.Distance(entity.GetCenterPosition(), pos_previous) > (direction.X * speed.X))
            {
                count_whenIsStuck += 1;

                if (count_whenIsStuck >= 400)
                {
                    if (destination_set)
                    {
                        //OnDestinationReached(); This caused a bug why was this here
                        OnStoppedFollowing();
                    }
                    else if (entity_toFollow != null)
                    {
                        OnStoppedFollowing();
                    }

                    count_whenIsStuck = 0;
                }
            }
        }

        public override void Update(float delta)
        {
            if (destination_set)
            {
                direction = (Vector2.Normalize(Vector2.Subtract(destination, entity.GetCenterPosition())));

                pos_previous = entity.GetCenterPosition();
                entity.pos += (direction * speed);

                if (GetDistanceFromDestination() < 32)
                {
                    OnDestinationReached();
                }

                CheckIfStuck(destination);
            }
            else if (entity_toFollow != null)
            {
                float dist = GetDistance();

                if (dist > distance_whenToSeparate) // if distance is big enough to separate
                {
                    State = STATE_CmpAI_Follower.NoneToFollow;
                }
                else if (dist > distance_whenToFollow) // if distance is big enough to follow but not separate
                {
                    direction = (Vector2.Normalize(Vector2.Subtract(entity_toFollow.GetCenterPosition(), entity.GetCenterPosition())));
                    entity.pos += (direction * speed);

                    State = STATE_CmpAI_Follower.Following;
                }

            }
            else
            {
                // State check before invoke, to prevent invoking event every frame
                if (State != STATE_CmpAI_Follower.NoneToFollow)
                {
                    OnStoppedFollowing();
                }
            }
        }
    }
}
