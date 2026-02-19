namespace ZetaSaasHRMSBackend.CustomModels
{
    public class RoleRequest
    {
        public long roleId { get; set; }
        public required string RoleName { get; set; }
        public string? Description { get; set; }
        public List<RoleMenuRightRequest> MenuRights { get; set; } = new();
    }

    public class RoleMenuRightRequest
    {
        public long MenuId { get; set; }

        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
