using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public abstract class AbstractMelee : AbstractBasicAttack
    {
        public AbstractMelee(int damage) : base(damage)
        {

        }
    }
}
