using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class EffectHeal : Effect
    {
        private const int TIME = 3, AMOUNT = 3;

        public EffectHeal() : base(TIME, AMOUNT)
        {
            name = "EffectHeal";
        }
        public override Effect copy()
        {
            EffectHeal effect = new EffectHeal();
            effect.bonus = this.bonus;
            return effect;
        }

        public override void applied()
        {
            Constants.writeLine("Target is surrounded by a misty green aura. They will heal each turn while this is active.");
        }

        public override void doEffect(GenericPerson creature)
        {
            int healedAmount = creature.heal(amount);
            time--;

            printInfo(creature, healedAmount);
        }

        protected override void printInfoCreature(GenericPerson creature, int num) {
            Constants.writeLine(creature.getName() + " has been healed for&7 " + num + "&14 HP.");
        }

        protected override void printInfoPlayer(GenericPerson player, int num) {
            Constants.writeLine("You have been healed for&7 " + num + "&14 HP.");
        }

        public override string ToString()
        {
            return "Heals target";
        }
    }
}
