using ConTech.Core;
using ConTech.Core.Features.View;
using ConTech.Data.Read.DtoClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ConTech.Web.Pages.View;

public class IndexModel : PageModel
{
    private readonly IProjectViewRepository _repo;
    private readonly IStringLocalizer<Global> _local;

    public IndexModel(IProjectViewRepository repo, IStringLocalizer<Global> local)
    {
        _repo = repo;
        _local = local;
    }

    public IList<ProjectViewLlblView> ProjectViewList { get; set; }

    public async Task OnGetAsync(int projectId)
    {
        var result = await _repo.GetProjectViewListAsync(projectId);

        ProjectViewList = result.Items;
    }
}