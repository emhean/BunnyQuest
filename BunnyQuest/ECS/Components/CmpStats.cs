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
            health = health - damage_taken;
        }

        public bool is_dead()
        {
            return (health < 0);
        }
    }

    
}
