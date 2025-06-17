namespace ConTech.Core.Features.Project;

public class ProjectRepository(DataAccessAdapter adapter, IStringLocalizer<Global> local) : BaseRepository(adapter, local), IProjectRepository
{

    public async Task<IQuerySetPaging<ProjectView>> GetProjectListByFilterAsync(ProjectFilter filter, ISort<ProjectEntity> sort)
    {
        try
        {
            var pagingQuery = new SortedPagingQueryCondition<ProjectEntity>(filter, sort, filter.CurrentPage, filter.PageSize);
            var projectList = _meta.Project.Where(x => x.ObjectStatus == ObjectStatus.Active).AsQueryable();
            var query = QueryBuilder.Paging(projectList, pagingQuery, _meta);
            int count = await query.Count.CountAsync();
            var result = await query.Listing.ProjectToProjectView().ToListAsync();
            return QuerySet.Paging(result, count).GetPagingInfo(filter.CurrentPage, filter.PageSize);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return QuerySet.PagingException<ProjectView>(ex).GetPagingInfoException();
        }
    }

    public async Task<IQuerySetMany<ProjectView>> GetProjectListByFilterAsync(IFilter<ProjectEntity> filter)
    {
        try
        {
            var queryCondition = new QueryCondition<ProjectEntity>(filter);
            var queryable = _meta.Project.OrderByDescending(c => c.Id).AsQueryable();
            var query = QueryBuilder.Many(queryable, queryCondition, _meta);

            var results = await query.Listing.ProjectToProjectView().ToListAsync();
            return QuerySet.Many(results);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return QuerySet.Many<ProjectView>([]);
        }
    }

    public async Task<IQuerySetOne<ProjectView?>> GetProjectByIdAsync(int id)
    {
        try
        {
            var query = await _meta.Project.Where(x => x.Id == id).ProjectToProjectView().FirstOrDefaultAsync();
            return QuerySet.One(query);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return QuerySet.One<ProjectView>(null);
        }
    }

    public async Task<Result<ProjectEntity?>> CreateProjectAsync(ProjectNewInput input, IByUser by)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(input);
            ArgumentNullException.ThrowIfNull(by);

            var project = input.ToEntity(by);
            bool isOK = await _adapter.SaveEntityAsync(project, refetchAfterSave: true);

            if (isOK is false)
                throw new SaveOperationException(_local["exception-new-project-cannot-be-saved"], project.SaveOperationType());


            return Result<ProjectEntity?>.True(project);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return Result<ProjectEntity?>.False(ex);
        }
    }


    public async Task<Result<ProjectEntity?>> UpdateProjectAsync(ProjectUpdateInput input, IByUser by)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(input);
            ArgumentNullException.ThrowIfNull(by);

            var e = input.ToEntity(by);

            bool isOK = await _adapter.SaveEntityAsync(e, refetchAfterSave: true);

            if (isOK is false)
                throw new SaveOperationException(_local["exception-update-project-cannot-be-saved"], e.SaveOperationType());

            return Result<ProjectEntity?>.True(e);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return Result<ProjectEntity?>.False(ex);
        }
    }

    public async Task<Result<ProjectEntity?>> DisableProjectByIdAsync(int id, IByUser by)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(by);

            var e = new ProjectEntity
            {
                Id = id,
                ObjectStatus = ObjectStatus.Disabled,
                LastModifiedUtc = DateTime.UtcNow,
                LastModifiedByUserId = by.UserId,
                IsNew = false
            };

            var result = await _adapter.SaveEntityAsync(e, refetchAfterSave: true);

            if (result is false)
                return Result<ProjectEntity?>.False(_local["msg-project-not-disabled"]);

            return Result<ProjectEntity?>.True(e);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return Result<ProjectEntity?>.False(ex);
        }
    }

}