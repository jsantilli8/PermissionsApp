namespace Application.Dtos
{
    public class GetPermissionsResponse
    {
        public int Id { get; set; }
        public string EmployeeForeName { get; set; } = string.Empty;
        public string EmployeeSurName { get; set; } = string.Empty;
        public int PermissionTypeId { get; set; }
        public string PermissionTypeName { get; set; } = string.Empty; 
        public DateTime Date { get; set; }
    }
}
