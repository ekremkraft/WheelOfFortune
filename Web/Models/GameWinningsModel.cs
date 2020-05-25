using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EarnApi.Models
{
    public class GameWinningsModel
    {
        public int ID { get; set; }
        public int Token { get; set; }
        public int Item { get; set; }
        public int Rotation { get; set; }
    }
}