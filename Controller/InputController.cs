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
            Console.WriteLine("4: Top 3 with most and least amount of calories per serving.");
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

                case 2: // Which are the Top 5 Cereals ...DOEEEESSSSNNNNN WORK ..Tomorrow's problem

                    Console.WriteLine("Top 5 Cereals are:");

                    var topCereals = _context.Cereals
                    .OrderByDescending(cereal => cereal.Rating) // Sort by rating in descending order
                    .Take(5) // Get top 5
                    .Select((cereal, index) => new
                    {
                        Rank = index + 1,
                        Name = cereal.Name,
                        Manufacturer = Manufacturers[cereal.Mfr],
                        Rating = cereal.Rating, //...to do: Round rating to 1 decimal place 
                    });

                    foreach (var cereal in topCereals)
                    {
                        Console.WriteLine($"{cereal.Rank}. place is {cereal.Name} produced by {cereal.Manufacturer} with a rating of {cereal.Rating}.");
                    }


                    /* foreach (var cereal in _context.Cereals)
                    {
                        Console.WriteLine($"{cereal.Name}: Raw Rating = {cereal.Rating}");
                    }
 */


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

                case 4: // Top 3 with most and least amount of calories
                
                var lines = File.ReadAllLines(@"cereal.csv");
                var count = lines.Length;
                    Console.WriteLine($"In total there are {count} cereals represented and from them:");


                    Console.WriteLine("Top 3 with the biggest amount of calories per serving.");
                    /* 
                    {1.} place is {name} prodused by {mfr} with {calories}.
                     */

                    Console.WriteLine("Top 3 with the smallest amount of calories per serving.");
                    /* 
                    {1.} place is {name} prodused by {mfr} with {calories}.
                     */

                    break;

                case 5: // Sort by vitamins and proteins.
                    Console.WriteLine("");

                    /* 
                    var query = QueryBuilder();
                    1. Do you need to increase your intake of vitamins and minerals?
                        y / n
                        if no => vitamims = 0
                        if yes => Is it your only way to intake vitamins and minerals?
                            if yes = vitamims = 100
                            if no = vitamims = 25
                    2. How many grams of protein do you want to intake (1-6)? 


                       Console.WriteLine("Here comes the results of your query:");
                     */

                    break;

                case 6: // Sort by Manufacturer, grams of fiber and carbohydrates.
                    Console.WriteLine("");

                    /* 
                    by manufacturer
                    by fiber grams
                        get average as middle point? => above vs below?
                        same with carbo 
                     need to involve sugars? 
                     */
                    break;



                /* 
                case 4: 
                                    var query = QueryBuilder();
                                    Console.WriteLine("Here comes the results of your query:");
                                    Console.WriteLine($"We found {query.Count()} squirrels.");
                                    foreach (var squirrel in query)
                                    {
                                        Console.WriteLine($"Squirrel with id {squirrel.SquirrelId} is an {squirrel.Age}, it's fur color is {squirrel.PrimaryFurColor}.");
                                    }
                                    break;
                 */


                default:
                    isRunning = false;
                    break;
            }
        }
    }

    /* Here we have a method where we create a LinQ interface, which will eventually "consume" and display a result to the user.
    We see again here that we can "chain" the methods as much as we want to see if the user wants to query against Age, 
    against PrimaryFurColor or both, or neither. */

    /* 
    public IQueryable<SquirrelData> QueryBuilder()
    {
        var queryStart = _context.Squirrels.AsQueryable();
        
        Console.WriteLine("Type an age you want to query by, or press enter to skip.");
        string ageinput = Console.ReadLine();
        if (!string.IsNullOrEmpty(ageinput))
        {
            queryStart = queryStart.Where(s => string.Equals(s.Age, ageinput, StringComparison.InvariantCultureIgnoreCase));
        }
        Console.WriteLine("Type a fur color you want to query by, or press enter to skip");
        string furinput = Console.ReadLine();
        if (!string.IsNullOrEmpty(furinput))
        {
            queryStart = queryStart.Where(s => string.Equals(s.PrimaryFurColor, furinput, StringComparison.InvariantCultureIgnoreCase));
        }
        return queryStart;
    } */

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
