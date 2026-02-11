namespace ZetaSaasHRMSBackend.CustomModels
{
    public class MenuResponse
    {
        public long MenuId { get; set; }
        public string MenuName { get; set; } = null!;
        public string? RouteUrl { get; set; }
        public string? Icon { get; set; }
        public long? ParentMenuId { get; set; }
        public decimal DisplayOrder { get; set; }

        // permissions (NOT DB columns)
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
