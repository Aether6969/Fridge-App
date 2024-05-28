using FrigeCore.Structures;
using System.Linq;
using System.Reflection;

namespace ArlaRecipeScraper
{
    public struct RecipeSurrogate //TODO: add static check to insure corrospondace to tests
    {
        public string Name;
        public string Link;
        public string RecipeType;
        public string TotalTimeMin;
        public string IsFreezable;
        public string Rating;
        public string IngrediantsAmount;
        public string ImageLink;
        public string EnergyKj;
        public string NutritionalInfo;
    }
}
