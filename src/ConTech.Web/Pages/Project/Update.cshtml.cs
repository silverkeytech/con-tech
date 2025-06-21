using ConTech.Core;
using ConTech.Core.Features.Identity;
using ConTech.Core.Features.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Diagnostics;

namespace ConTech.Web.Pages.Project;

public class UpdateModel : PageModel
{

    private readonly IProjectRepository _repo;
    private readonly IStringLocalizer<Global> _local;

    public UpdateModel(IProjectRepository repo, IStringLocalizer<Global> local)
    {
        _repo = repo;
        _local = local;
    }

    [BindProperty]
    public ProjectUpdateInput Project { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var project = await _repo.GetProjectByIdAsync(id);

        if (project.IsNotFound)
            return NotFound();

        Project = new(project.Item!);
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
        var result = await _repo.UpdateProjectAsync(Project, currentUser);

        return RedirectToPage("./Index");
    }

}
