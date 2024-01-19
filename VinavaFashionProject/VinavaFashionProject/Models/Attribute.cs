using System.Reflection;

namespace VinavaFashionProject.Models
{
    public class Attribute
    {
        public int Id { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }

        public static Color FromName(string colorName)
        {
            System.Drawing.Color systemColor = System.Drawing.Color.FromName(colorName);
            return new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
        }
    }

}