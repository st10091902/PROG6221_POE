namespace RecipeApplication
{
    
    class Recipe
    {
        private string[] ingredients;
        private double[] quantities;
        private string[] units;
        private string[] steps;

        public void EnterDetails()
        {
            Console.Write("Enter the number of ingredients: ");
            int numIngredients = int.Parse(Console.ReadLine());
            ingredients = new string[numIngredients];
            quantities = new double[numIngredients];
            units = new string[numIngredients];
        }
    }
















    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
