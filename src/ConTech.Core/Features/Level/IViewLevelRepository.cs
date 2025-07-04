﻿namespace ConTech.Core.Features.Level;

public interface IViewLevelRepository
{
    Task<IQuerySetMany<ViewLevelListView>> GetViewLevelListAsync(int viewId);

    Task<IQuerySetMany<ViewLevelListView>> GetViewLevelListByFilterAsync(IFilter<ViewLevelEntity> filter);

    Task<IQuerySetPaging<ViewLevelListView>> GetViewLevelListByFilterAsync(ViewLevelFilter filter, ISort<ViewLevelEntity> sort);

    Task<IQuerySetOne<ViewLevelView?>> GetViewLevelByIdAsync(string id);

    Task<Result<ViewLevelEntity?>> CreateViewLevelAsync(ViewLevelNewInput input);

    Task<Result<ViewLevelEntity?>> UpdateViewLevelAsync(ViewLevelUpdateInput input);
    Task<Result<ViewLevelEntity?>> UpdateViewLevelTransitionAsync(ViewLevelUpdateTransitionInput input);
    Task<Result<LevelChildEntity?>> AddLevelChildAsync(LevelChildNewInput input);
    Task<Result<LevelChildEntity?>> UpdateLevelChildAsync(LevelChildUpdateInput input);

    Task<Result<ViewLevelEntity?>> DisableViewLevelByIdAsync(Guid id);
    Task<Result<LevelChildEntity?>> DisableLevelChildByIdAsync(Guid id);
}