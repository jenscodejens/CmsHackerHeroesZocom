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
        // Method to convert to Dictionary<string, string>
        public Dictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>();


            foreach (var item in Items)
            {
                dictionary[item.Key] = item.Value;
            }

            return dictionary;
        }
    }
}
