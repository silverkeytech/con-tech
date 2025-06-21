namespace ConTech.Core.Features.Project;

public interface IProjectRepository
{
    Task<IQuerySetMany<ProjectView>> GetProjectListAsync();

    Task<IQuerySetMany<ProjectView>> GetProjectListByFilterAsync(IFilter<ProjectEntity> filter);

    Task<IQuerySetPaging<ProjectView>> GetProjectListByFilterAsync(ProjectFilter filter, ISort<ProjectEntity> sort);

    Task<IQuerySetOne<ProjectView?>> GetProjectByIdAsync(int id);

    Task<Result<ProjectEntity?>> CreateProjectAsync(ProjectNewInput input, IByUser by);

    Task<Result<ProjectEntity?>> UpdateProjectAsync(ProjectUpdateInput input, IByUser by);

    Task<Result<ProjectEntity?>> DisableProjectByIdAsync(int id, IByUser by);
}