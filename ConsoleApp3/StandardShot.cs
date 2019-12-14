//standard ranged attack, does nothing special

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class StandardShot : AbstractRanged
    {

        public StandardShot() : base(1)
        {
        }

        public override string ToString()
        {
            return "Standard ranged attack";
        }
    }
}
