namespace Application.Dtos
{
    public class AddPermissionRequest
    {
        public string EmployeeForeName { get; set; } = string.Empty;
        public string EmployeeSurName { get; set; } = string.Empty;
        public int PermissionTypeId { get; set; }
    }
}
