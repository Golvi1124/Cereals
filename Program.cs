namespace Cereals;
using Cereals.Models;
using Cereals.Controller;

class Program
{
    static void Main(string[] args)
    {
        // Here we create an instance of our data context, the one that will be our model of the file itself.
        var context = new DataContext();
        Console.WriteLine("Welcome to the cereal explorer!");
        var controller = new InputController(context);
        controller.Run();
    }
}
