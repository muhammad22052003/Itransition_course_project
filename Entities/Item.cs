﻿using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CourseProject_backend.Entities
{
    [Index(nameof(Name))]
    [Index(nameof(CreatedTime))]
    public class Item : IDBModel
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string? ImageUrl { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }
        [Required]
        public MyCollection Collection { get; set; }
        [Required]
        public ICollection<PositiveReaction> PositiveReact { get; set; }
        [Required]
        public ICollection<Comment> Comments { get; set; }
        [Required] 
        public ICollection<ViewModel> Views { get; set; }
        [Required]
        public ICollection<Tag> Tags { get; set; }
        public string? CustomString1 { get; set; }
        public string? CustomString2 { get; set; }
        public string? CustomString3 { get; set; }
        public string? CustomText1 { get; set; }
        public string? CustomText2 { get; set; }
        public string? CustomText3 { get; set; }
        public int? CustomInt1 { get; set; }
        public int? CustomInt2 { get; set; }
        public int? CustomInt3 { get; set; }
        public bool? CustomBool1 { get; set; }
        public bool? CustomBool2 { get; set; }
        public bool? CustomBool3 { get; set; }
        public DateTime? CustomDate1 { get; set; }
        public DateTime? CustomDate2 { get; set; }
        public DateTime? CustomDate3 { get; set; }

        public NpgsqlTsVector SearchVector { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
