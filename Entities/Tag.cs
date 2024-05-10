using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crmf;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CourseProject_backend.Entities
{
    [Index(nameof(Name))]
    public class Tag : IDBModel
    {
        public Tag()
        {
            
        }

        public Tag(string name, Item item)
        {
            Id = Guid.NewGuid().ToString().Replace("-", "");
            Name = name;
            Item = item;
        }

        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Item Item { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
