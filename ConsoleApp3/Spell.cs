using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp3
{
    public class Spell
    {
        private readonly int rank;//what level this spell is regarded as
        private int damage;//represents the base damage this spell does without any modifiers.
        private readonly Effect effect;
        private readonly Buff buff;
        private readonly String description;

        //standard constructor for a spell
        public Spell(int r)
        {
            rank = r;
            damage = setDamage();
            effect = setEffect();
            buff = setBuff();
            description = setDescription();
        }

        //constructor for loading an already made spell
        public Spell(JObject info)
        {
            rank = (int)info["Rank"];
            damage = (int)info["Damage"];
            description = (String)info["Description"];
            String effectName = (String)info["EffectName"];

            if (!effectName.Equals(Constants.DNE.ToString()))
            {
                for(int i = 0; i < Constants.allEffects.getLength(); i++)
                {
                    Effect currEffect = (Effect)Constants.allEffects.getItem(i);
                    if(effectName.Equals(currEffect.getName()))
                    {
                        effect = currEffect.copy();
                    }
                }
            }
        }

        //randomly select an effect to be added to this spell from a master list of all effects
        private Effect setEffect()
        {
            Effect e = null;
            Effect eCopy = null;

            if(Constants.rand.NextDouble() < Constants.GETS_EFFECT + (rank / 10.0))
            {//random chance to have an effect added to the spell or not
                int numEffect = Constants.rand.Next(-1, Constants.allEffects.getLength());
                e = (Effect)Constants.allEffects.getItem(numEffect);
                eCopy = e.copy();
                if (e.getName().Equals("EffectHeal"))
                {
                    damage = -damage;
                }
            }

            return eCopy;
        }

        private Buff setBuff() 
        {
            Buff ret = null;

            

            return ret;
        }

        //sets the damage for this spell based on the rank with a variance
        private int setDamage()
        {
            int damage = rank + (rank*Constants.DAMAGE_VARIANCE);//first set this to the average damage a spell would do at it's rank
            int variance = Constants.rand.Next(0-Constants.DAMAGE_VARIANCE-rank+1, Constants.DAMAGE_VARIANCE+rank-1);//get how much the spell is to vary in damage
            return damage + variance;
        }

        //creates a description of this spell, gives basic information of how much damage it does and what the effect and/or buff does
        private String setDescription()
        {
            String desc = "";

            if (hasEffect())
                desc = effect.ToString();

            return desc;
        }

        public void updateEffect(int change)
        {
            effect.update(change);
        }

        public bool hasEffect()
        {
            return effect != null;
        }

        public bool hasBuff()
        {
            return buff != null;
        }

        public int getRank()
        {
            return rank;
        }

        public String getDescription()
        {
            return description;
        }

        //return the base damage this ability does
        public int getDamage()
        {
            return damage;
        }

        public Effect getEffect()
        {
            return effect;
        }

        public Buff getBuff() {
            return buff;
        }

        //every spell must have a ToString
        public override String ToString()
        {
            String ret;
            if (damage >= 0)
                ret = "Spell of level: " + rank + ". This spell does " + damage + " damage.";
            else
                ret = "Spell of level: " + rank +" . This spell heals for " + -damage + " HP.";

            if (description != null)
                ret += " " + description;
            return ret;
        }

        //calculate what this spell would cost for the player to purchase it
        public int getCost()
        {
            int totalCost = (Math.Abs(damage) * 4) / rank;//functional relation based on just the rank and the damage, the higher the damage with a low rank makes it cost more

            if (hasEffect())
                totalCost = (int)(totalCost * 2.5);
            if (hasBuff())
                totalCost = (int)(totalCost * 2.5);

            return totalCost;
        }

        public void addEffectToList(LinkedList ll)
        {
            ll.newItem(effect);
        }

        public String getInfo()
        {
            String info = rank + " " + damage + " ";
            if (hasEffect())
                info += effect.getName();
            else
                info += "NoEffect";
            return info;
        }

        public void save(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Rank"); writer.WriteValue(rank);
            writer.WritePropertyName("Damage"); writer.WriteValue(damage);
            writer.WritePropertyName("EffectName");
            if (hasEffect())
                writer.WriteValue(effect.getName());
            else
                writer.WriteValue(Constants.DNE);
            writer.WritePropertyName("Description"); writer.WriteValue(description);
            writer.WriteEndObject();
        }
    }
}
