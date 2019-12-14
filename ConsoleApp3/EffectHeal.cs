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

        public override string description()
        {
            return "Target is surrounded by a misty green aura. They will heal each turn while this is active.";
        }

        public override void doEffect(GenericPerson creature)
        {
            creature.adjustHealth(amount);
            time--;

        }

        public override string info()
        {
            return "healed for&4 " + amount + "&15 HP.";
        }

        public override string ToString()
        {
            return "Heals target";
        }
    }
}
