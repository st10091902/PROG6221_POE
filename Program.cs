using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RecipeManager recipeManager = new RecipeManager();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Enter a new recipe");
                Console.WriteLine("2. Display all recipes");
                Console.WriteLine("3. Select a recipe to display");
                Console.WriteLine("4. Scale a recipe");
                Console.WriteLine("5. Reset quantities");
                Console.WriteLine("6. Clear all data");
                Console.WriteLine("7. Exit");
                Console.Write("Choice: ");
                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        recipeManager.AddRecipe();
                        break;
                    case "2":
                        recipeManager.DisplayAllRecipes();
                        break;
                    case "3":
                        recipeManager.SelectRecipeToDisplay();
                        break;
                    case "4":
                        recipeManager.ScaleRecipe();
                        break;
                    case "5":
                        recipeManager.ResetQuantities();
                        break;
                    case "6":
                        recipeManager.ClearData();
                        break;
                    case "7":
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

    /// <summary>
    /// Manages multiple recipes and their operations.
    /// </summary>
    public class RecipeManager
    {
        private List<Recipe> recipes = new List<Recipe>();

        /// <summary>
        /// Adds a new recipe by collecting its details from the user.
        /// </summary>
        public void AddRecipe()
        {
            Recipe recipe = new Recipe();
            recipe.OnCaloriesExceeded += Recipe_OnCaloriesExceeded;
            recipe.EnterDetails();
            recipes.Add(recipe);
        }

        private void Recipe_OnCaloriesExceeded(double totalCalories)
        {
            Console.WriteLine($"Warning: Total calories exceed 300. Current total: {totalCalories}");
        }

        /// <summary>
        /// Displays all recipes in alphabetical order by name.
        /// </summary>
        public void DisplayAllRecipes()
        {
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes available.");
                return;
            }

            var sortedRecipes = recipes.OrderBy(r => r.Name).ToList();
            Console.WriteLine("Recipes:");
            foreach (var recipe in sortedRecipes)
            {
                Console.WriteLine(recipe.Name);
            }
        }

        /// <summary>
        /// Allows the user to select a recipe by name to display its details.
        /// </summary>
        public void SelectRecipeToDisplay()
        {
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes available.");
                return;
            }

            DisplayAllRecipes();
            Console.Write("Enter the name of the recipe you want to display: ");
            string recipeName = Console.ReadLine();

            Recipe recipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
            if (recipe != null)
            {
                recipe.DisplayRecipe();
            }
            else
            {
                Console.WriteLine("Recipe not found.");
            }
        }

        /// <summary>
        /// Scales the quantities of the ingredients by a given factor.
        /// </summary>
        public void ScaleRecipe()
        {
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes available to scale.");
                return;
            }

            DisplayAllRecipes();
            Console.Write("Enter the name of the recipe you want to scale: ");
            string recipeName = Console.ReadLine();

            Recipe recipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
            if (recipe != null)
            {
                recipe.ScaleRecipe();
            }
            else
            {
                Console.WriteLine("Recipe not found.");
            }
        }

        /// <summary>
        /// Resets the quantities of the ingredients to their original values.
        /// </summary>
        public void ResetQuantities()
        {
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes available to reset.");
                return;
            }

            DisplayAllRecipes();
            Console.Write("Enter the name of the recipe you want to reset: ");
            string recipeName = Console.ReadLine();

            Recipe recipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
            if (recipe != null)
            {
                recipe.ResetQuantities();
            }
            else
            {
                Console.WriteLine("Recipe not found.");
            }
        }

        /// <summary>
        /// Clears all data of the recipe.
        /// </summary>
        public void ClearData()
        {
            recipes.Clear();
            Console.WriteLine("All recipe data cleared.");
        }
    }

    /// <summary>
    /// Represents a single recipe with ingredients and steps.
    /// </summary>
    public class Recipe
    {
        public string Name { get; private set; }
        public List<Ingredient> Ingredients { get; private set; }
        private List<string> steps;
        private List<(double Quantity, string Unit, double Calories)> originalQuantitiesAndUnits; 
        public delegate void CalorieNotification(double totalCalories);
        public event CalorieNotification OnCaloriesExceeded;

        public Recipe()
        {
            Ingredients = new List<Ingredient>();
            steps = new List<string>();
            originalQuantitiesAndUnits = new List<(double Quantity, string Unit, double Calories)>();
        }

        /// <summary>
        /// Collects the details of the recipe from the user.
        /// </summary>
        public void EnterDetails()
        {
            Console.Write("Enter the recipe name: ");
            Name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(Name))
            {
                Console.Write("Invalid input. Please enter a valid name: ");
                Name = Console.ReadLine();
            }

            int numIngredients;
            Console.Write("Enter the number of ingredients: ");
            while (!int.TryParse(Console.ReadLine(), out numIngredients) || numIngredients <= 0)
            {
                Console.Write("Invalid input. Please enter a positive number for the number of ingredients: ");
            }

            for (int i = 0; i < numIngredients; i++)
            {
                Console.WriteLine($"Enter details for ingredient #{i + 1}:");

                Ingredient ingredient = new Ingredient();

                Console.Write("Name: ");
                ingredient.Name = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(ingredient.Name))
                {
                    Console.Write("Invalid input. Please enter a valid name: ");
                    ingredient.Name = Console.ReadLine();
                }

                double quantity;
                Console.Write("Quantity: ");
                while (!double.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                {
                    Console.Write("Invalid input. Please enter a positive number for the quantity: ");
                }
                ingredient.Quantity = quantity;

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
                            ingredient.Unit = "L";
                            break;
                        case 2:
                            ingredient.Unit = "mL";
                            break;
                        case 3:
                            ingredient.Unit = "g";
                            break;
                        case 4:
                            ingredient.Unit = "kg";
                            break;
                        case 5:
                            ingredient.Unit = "tsp";
                            break;
                        case 6:
                            ingredient.Unit = "tbsp";
                            break;
                    }

                    validUnit = true;
                } while (!validUnit);

                double calories;
                Console.Write("Calories: ");
                while (!double.TryParse(Console.ReadLine(), out calories) || calories < 0)
                {
                    Console.Write("Invalid input. Please enter a non-negative number for the calories: ");
                }
                ingredient.Calories = calories;

                Console.Write("Food Group: ");
                ingredient.FoodGroup = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(ingredient.FoodGroup))
                {
                    Console.Write("Invalid input. Please enter a valid food group: ");
                    ingredient.FoodGroup = Console.ReadLine();
                }

                Ingredients.Add(ingredient);
                originalQuantitiesAndUnits.Add((ingredient.Quantity, ingredient.Unit, ingredient.Calories)); // Store original quantity, unit, and calories
            }

            int numSteps;
            Console.Write("Enter the number of steps: ");
            while (!int.TryParse(Console.ReadLine(), out numSteps) || numSteps <= 0)
            {
                Console.Write("Invalid input. Please enter a positive number for the number of steps: ");
            }

            for (int i = 0; i < numSteps; i++)
            {
                Console.Write($"Enter step #{i + 1}: ");
                string step = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(step))
                {
                    Console.Write("Invalid input. Please enter a valid step: ");
                    step = Console.ReadLine();
                }
                steps.Add(step);
            }

            double totalCalories = CalculateTotalCalories();
            if (totalCalories > 300)
            {
                OnCaloriesExceeded?.Invoke(totalCalories);
            }
        }

        /// <summary>
        /// Displays the details of the recipe.
        /// </summary>
        public void DisplayRecipe()
        {
            if (Ingredients.Count == 0 || steps.Count == 0)
            {
                Console.WriteLine("No recipe details available.");
                return;
            }

            Console.WriteLine($"Recipe: {Name}");
            Console.WriteLine("Ingredients:");

            foreach (var ingredient in Ingredients)
            {
                Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} - {ingredient.Calories} calories, Food Group: {ingredient.FoodGroup}");
            }

            double totalCalories = CalculateTotalCalories();
            Console.WriteLine($"\nTotal Calories: {totalCalories}");

            Console.WriteLine("\nSteps:");
            for (int i = 0; i < steps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {steps[i]}");
            }
        }

        /// <summary>
        /// Scales the quantities of the ingredients by a given factor.
        /// </summary>
        public void ScaleRecipe()
        {
            if (Ingredients.Count == 0)
            {
                Console.WriteLine("No ingredients available to scale.");
                return;
            }

            Console.Write("Enter the scaling factor (e.g., 0.5 for half, 2 for double): ");
            double scalingFactor;
            while (!double.TryParse(Console.ReadLine(), out scalingFactor) || scalingFactor <= 0)
            {
                Console.Write("Invalid input. Please enter a positive number for the scaling factor: ");
            }

            for (int i = 0; i < Ingredients.Count; i++)
            {
                Ingredients[i].Quantity = originalQuantitiesAndUnits[i].Quantity * scalingFactor;
                Ingredients[i].Calories = originalQuantitiesAndUnits[i].Calories * scalingFactor;

                ConvertUnits(Ingredients[i]);
            }

            Console.WriteLine("Recipe scaled successfully.");
        }

        /// <summary>
        /// Resets the quantities of the ingredients to their original values.
        /// </summary>
        public void ResetQuantities()
        {
            if (Ingredients.Count == 0)
            {
                Console.WriteLine("No ingredients available to reset.");
                return;
            }

            for (int i = 0; i < Ingredients.Count; i++)
            {
                Ingredients[i].Quantity = originalQuantitiesAndUnits[i].Quantity;
                Ingredients[i].Unit = originalQuantitiesAndUnits[i].Unit;
                Ingredients[i].Calories = originalQuantitiesAndUnits[i].Calories;

                ConvertUnits(Ingredients[i]);
            }

            Console.WriteLine("Quantities and calories reset to original values.");
        }

        /// <summary>
        /// Converts all the units to the correct format
        /// </summary>
        private void ConvertUnits(Ingredient ingredient)
        {
            if (ingredient.Unit == "g" && ingredient.Quantity >= 1000)
            {
                ingredient.Quantity /= 1000;
                ingredient.Unit = "kg";
            }
            else if (ingredient.Unit == "kg" && ingredient.Quantity < 1)
            {
                ingredient.Quantity *= 1000;
                ingredient.Unit = "g";
            }
            else if (ingredient.Unit == "mL" && ingredient.Quantity >= 1000)
            {
                ingredient.Quantity /= 1000;
                ingredient.Unit = "L";
            }
            else if (ingredient.Unit == "L" && ingredient.Quantity < 1)
            {
                ingredient.Quantity *= 1000;
                ingredient.Unit = "mL";
            }
            else if (ingredient.Unit == "tsp" && ingredient.Quantity >= 3)
            {
                ingredient.Quantity /= 3;
                ingredient.Unit = "tbsp";
            }
            else if (ingredient.Unit == "tbsp" && ingredient.Quantity < 1)
            {
                ingredient.Quantity *= 3;
                ingredient.Unit = "tsp";
            }
        }

        /// <summary>
        /// Calculates the total calories for the recipe
        /// </summary>
        public double CalculateTotalCalories()
        {
            return Ingredients.Sum(ingredient => ingredient.Calories);
        }
    }

    /// <summary>
    /// Represents an ingredient in a recipe.
    /// </summary>
    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }
    }
}
