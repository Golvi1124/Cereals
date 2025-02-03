using System;
using System.Globalization;

namespace Cereals.Models;

public class CerealData
{
    // Didn't use Shelf, Weight or Cups, since didn't see use for that.
    // Also dropped ... , because in overall all are numbers and would be the same methods. 
    public string Name { get; set; }
    public char Mfr { get; set; }
    public char Type { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Sodium { get; set; }
    public double Fiber { get; set; }
    public double Carbo { get; set; }
    public double Sugars { get; set; }
    public double Potass { get; set; }
    public double Vitamins { get; set; }
    public double Rating { get; set; }

    // Here we create a constructor, which will create a CerealData object.
    // We take in a string, which represents a Comma Separated Values ​​string or an arbitrary line in our file. */

    public CerealData(string csv)
    {
        string[] values = csv.Split(",");

        Name = values[0];

        if (char.TryParse(values[1], out char mfr))
            Mfr = mfr;

        if (char.TryParse(values[2], out char type))
            Type = type;

        if (double.TryParse(values[3], CultureInfo.InvariantCulture, out double calories))
            Calories = calories;

        if (double.TryParse(values[4], CultureInfo.InvariantCulture, out double protein))
            Protein = protein;

        if (double.TryParse(values[5], CultureInfo.InvariantCulture, out double fat))
            Fat = fat;

        if (double.TryParse(values[6], CultureInfo.InvariantCulture, out double sodium))
            Sodium = sodium;

        if (double.TryParse(values[7], CultureInfo.InvariantCulture, out double fiber))
            Fiber = fiber;

        if (double.TryParse(values[8], CultureInfo.InvariantCulture, out double carbo))
            Carbo = carbo;

        if (double.TryParse(values[9], CultureInfo.InvariantCulture, out double sugars))
            Sugars = sugars;

        if (double.TryParse(values[10], CultureInfo.InvariantCulture, out double potass))
            Potass = potass;

        if (double.TryParse(values[11], CultureInfo.InvariantCulture, out double vitamins))
            Vitamins = vitamins;

        if (double.TryParse(values[15], CultureInfo.InvariantCulture, out double rating))
            Rating = rating;

    }

}
