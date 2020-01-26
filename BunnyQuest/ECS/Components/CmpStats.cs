using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.ECS.Components
{
    class CmpStats : Component
    {
        public CmpStats(Entity owner) : base(owner)
        {

        }

        public int health;
        public int health_cap;
        public int damage;
        public int damage_reduction;
        
        public int get_health()
        {
            return health;
        }

        public int get_damage()
        {
            return damage;
        }


        public void increase_health(int health_gain)
        {
            health = health + health_gain;
        }

        public void take_damage(int damage_taken)
        {
            health = health + damage_reduction - damage_taken;
        }

        public bool is_dead()
        {
            return (health < 0);
        }


        public void increase_damage(int dmg_gain)
        {
            damage = damage + dmg_gain;
        }

        public void decrease_damage(int dmg_loss)
        {
            damage = damage - dmg_loss;
        }


        public void increase_dmg_red(int red_gain)
        {
            damage_reduction = damage_reduction + red_gain;
        }

        public void decrease_dmg_red(int red_loss)
        {
            damage_reduction = damage_reduction - red_loss;
        }
    }

    
}
