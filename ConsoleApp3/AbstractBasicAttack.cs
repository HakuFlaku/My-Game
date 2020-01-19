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
        protected Buff buff;

        public AbstractBasicAttack(int damage)
        {
            this.damage = damage;
            this.effect = null;
            buff = null;
        }

        public AbstractBasicAttack(int damage, Effect effect)
        {
            this.damage = damage;
            this.effect = effect;
            buff = null;
        }

        public AbstractBasicAttack(int damage, Buff buff)
        {
            this.damage = damage;
            this.effect = null;
            this.buff = buff;
        }

        public AbstractBasicAttack(int damage, Effect effect, Buff buff)
        {
            this.damage = damage;
            this.effect = effect;
            this.buff = buff;
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

        public Buff getBuff()
        {
            return buff;
        }

        public bool hasBuff()
        {
            return buff != null;
        }

        public override abstract String ToString();
    }
}
