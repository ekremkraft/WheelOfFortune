using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConnectResultModel
{
    public bool Status { get; set; }
    public string Message { get; set; }
    public GameConnectModel Data = new GameConnectModel();
    public List<GameWinningsModel> Winnings = new List<GameWinningsModel>();
}