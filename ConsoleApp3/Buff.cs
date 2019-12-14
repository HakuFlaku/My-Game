using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public abstract class Buff
    {
        protected int time;//how many turns this buff lasts for.
        protected String name;//use for identification purpose

        public Buff(int time)
        {
            this.time = time;
        }

        public bool isActive()
        {
            return time > 0;
        }

        public abstract void doBuff(GenericPerson person);
    }
}
