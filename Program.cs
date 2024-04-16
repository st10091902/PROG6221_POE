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
        private double[] originalQuantities;

        public void EnterDetails()
        {
            Console.Write("Enter the number of ingredients: ");
            numIngredients = int.Parse(Console.ReadLine());

            ingredients = new string[numIngredients];
            quantities = new double[numIngredients];
            units = new string[numIngredients];
            originalQuantities = new double[numIngredients];

            for (int i = 0; i < numIngredients; i++)
            {
                Console.WriteLine($"Enter details for ingredient #{i + 1}:");
                Console.Write("Name: ");
                ingredients[i] = Console.ReadLine();
                Console.Write("Quantity: ");
                quantities[i] = double.Parse(Console.ReadLine());               

                bool validUnit = false;
                do
                {
                    Console.WriteLine("Select unit:");
                    Console.WriteLine("1. litres (L)");
                    Console.WriteLine("2. millilitres (mL)");
                    Console.WriteLine("3. grams (g)");
                    Console.WriteLine("4. kilograms (kg)");
                    Console.WriteLine("5. teaspoons (tsp)");
                    Console.WriteLine("6. tablespoons (tbsp)");
                    Console.Write("Choice: ");
                    int unitChoice;
                    if (!int.TryParse(Console.ReadLine(), out unitChoice) || unitChoice < 1 || unitChoice > 6)
                    {
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 6.");
                        continue;
                    }

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
                        case 5:
                            units[i] = "tsp";
                            break;
                        case 6:
                            units[i] = "tbsp";
                            break;
                    }

                    validUnit = true;
                } while (!validUnit);
                originalQuantities[i] = quantities[i]; // Store original quantity
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
            double factor;
            bool validScale = false;

            do
            {
                Console.Write("Enter scaling factor (0.5, 2, or 3): ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out factor))
                {
                    if (factor == 0.5 || factor == 2 || factor == 3)
                    {
                        validScale = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid scaling factor. Please enter 0.5, 2, or 3.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            } while (!validScale);

            for (int i = 0; i < numIngredients; i++)
            {
                double originalQuantity = quantities[i];
                quantities[i] *= factor; // Scale quantity

                if (units[i] == "g" || units[i] == "kg") // Handle grams and kilograms
                {
                    if (quantities[i] < 1 && quantities[i] >= 0.001) // Convert to grams if scaled down and greater than or equal to 1 gram
                    {
                        quantities[i] *= 1000;
                        units[i] = "g";
                    }
                    else if (quantities[i] < 0.001) // Convert to grams if scaled down and less than 1 gram
                    {
                        quantities[i] *= 1000000;
                        units[i] = "mg";
                    }
                    else if (quantities[i] >= 1000) // Convert to kilograms if scaled up
                    {
                        quantities[i] /= 1000;
                        units[i] = "kg";
                    }
                }
                else if (units[i] == "mL" || units[i] == "L") // Handle milliliters and liters
                {
                    if (quantities[i] < 1 && quantities[i] >= 0.001) // Convert to milliliters if scaled down and greater than or equal to 1 milliliter
                    {
                        quantities[i] *= 1000;
                        units[i] = "mL";
                    }
                    else if (quantities[i] < 0.001) // Convert to milliliters if scaled down and less than 1 milliliter
                    {
                        quantities[i] *= 1000000;
                        units[i] = "μL";
                    }
                    else if (quantities[i] >= 1000) // Convert to liters if scaled up
                    {
                        quantities[i] /= 1000;
                        units[i] = "L";
                    }
                }
                else if (units[i] == "tsp" || units[i] == "tbsp") // Handle teaspoons and tablespoons
                {
                    if (quantities[i] < 1 && quantities[i] >= 0.333) // Convert to teaspoons if scaled down and greater than or equal to 1 teaspoon
                    {
                        quantities[i] *= 3;
                        units[i] = "tsp";
                    }
                    else if (quantities[i] < 0.333) // Convert to teaspoons if scaled down and less than 1 teaspoon
                    {
                        quantities[i] *= 9;
                        units[i] = "tsp";
                    }
                    else if (quantities[i] >= 3) // Convert to tablespoons if scaled up
                    {
                        quantities[i] /= 3;
                        units[i] = "tbsp";
                    }
                }
            }

            Console.WriteLine("Recipe scaled successfully.");
        }


        public void ResetQuantities()
        {
            for (int i = 0; i < numIngredients; i++)
            {
                quantities[i] = originalQuantities[i]; // Reset quantities to original values

                // Reset units to original values based on original units
                switch (units[i])
                {
                    case "kg":
                    case "g":
                        units[i] = "g";
                        break;
                    case "L":
                    case "mL":
                    case "μL":
                        units[i] = "mL";
                        break;
                    case "tsp":
                    case "tbsp":
                        units[i] = "tsp";
                        break;
                }
            }

            Console.WriteLine("Quantities and units reset to original values.");
        }





        public void ClearData()
        {
            numIngredients = 0;
            numSteps = 0;
            ingredients = null;
            quantities = null;
            units = null;
            steps = null;
            Console.WriteLine("Data cleared successfully.");
        }

    }



    class Program
    {
        static void Main(string[] args)
        {
            Recipe recipe = new Recipe();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Enter Recipe Details");
                Console.WriteLine("2. Display Recipe");
                Console.WriteLine("3. Scale Recipe");
                Console.WriteLine("4. Reset Quantities");
                Console.WriteLine("5. Clear Data");
                Console.WriteLine("6. Exit");
                Console.Write("Choice: ");
                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        recipe.EnterDetails();
                        break;
                    case "2":
                        recipe.DisplayRecipe();
                        break;
                    case "3":
                        recipe.ScaleRecipe();
                        break;
                    case "4":
                        recipe.ResetQuantities();
                        break;
                    case "5":
                        recipe.ClearData();
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}
