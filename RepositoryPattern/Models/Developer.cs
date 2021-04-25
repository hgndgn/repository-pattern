using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using RepositoryPattern.Api.DTOs;

namespace RepositoryPattern.Models
{
    [Table("Developer")]
    public class Developer : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public virtual ICollection<DeveloperProjectRelation> DeveloperProjectRelations { get; set; } = new List<DeveloperProjectRelation>();

        public IEnumerable<Project> GetProjects()
        {
            return DeveloperProjectRelations.Select(rel => rel.Project);
        }

        public DeveloperDto ToDto()
        {
            return new DeveloperDto()
            {
                Id = Id,
                Email = Email,
                Username = Username
            };
        }
    }
}