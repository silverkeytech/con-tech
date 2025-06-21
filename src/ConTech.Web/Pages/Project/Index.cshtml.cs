using ConTech.Core;
using ConTech.Core.Features.Project;
using ConTech.Data.Read.DtoClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ConTech.Web.Pages.Project;

public class IndexModel : PageModel
{
    private readonly IProjectRepository _repo;
    private readonly IStringLocalizer<Global> _local;

    public IndexModel(IProjectRepository repo, IStringLocalizer<Global> local)
    {
        _repo = repo;
        _local = local;
    }

    public IList<ProjectView> ProjectList { get; set; }

    public async Task OnGetAsync()
    {
        var result = await _repo.GetProjectListAsync();

        ProjectList = result.Items;
    }
}