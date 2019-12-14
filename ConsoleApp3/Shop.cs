using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Shop
    {
        Player player;
        int modifier;
        LinkedList spellsForSale;

        public Shop(Player player, int modifier)
        {
            this.player = player;
            this.modifier = modifier;
            spellsForSale = loadSpells();
        }

        //returns a linkedlist of spells that the player can choose to buy
        private LinkedList loadSpells()
        {
            LinkedList ret = new LinkedList();

            for (int i = 0; i < Constants.NUM_SPELLS; i++)
            {
                int rank = (modifier-1) / 5;
                rank += Constants.rand.Next(0 - Constants.SPELL_RANK_VARIANCE, Constants.SPELL_RANK_VARIANCE);

                if (rank < 1)
                    rank = 1;
                if (rank > Constants.MAX_SPELL_LVL)
                    rank = Constants.MAX_SPELL_LVL;

                ret.newItem(new Spell(rank));
            }

            return ret;
        }

        public void buySpell(int num)
        {
            Spell spell = (Spell)spellsForSale.getItem(num);
            Constants.writeLine("This spell would cost " + spell.getCost() + " gold. Would you like to purchase it?\n0. No\n1. Yes");
            int choice = Constants.getUserInput(0, 1);
            if (choice == 1)
            {
                if (player.buySpell(spell.getCost(), spell))
                    spellsForSale.removeItem(num);
            }
        }

        public String getSpells()
        {
            return spellsForSale.enumerateToString();
        }
    }
}
