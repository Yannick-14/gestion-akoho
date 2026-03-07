using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AkohoAspx.ViewsCodeBehind.Lot
{
    public class LotIndexPage : ViewPage<IEnumerable<AkohoAspx.Models.Lot>>
    {
        public IEnumerable<AkohoAspx.Models.Race> RaceOptions
        {
            get
            {
                return ViewBag.Races as IEnumerable<AkohoAspx.Models.Race> ?? Enumerable.Empty<AkohoAspx.Models.Race>();
            }
        }
    }
}
