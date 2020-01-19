using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public abstract class GenericCreature : GenericPerson
    {
        protected bool[] resistances;//Note each entry will line up with the enumeration of the different effect types

        public GenericCreature(String name, int hp, int level) : base(name, hp, level)
        {
            //resistances = new bool[Constants.NUM_EFFECTS];
            for (int i = 1; i < level; i++)//level up this creature to the inputed level
            {
                lvlUp();
                //this.level = level;
            }
        }

        public override void attacked(Attack attack)
        {
            int damage = getDamage(attack);

            if (Constants.rand.NextDouble() > Constants.HIT_CHANCE)
            {
                Constants.writeLine("You hit for&4 " + damage + "&15 damage!");
                currHP -= damage;
                if(attack.hasEffect())
                    applyEffect(attack.effect);
            }
            else
                Constants.writeLine("You missed your attack.");
        }

        //this is a generic setting for the creature for a turn to be made, it randomly selects an action to perform
        public override Object doTurn()
        {
            isDefending = false;//reset the defending effect, they must choose to defend this turn to be defending, it does not carry over from the last turn
            int decision;
            Object attack = null;
            if (spellSlots.isEmpty())
                decision = Constants.rand.Next(4);
            else
                decision = Constants.rand.Next(5);

            if (decision == 1)//defending
                defend();
            else if (decision == 2)//melee attack
                attack = getMeleeAttack();
            else if (decision == 3)//ranged attack
                attack = getRangedAttack();
            else if (decision == 4)
                attack = getMagicalAttack();

            return attack;
        }

        protected override Spell getMagicalAttack()
        {
            int choice = Constants.rand.Next(spellSlots.getLength() + 1);
            return (Spell)spellSlots.getItem(choice - 1);
        }

        protected override AbstractMelee getMeleeAttack()
        {
            int choice = Constants.rand.Next(meleeAttacks.getLength() + 1);
            return (AbstractMelee)meleeAttacks.getItem(choice - 1);
        }

        protected override AbstractRanged getRangedAttack()
        {
            int choice = Constants.rand.Next(rangedAttacks.getLength() + 1);
            return (AbstractRanged)rangedAttacks.getItem(choice - 1);
        }

        //take damage of each type of effect applied to the creature, as well as add/apply buffs
        public override void doEffects()
        {
            int i = 0;
            Object curr = effects.getItem(i);

            while(curr!=null)
            {
                if(curr.GetType().IsSubclassOf(typeof(Effect)))
                {
                    Effect effect = (Effect)curr;
                    if (effect.isActive())
                    {
                        effect.doEffect(this);
                        //if (resistances[effect.getType()])
                        //damage /= 2;
                    }
                    else
                        effects.removeItem(i--);//must reduce the count as well since we've removed an item.
                }
                i++;
                curr = effects.getItem(i);
            }
        }

        public override string ToString()
        {
            return name + " LVL&11 " + level + "&15 HP&2 " + currHP + "&15 ";
        }

        protected abstract void setResistances();
        public abstract int xpValue();//returns how much xp the creature was worth for killing
        public abstract int goldValue();
    }
}
