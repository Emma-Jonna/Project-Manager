#nullable disable

namespace Project_Manager.Models
{
    public class EditProject
    {
        public virtual ICollection<Category> Category { get; set; } = new List<Category>();

        public virtual ICollection<Type> Type { get; set; } = new List<Type>();

        public virtual ICollection<Status> Status { get; set; } = new List<Status>();

        public virtual Project Project { get; set; }
    }
}
