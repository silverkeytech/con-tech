using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace ConTech.Core.Features.Project;

public class ProjectNewInput
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public ProjectEntity ToEntity(IByUser by)
    {
        var e = new ProjectEntity
        {
            ObjectStatus = ObjectStatus.Active,
            CreatedByUserId = by.UserId,
            DateCreatedUtc = DateTime.UtcNow,
            IsNew = true
        };

        return e;
    }


    public class Validator : AbstractValidator<ProjectNewInput>
    {
        public Validator(IStringLocalizer<Global> local)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(local["validate-project-name-required"]);
        }
    }
}

public class ProjectUpdateInput
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Description { get; set; }

    public ProjectEntity ToEntity(IByUser by)
    {
        var e = new ProjectEntity
        {
            Name = Name,
            Description = Description,
            LastModifiedByUserId = by.UserId,
            LastModifiedUtc = Stamp.DateTimeUtc(),
            IsNew = false
        };

        return e;
    }

    public class Validator : AbstractValidator<ProjectUpdateInput>
    {
        public Validator(IStringLocalizer<Global> local)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(local["validate-project-name-required"]);
        }
    }
}


public class ProjectFilter : IFilter<ProjectEntity>
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = Constants.CommonPageSize;

    public IQueryable<ProjectEntity> Filter(IQueryable<ProjectEntity> query, LinqMetaData? meta = null)
    {
        

        return query;
    }
}
