namespace RecipeWebScraper.Arla
{
    public static partial class RecipeLinksScraping
    {
        private static readonly string ArlaRecipesBaseUrl = "https://www.arla.dk/opskrifter";

        private static readonly string projectPath = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName; //TODO: posibly rework
        private static readonly string DirName = "Data";

        private static readonly string LinksFileName = "ArlaRecepies.txt";
        public static readonly string RecipeLinksPath = Path.Combine(projectPath, DirName, LinksFileName);

        private static readonly string RawRecipesCSVFileName = "RawRecipes.csv";
        public static readonly string RawRecipesPath = Path.Combine(projectPath, DirName, RawRecipesCSVFileName);
    }
}
