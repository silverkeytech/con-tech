using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static System.Collections.Specialized.BitVector32;

namespace ConTech.Core.Features.Level;

public class ViewLevelNewInput
{
    public string? Description { get; set; }
    public int ViewId { get; set; }
    //public string? LevelId { get; set; }
    public string? LevelName { get; set; }
    public int LevelScale { get; set; }
    public List<FileInfo> FileInfo { get; set; } = new();

    public IFormFile DxfFile { get; set; }
    public byte[]? DxfContent { get; set; }

    public IFormFile ExcelFile { get; set; }
    public byte[]? ExcelContent { get; set; }

    //public string FileName { get; set; }
    //public string ContentType { get; set; }
    //public DateTime UploadDate { get; set; }
    //public long FileSize { get; set; }
    public ViewLevelEntity ToEntity()
    {
        var e = new ViewLevelEntity
        {
            Id = Guid.NewGuid(),
            Name = LevelName,
            Description = Description,
            ViewId = ViewId,
            ObjectStatus = ObjectStatus.Active,
            //CreatedByUserId = by.UserId,
            DateCreatedUtc = DateTime.UtcNow,
            IsNew = true
        };

        return e;
    }


    public class Validator : AbstractValidator<ViewLevelNewInput>
    {
        public Validator(IStringLocalizer<Global> local)
        {
            RuleFor(x => x.LevelName).NotNull().WithMessage(local["validate-project-name-required"]);
        }
    }
}

public class ViewLevelUpdateInput
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int ViewId { get; set; }
    public string? LevelId { get; set; }
    public string? LevelName { get; set; }
    public int LevelScale { get; set; }
    public List<FileInfo> FileInfo { get; set; } = new();

    public IFormFile DxfFile { get; set; }
    public byte[]? DxfContent { get; set; }

    public IFormFile ExcelFile { get; set; }
    public byte[]? ExcelContent { get; set; }

    public ViewLevelUpdateInput()
    {

    }

    public ViewLevelUpdateInput(ViewLevelView v)
    {
        Name = v.Name;
        Description = v.Description;
        Id = v.Id.ToString();
        ViewId = v.ViewId;
    }

    public ViewLevelEntity ToEntity(IByUser by)
    {
        var e = new ViewLevelEntity
        {
            Id = new Guid(Id),
            Name = Name,
            Description = Description,
            //LastModifiedByUserId = by.UserId,
            LastModifiedUtc = Stamp.DateTimeUtc(),
            IsNew = false
        };

        return e;
    }

    public class Validator : AbstractValidator<ViewLevelUpdateInput>
    {
        public Validator(IStringLocalizer<Global> local)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(local["validate-project-name-required"]);
        }
    }
}


public class ViewLevelFilter : IFilter<ViewLevelEntity>
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = Constants.CommonPageSize;

    public IQueryable<ViewLevelEntity> Filter(IQueryable<ViewLevelEntity> query, LinqMetaData? meta = null)
    {


        return query;
    }
}

public class FileInfo
{

    public string? OriginalName { get; set; }
    public long Size { get; set; }
    public string? Type { get; set; }
}