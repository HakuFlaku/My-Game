using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class BuffFrozen : Buff
    {
        public BuffFrozen(int time) : base(time)
        {
            name = "BuffFrozen";
        }

        //"Freeze" the target, making them unable to move/ attack
        public override void doBuff(GenericPerson person)
        {
            person.attacked();//by doing this, it prevents this person from doing another attack
            time--;
        }

        public override void applied() {
            
        }
    }
}
