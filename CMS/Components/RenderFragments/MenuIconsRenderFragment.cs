using Microsoft.AspNetCore.Components;

namespace CMS.Components.RenderFragments
{
    public static class MenuIconsRenderFragment
    {
        public static RenderFragment MenuIcons => builder =>
        {
            // Wbsite card icons starting from left to right. 
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "class", "material-icons");
            builder.AddAttribute(2, "style", "font-size: 36px;");
            builder.AddAttribute(3, "title", "Add image");
            builder.AddContent(4, "image");
            builder.CloseElement();

            builder.OpenElement(5, "button");
            builder.AddAttribute(6, "class", "material-icons");
            builder.AddAttribute(7, "style", "font-size: 36px;");
            builder.AddAttribute(8, "title", "Add a list");
            builder.AddContent(9, "list");
            builder.CloseElement();

            builder.OpenElement(10, "button");
            builder.AddAttribute(11, "class", "material-icons");
            builder.AddAttribute(12, "style", "font-size: 36px;");
            builder.AddAttribute(13, "title", "Add a text");
            builder.AddContent(14, "description");
            builder.CloseElement();
        };
    }
}
