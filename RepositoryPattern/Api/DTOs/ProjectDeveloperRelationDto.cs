using System.Collections.Generic;
using RepositoryPattern.Models;

namespace RepositoryPattern.Api.DTOs

{
    public class ProjectDeveloperRelationDto
    {
        public Project Project { get; set; }
        public IEnumerable<Developer> Developers { get; set; }
    }
}