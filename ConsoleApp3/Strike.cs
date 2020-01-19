//a strike class will describe what an attack is, that being it's type of attack, those being: Melee, Ranged, Spell.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Strike
    {
        Spell spell;
        AbstractBasicAttack melee, ranged;
        GenericPerson thing;

        //create a strike class for the player
        public Strike(Player player)
        {
            spell = null;
            melee = null;
            ranged = null;
            thing = player;
            getAttack();
        }

        //create a strike class for a creature
        public Strike(GenericCreature creature)
        {
            thing = creature;
            getAttack();
        }

        private void getAttack()
        {
            Object attack = thing.doTurn();
            if (attack != null)
            {
                if (attack.GetType().IsSubclassOf(typeof(AbstractMelee)))
                    melee = (AbstractMelee)attack;
                else if (attack.GetType().IsSubclassOf(typeof(AbstractRanged)))
                    ranged = (AbstractRanged)attack;
                else if (attack.GetType() == (typeof(Spell)))
                    spell = (Spell)attack;
            }
        }

        public bool isStriking()
        {
            return spell != null || melee != null || ranged != null;
        }

        //returns the damage of the ability, note this does not include the effect's damage
        public int getDamage()
        {
            int damage = 0;
            if(spell!=null) 
            {
                damage = spell.getDamage();
                if(damage >= 0)
                    damage += thing.getMagicalAP();
                else
                    damage -= thing.getMagicalAP();
            }
            else if(melee!=null)
                damage = melee.getDamage() + thing.getMeleeAP();
            else if(ranged!=null)
                damage = ranged.getDamage() + thing.getRangedAP();
            return damage;
        }

        public Effect getEffect()
        {
            Effect effect = null;

            if (spell!=null && spell.hasEffect())
                effect = spell.getEffect();
            else if (melee!=null && melee.hasEffect())
                effect = melee.getEffect();
            else if (ranged!=null && ranged.hasEffect())
                effect = ranged.getEffect();

            return effect;
        }

        public Buff getBuff()
        {
            Buff buff = null;

            if (spell!=null && spell.hasBuff())
                buff = spell.getBuff();
            else if (melee!=null && melee.hasBuff())
                buff = melee.getBuff();
            else if (ranged!=null && ranged.hasBuff())
                buff = ranged.getBuff();

            return buff;
        }

        public bool isBasic()
        {
            return spell == null;
        }

        public bool isRanged()
        {
            return ranged != null;
        }
        
        public bool isMelee()
        {
            return melee != null;
        }

        public bool isMagical()
        {
            return spell != null;
        }
        
    }
}
