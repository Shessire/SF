namespace SF.Models
{
    public class RoleGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<RoleGroupRoles> RoleGroupRoles { get; set; }
    }
}
