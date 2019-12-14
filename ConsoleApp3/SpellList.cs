//Holds a LL of spells, keeps track of how many spells there are of it's level, should only contain spells of it's rank

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
    class SpellList
    {
        private int maxSlots, currSlots;
        private readonly int spellLvl;
        private LinkedList spells;

        //constructor for a brand new spellList
        public SpellList(int lvl)
        {
            spellLvl = lvl;//signifys what level of spells are stored in this LL
            maxSlots = currSlots = 0;
            spells = new LinkedList();
        }

        //constructor for when re-loading a spellList
        public SpellList(JObject spellList)
        {
            maxSlots = (int)spellList["maxSlots"];
            currSlots = (int)spellList["currSlots"];
            spellLvl = (int)spellList["spellLevel"];
            spells = new LinkedList();
            JArray spellArr = (JArray)spellList["Spell"];
            for(int i = 0; i < spellArr.Count; i++)
            {
                spells.newItem(new Spell((JObject)spellArr[i]));
            }
        }

        public int getLevel()
        {
            return spellLvl;
        }

        //player used a spell of this level, reduce the amount of times the player can use this type of spell
        public void usedSpell()
        {
            if(currSlots>0)
                currSlots--;
        }

        public bool canCast()
        {
            return currSlots>0;
        }

        public void resetSlots()
        {
            currSlots = maxSlots;
        }

        public void newSpell(Spell spell)
        {
            spells.newItem(spell);
        }

        public void increase()
        {
            maxSlots++;
        }

        public int getMax()
        {
            return maxSlots;
        }

        public override String ToString()
        {
            String ret = "Spell slot for spells of level " + spellLvl + " with a maximum number of casts equal to " + maxSlots +
                "\nCurrently known spells of this level are the following:\n" + spells.ToString();
            if (maxSlots == 0)
                ret = null;
            return ret;
        }

        public LinkedList spellList()
        {
            return spells;
        }

        public int getNumSpells()
        {
            return spells.getLength();
        }

        public void save(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("maxSlots"); writer.WriteValue(maxSlots);
            writer.WritePropertyName("currSlots"); writer.WriteValue(currSlots);
            writer.WritePropertyName("spellLevel"); writer.WriteValue(spellLvl);

            writer.WritePropertyName("Spell");
            writer.WriteStartArray();
            for(int i = 0; i<spells.getLength(); i++)
            {
                Spell temp = (Spell)spells.getItem(i);
                temp.save(writer);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
