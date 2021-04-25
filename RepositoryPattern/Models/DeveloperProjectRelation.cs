using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryPattern.Models
{
    [Table("DeveloperProjectRelation")]
    public class DeveloperProjectRelation : BaseEntity
    {
        public Guid DeveloperId { get; set; }
        public Developer Developer { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}