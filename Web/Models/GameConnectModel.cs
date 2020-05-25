using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EarnApi.Models
{
    public class GameConnectModel
    {
        public int ID { get; set; }
        public string Nickname { get; set; }
        public string Token { get; set; }
        public int Jeton { get; set; }
        public int Type { get; set; }
        public List<ItemModel> Items { get; set; }
        public string Description { get; set; }
        public string CloseUrl { get; set; }
    }
}