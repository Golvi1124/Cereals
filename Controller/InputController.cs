using System;
using Cereals.Models;

namespace Cereals.Controller;

public class InputController(DataContext context)
{
    // Here we have moved the responsibility for input to a separate controller.
    // We retrieve a reference to our data context (to our csv file)

    private readonly DataContext _context = context;
    private bool isRunning { get; set; } = true;

    public void Run()
    {
        Console.Clear();
        while (isRunning)
        {

            Console.WriteLine("Here are the controls:");
            Console.WriteLine("1: Which manufacturers are represented?");
            Console.WriteLine("2: Which are the Top 5 Cereals by consumer reports?");
            Console.WriteLine("3: Did you know that there are cereals served hot?");
            Console.WriteLine("4: Top 5 with most and least amount of calories per serving.");
            Console.WriteLine("\nNeed help with choosing right cereal?");
            Console.WriteLine("5: Sort by vitamins and proteins.");
            Console.WriteLine("6: Sort by Manufacturer, grams of fiber and carbohydrates.");
            Console.WriteLine("7: ...exit program");

            var input = Console.ReadLine();
            int command;

            // Here we need the user to enter a text that can be parsed into an integer.
            while (!int.TryParse(input, out command))
            {
                Console.WriteLine("Please choose a valid command number");
                input = Console.ReadLine();
            }

            switch (command)
            {
                case 1: // Which manufacturers are represented? ✔
                    Console.WriteLine($"In total there are {Manufacturers.Count} manufacturers:");
                    foreach (var manu in Manufacturers)
                    {
                        Console.WriteLine($"\t- {manu.Value}");
                    }
                    break;

                case 2: // Which are the Top 5 Cereals ✔

                    Console.WriteLine("Top 5 Cereals are:");

                    var topCereals = _context.Cereals
                                            .OrderByDescending(cereal => cereal.Rating)
                                            .Take(5)
                                            .ToList() // Force execution here
                                            .Select((cereal, index) => new
                                            {
                                                Rank = index + 1,
                                                cereal.Name,
                                                Manufacturer = Manufacturers[cereal.Mfr],
                                                Rating = Math.Round(cereal.Rating, 1), // Round rating
                                            });


                    foreach (var cereal in topCereals)
                    {
                        Console.WriteLine($"{cereal.Rank}. place is {cereal.Name} produced by {cereal.Manufacturer} with a rating of {cereal.Rating}.");
                    }

                    break;

                case 3: // cereals served hot ✔

                    Console.WriteLine("These ones you should warm up before eating!");
                    Console.WriteLine($"There are {_context.Cereals.Where(cereal => cereal.Type == 'H').Count()} in total:");

                    foreach (var cereal in _context.Cereals.Where(cereal => cereal.Type == 'H'))
                    {
                        char mfrKey = cereal.Mfr;
                        Console.WriteLine($"- {cereal.Name} produced by {Manufacturers[cereal.Mfr]}.");
                    }
                    break;

                case 4: // Top 5 with most and least amount of calories ✔

                    var lines = File.ReadAllLines(@"cereal.csv");
                    var count = lines.Length;
                    Console.WriteLine($"In total there are {count} cereals represented and from them:");


                    Console.WriteLine("- Top 5 with the biggest amount of calories per serving:");

                    var topUpCereals = _context.Cereals
                                            .OrderByDescending(cereal => cereal.Calories)
                                            .Take(5)
                                            .ToList() // Force execution here
                                            .Select((cereal, index) => new
                                            {
                                                Rank = index + 1,
                                                cereal.Name,
                                                Manufacturer = Manufacturers[cereal.Mfr],
                                                cereal.Calories,
                                            });


                    foreach (var cereal in topUpCereals)
                    {
                        Console.WriteLine($"\t{cereal.Rank}. place is {cereal.Name} produced by {cereal.Manufacturer} with a calory count of {cereal.Calories}.");
                    }

                    Console.WriteLine("- Top 5 with the smallest amount of calories per serving:");
                    var topDownCereals = _context.Cereals
                                             .OrderBy(cereal => cereal.Calories)
                                             .Take(5)
                                             .ToList() // Force execution here
                                             .Select((cereal, index) => new
                                             {
                                                 Rank = index + 1,
                                                 cereal.Name,
                                                 Manufacturer = Manufacturers[cereal.Mfr],
                                                 cereal.Calories,
                                             });


                    foreach (var cereal in topDownCereals)
                    {
                        Console.WriteLine($"\t{cereal.Rank}. place is {cereal.Name} produced by {cereal.Manufacturer} with a calory count of {cereal.Calories}.");
                    }

                    break;

                case 5: // Sort by vitamins and proteins.
                    Console.WriteLine("Here you will be able to choose cereals by Vitamin and Protein amounts.");

                    var query = QueryVitaPro();
                    Console.WriteLine("\nHere comes the results of your query:");
                    Console.WriteLine($"We found {query.Count()} matches.");

                    foreach (var cereal in query)
                    {
                        Console.WriteLine($"\t- {cereal.Name}. Also known to have {cereal.Fat} g of Fat and {cereal.Sodium} mg of Sodium");
                    }

                    break;

                case 6: // Sort by Manufacturer and grams of fiber.
                    Console.WriteLine("Here you will be able to choose cereals by Manufacturer and Fiber amounts.");

                    var query2 = QueryManuFiber();
                    Console.WriteLine("\nHere comes the results of your query:");
                    Console.WriteLine($"We found {query2.Count()} matches.");

                    foreach (var cereal in query2)
                    {
                        Console.WriteLine($"\t- {cereal.Name}. Also known to have {cereal.Sugars} g of Sugars and {cereal.Potass} mg of Potass");
                    }

                    break;


                default:
                    isRunning = false;
                    break;
            }
        }
    }





