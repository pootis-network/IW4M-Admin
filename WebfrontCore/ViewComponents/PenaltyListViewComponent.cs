using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace IW4MAdmin.WebfrontCore.ViewComponents
{
    public class PenaltyListViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int offset, int count, EFPenalty.PenaltyType showOnly, bool ignoreAutomated)
        {
            var penalties = await Program.Manager.GetPenaltyService().GetRecentPenalties(count, offset, showOnly, ignoreAutomated);
            penalties = User.Identity.IsAuthenticated ? penalties : penalties.Where(p => !p.Sensitive).ToList();

            return View("~/Views/Penalty/PenaltyInfoList.cshtml", penalties);
        }
    }
}
