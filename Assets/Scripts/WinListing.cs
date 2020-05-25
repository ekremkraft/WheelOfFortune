using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinListing : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    public void SetWinInfo(string text)
    {
        _text.text = text;
    }
}
