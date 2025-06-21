using ConTech.Core;
using ConTech.Core.Features.Identity;
using ConTech.Core.Features.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Diagnostics;

namespace ConTech.Web.Pages.View;

public class UpdateModel : PageModel
{

    private readonly IProjectViewRepository _repo;
    private readonly IStringLocalizer<Global> _local;

    public UpdateModel(IProjectViewRepository repo, IStringLocalizer<Global> local)
    {
        _repo = repo;
        _local = local;
    }

    [BindProperty]
    public ProjectViewUpdateInput View { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var project = await _repo.GetProjectViewByIdAsync(id);

        if (project.IsNotFound)
            return NotFound();

        View = new(project.Item!);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Check what values made it through binding
        var keys = ModelState.Keys.Where(k => ModelState[k].AttemptedValue != "");
        foreach (var key in keys)
        {
            Debug.WriteLine($"{key}: {ModelState[key].AttemptedValue}");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var currentUser = new IdentityInfo(HttpContext);

        //if (Project != null)
        var result = await _repo.UpdateProjectViewAsync(View, currentUser);

        return RedirectToPage("./Index");
    }

}
