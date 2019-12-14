using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    static class Constants
    {
        public const int MAX_SPELL_LVL = 3;//how many level's of spells there are
        public const int DNE = -1;
        public static Random rand = new Random();
        public static LinkedList allEffects = new LinkedList();//stores an object instance of the effect type

        //Control's for the shop
        public const int NUM_SPELLS = 4;
        public const int SPELL_RANK_INC = 10;
        public const int SPELL_RANK_VARIANCE = 1;

        //Control spell creation
        public const int DAMAGE_VARIANCE = 2;
        public const double GETS_EFFECT = 0.2;

        //Things for combat control
        public const double HIT_CHANCE = 0.15;//gives a value less than 1, to indicate the chance that an attack misses
        public const double STATUS_EFFECT_CHANCE = 0.25;//gives a value of the chance a status effect is NOT applied
        public const int BASE_CREATURES = 1;
        public const int UPDATE_CHALLENGE = 2;
        public const double DEFENDING_MULTIPLIER = 1.5;
        public const int MAX_CREATURES = 4;

        //Things for floor control
        public const int GO_TO_TOP = 3;
        public const int STARTING_FLOOR = 1;

        //Things for the player
        public const int LEVEL_FOR_SPELL = 3;
        public const int LEVEL_FOR_STAT = 2;
        public const int STARTING_HEALTH = 8;
        public const double LVLXP_MULTIPLIER = 1.4;
        public const int CHANGE_LVLXP = 15;//says when to switch the style of xp increase to prevent an extreme exponential increase
        public const int LVLXP_INCREASE = 5000;//this says how much xp to be added when the style has been switched to just be a flat increase

        //identifying numbers for checking user input
        public enum Type {MELEE, RANGE, MAGICAL, DEFENSE}
        public const int MAGIC_D = 1;
        public const int BASIC_D = 2;

        //constants for status effects
        public const int BLEEDING_DAMAGE = 1;
        public const int BLEEDING_TIME = 5;
        public const int POISON_DAMAGE = 3;
        public const int POISON_TIME = 2;
        public const int HEAL_AMOUNT = 5;
        public const int BOOST_DEFENSE_AMOUNT = 3;

        //stat basic starter numbers
        public const int STARTING_MELEE_AP = 0;
        public const int STARTING_RANGED_AP = 0;
        public const int STARTING_MAGIC_AP = 0;
        public const int STARTING_BASIC_DEFENSE = 0;
        public const int STARTING_MAGIC_DEFENSE = 0;

        //base increase to a stat number
        public const int BASIC_D_INC = 1;
        public const int MAGIC_D_INC = 1;
        public const int MELEE_AP_INC = 1;
        public const int RANGE_AP_INC = 1;
        public const int MAGICAL_AP_INC = 1;

        public static int getUserInput(int min, int max)
        {
            String temp;
            ConsoleKeyInfo cki;
            int type = Constants.DNE;

            while (type == Constants.DNE)
            {
                cki = Console.ReadKey(true);
                temp = cki.KeyChar + "\n";
                try
                {
                    type = int.Parse(temp);
                    if (type < min || type > max)
                    {
                        Constants.writeLine("Out of range.");
                        type = Constants.DNE;
                    }
                }
                catch
                {
                    Constants.writeLine("Invalid input.");
                }
            }

            return type;
        }

        public static void writeLine(String s)
        {
            write(s + "\n");
        }

        public static void write(String s)
        {
            for(int i = 0; i < s.Length; i++)
            {
                if (s[i] == '&')
                {
                    int colour;
                    String temp = s[i + 1] + "";
                    if (s[i + 2] >= 48 && s[i + 2] <= 57)
                    {
                        temp += s[i + 2];
                        i++;
                    }
                    colour = int.Parse(temp);
                    changeColour(colour);
                    i++;//advance one extra step to skip printing the number to the console
                }
                else if(s[i] == '/')
                {
                    String temp = s[i] + "" +  s[i + 1];
                    Constants.write(temp);
                    i++;//skip the next step to not print the special character
                }
                else
                    Console.Write(s[i]);
                System.Threading.Thread.Sleep(5);
            }
        }

        private static void changeColour(int colour)
        {
            ConsoleColor[] colours = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            if(colour >= 0 && colour <= 15)
            {
                Console.ForegroundColor = colours[colour];
            }
        }
    }
}
