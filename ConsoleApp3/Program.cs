using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Newtonsoft.Json.Linq;

//array with options save names/ new games
//get user choice for which save to use
//pull that option from array and process accordingly

namespace ConsoleApp3
{
    class Program
    {
        private static String newGame = "New Game";
        private static String[] saves = new string[3];
        private static String[] savePaths;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            loadEffects();

            int choice;
            Game game = null;
            do
            {
                loadSaves();
                getNames();
                Constants.writeLine("Enter the number for what you would like to do.\n0. Delete a save");
                for (int i = 0; i < saves.Length; i++)
                {
                    Constants.writeLine(saves[i]);
                }
                Constants.writeLine("4. Exit");

                choice = Constants.getUserInput(0, 4);
                if (choice == 0)
                    deleteSave();
                else if (choice == 1 || choice == 2 || choice == 3)
                { 
                    if (saves[choice - 1].Contains(newGame))
                    {
                        game = new Game();
                    }
                    else
                    {
                        JObject loadSave = JObject.Parse(File.ReadAllText(savePaths[choice - 1]));
                        String name = saves[choice - 1];
                        name = name.Remove(0, name.LastIndexOf('.') + 2);
                        game = new Game(loadSave, name);
                    }
                }
                else if (choice == 4)
                {
                    System.Environment.Exit(0);
                }
            } while (choice == 0 || game!=null);
        }

        private static void getNames()
        {
            for(int i = 0; i < saves.Length; i++)
            {
                saves[i] = i+1 + ". " + newGame;
            }

            for (int i = 0; i < savePaths.Length; i++)
            {
                if (savePaths[i] != null)
                {
                    String name = savePaths[i];
                    name = name.Remove(0, name.LastIndexOf('\\') + 1);
                    name = name.Remove(name.LastIndexOf('.'));
                    saves[i] = i + 1 + ". " + name;
                }
            }
        }

        private static void deleteSave()
        {
            Boolean hasPath = false;
            for(int i = 0; i< savePaths.Length; i++)
            {
                if(savePaths[i]!=null)
                {
                    hasPath = true;
                    break;
                }
            }
            if(hasPath)
            {
                Constants.writeLine("Which save would you like to delete?\n0. Back");
                int j = 1;
                for(int i = 0; i < savePaths.Length; i++)
                {//display just the array slots with actual paths
                    if (savePaths[i] != null)
                    {
                        String name = savePaths[i];
                        name = name.Remove(0, name.LastIndexOf('\\') + 1);
                        name = name.Remove(name.LastIndexOf('.'));
                        Constants.writeLine(j++ + ". " + name);
                    }
                }
                int choice = Constants.getUserInput(0, j);
                if (choice == 0)
                    return;
                else
                {
                    File.Delete(savePaths[choice - 1]);
                }
            }
        }

        private static void loadSaves()
        {
            String[] files = Directory.GetFiles(Environment.CurrentDirectory);
            savePaths = new string[3];
            for (int i = 0, j = 0; i < files.Length; i++)
            {
                if (files[i].Contains(".json"))
                {
                    savePaths[j++] = files[i];
                }
            }
        }

        private static void loadEffects()
        {
            Constants.allEffects.newItem(new EffectFire());
            Constants.allEffects.newItem(new EffectIce());
            Constants.allEffects.newItem(new EffectHeal());
        }
    }
}
