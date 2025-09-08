namespace uxcomex.Presentation.ViewModels
{
    public class DataTableViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public string CreateUrl { get; set; } = string.Empty;
        public string EditUrlTemplate { get; set; } = string.Empty;
        public string DetailUrlTemplate { get; set; } = string.Empty;
        public string DeleteHandler { get; set; } = string.Empty;
        public string[] Columns { get; set; } = Array.Empty<string>();
        public bool? AllowEdit { get; set; } = null;
        public List<DataTableItem> Items { get; set; } = new();
    }

    public class DataTableItem
    {
        public Guid Id { get; set; }
        public string[] Values { get; set; } = Array.Empty<string>();
    }
}
