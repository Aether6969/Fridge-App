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
        public (string, int)[] IngrediantsAmount;

        public override string ToString()
        {
            return $"({Name}, {Enum.GetName(RecipeType)}, {TotalTimeMin}, {IsFreezable}, {Rating}%)";
        }
    }
}
