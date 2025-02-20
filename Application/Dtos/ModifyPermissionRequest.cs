namespace Application.Dtos
{
    public class ModifyPermissionRequest
    {
        public int Id { get; set; }
        public string EmployeForeName { get; set; } = string.Empty;
        public string EmployeeSurName { get; set; } = string.Empty;
        public int PermissionTypeId { get; set; }
    }
}
