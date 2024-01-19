using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Utilities
{
    public class AttributeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ColorTemplate { get; set; }
        public DataTemplate SizeTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ProductAttribute pa)
            {
                if (pa.Attribute.AttributeName == "color")
                {
                    return ColorTemplate;
                }
                else if (pa.Attribute.AttributeName == "size")
                {
                    return SizeTemplate;
                }
            }
            return null;
        }
    }
}
