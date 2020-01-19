/* This class is the brain to the game, it goes through what a turn is, and initializes all the things needed.
 * Roughly put for turn ordering: Player attack -> creature take damage -> creature attack -> player take damage -> effects happen
 * At the end of combat, refresh all the player spell slots, and save the game.
 * Two options when a "floor" is completed, go to the next floor, or go to the store.
 * Note if the player goes to the store, reset the players progression to floor 0.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Json;

namespace ConsoleApp3
{
    class Game
    {
        private int floor;//what floor the player is currently on
        private int challengeMultiplier;//multiplier for creature level
        private LinkedList creatures;
        private Player player;
        private const int MENU_RETURN = 5;

        //constructor for a new game being made
        public Game()
        {
            floor = Constants.STARTING_FLOOR;
            setChallengeMultiplier();
            Constants.writeLine("Enter the name of your character...");
            this.player = new Player(Console.ReadLine(), Constants.STARTING_HEALTH);
            playGame();
        }

        //constructor for loading a previous save
        public Game(JObject jObject, String name)
        {
            player = new Player(name, jObject);
            floor = Constants.STARTING_FLOOR;
            setChallengeMultiplier();
            int choice = goToShop();
            if(choice != 5) 
            {
                playGame();
            }
        }

        private void setChallengeMultiplier()
        {
            challengeMultiplier = (floor / 2) + 1;
        }

        private void playGame()
        {
            while (player.isAlive())
            {
                doFloor();
                if (!player.isAlive())
                    break;
                next();
                finishFloor();
            }
        }

        //returns what the user chooses to do next after clearing a floor, options are to display stats, go to the top floor aka the shop, or proceed to the next floor
        private void next()
        {
            Constants.writeLine("\nYou killed all the creatures on this floor!");
            String info = "What would you like to do next?\n0. Exit game\n1. Display stats\n2. Proceed to next floor";
            int choice;
            if(floor%Constants.GO_TO_TOP==0)
            {
                info += "\n3. Go to shop (Back to the top of the crypt)";
                do
                {
                    Constants.writeLine(info);
                    choice = Constants.getUserInput(0, 3);
                    if (choice == 0)
                        System.Environment.Exit(0);
                    else if (choice == 1)
                        Constants.writeLine(player.ToString());
                    else if (choice == 3)
                    {
                        goToShop();
                        break;
                    }
                } while (choice != 2);
            }
            else
            {
                do
                {
                    Constants.writeLine(info);
                    choice = Constants.getUserInput(0, 2);
                    if (choice == 0)
                        System.Environment.Exit(0);
                    else if (choice == 1)
                        Constants.writeLine(player.ToString());
                } while (choice != 2);
            }
        }

        //displays the stats of the player on screen
        private void displayStats()
        {
            Constants.writeLine(player.ToString());
        }

        //load the shop with a random inventory of spells for the player to choose
        private int goToShop()
        {
            Shop shop = new Shop(player, challengeMultiplier);
            String shopOptions = "0. Leave shop\n1. Buy a spell\n2. Heal\n3. Display stats\n4. Save game\n5. Exit game";
            Constants.writeLine("Welcome to the shop " + player.getName() + ", you currently have&6 " + player.goldAmount() + " &15gold, what would you like to do?");
            int choice;
            do
            {
                Constants.writeLine(shopOptions);
                choice = Constants.getUserInput(0, 5);
                if (choice == 1)
                    buySpell(shop);
                else if (choice == 2)
                    heal();
                else if (choice == 3)
                    displayStats();
                else if (choice == 4)
                {
                    saveGame();
                }
                else if (choice == 5)
                {
                    Constants.writeLine("Are you sure you want to quit? Any unsaved progress will be lost.\n0. Yes\n1. No");
                    choice = Constants.getUserInput(0, 1);
                    if(choice == 0)
                        return MENU_RETURN;
                }
            } while (choice != 0);
            return choice;
        }

        private void buySpell(Shop shop)
        {
            Constants.writeLine("What spell would you like to purchase?");
            int choice;
            do
            {
                Constants.writeLine("0. Back\n" + shop.getSpells());
                choice = Constants.getUserInput(0, Constants.NUM_SPELLS);
                if (choice != 0)
                {
                    shop.buySpell(choice - 1);
                    Constants.writeLine("Would you like to buy anything else?");
                }
            } while (choice != 0);
            Constants.writeLine("Thank you, come again!");
        }

        private void heal()
        {
            int healCost = player.healCost();
            if (healCost > 0)
            {
                Constants.writeLine("Healing will cost&6 " + healCost + "&15 gold. Would you like to be healed?\n0. No\n1. Yes");
                int choice = Constants.getUserInput(0, 1);
                if (choice == 1)
                {
                    if (player.goldAmount() >= healCost)
                    {
                        player.maxHeal();
                        Constants.writeLine("You have been healed to your max health of&4 " + player.getCurrHP() + "&15 ");
                    }
                    else
                        Constants.writeLine("You don't have enough gold to be healed.");
                }
            }
            else
                Constants.writeLine("You already have max hp.");
        }

        //save the game to a text file which can be read later
        private void saveGame()
        {
            Constants.writeLine("Game saved succesfully.");
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                player.save(writer);
                writer.WriteEndObject();
            }

            File.WriteAllText(Environment.CurrentDirectory + "\\" + player.getName() + ".json", sw.ToString());
        }

        //when a floor is cleared, go to the next floor and increase the challenge modifier
        private void finishFloor()
        {
            floor++;
            if (floor % Constants.UPDATE_CHALLENGE == 0)
                challengeMultiplier++;
        }

        //start the floor, do combat till all creatures are dead or player is dead, if player is alive at the end, award them xp and gold for the floor
        private void doFloor()
        {
            creatures = new LinkedList();
            loadCreatures();
            int floorXP = getTotalXP();
            int floorGold = getTotalGold();
            displayFloorInfo(floorXP);

            while (player.isAlive() && !creatures.isEmpty())
                doRound();

            if (player.isAlive())
            {
                player.finishCombat(floorXP);
                Constants.writeLine("You earned&6 " + floorGold + "&15 gold for this floor.");
                player.earnGold(floorGold);
            }
            else
            {
                Constants.writeLine("&12You have died on floor " + floor + "&15");
            }
            System.Threading.Thread.Sleep(750);
        }

        //when round of combat goes player attacks -> creature(s) attack -> status effects proc
        private void doRound()
        {
            doPlayerEffects();

            Constants.writeLine("\nYou have&4 " + player.getCurrHP() + "&15 hit points left.");

            playerAttack();

            doCreatureEffects();

            creaturesAttack();

            resetAttacks();
        }

        
        private void resetAttacks()
        {
            player.resetAttack();
            for (int i = 0; i < creatures.getLength(); i++)
            {
                GenericCreature creature = (GenericCreature) creatures.getItem(i);
                creature.resetAttack();
            }
        }

        private int getTotalXP()
        {
            int total = 0;
            GenericCreature currCreature;

            for(int i = 0; i < creatures.getLength(); i++)
            {
                currCreature = (GenericCreature)creatures.getItem(i);
                total += currCreature.xpValue();
            }

            total = (int)(total * (1.0 + (floor / 10.0)));

            return total;
        }

        private int getTotalGold()
        {
            int total = 0;
            GenericCreature currCreature;

            for (int i = 0; i < creatures.getLength(); i++)
            {
                currCreature = (GenericCreature)creatures.getItem(i);
                total += currCreature.goldValue();
            }

            total = (int)(total * (1.0 + (floor / 20.0)));

            return total;
        }

        private void doPlayerEffects()
        {
            player.doEffects();
        }

        private void doCreatureEffects()
        {
            GenericCreature currCreature;
            for(int i = 0; i < creatures.getLength(); i++)
            {
                currCreature = (GenericCreature)creatures.getItem(i);
                currCreature.doEffects();
                if(!currCreature.isAlive())
                {
                    Constants.writeLine(currCreature.getName() + " has died.");
                    creatures.removeItem(i);//remove the now dead creature
                    i--;//we need to decrease our counter of creatures that we're going through as we just removed a creature from the list
                }
            }
            // player.doEffects();
        }

        //let's the player attack a creature, if theres only one creature just attacks that creature
        private void playerAttack()
        {
            Constants.writeLine("It's your turn.");
            int choice = 0;
            Strike playerAttack;
            do
            {
                playerAttack = new Strike(player);
                if (!playerAttack.isStriking())
                    return;
                Constants.writeLine("Who would you like to target?");
                choice = chooseCreature();
            } while (choice == 0);

            Attack attack = new Attack(playerAttack);

            if(choice==1)
            {
                Constants.writeLine("You targeted yourself.");
                player.attacked(attack);
            }
            else
            {
                GenericCreature target = (GenericCreature)creatures.getItem(choice-2);
                Constants.writeLine("You targeted " + target.getName() + ".");
                target.attacked(attack);
                if (!target.isAlive())
                {
                    Constants.writeLine("The creature falls to the ground dead.");
                    creatures.removeItem(choice - 2);
                }
                System.Threading.Thread.Sleep(750);
            }
        }

        //initializes the creatures for the floor that the player is currently on
        private void loadCreatures()
        {
            int points = challengeMultiplier - 1;//the floor will get a certain number of "points" that can be used for either more creatures (with a max), or to increase an laready existing creatures level
            creatures.newItem(new TestCreature(1));//must have at least one creature at level 1 for the floor

            while(points > 0)
            {
                int nextPoint = Constants.rand.Next(2);//returns 0<=nextPoint<2, essentially, flip a coin
                if(nextPoint==0 && creatures.getLength() < Constants.MAX_CREATURES)//one choice, add another creature, so long as we haven't hit our max
                {
                    creatures.newItem(new TestCreature(1));
                }
                else//otherwise, we want to increase the level of a creature already in the list
                {
                    int selectedCreature = Constants.rand.Next(creatures.getLength());
                    GenericCreature creature = (GenericCreature) creatures.getItem(selectedCreature);
                    creature.lvlUp();
                }
                points--;
            }     
        }

        //displays how many creatures there are, what the multiplier is, what floor they're on, and how much xp this floor is worth
        private void displayFloorInfo(int xp)
        {
            Constants.writeLine("\n--------------------------------------------------\nFloor:&13 " + floor + 
                "\n&15This floor has&4 " + creatures.getLength() + "&15 creature(s)." +
                "\n&15Total xp to be earned on this floor:&11 " + xp +
                "\n&15--------------------------------------------------\n");
            Constants.writeLine("In front of you, you see:\n" + creatures.ToString());
        }

        //used to get user input for what creature if there are multiple creatures they would like to attack
        private int chooseCreature()
        {
            String display = "\n0. Back\n1. Self\n";
            GenericCreature creature;

            for(int i = 1; i < creatures.getLength()+1; i++)
            {
                creature = (GenericCreature) creatures.getItem(i-1);
                display += (i + 1) + ". " + creature.ToString() + "\n";
            }

            Constants.writeLine(display);
            return Constants.getUserInput(0, creatures.getLength()+2);//+2 since there's two extra options
        }

        //Has all the creatures left standing attack the player
        private void creaturesAttack()
        {
            GenericCreature currCreature;
            for (int i = 0; i < creatures.getLength(); i++)
            {
                currCreature = (GenericCreature)creatures.getItem(i);
                if (currCreature.checkAttack())
                {
                    Strike strike = new Strike(currCreature);
                    Attack attack = new Attack(strike);
                    if (strike.isStriking())
                    {
                        String output = currCreature.getName() + " targets you with ";
                        if (strike.isMelee())
                            output += "a melee attack.";
                        else if (strike.isRanged())
                            output += "a ranged attack.";
                        else if (strike.isMagical())
                            output += "a spell.";
                        Constants.writeLine(output);
                        player.attacked(attack);
                        currCreature.attacked();
                    }
                    else
                        Constants.writeLine(currCreature.getName() + " has begun defending.");
                }
                System.Threading.Thread.Sleep(750);
            }
        }
    }
}
