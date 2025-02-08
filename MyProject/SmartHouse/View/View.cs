using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace SmartHouseSimulator.View
{
    public static class View
    {
        public static void Print(object obj)
        {
            Console.Write(obj);
        }

        public static void LoadMenu()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("[*] Welcome to Smart House Simulator");
            s.AppendLine("[?] Load the data:");
            s.AppendLine("   [1] From binary file.");
            s.AppendLine("   [2] From text file.");
            s.AppendLine("   [3] Quit from program.");
            s.Append("[>] Option (b to go back): ");
            Console.Write(s.ToString());
        }

        public static void LoadFromFilePrompt()
        {
            Console.Write("[?] What's the path for the file? >> ");
        }

        public static void MainMenu()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("[*] Welcome to Smart House Simulator");
            s.AppendLine("[?] Choose an action:");
            s.AppendLine("   [1] Edit data.");
            s.AppendLine("   [2] Simulate passage of time.");
            s.AppendLine("   [3] List stored data.");
            s.AppendLine("   [4] Execute queries.");
            s.AppendLine("   [5] Save current state.");
            s.AppendLine("   [6] Quit from program.");
            s.Append("[>] Option: ");
            Console.Write(s.ToString());
        }

        public static void EditPrompt()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("[*] Edit Menu");
            s.AppendLine("[?] Choose an element to edit:");
            s.AppendLine("   [1] Energy provider.");
            s.AppendLine("   [2] Switch house devices.");
            s.AppendLine("   [3] House.");
            s.Append("[>] Option: ");
            Console.Write(s.ToString());
        }

        public static void EditOption(int option)
        {
            if (option == 1) Console.Write("[?] Enter the name of the energy provider: ");
            if (option == 2) Console.Write("[?] Enter the name of the house owner: ");
        }

        public static void EditHouse()
        {
            Console.WriteLine("[?] Choose an element to edit:");
            Console.WriteLine("   [1] Energy Provider.");
            Console.Write("[>] Option: ");
        }

        public static void EditProvider()
        {
            Console.WriteLine("   [1] Edit base cost (double).");
            Console.WriteLine("   [2] Edit tax value (double).");
            Console.WriteLine("   [3] Edit formula (string).");
            Console.Write("[>] Option: ");
        }

        public static void ListPrompt()
        {
            Console.WriteLine("[*] Listing Menu");
            Console.WriteLine("[?] Choose an option to list:");
            Console.WriteLine("   [1] List houses only.");
            Console.WriteLine("   [2] List houses and its rooms only.");
            Console.WriteLine("   [3] List houses, rooms and devices.");
            Console.WriteLine("   [4] List energy providers.");
            Console.WriteLine("   [5] List bills sent by an energy provider.");
            Console.Write("[>] Option: ");
        }

        public static void QueryPrompt()
        {
            Console.WriteLine("[*] Query Menu");
            Console.WriteLine("[?] Choose a query to execute:");
            Console.WriteLine("   [1] What house used more energy during the simulation period.");
            Console.WriteLine("   [2] Which energy provider has the largest volume of billing.");
            Console.WriteLine("   [3] List bills by a given energy provider.");
            Console.WriteLine("   [4] Sort the largest energy users on a period of time.");
            Console.Write("[>] Option: ");
        }

        public static void PromptEnterKey()
        {
            Console.Write("Press \"ENTER\" to continue...");
            Console.ReadLine();
        }

        public static void ExitMessage()
        {
            Console.WriteLine("Exiting Simulator...");
            Environment.Exit(0);
        }

        public static void SavingPrompt()
        {
            Console.WriteLine("[?] Where do you wish to save the file?");
            Console.Write("   [>] Path: ");
        }
    }
}
