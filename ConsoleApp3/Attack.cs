//This describes what an attack is, in a more raw like state, that being, how much damage is done, and contains the effect and buff if there is one

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Attack
    {
        public readonly bool isBasic;
        public readonly int damage;//how much damage is being done, note this can be negative to show that it heals.
        public readonly Effect effect;//the effect that is being applied
        //public readonly Buff buff;//the buff that is being applied WIP

        public Attack(Strike strike)
        {
            isBasic = strike.isBasic();
            damage = strike.getDamage();
            effect = strike.getEffect();
            //buff = strike.getBuff();
        }

        public bool hasEffect()
        {
            return effect != null;
        }
    }
}
