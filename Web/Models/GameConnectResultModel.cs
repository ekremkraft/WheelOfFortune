using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EarnApi.Models
{
    public class GameConnectResultModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public GameConnectModel Data = new GameConnectModel();
        public List<GameWinningsModel> Winnings = new List<GameWinningsModel>();
    }
}