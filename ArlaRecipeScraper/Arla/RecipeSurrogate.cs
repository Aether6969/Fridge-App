using FrigeCore.Structures;
using System.Reflection;

namespace RecipeWebScraper.Arla
{
    public struct RecipeSurrogate
    {
        public string Name;
        public string Link;
        public string RecipeType;
        public string TotalTimeMin;
        public string IsFreezable;
        public string Rating;
        public string ImageLink;
        public string IngrediantsAmount;

        static RecipeSurrogate()
        {
            FieldInfo[] surrogateFields = typeof(RecipeSurrogate).GetFields(BindingFlags.Instance | BindingFlags.Public);
            FieldInfo[] fields = typeof(Recipe).GetFields(BindingFlags.Instance | BindingFlags.Public);

            if (surrogateFields.Length != fields.Length)
            {
                throw new Exception(); //TODO:
            }

            for(int i = 0; i < surrogateFields.Length; i++)
            {
                FieldInfo surrogateField = surrogateFields[i];
                FieldInfo filed = fields[i];

                if (filed.Name != surrogateField.Name)
                {
                    throw new Exception(); //TODO:
                }
            }
        }
    }
}
