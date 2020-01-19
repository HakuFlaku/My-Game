using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class EffectFire : Effect
    {

        const int FIRE_DAMAGE = 2;
        const int FIRE_TIME = 3;

        public EffectFire() : base(FIRE_TIME, FIRE_DAMAGE)
        {
            name = "EffectFire";
        }

        //decrease time as effect has happened, and adjust the creatures health
        public override void doEffect(GenericPerson creature)
        {
            time--;
            int damage = bonus + amount;
            int burnt = creature.hurt(damage);
            printInfo(creature, burnt);
        }

        public override void applied()
        {
            Constants.writeLine("Attack ignited this creature!");
        }

        public override string ToString()
        {
            return "Lights target on fire.";
        }

        protected override void printInfoCreature(GenericPerson creature, int num) {
            Constants.writeLine(creature.getName() + " has been burnt for&4 " + num + " &15HP.");
        }

        protected override void printInfoPlayer(GenericPerson player, int num) {
            Constants.writeLine("You have been burnt for&4 " + num + " &15HP.");
        }

        public override Effect copy()
        {
            EffectFire newEffect = new EffectFire();
            newEffect.bonus = this.bonus;
            return newEffect;
        }
    }
}
