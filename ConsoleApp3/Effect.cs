//An effect is a single use thing, when created it will contain all relevent information for when it started, and will then be destroyed and created again later

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public abstract class Effect
    {
        protected int time;//represents how many more turns this effect lasts for
        protected int amount;//represents how much this effect does
        protected int bonus;//this is an additional modifier based on the rank of the spell and skill of the player.
        protected String name;//the name of this effect for comparison purposes
        protected Buff buff;

        protected Effect(int t, int a)
        {
            time = t;
            amount = a;
            bonus = 0;
            name = "SampleName";
            buff = null;
        }

        public bool hasBuff()
        {
            return buff != null;
        }

        public Buff getBuff()
        {
            return buff;
        }

        public bool isActive()
        {
            return time > 0;
        }

        public bool equals(String other)
        {
            return name.Equals(other);
        }

        public String getName()
        {
            return name;
        }

        public void update(int val)
        {
            setBonus(val);
        }

        private void setBonus(int b)
        {
            bonus = b;
        }

        protected void printInfo(GenericPerson creature, int num) {
            if(creature.GetType().IsSubclassOf(typeof(GenericCreature)))
            {//this is a creature
               printInfoCreature(creature, num);
            }
            else 
            {//otherwise, it's the player
               printInfoPlayer(creature, num);
            }
        }

        public abstract Effect copy();
        public abstract void doEffect(GenericPerson creature);
        public abstract void applied();
        public abstract override string ToString();
        protected abstract void printInfoCreature(GenericPerson creature, int num);
        protected abstract void printInfoPlayer(GenericPerson player, int num);
    }
}
