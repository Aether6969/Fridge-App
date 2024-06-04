namespace FrigeCore.Structures
{
    public struct Recipe
    {
        public string Name;
        public string Link;
        public RecipeType RecipeType;
        public int TotalTimeMin;
        public bool IsFreezable;
        public int Rating;
        public string ImageLink;
        public RecipeIngredient[] IngrediantsAmount;
    }
}
