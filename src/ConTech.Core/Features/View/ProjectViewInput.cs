using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ConTech.Core.Features.View;

public class ProjectViewNewInput
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int ProjectId { get; set; }

    public IFormFile PdfFile { get; set; }

    public byte[]? FileContent { get; set; }
    //public string FileName { get; set; }
    //public string ContentType { get; set; }
    //public DateTime UploadDate { get; set; }
    //public long FileSize { get; set; }
    public ProjectViewEntity ToEntity(IByUser by)
    {
        var e = new ProjectViewEntity
        {
            Name = Name,
            Description = Description,
            ProjectId = ProjectId,
            ObjectStatus = ObjectStatus.Active,
            //CreatedByUserId = by.UserId,
            DateCreatedUtc = DateTime.UtcNow,
            IsNew = true
        };

        return e;
    }


    public class Validator : AbstractValidator<ProjectViewNewInput>
    {
        public Validator(IStringLocalizer<Global> local)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(local["validate-project-name-required"]);
        }
    }
}

public class ProjectViewUpdateInput
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public IFormFile PdfFile { get; set; }

    public byte[]? FileContent { get; set; }

    public ProjectViewUpdateInput()
    {
            
    }

    public ProjectViewUpdateInput(ProjectViewLlblView v)
    {
        Name = v.Name;
        Description = v.Description;
        Id = v.Id;
        ProjectId = v.ProjectId;
    }

    public ProjectViewEntity ToEntity(IByUser by)
    {
        var e = new ProjectViewEntity
        {
            Id = Id,
            Name = Name,
            Description = Description,
            //LastModifiedByUserId = by.UserId,
            LastModifiedUtc = Stamp.DateTimeUtc(),
            IsNew = false
        };

        return e;
    }

    public class Validator : AbstractValidator<ProjectViewUpdateInput>
    {
        public Validator(IStringLocalizer<Global> local)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(local["validate-project-name-required"]);
        }
    }
}


public class ProjectViewFilter : IFilter<ProjectViewEntity>
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = Constants.CommonPageSize;

    public IQueryable<ProjectViewEntity> Filter(IQueryable<ProjectViewEntity> query, LinqMetaData? meta = null)
    {
        

        return query;
    }
}
