namespace CMS.Models
{
    public class Template
    {
        public int TemplateId { get; set; }       // ID for the template
        public string? TemplateType { get; set; }  // Type of template (header, body, footer)
        public string? TemplatePath { get; set; }  // Path to the Razor file
        public string? InputFormPath { get; set; } //Path to which input form we need to use
    }
}
