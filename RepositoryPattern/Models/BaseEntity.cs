using System;
using Newtonsoft.Json;

namespace RepositoryPattern.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        [JsonIgnore] public bool IsActive { get; set; } = true;

        [JsonIgnore] public DateTime CreatedOn { get; set; } = DateTime.Now;
        [JsonIgnore] public string CreatedBy { get; set; }
        [JsonIgnore] public DateTime LastModifiedOn { get; set; } = DateTime.Now;
        [JsonIgnore] public string LastModifiedBy { get; set; }
    }
}