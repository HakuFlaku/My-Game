/* This class is for the person playing the game, it will contain a list of all the spells the user knows with the according levels to those spells, how many slots the user has, that being
 * how many more times the user can you a spell of that level. With that, it tracks health, xp, and base damage with basic and magical attacks.
 * Note, the player can level up and will be procked for user input on what they want to level up, more detail on leveling up below.
 * */

using Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Player : GenericPerson
    {
        private long currxp;//tracks how much xp the player is currently at
        private long lvlxp;//tracks how much xp is needed to level up again
        private int money;//tracks how much money the player has
        private LinkedList preparedSpells;


        public Player(String name, int hp) : base(name, hp, 1)
        {
            hPIncMax = 4;
            currxp = 0;
            lvlxp = 50;
            preparedSpells = new LinkedList();
            initializeSpellSlots();
        }

        public Player(string name, JObject jObject) : base(name, (int)jObject["MaxHP"], (int)jObject["Level"])
        {
            //get the raw stats to the player
            currxp = (int)jObject["CurrXP"];
            currHP = (int)jObject["CurrHP"];
            lvlxp = (int)jObject["LvlXP"];
            meleeAP = (int)jObject["MeleeAP"];
            rangeAP = (int)jObject["RangeAP"];
            magicalAP = (int)jObject["MagicalAP"];
            basicDefense = (int)jObject["BasicDefense"];
            magicalDefense = (int)jObject["MagicalDefense"];
            money = (int)jObject["Money"];

            //load the spells in
            for(int i = 0; i < Constants.MAX_SPELL_LVL; i++)
            {
                JObject spellLevel = (JObject)jObject["SpellLevels"][i];
                spellSlots.newItem(new SpellList(spellLevel));
            }
        }

        //checks if a level up is available
        private bool checkLvlUp()
        {
            return currxp >= lvlxp;
        }

        //does everything for a level up, still need to make it allow for an increase in attack power for basic and magical attacks
        public override void lvlUp()
        {
            currxp -= lvlxp;
            if (level < Constants.CHANGE_LVLXP)
                lvlxp = (int)(lvlxp * Constants.LVLXP_MULTIPLIER);
            else
                lvlxp = Constants.LVLXP_INCREASE;
            level++;

            Constants.writeLine("\nYou've leveled up! You are now level&11 " + level + "&15\nXP for next level is: " + lvlxp);

            increaseHP();

            if (level % Constants.LEVEL_FOR_STAT == 0)
            {
                Constants.writeLine("You got a stat increase!");
                increaseStat();
            }

            if (level % Constants.LEVEL_FOR_SPELL == 0)
            {
                Constants.writeLine("Your knowledge grows and you may now have another spell slot!");
                newSpellSlot();
            }
            
            Constants.writeLine("You feel a surge of power return to your body, (You have been fully healed, and had your spell slots renewed)\nYour stats are now: " + displayStats() + "\n" + spellSlots.ToString());
        }

        private void increaseHP()
        {
            int healthIncrease = Constants.rand.Next(1, hPIncMax + 1);
            maxHP += healthIncrease;
            maxHeal();
            Constants.writeLine("Your max health also increases by&4 " + healthIncrease + "&15");
        }

        private void newSpellSlot()
        {
            Constants.writeLine("Select what level of spell slot you would like to acquire.");
            displaySpellSlots();
            //this is a loop that will keep iterating till the user provides a valid number for the spell slot that they want upgraded
            
            int currMaxLevel = ((level / Constants.LEVEL_FOR_SPELL) + 1);
            int slotLevel = Constants.getUserInput(1,currMaxLevel)-1;
            Object temp = spellSlots.getItem(slotLevel-1);
            SpellList increaseLevel = (SpellList)temp;
            increaseLevel.increase();
            increaseLevel.resetSlots();
        }

        private void increaseStat()
        {
            Constants.writeLine("Which stat would you like to choose to increase?\n1. Melee attack power\n2. Ranged attack power\n3. Magical attack power\n4. Basic defense");
            int choice = Constants.getUserInput(1, 4)-1;

            if (choice == (int)Constants.Type.MELEE)
                meleeAP += Constants.MELEE_AP_INC;
            else if (choice == (int)Constants.Type.RANGE)
                rangeAP += Constants.RANGE_AP_INC;
            else if (choice == (int)Constants.Type.MAGICAL)
                magicalAP += Constants.MAGICAL_AP_INC;
            else if(choice == (int)Constants.Type.DEFENSE)
                basicDefense += Constants.BASIC_D_INC;

        }

        private String displayStats()
        {
            return "\nMax health:&4 " + maxHP + "&15\nMelee attack power:&12 " + meleeAP + "&15\nRanged attack power:&12 " + rangeAP + "&15\nMagical attack power:&12 " + magicalAP + "&15\nDefense:&9 " + basicDefense + "&15";
        }

        //displays all the spells the user knows along with how many of each level of spell they have left
        private void displaySpellSlots()
        {
            for (int i = 0; i < ((level/Constants.LEVEL_FOR_SPELL)) && i < Constants.MAX_SPELL_LVL; i++)//Note we only show the spell levels that are equal and less than the players level, untill they have a level higher than the max spell level
            {
                Object itemI = spellSlots.getItem(i);
                if (itemI!=null && itemI.GetType() == typeof(SpellList))
                {
                    SpellList castItem = (SpellList)itemI;
                    Constants.writeLine((i+1) + "." + " Spell level " + (i + 1) + " with a current maximum slots of " + castItem.getMax());
                }
            }
        }

        //initializes all the LL that holds the LL's of spells, increase the first level spell slot by 1
        private void initializeSpellSlots()
        {
            for(int i = 0; i < Constants.MAX_SPELL_LVL; i++)
                spellSlots.newItem(new SpellList(i + 1));
        }

        //when combat is finished, check for level up and increase the players xp by what was given, and reset all the players spell slots
        public void finishCombat(int xp)
        {
            currxp += xp;
            while (checkLvlUp())
                lvlUp();
            for(int i = 0; i < Constants.MAX_SPELL_LVL; i++)
            {
                Object temp = spellSlots.getItem(i);
                if(temp.GetType() == typeof(SpellList))
                {
                    SpellList casted = (SpellList)temp;
                    casted.resetSlots();
                }
            }
        }

        //get a players turn
        public override Object doTurn()
        {
            Object ret = null;
            isDefending = false;

            while (ret == null && !isDefending)
            {
                Constants.writeLine("What would you like to do?\n1. Melee attack\n2. Ranged attack\n3. Magical attack\n4. Defend");
                int type = Constants.getUserInput(1, 4);

                if (type == 1)
                {//1 represents a choice of a basic attack
                    ret = getMeleeAttack();
                }
                else if (type == 2)
                    ret = getRangedAttack();
                else if (type == 3)
                {//otherwise the player entered 3, and that represents a magical attack
                    ret = getMagicalAttack();
                }
                else if (type == 4)
                    defend();
            }

            return ret;
        }

        protected override void defend()
        {
            Constants.writeLine("You begin to defend for an attack...");
            base.defend();
        }

        //returns a meleeAttack object of known melee attacks to the player
        protected override AbstractMelee getMeleeAttack()
        {
            AbstractMelee attack = null;

            Constants.writeLine("What type of melee attack would you like to do?\n0. Back\n" + meleeAttacks.enumerateToString());
            int choice = Constants.getUserInput(0, meleeAttacks.getLength());
            if(choice!=0)
                attack = (AbstractMelee)meleeAttacks.getItem(choice-1);

            return attack;
        }

        //returns a rangedAttack object of known ranged attacks to the player
        protected override AbstractRanged getRangedAttack()
        {
            AbstractRanged attack = null;

            Constants.writeLine("What type of ranged attack would you like to do?\n0. Back\n" + rangedAttacks.enumerateToString());
            int choice = Constants.getUserInput(0, rangedAttacks.getLength());
            if (choice != 0)
                attack = (AbstractRanged)rangedAttacks.getItem(choice-1);

            return attack;
        }

        //returns a spell from a list of prepared spells that the player has, WIP
        protected override Spell getMagicalAttack()
        {
            Constants.writeLine("Displaying known spells that can be cast.\n0. Back");
            int maxSpells = 0;
            Object temp = null;
            for(int i = 0; i<Constants.MAX_SPELL_LVL; i++)
            {
                temp = spellSlots.getItem(i);

                if(temp.GetType() == typeof(SpellList))
                {
                    SpellList currSpells = (SpellList)temp;
                    if(currSpells.canCast())
                    {
                        LinkedList spells = currSpells.spellList();
                        for(int j = 0; j<currSpells.getNumSpells(); j++)
                        {
                            temp = spells.getItem(j);
                            if(temp.GetType()==typeof(Spell))
                            {
                                Constants.writeLine((j+maxSpells+1) + ". " + temp.ToString());
                            }
                        }

                        maxSpells += currSpells.getNumSpells();
                    }
                }
            }

            int userInput = Constants.getUserInput(0, maxSpells);
            Spell spell = null;

            for(int i = 0; i < Constants.MAX_SPELL_LVL; i++)
            {
                temp = spellSlots.getItem(i);

                if(temp.GetType() == typeof(SpellList))
                {
                    SpellList spells = (SpellList)temp;

                    if (userInput <= spells.getNumSpells())
                    {//casting a spell from this list
                        spell = (Spell)spells.spellList().getItem(userInput-1);
                        spells.usedSpell();
                    }
                    else
                        userInput -= spells.getNumSpells();//not casting any spell from this list, so we "remove" them from the possibilities of being selected
                }
            }

            return spell;
        }

        private void getNewSpell(Spell spell)
        {
            Object temp = spellSlots.getItem(spell.getRank()-1);
            if(temp.GetType()==typeof(SpellList))
            {
                SpellList spellList = (SpellList)temp;
                spellList.newSpell(spell);
            }
        }

        public void levelUP()
        {
            lvlUp();
        }

        //the player can only be attacked with basic attacks, and so, just subtract the damage that would be dealt and deal the remaining to the player if it's positive
        //need to allow for magic damage to be dealt to the player
        public override void attacked(Attack attack)
        {
            int damage = getDamage(attack);

            if (Constants.rand.NextDouble() > Constants.HIT_CHANCE)
            {
                if (damage >= 0)
                {
                    hurt(damage);
                    Constants.writeLine("You were hit for&4 " + damage + "&15!");
                }
                else
                {
                    int amn = heal(-damage);
                    Constants.writeLine("You were healed for&4 " + amn + "&15!");
                }
                if (attack.hasEffect())
                    applyEffect(attack.effect);
            }
            else
                Constants.writeLine("You were missed!");
        }

        public override void doEffects()
        {
            Effect eff = null;
            int i = 0;

            do
            {
                eff = (Effect) effects.getItem(i);
                if (eff != null)
                {
                    if (eff.isActive())
                    {
                        eff.doEffect(this);
                    }
                    else
                    {
                        effects.removeItem(i);
                        i--;//gotta reduce the count since we're removing an item from the list
                    }
                }
                i++;
            } while (eff != null);
        }

        public int getCurrHP()
        {
            return currHP;
        }

        //healing is a funcitonal relation where it's the difference from max to curr HP multiplied by the current level to make healing cost more as you level up
        public int healCost()
        {
            return (maxHP - currHP) * level;
        }

        public override string ToString()
        {
            return "Current level:&11 " + level + 
                "\n&15Max hp:&4 " + maxHP + "&15 current hp:&4 " + currHP + "&15 defense:&9 " + basicDefense + 
                "\n&15Melee attack power:&12 " + meleeAP + "&15 ranged attack power:&12 " + rangeAP + "&15 magical attack power:&12 " + magicalAP +
                "\n&15Gold:&6 " + money +
                "\n&15Xp needed to level up: " + lvlxp + " current xp: " + currxp + "\n" + spellSlots.ToString();
        }

        //return how much gold the player currently has
        public int goldAmount()
        {
            return money;
        }

        //player earned gold
        public void earnGold(int amount)
        {
            money += amount;
        }

        //player is trying to buy a spell, return a boolean for whether it was succesful or not
        public bool buySpell(int cost, Spell abil)
        {
            bool bought = false;
            if (money < cost)
                Constants.writeLine("You don't have enough money for this item.");
            else
            {
                money -= cost;
                getNewSpell(abil);
                bought = true;
            }
            return bought;
        }

        public void maxHeal()
        {
            currHP = maxHP;
        }

        public void save(JsonWriter writer)
        {
            writer.WritePropertyName("MaxHP"); writer.WriteValue(maxHP);
            writer.WritePropertyName("CurrHP"); writer.WriteValue(currHP);
            writer.WritePropertyName("Level"); writer.WriteValue(level);
            writer.WritePropertyName("CurrXP"); writer.WriteValue(currxp);
            writer.WritePropertyName("LvlXP"); writer.WriteValue(lvlxp);
            writer.WritePropertyName("MeleeAP"); writer.WriteValue(meleeAP);
            writer.WritePropertyName("RangeAP"); writer.WriteValue(rangeAP);
            writer.WritePropertyName("MagicalAP"); writer.WriteValue(magicalAP);
            writer.WritePropertyName("BasicDefense"); writer.WriteValue(basicDefense);
            writer.WritePropertyName("MagicalDefense"); writer.WriteValue(magicalDefense);
            writer.WritePropertyName("Money"); writer.WriteValue(money);
            writer.WritePropertyName("SpellLevels");
            writer.WriteStartArray();
            for (int i = 1; i <= Constants.MAX_SPELL_LVL; i++)
            {
                SpellList temp = (SpellList)spellSlots.getItem(i - 1);
                try
                {
                    temp.save(writer);
                }
                catch
                {

                }
            }
            writer.WriteEndArray();
        }
    }
}
