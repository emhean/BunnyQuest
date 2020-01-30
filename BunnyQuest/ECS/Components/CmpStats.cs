using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.ECS.Components
{
    class CmpStats : Component
    {
        public CmpStats(Entity owner, int health, int damage, bool isFriendly) : base(owner)
        {
            this.health = health;
            this.damage = damage;
            this.health_cap = health;
        }

        public int health;
        public int health_cap;
        public int damage;
        public int damage_reduction;

        public bool isFriendly;
        public float iframes;

        public override void Update(float delta)
        {
            entity.expired = IsDead();

            if (iframes > 0)
            {
                iframes = iframes - delta;
            }
        }

        public int GetHealth()
        {
            return health;
        }

        public int GetDamage()
        {
            return damage;
        }


        public void IncreaseHealth(int health_gain)
        {
            if ((health + health_gain) < health_cap)
            {
                health += health_gain;
            }
            else
            {
                health = health_cap;
            }

        }

        public void TakeDamage(int damage_taken)
        {
            health = health + damage_reduction - damage_taken;
        }

        public bool IsDead()
        {
            return (health < 0);
        }


        public void IncreaseDamage(int dmg_gain)
        {
            damage = damage + dmg_gain;
        }

        public void DecreaseDamage(int dmg_loss)
        {
            damage = damage - dmg_loss;
        }


        public void IncreaseDamageReduction(int reduction_gain)
        {
            damage_reduction = damage_reduction + reduction_gain;
        }

        public void DecreaseDamageReduction(int reduction_loss)
        {
            damage_reduction = damage_reduction - reduction_loss;
        }
    }

    
}
