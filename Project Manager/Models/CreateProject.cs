﻿#nullable disable

namespace Project_Manager.Models
{
    public class CreateProject
    {
        public virtual ICollection<Category> Category { get; set; } = new List<Category>();

        public virtual ICollection<Type> Type { get; set; } = new List<Type>();

        public virtual ICollection<Status> Status { get; set; } = new List<Status>();

        public virtual ICollection<Material> Material { get; set; } = new List<Material>();

    }
}
