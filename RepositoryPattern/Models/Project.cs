using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using RepositoryPattern.Api.DTOs;

namespace RepositoryPattern.Models
{
    [Table("Project")]
    public class Project : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<DeveloperProjectRelation> DeveloperProjectRelations { get; set; } = new List<DeveloperProjectRelation>();

        public IEnumerable<Developer> GetDevelopers()
        {
            return DeveloperProjectRelations.Select(rel => rel.Developer);
        }

        public ProjectDto ToDto()
        {
            return new ProjectDto()
            {
                Id = Id,
                Title = Title,
                Description = Description
            };
        }
    }
}