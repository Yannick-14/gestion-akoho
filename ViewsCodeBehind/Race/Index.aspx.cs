using System.Collections.Generic;
using System.Web.Mvc;
using AkohoAspx.Models;

namespace AkohoAspx.ViewsCodeBehind.Race
{
    public class RaceIndexPage : ViewPage<IEnumerable<AkohoAspx.Models.Race>>
    {
        public int CurrentRaceId
        {
            get
            {
                object currentRaceObj = ViewBag.CurrentRaceId;
                int currentRaceId;
                return currentRaceObj != null && int.TryParse(currentRaceObj.ToString(), out currentRaceId) ? currentRaceId : 0;
            }
        }

        public bool HasCurrentRace
        {
            get { return CurrentRaceId > 0; }
        }
    }
}
