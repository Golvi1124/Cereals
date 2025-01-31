using System;

namespace Cereals.Models;
// Here we create a model for our entire file.
public class DataContext
{
    // We say that the file represents a list of SquirrelData objects.
    public List<CerealData> Cereals { get; set; } = [];

    // We create a constructor that reads the file and creates a list based on the data there.
    public DataContext()
    {
        var rawData = File.ReadLines("cereal.csv");

        /* Here we use LinQ to create the list of CerealData.
        We "chain" several methods together into a method chain.
        Skip allows us to skip x number (in our case 1) elements at the beginning of our sequence before the next operation.
        Select says, based on an arbitrary element, what to return.
        ToList() says what to store the result as. */

        Cereals = rawData.Skip(1).Select(dataString => new CerealData(dataString)).ToList();
        //I prinsipp det samme som: 
        /* var data = rawData.ToList();
        for (int i = 1; i < data.Count; i++)
        {
         Squirrels.Add(new SquirrelData(data[i]));
        } */

        

    }
}
