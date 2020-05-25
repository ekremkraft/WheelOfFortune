using EarnApi.Functions;
using EarnApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EarnApi.Controllers
{
    public class GameController : Controller
    {
        public ActionResult Connect(string token)
        {
            var model = new GameConnectResultModel();
            if (!String.IsNullOrEmpty(token))
            {
                var tokenData = Database.ExecSelect(string.Format("select * from Tokens where Token = '{0}'", token));
                if (tokenData != null && tokenData.Rows.Count > 0)
                {
                    model.Data.ID = Convert.ToInt32(tokenData.Rows[0]["ID"]);
                    model.Data.Token = tokenData.Rows[0]["Token"].ToString();
                    model.Data.Nickname = tokenData.Rows[0]["Nickname"].ToString();
                    model.Data.Jeton = Convert.ToInt32(tokenData.Rows[0]["Jeton"]);
                    var orderData = Database.ExecSelect(string.Format("select * from Orders where ID = '{0}'", tokenData.Rows[0]["Order"]));
                    if (orderData != null && orderData.Rows.Count > 0)
                    {
                        model.Data.Type = Convert.ToInt32(orderData.Rows[0]["Type"]);
                        model.Data.Items = GetItems(orderData.Rows[0]["Items"].ToString());
                        var userData = Database.ExecSelect(string.Format("select * from Users where ID = '{0}'", orderData.Rows[0]["User"]));
                        if (userData != null && userData.Rows.Count > 0)
                        {
                            model.Status = true;
                            model.Data.Description = userData.Rows[0]["Description"].ToString();
                            model.Data.CloseUrl = userData.Rows[0]["CloseUrl"].ToString();
                            var winningsData = Database.ExecSelect(String.Format("select * from Winnings where Token = '{0}'", model.Data.ID));
                            if (winningsData != null && winningsData.Rows.Count > 0)
                            {
                                model.Data.Jeton -= winningsData.Rows.Count;
                                for (int i = 0; i < winningsData.Rows.Count; i++)
                                {
                                    model.Winnings.Add(new GameWinningsModel
                                    {
                                        ID = Convert.ToInt32(winningsData.Rows[i]["ID"]),
                                        Token = Convert.ToInt32(winningsData.Rows[i]["Token"]),
                                        Item = Convert.ToInt32(winningsData.Rows[i]["Item"]),
                                        Rotation = Convert.ToInt32(winningsData.Rows[i]["Rotation"])
                                    });
                                }
                            }
                        }
                        else
                        {
                            model.Status = false;
                            model.Message = "Invalid user.";
                        }
                    }
                    else
                    {
                        model.Status = false;
                        model.Message = "Invalid product.";
                    }
                }
                else
                {
                    model.Status = false;
                    model.Message = "Invalid token.";
                }
            }
            else
            {
                model.Status = false;
                model.Message = "Invalid parameters.";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Winnings(string token, string item, string rotation)
        {
            var model = new GameWinningsResultModel();
            if (!String.IsNullOrEmpty(token) && !String.IsNullOrEmpty(item) && !String.IsNullOrEmpty(rotation))
            {
                int insertResult = Database.ExecQuery(String.Format("insert into Winnings (Token,Item,Rotation) values ('{0}','{1}','{2}')", token, item, rotation.Replace(",", ".")));
                if (insertResult != -1)
                {
                    model.Status = true;
                }
                else
                {
                    model.Status = false;
                    model.Message = "Database error.";
                }
            }
            else
            {
                model.Status = false;
                model.Message = "Invalid parameters.";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public List<ItemModel> GetItems(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<ItemModel>>(json);
            }
            catch
            {
                return new List<ItemModel>();
            }
        }
    }
}