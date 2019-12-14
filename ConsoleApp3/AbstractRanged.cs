using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public abstract class AbstractRanged : AbstractBasicAttack
    {
        public AbstractRanged(int damage) : base(damage)
        {

        }
    }
}
