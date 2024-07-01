using System.Threading.Tasks;
using IW4MAdmin.WebfrontCore.QueryHelpers.Models;
using Microsoft.AspNetCore.Mvc;
using SharedLibraryCore;
using SharedLibraryCore.Interfaces;

namespace IW4MAdmin.WebfrontCore.Controllers.API;

[Route("api/[controller]")]
public class PenaltyController : BaseController
{
    private readonly IResourceQueryHelper<BanInfoRequest, BanInfo> _banInfoQueryHelper;

    public PenaltyController(IManager manager, IResourceQueryHelper<BanInfoRequest, BanInfo> banInfoQueryHelper) : base(manager)
    {
        _banInfoQueryHelper = banInfoQueryHelper;
    }

    [HttpGet("BanInfo/{clientName}")]
    public async Task<IActionResult> BanInfo(BanInfoRequest request)
    {
        var result = await _banInfoQueryHelper.QueryResource(request);
        return Json(result);
    }
}
