namespace Halwani.Data.Entities.User
{
    public class Role : Entity<long>
    {
        public string RoleName { get; set; }
        public string Permissions { get; set; }
    }
}
