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
        public override void doEffect(GenericPerson Creature)
        {
            time--;
            int damage = bonus + amount;
            Creature.adjustHealth(-damage);
        }

        public override String description()
        {
            return "Attack ignited this creature!";
        }

        public override string ToString()
        {
            return "Lights target on fire.";
        }

        public override string info()
        {
            return amount + " fire damage.";
        }

        public override Effect copy()
        {
            EffectFire newEffect = new EffectFire();
            newEffect.bonus = this.bonus;
            return newEffect;
        }
    }
}
