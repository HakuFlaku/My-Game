//A basic attack should define how much damage it does and any effects/ buffs that pertain to that attack in the class itself.
//Nothing should be getting passed in normally.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public abstract class AbstractBasicAttack
    {
        protected int damage;
        protected Effect effect;

        public AbstractBasicAttack(int damage)
        {
            this.damage = damage;
            this.effect = null;
        }

        public AbstractBasicAttack(int damage, Effect effect)
        {
            this.damage = damage;
            this.effect = effect;
        }

        public int getDamage()
        {
            return damage;
        }

        public Effect getEffect()
        {
            return effect;
        }

        public bool hasEffect()
        {
            return effect != null;
        }

        public override abstract String ToString();
    }
}
