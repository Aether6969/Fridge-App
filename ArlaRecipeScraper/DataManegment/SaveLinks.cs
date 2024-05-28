using System.Text;

namespace ArlaRecipeScraper
{
    public static partial class DataManegement
    {
        public static void SaveLinks(string path, string[] links)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < links.Length; i++)
            {
                string e = links[i];
                sb.AppendLine(e);
            }
            File.WriteAllText(path, sb.ToString());
        }
    }
}
