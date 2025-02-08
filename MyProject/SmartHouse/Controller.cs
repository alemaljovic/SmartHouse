using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class Controller
{
    public static Simulation LoadData()
    {
        Simulation mySimulation = null;
        View.LoadMenu();
        int option = int.Parse(Console.ReadLine());

        switch (option)
        {
            case 1:
                View.LoadFromFilePrompt();
                string binPath = Console.ReadLine();
                try
                {
                    using (FileStream fileIn = new FileStream(binPath, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        mySimulation = (Simulation)formatter.Deserialize(fileIn);
                    }
                }
                catch (Exception e)
                {
                    View.ExceptionPrinter(e);
                }
                break;

            case 2:
                View.LoadFromFilePrompt();
                string path = Console.ReadLine();
                try
                {
                    FileLoader loader = new FileLoader();
                    loader.LoadFromFile(path);
                    mySimulation = new Simulation(loader.EnergyProviders, loader.Houses);
                }
                catch (Exception e)
                {
                    View.ExceptionPrinter(e);
                    Environment.Exit(0);
                }
                break;

            case 3:
                View.ExitMessage();
                break;
        }
        return mySimulation;
    }

    public static void ListMenu(Simulation sim, int option)
    {
        switch (option)
        {
            case 1: View.Print(sim.ListHouses()); break;
            case 2: View.Print(sim.ListHousesRooms()); break;
            case 3: View.Print(sim.ListAll()); break;
            case 4: View.Print(sim.ListEnergyProviders()); break;
            case 5:
                View.PrintProviderBillPrompt();
                View.Print(sim.ListBillsFromProvider(Console.ReadLine()));
                break;
        }
        View.PromptEnterKey();
    }

    public static void ExecuteQuery(Simulation sim, int option)
    {
        switch (option)
        {
            case 1:
                View.Print(sim.DisplayQueryResultOne(sim.QueryHouseSpentMore()));
                break;
            case 2:
                View.Print(sim.DisplayQueryResultTwo(sim.GetProviderMostBills()));
                break;
            case 3:
                View.PrintProviderBillPrompt();
                View.Print(sim.ListBillsFromProvider(Console.ReadLine()));
                break;
            case 4:
                try
                {
                    View.DisplayCurrentDate(sim.CurrentDate);
                    View.ForthQueryPrompt(true);
                    DateTime dateStart = DateTime.Parse(Console.ReadLine());
                    View.ForthQueryPrompt(false);
                    DateTime dateEnd = DateTime.Parse(Console.ReadLine());
                    var result = sim.LargestConsumerOnTimeInterval(dateStart, dateEnd);
                    View.Print(sim.DisplayQueryResultThree(result));
                }
                catch (Exception e)
                {
                    View.ExceptionPrinter(e);
                }
                break;
        }
        View.PromptEnterKey();
    }

    public static void EditMenu(Simulation simulation, int option)
    {
        try
        {
            View.EditOption(option);
            if (option == 1)
            {
                View.Print(simulation.GetAvailableProvidersAsString());
                string chosenProvider = Console.ReadLine();
                View.EditProvider();
                int providerOption = int.Parse(Console.ReadLine());
                switch (providerOption)
                {
                    case 1:
                        View.Print("New value (double): ");
                        simulation.ChangeBaseCostProvider(chosenProvider, double.Parse(Console.ReadLine()));
                        break;
                    case 2:
                        View.Print("New value (double): ");
                        simulation.ChangeTaxProvider(chosenProvider, double.Parse(Console.ReadLine()));
                        break;
                    case 3:
                        View.Print("New value (string): ");
                        simulation.ChangeFormulaProvider(chosenProvider, Console.ReadLine());
                        break;
                }
            }
        }
        catch (Exception e)
        {
            View.ExceptionPrinter(e);
        }
    }

    public static void Run()
    {
        Simulation simulation = LoadData();
        if (simulation == null)
        {
            View.Print("Error loading simulation data. Check file integrity.");
            Environment.Exit(1);
        }
        while (true)
        {
            View.MainMenu();
            try
            {
                int option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        View.EditPrompt();
                        EditMenu(simulation, int.Parse(Console.ReadLine()));
                        break;
                    case 2:
                        View.SimulationPrompt(simulation.CurrentDate);
                        simulation.Simulate(DateTime.Parse(Console.ReadLine()));
                        View.Print("\nSimulation completed!\n\n");
                        View.PromptEnterKey();
                        break;
                    case 3:
                        View.ListPrompt();
                        ListMenu(simulation, int.Parse(Console.ReadLine()));
                        break;
                    case 4:
                        View.QueryPrompt();
                        ExecuteQuery(simulation, int.Parse(Console.ReadLine()));
                        break;
                    case 5:
                        View.SavingPrompt();
                        try
                        {
                            using (FileStream fileOut = new FileStream(Console.ReadLine(), FileMode.Create))
                            {
                                BinaryFormatter formatter = new BinaryFormatter();
                                formatter.Serialize(fileOut, simulation);
                            }
                        }
                        catch (Exception e)
                        {
                            View.ExceptionPrinter(e);
                        }
                        break;
                    case 6:
                        View.ExitMessage();
                        return;
                }
            }
            catch (FormatException)
            {
                View.Print("Exception: Expected a number but received something else.");
            }
        }
    }
}
