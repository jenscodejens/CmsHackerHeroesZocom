namespace CMS.Shared
{
    public class MenuItemsWrapper
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Items { get; set; }
        public MenuItemsWrapper(string name, string type, Dictionary<string, string> items)
        {
            Name = name;
            Type = type;
            Items = items;
        }
    }
}
