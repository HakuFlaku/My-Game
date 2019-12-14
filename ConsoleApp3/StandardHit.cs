//standard melee attack, does nothing special

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class StandardHit : AbstractMelee
    {

        public StandardHit() : base(1)
        {
        }

        public override string ToString()
        {
            return "Basic melee attack";
        }
    }
}
