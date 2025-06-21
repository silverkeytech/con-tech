using ConTech.Data.Read.DtoClasses;

namespace ConTech.Core.Features.View;

public class ProjectViewRepository(DataAccessAdapter adapter, IStringLocalizer<Global> local) : BaseRepository(adapter, local), IProjectViewRepository
{

    public async Task<IQuerySetMany<ProjectViewLlblView>> GetProjectViewListAsync(int projectId)
    {
        try
        {
            var projectViewList = await _meta.ProjectView.Where(x => x.ObjectStatus == ObjectStatus.Active && x.ProjectId == projectId).ProjectToProjectViewLlblView().ToListAsync();
            return QuerySet.Many<ProjectViewLlblView>(projectViewList);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return QuerySet.ManyException<ProjectViewLlblView>(ex);
        }
    }

    public async Task<IQuerySetPaging<ProjectViewLlblView>> GetProjectViewListByFilterAsync(ProjectViewFilter filter, ISort<ProjectViewEntity> sort)
    {
        try
        {
            var pagingQuery = new SortedPagingQueryCondition<ProjectViewEntity>(filter, sort, filter.CurrentPage, filter.PageSize);
            var projectViewList = _meta.ProjectView.Where(x => x.ObjectStatus == ObjectStatus.Active).AsQueryable();
            var query = QueryBuilder.Paging(projectViewList, pagingQuery, _meta);
            int count = await query.Count.CountAsync();
            var result = await query.Listing.ProjectToProjectViewLlblView().ToListAsync();
            return QuerySet.Paging(result, count).GetPagingInfo(filter.CurrentPage, filter.PageSize);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return QuerySet.PagingException<ProjectViewLlblView>(ex).GetPagingInfoException();
        }
    }

    public async Task<IQuerySetMany<ProjectViewLlblView>> GetProjectViewListByFilterAsync(IFilter<ProjectViewEntity> filter)
    {
        try
        {
            var queryCondition = new QueryCondition<ProjectViewEntity>(filter);
            var queryable = _meta.ProjectView.OrderByDescending(c => c.Id).AsQueryable();
            var query = QueryBuilder.Many(queryable, queryCondition, _meta);

            var results = await query.Listing.ProjectToProjectViewLlblView().ToListAsync();
            return QuerySet.Many(results);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return QuerySet.Many<ProjectViewLlblView>([]);
        }
    }

    public async Task<IQuerySetOne<ProjectViewLlblView?>> GetProjectViewByIdAsync(int id)
    {
        try
        {
            var query = await _meta.ProjectView.Where(x => x.Id == id).ProjectToProjectViewLlblView().FirstOrDefaultAsync();
            return QuerySet.One(query);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return QuerySet.One<ProjectViewLlblView>(null);
        }
    }

    public async Task<Result<ProjectViewEntity?>> CreateProjectViewAsync(ProjectViewNewInput input, IByUser by)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(input);
            ArgumentNullException.ThrowIfNull(by);

            var project = input.ToEntity(by);
            bool isOK = await _adapter.SaveEntityAsync(project, refetchAfterSave: true);

            if (isOK is false)
                throw new SaveOperationException(_local["exception-new-project-cannot-be-saved"], project.SaveOperationType());


            return Result<ProjectViewEntity?>.True(project);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return Result<ProjectViewEntity?>.False(ex);
        }
    }


    public async Task<Result<ProjectViewEntity?>> UpdateProjectViewAsync(ProjectViewUpdateInput input, IByUser by)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(input);
            ArgumentNullException.ThrowIfNull(by);

            var e = input.ToEntity(by);

            bool isOK = await _adapter.SaveEntityAsync(e, refetchAfterSave: true);

            if (isOK is false)
                throw new SaveOperationException(_local["exception-update-project-cannot-be-saved"], e.SaveOperationType());

            return Result<ProjectViewEntity?>.True(e);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return Result<ProjectViewEntity?>.False(ex);
        }
    }

    public async Task<Result<ProjectViewEntity?>> DisableProjectViewByIdAsync(int id, IByUser by)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(by);

            var e = new ProjectViewEntity
            {
                Id = id,
                ObjectStatus = ObjectStatus.Disabled,
                LastModifiedUtc = DateTime.UtcNow,
                LastModifiedByUserId = by.UserId,
                IsNew = false
            };

            var result = await _adapter.SaveEntityAsync(e, refetchAfterSave: true);

            if (result is false)
                return Result<ProjectViewEntity?>.False(_local["msg-project-not-disabled"]);

            return Result<ProjectViewEntity?>.True(e);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return Result<ProjectViewEntity?>.False(ex);
        }
    }

}