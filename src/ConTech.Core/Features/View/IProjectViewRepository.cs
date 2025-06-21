namespace ConTech.Core.Features.View;

public interface IProjectViewRepository
{
    Task<IQuerySetMany<ProjectViewLlblView>> GetProjectViewListAsync(int projectId);

    Task<IQuerySetMany<ProjectViewLlblView>> GetProjectViewListByFilterAsync(IFilter<ProjectViewEntity> filter);

    Task<IQuerySetPaging<ProjectViewLlblView>> GetProjectViewListByFilterAsync(ProjectViewFilter filter, ISort<ProjectViewEntity> sort);

    Task<IQuerySetOne<ProjectViewLlblView?>> GetProjectViewByIdAsync(int id);

    Task<Result<ProjectViewEntity?>> CreateProjectViewAsync(ProjectViewNewInput input, IByUser by);

    Task<Result<ProjectViewEntity?>> UpdateProjectViewAsync(ProjectViewUpdateInput input, IByUser by);

    Task<Result<ProjectViewEntity?>> DisableProjectViewByIdAsync(int id, IByUser by);
}