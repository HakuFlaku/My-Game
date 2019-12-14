using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class EffectIce : Effect
    {

        private const int ICE_DAMAGE = 1;
        private const int ICE_TIME = 3;

        public EffectIce() : base(ICE_TIME, ICE_DAMAGE)
        {
            name = "EffectIce";
            buff = new BuffFrozen(3);
        }

        public override Effect copy()
        {
            EffectIce effect = new EffectIce();
            effect.bonus = this.bonus;
            return effect;
        }

        public override string description()
        {
            return "Target is frozen. They can't attack till they unfreeze.";
        }

        public override void doEffect(GenericPerson creature)
        {
            creature.adjustHealth(-amount);
            if (buff.isActive())
            {
                buff.doBuff(creature);
            }
            time--;
        }

        public override string info()
        {
            return amount + " ice damage and can't attack this turn.";
        }

        public override string ToString()
        {
            return "Freezes target.";
        }
    }
}
