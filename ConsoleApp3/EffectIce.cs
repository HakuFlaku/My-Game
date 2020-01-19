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

        public override void applied()
        {
            Constants.writeLine("Target is frozen. They can't attack till they unfreeze.");
        }

        public override void doEffect(GenericPerson creature)
        {
            int num = creature.hurt(amount + bonus);
            if (buff.isActive())
            {
                buff.doBuff(creature);
            }
            time--;
            printInfo(creature, num);
        }

        //change these yet
        protected override void printInfoCreature(GenericPerson creature, int num) {
            Constants.writeLine(creature.getName() + " has been froze for&7 " + num + "&14 HP");
        }

        protected override void printInfoPlayer(GenericPerson player, int num) {
            Constants.writeLine("You have been froze for&7 " + num + "&14 HP.");
        }

        public override string ToString()
        {
            return "Freezes target.";
        }
    }
}
