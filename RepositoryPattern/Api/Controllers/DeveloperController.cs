using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.Api.DTOs;
using RepositoryPattern.Api.Requests;
using RepositoryPattern.Models;
using RepositoryPattern.Pagination;
using RepositoryPattern.Services;

namespace RepositoryPattern.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]s")]
    [ApiVersion("1.0")]
    [ApiController]
    public class DeveloperController : ControllerBase
    {
        private readonly IDeveloperService _developerService;

        public DeveloperController(IDeveloperService developerService)
        {
            _developerService = developerService;
        }

        [HttpGet]
        public IActionResult FindAll([FromQuery] PagingParameters pagingParameters)
        {
            pagingParameters.WithRoute(Request.Path.Value!);

            var allDevelopers = _developerService.FindAll().ToList();
            var paginatedDevelopers = PaginationUtils.Paginate(allDevelopers, pagingParameters);
            var converted = PaginationUtils.ConvertItems(paginatedDevelopers, developer => developer.ToDto());

            return StatusCode(StatusCodes.Status200OK, converted);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Create([FromBody] CreateOrUpdateDeveloperRequest createDeveloper)
        {
            // TODO: validate properties of createDeveloper
            var developer = new Developer()
            {
                Email = createDeveloper.Email,
                Username = createDeveloper.Username
            };
            var created = _developerService.Add(developer);
            return StatusCode(StatusCodes.Status201Created, created.ToDto());
        }

        [HttpGet]
        [Route("{developerId}/projects")]
        public IActionResult GetProjects([FromRoute] Guid developerId, [FromQuery] bool includeDevelopers = false)
        {
            var developer = _developerService.FindById(developerId);

            if (developer == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            if (includeDevelopers)
            {
                return StatusCode(StatusCodes.Status201Created, GetProjectsWithDevelopers(developerId));
            }

            var projects = developer.DeveloperProjectRelations.Where(rel => rel.DeveloperId.Equals(developerId));

            return StatusCode(StatusCodes.Status201Created, projects);
        }

        private List<ProjectDeveloperRelationDto> GetProjectsWithDevelopers([FromRoute] Guid developerId)
        {
            var developer = _developerService.FindById(developerId);
            var relations = new List<ProjectDeveloperRelationDto>();

            developer.DeveloperProjectRelations.ToList().ForEach(rel =>
            {
                if (rel.DeveloperId.Equals(developerId))
                {
                    relations.Add(new ProjectDeveloperRelationDto()
                    {
                        Project = rel.Project,
                        Developers = rel.Project.GetDevelopers()
                    });
                }
            });

            return relations;
        }
    }
}