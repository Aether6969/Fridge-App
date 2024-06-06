namespace RecipeWebScraper.Arla
{
    public static partial class RecipeLinksScraping
    {
        private static readonly string ArlaRecipesBaseUrl = "https://www.arla.dk/opskrifter";

        private static readonly string projectPath = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName; //TODO: posibly rework
        private static readonly string DirName = "Data";

        private static readonly string RecipeLinksFileName = "ArlaRecepies.txt";
        public static readonly string RecipeLinksPath = Path.Combine(projectPath, DirName, RecipeLinksFileName);

        private static readonly string RawRecipesCSVFileName = "RawRecipes.csv";
        public static readonly string RawRecipesPath = Path.Combine(projectPath, DirName, RawRecipesCSVFileName);

        private static readonly string CleanRecipesCSVFileName = "CleanRecipes.csv";
        public static readonly string CleanRecipesPath = Path.Combine(projectPath, DirName, CleanRecipesCSVFileName);

        private static readonly string CleanRecipeIngrediantsCSVFileName = "CleanRecipeIngrediants.csv";
        public static readonly string CleanRecipeIngrediantsPath = Path.Combine(projectPath, DirName, CleanRecipeIngrediantsCSVFileName);


        private static readonly string IngrediantsFileName = "BilkaIngrediants.txt";
        public static readonly string IngrediantsPath = Path.Combine(projectPath, DirName, IngrediantsFileName);
    }
}
