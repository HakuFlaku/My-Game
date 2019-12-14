/* This class will be for testing out creatures, how to do stats, and anything else needed
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class TestCreature : GenericCreature
    {
        private const int STARTING_HP = 1;
        public TestCreature(int level) : base("Goblin" /*+ (char) Constants.rand.Next('a','z')*/, STARTING_HP, level)
        {
            hPIncMax = 3;
            meleeAP = 1;
            rangeAP = 1;
        }

        public override void lvlUp()
        {
            level++;
            maxHP += Constants.rand.Next(1, hPIncMax+1);
            heal(maxHP - currHP);
            int levelType = Constants.rand.Next(1, 5);
            if (levelType == (int)Constants.Type.MELEE)
                meleeAP += Constants.MELEE_AP_INC;
            else if (levelType == (int)Constants.Type.RANGE)
                rangeAP += Constants.RANGE_AP_INC;
            else if (levelType == Constants.MAGIC_D)
                magicalDefense += Constants.MAGIC_D_INC;
            else if (levelType == Constants.BASIC_D)
                basicDefense += Constants.BASIC_D_INC;
        }

        protected override void setResistances()
        {
            throw new NotImplementedException();
        }

        public override int xpValue()
        {
            return (int)(25 * (level / 1.5));
        }

        public override int goldValue()
        {
            return (int)(10 * (level / 1.8));
        }
    }
}
