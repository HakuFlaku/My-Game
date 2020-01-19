/* Abstract Class that describes what a person should look like
 * A person is very basic, they have an amount of health, a check for if they're alive or dead
 * */

using System;

namespace ConsoleApp3
{
    public abstract class GenericPerson
    {
        protected int currHP, maxHP, hPIncMax, meleeAP, rangeAP, magicalAP, magicalDefense, basicDefense, level;
        protected bool isDefending, canAttack;
        protected String name;
        protected LinkedList effects, buffs;
        protected LinkedList spellSlots;//this is a list that contains lists of the different levels of spells and how many slots are left and the max of that
        protected LinkedList meleeAttacks;//can have different types of melee attacks to choose from, same with ranged attacks
        protected LinkedList rangedAttacks;

        protected GenericPerson(String name, int hp, int level)
        {
            isDefending = false;
            this.level = level;
            meleeAP = Constants.STARTING_MELEE_AP;
            rangeAP = Constants.STARTING_RANGED_AP;
            magicalAP = Constants.STARTING_MAGIC_AP;
            basicDefense = Constants.STARTING_BASIC_DEFENSE;
            magicalDefense = Constants.STARTING_MAGIC_DEFENSE;
            this.name = name;
            currHP = maxHP = hp;
            effects = new LinkedList();
            buffs = new LinkedList();
            spellSlots = new LinkedList();
            meleeAttacks = new LinkedList();
            rangedAttacks = new LinkedList();
            getStartingAttacks();
            canAttack = true;
        }

        protected virtual void defend()
        {
            isDefending = true;
        }

        protected int getDamage(Attack attack)
        {
            int damage = attack.damage;
            int defense;

            if (attack.isBasic)
                defense = basicDefense;
            else
                defense = magicalDefense;
            if (isDefending)
            {
                defense = defense + (int)(Constants.DEFENDING_MULTIPLIER * defense);
                if (defense == 0)
                    defense = 1;
            }

            if (damage > 0)
            {
                damage -= defense;

                if (damage < 0)
                    damage = 0;
            }

            return damage;
        }

        public void attacked()
        {
            canAttack = false;
        }

        public bool checkAttack()
        {
            return canAttack;
        }

        public void resetAttack()
        {
            canAttack = true;
        }

        //deal damage to this creature
        public int hurt(int num)
        {
            if(num < 0)//try'd to hurt with a negative number, this is an error
            {
                Constants.writeLine("&4ERROR&15:: tried to hurt with a negative number.");
            }
            else 
            {
                currHP -= num;
            }
            return num;
        }

        //checks if this creature is still alive, simply a creature is alive if it has a positive integer for it's health, and dead otherwise
        public bool isAlive()
        {
            return currHP > 0;
        }

        //heal the creature by the provided number
        public int heal(int num)
        {
            int amtHeald = num;
            if(num < 0)//try'd to heal with a negative number, this is an error
            {
                Constants.writeLine("&4ERROR&15:: tried to heal with a negative number.");
            }
            else if(num > missingHealth())//only heal to max health, don't heal over max
            {
                amtHeald = missingHealth();
                if(amtHeald < 0)
                    amtHeald = 0;
                currHP += missingHealth();
            }
            else 
            {
                currHP += num;
            }
            return amtHeald;
        }

        protected int missingHealth() 
        {
            return maxHP - currHP;
        }

        internal int getMagicalAP()
        {
            return magicalAP;
        }

        internal int getMeleeAP()
        {
            return meleeAP;
        }

        internal int getRangedAP()
        {
            return rangeAP;
        }

        internal String getName()
        {
            return name;
        }

        //gives the person the starting types of basic attack, note it creates the attacks, it adds the bonus of the creatures AP when attacking, not here.
        protected void getStartingAttacks()
        {
            meleeAttacks.newItem(new StandardHit());
            rangedAttacks.newItem(new StandardShot());
        }

        //try's to add the given effect to the list of effects inflicted on this creature
        protected void applyEffect(Effect effect)
        {
            if (Constants.rand.NextDouble() > Constants.STATUS_EFFECT_CHANCE)
            {
                effect.applied();
                effects.newItem(effect);
            }
        }

        protected void applyBuff(Buff buff) {
            if (Constants.rand.NextDouble() > Constants.BUFF_APPLY_CHANCE)
            {
                buff.applied();
                buffs.newItem(buff);
            }
        }

        protected abstract AbstractMelee getMeleeAttack();

        protected abstract AbstractRanged getRangedAttack();

        protected abstract Spell getMagicalAttack();

        //every person much have a way of being attacked
        public abstract void attacked(Attack attack);
        //every creature must have a defined way of leveling up
        public abstract void lvlUp();
        //doTurn gets what the thing wants to do for it's turn, that being attack or defend, and with that, it's the specific attack
        public abstract Object doTurn();
        //needs to be able to have effects apply to themself
        public abstract void doEffects();

    }
}

