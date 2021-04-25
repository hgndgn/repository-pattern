using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.Api.Requests;
using RepositoryPattern.Models;
using RepositoryPattern.Pagination;
using RepositoryPattern.Services;

namespace RepositoryPattern.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]s")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public IActionResult FindAll([FromQuery] PagingParameters pagingParameters)
        {
            pagingParameters.WithRoute(Request.Path.Value!);

            var allProjects = _projectService.FindAll().ToList();
            var paginatedProjects = PaginationUtils.Paginate(allProjects, pagingParameters);
            var converted = PaginationUtils.ConvertItems(paginatedProjects, project => project.ToDto());

            return StatusCode(StatusCodes.Status200OK, converted);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Create([FromBody] CreateOrUpdateProjectRequest createProject)
        {
            // TODO: validate properties of createProject
            var project = new Project()
            {
                Title = createProject.Title,
                Description = createProject.Description
            };
            var created = _projectService.Add(project);
            return StatusCode(StatusCodes.Status201Created, created.ToDto());
        }

        [HttpGet]
        [Route("{projectId}/developers")]
        public IActionResult GetDevelopers([FromRoute] Guid projectId, [FromQuery] PagingParameters pagingParameters)
        {
            var project = _projectService.FindById(projectId);

            if (project == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            pagingParameters.WithRoute(Request.Path.Value!);

            var filtered = project.DeveloperProjectRelations.Where(rel => rel.ProjectId.Equals(projectId));
            var developers = filtered.Select(f => f.Developer);
            var paginated = PaginationUtils.Paginate(developers.ToList(), pagingParameters);
            return StatusCode(StatusCodes.Status201Created, paginated);
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] CreateOrUpdateProjectRequest updateProject)
        {
            var existingProject = _projectService.FindById(id);

            if (existingProject == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            // TODO: validate properties of updateProject
            existingProject.Title = updateProject.Title;
            existingProject.Description = updateProject.Description;
            existingProject.LastModifiedOn = DateTime.Now;
            existingProject = _projectService.Update(existingProject);

            return StatusCode(StatusCodes.Status200OK, existingProject.ToDto());
        }
    }
}