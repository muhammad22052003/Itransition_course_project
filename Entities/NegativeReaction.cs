﻿using CourseProject_backend.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Entities
{
    public class NegativeReaction : IReaction
    {
        public NegativeReaction()
        {
            
        }

        public NegativeReaction(string name, User user, Item item, string iconUrl = "")
        {
            Id = Guid.NewGuid().ToString().Replace("-", "");
            Name = name;
            User = user;
            Item = item;
            IconUrl = iconUrl;
        }

        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string IconUrl { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Item Item { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
