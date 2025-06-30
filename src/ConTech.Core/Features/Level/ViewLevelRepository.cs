using ConTech.Core.Features.View;
using ConTech.Data.Read.DtoClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ConTech.Core.Features.Level;

public class ViewLevelRepository(DataAccessAdapter adapter, IStringLocalizer<Global> local) : BaseRepository(adapter, local), IViewLevelRepository
{

    public async Task<IQuerySetMany<ViewLevelListView>> GetViewLevelListAsync(int viewId)
    {
        try
        {
            var projectViewList = await _meta.ViewLevel.Where(x => x.ObjectStatus == ObjectStatus.Active && x.ViewId == viewId).ProjectToViewLevelListView().ToListAsync();
            return QuerySet.Many<ViewLevelListView>(projectViewList);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return QuerySet.ManyException<ViewLevelListView>(ex);
        }
    }

    public async Task<IQuerySetPaging<ViewLevelListView>> GetViewLevelListByFilterAsync(ViewLevelFilter filter, ISort<ViewLevelEntity> sort)
    {
        try
        {
            var pagingQuery = new SortedPagingQueryCondition<ViewLevelEntity>(filter, sort, filter.CurrentPage, filter.PageSize);
            var projectViewList = _meta.ViewLevel.Where(x => x.ObjectStatus == ObjectStatus.Active).AsQueryable();
            var query = QueryBuilder.Paging(projectViewList, pagingQuery, _meta);
            int count = await query.Count.CountAsync();
            var result = await query.Listing.ProjectToViewLevelListView().ToListAsync();
            return QuerySet.Paging(result, count).GetPagingInfo(filter.CurrentPage, filter.PageSize);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return QuerySet.PagingException<ViewLevelListView>(ex).GetPagingInfoException();
        }
    }

    public async Task<IQuerySetMany<ViewLevelListView>> GetViewLevelListByFilterAsync(IFilter<ViewLevelEntity> filter)
    {
        try
        {
            var queryCondition = new QueryCondition<ViewLevelEntity>(filter);
            var queryable = _meta.ViewLevel.OrderByDescending(c => c.Id).AsQueryable();
            var query = QueryBuilder.Many(queryable, queryCondition, _meta);

            var results = await query.Listing.ProjectToViewLevelListView().ToListAsync();
            return QuerySet.Many(results);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return QuerySet.Many<ViewLevelListView>([]);
        }
    }

    public async Task<IQuerySetOne<ViewLevelView?>> GetViewLevelByIdAsync(string id)
    {
        try
        {
            var query = await _meta.ViewLevel.Where(x => x.Id == new Guid(id)).ProjectToViewLevelView().FirstOrDefaultAsync();
            return QuerySet.One(query);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return QuerySet.One<ViewLevelView>(null);
        }
    }

    public async Task<Result<ViewLevelEntity?>> CreateViewLevelAsync(ViewLevelNewInput input)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(input);
            //ArgumentNullException.ThrowIfNull(by);

            var e = input.ToEntity();

            using (var memoryStream = new MemoryStream())
            {
                await input.DxfFile.CopyToAsync(memoryStream);

                e.DxfFile = memoryStream.ToArray();


                //var document = new PdfDocument
                //{
                //    FileContent = memoryStream.ToArray(),
                //    FileName = PdfDocument.PdfFile.FileName,
                //    ContentType = PdfDocument.PdfFile.ContentType,
                //    FileSize = PdfDocument.PdfFile.Length,
                //};
            }

            using (var memoryStream = new MemoryStream())
            {
                await input.ExcelFile.CopyToAsync(memoryStream);

                e.ExcelFile = memoryStream.ToArray();


                //var document = new PdfDocument
                //{
                //    FileContent = memoryStream.ToArray(),
                //    FileName = PdfDocument.PdfFile.FileName,
                //    ContentType = PdfDocument.PdfFile.ContentType,
                //    FileSize = PdfDocument.PdfFile.Length,
                //};
            }

            bool isOK = await _adapter.SaveEntityAsync(e, refetchAfterSave: true);

            if (isOK is false)
                throw new SaveOperationException(_local["exception-new-view-cannot-be-saved"], e.SaveOperationType());


            return Result<ViewLevelEntity?>.True(e);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return Result<ViewLevelEntity?>.False(ex);
        }
    }


    public async Task<Result<ViewLevelEntity?>> UpdateViewLevelAsync(ViewLevelUpdateInput input)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(input);
            //ArgumentNullException.ThrowIfNull(by);

            var e = input.ToEntity();

            using (var memoryStream = new MemoryStream())
            {
                await input.DxfFile.CopyToAsync(memoryStream);

                e.DxfFile = memoryStream.ToArray();


                //var document = new PdfDocument
                //{
                //    FileContent = memoryStream.ToArray(),
                //    FileName = PdfDocument.PdfFile.FileName,
                //    ContentType = PdfDocument.PdfFile.ContentType,
                //    FileSize = PdfDocument.PdfFile.Length,
                //};
            }

            using (var memoryStream = new MemoryStream())
            {
                await input.ExcelFile.CopyToAsync(memoryStream);

                e.ExcelFile = memoryStream.ToArray();


                //var document = new PdfDocument
                //{
                //    FileContent = memoryStream.ToArray(),
                //    FileName = PdfDocument.PdfFile.FileName,
                //    ContentType = PdfDocument.PdfFile.ContentType,
                //    FileSize = PdfDocument.PdfFile.Length,
                //};
            }


            bool isOK = await _adapter.SaveEntityAsync(e, refetchAfterSave: true);

            if (isOK is false)
                throw new SaveOperationException(_local["exception-update-project-cannot-be-saved"], e.SaveOperationType());

            return Result<ViewLevelEntity?>.True(e);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return Result<ViewLevelEntity?>.False(ex);
        }
    }

    public async Task<Result<ViewLevelEntity?>> DisableViewLevelByIdAsync(Guid id)
    {
        try
        {
           // ArgumentNullException.ThrowIfNull(by);

            var e = new ViewLevelEntity
            {
                Id = id,
                ObjectStatus = ObjectStatus.Disabled,
                LastModifiedUtc = DateTime.UtcNow,
                //LastModifiedByUserId = by.UserId,
                IsNew = false
            };

            var result = await _adapter.SaveEntityAsync(e, refetchAfterSave: true);

            if (result is false)
                return Result<ViewLevelEntity?>.False(_local["msg-project-not-disabled"]);

            return Result<ViewLevelEntity?>.True(e);
        }
        catch (Exception ex)
        {
            CodeTemplate.HandleException(ex);
            return Result<ViewLevelEntity?>.False(ex);
        }
    }

}