    public IQueryable<CerealData> QueryVitaPro()
    {
        var queryStart = _context.Cereals.AsQueryable();

        Console.WriteLine("Following the typical percentage of FDA recommended, enter:");
        Console.WriteLine("\t0 - if not looking for extra vitamin intake.\n\t25 - if your vitamin intake is normal.\n\t100 - if it's your only vitamin intake.\n\t...or press enter to skip.");
        string vitaminIntake = Console.ReadLine();
        if (!string.IsNullOrEmpty(vitaminIntake) && double.TryParse(vitaminIntake, out double vitaminIn))
        {
            queryStart = queryStart.Where(s => s.Vitamins == vitaminIn);
        }

        Console.WriteLine($"How many grams of protein do you want to intake? Min: {_context.Cereals.Min(c => c.Protein)}, Max: {_context.Cereals.Max(c => c.Protein)} ?\n...or press enter to skip.");
        string proteinIntake = Console.ReadLine();
        if (!String.IsNullOrEmpty(proteinIntake) && double.TryParse(proteinIntake, out double proteinIn))
        {
            queryStart = queryStart.Where(s => s.Protein == proteinIn);
        }

        return queryStart;
    }

    public IQueryable<CerealData> QueryManuFiber()
    {
        var queryStart = _context.Cereals.AsQueryable();
        // Manufacturer
        Console.WriteLine("Start with choosing Manufacturer. Type in corresponding letter from list below.");
        foreach (var manu in Manufacturers)
        {
            Console.WriteLine($"\t{manu.Key} = {manu.Value}");
        }
        string manuInput = Console.ReadLine();

        if (!string.IsNullOrEmpty(manuInput) && manuInput.Length == 1) // Ensure a single character was entered
        {
            char manuChar = manuInput[0]; // Extract the first character
            queryStart = queryStart.Where(s => s.Mfr == manuChar);
        }

        // Fiber grams
        Console.WriteLine("How many grams of Fiber do you want to intake?");
        Console.WriteLine($"Dataset has range from {_context.Cereals.Min(c => c.Fiber)} to {_context.Cereals.Max(c => c.Fiber)}");
        Console.WriteLine("\tType 1 if looking for smaller amount.\n\tType 2 if looking for bigger amount.");
        string fiberInput = Console.ReadLine();



        if (!String.IsNullOrEmpty(fiberInput) && int.TryParse(fiberInput, out int choice))
        {
            if (choice == 1)
            {
                queryStart = queryStart.Where(c => c.Fiber >= 0 && c.Fiber <= 2);
            }
            else if (choice == 2)
            {
                queryStart = queryStart.Where(c => c.Fiber > 2);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 1 or 2.");
            }
        }

        //Dataset has range from 0 to 14
        // small 0-2 , big 3-14 to have more options






        return queryStart;
    }
    Dictionary<char, string> Manufacturers = new Dictionary<char, string>
        {
            {'A', "American Home Food Products"},
            {'G', "General Mills"},
            {'K', "Kelloggs"},
            {'N', "Nabisco"},
            {'P', "Post"},
            {'Q', "Quaker Oats"},
            {'R', "Ralston Purina"}
        };
}
