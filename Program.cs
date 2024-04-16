namespace RecipeApplication
{
    
    class Recipe
    {
        private string[] ingredients;
        private double[] quantities;
        private string[] units;
        private string[] steps;
        private int numIngredients;
        private int numSteps;

        public void EnterDetails()
        {
            Console.Write("Enter the number of ingredients: ");
            numIngredients = int.Parse(Console.ReadLine());

            ingredients = new string[numIngredients];
            quantities = new double[numIngredients];
            units = new string[numIngredients];

            for (int i = 0; i < numIngredients; i++)
            {
                Console.WriteLine($"Enter details for ingredient #{i + 1}:");
                Console.Write("Name: ");
                ingredients[i] = Console.ReadLine();
                Console.Write("Quantity: ");
                quantities[i] = double.Parse(Console.ReadLine());
                Console.WriteLine("Select unit:");
                Console.WriteLine("1. litres (L)");
                Console.WriteLine("2. millilitres (mL)");
                Console.WriteLine("3. grams (g)");
                Console.WriteLine("4. kilograms (kg)");
                Console.Write("Choice: ");
                int unitChoice = int.Parse(Console.ReadLine());
                switch (unitChoice)
                {
                    case 1:
                        units[i] = "L";
                        break;
                    case 2:
                        units[i] = "mL";
                        break;
                    case 3:
                        units[i] = "g";
                        break;
                    case 4:
                        units[i] = "kg";
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Defaulting to grams (g).");
                        units[i] = "g";
                        break;
                }
            }

            Console.Write("Enter the number of steps: ");
            numSteps = int.Parse(Console.ReadLine());
            steps = new string[numSteps];

            for (int i = 0; i < numSteps; i++)
            {
                Console.Write($"Enter step #{i + 1}: ");
                steps[i] = Console.ReadLine();
            }

        }

        public void DisplayRecipe()
        {
            Console.WriteLine("Recipe:");
            Console.WriteLine("Ingredients:");

            for (int i = 0; i < numIngredients; i++)
            {
                Console.WriteLine($"{quantities[i]} {units[i]} of {ingredients[i]}");
            }

            Console.WriteLine("\nSteps:");

            for (int i = 0; i < numSteps; i++)
            {
                Console.WriteLine($"{i + 1}. {steps[i]}");
            }
        }

        public void ScaleRecipe()
        {
            Console.Write("Enter scaling factor (0.5, 2, or 3): ");
            double factor = double.Parse(Console.ReadLine());

            if (factor == 0.5 || factor == 2 || factor == 3)
            {
                for (int i = 0; i < numIngredients; i++)
                {
                    // Scale grams to kilograms if necessary
                    if (units[i] == "g" && factor != 1) 
                    {
                        // Convert grams to kilograms
                        quantities[i] /= 1000; 
                        units[i] = "kg";
                    }
                    // Scale milliliters to liters if necessary
                    else if (units[i] == "mL" && factor != 1) 
                    {
                        // Convert milliliters to liters
                        quantities[i] /= 1000; 
                        units[i] = "L";
                    }

                    quantities[i] *= factor;
                }

                Console.WriteLine("Recipe scaled successfully.");
            }
            else
            {
                Console.WriteLine("Invalid scaling factor. Please enter 0.5, 2, or 3.");
            }
        }
    }
















    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
