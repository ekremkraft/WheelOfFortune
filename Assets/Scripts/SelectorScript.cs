using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorScript : MonoBehaviour
{
    public static SelectorScript Instance;
    public bool collected = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PlayerScript.Instance.isStopped)
            return;

        if (collected)
            return;

        if (!collected)
        {
            collected = true;
            string item = collision.gameObject.name;
            string text = collision.gameObject.transform.GetChild(0).GetComponent<Text>().text;
            StartCoroutine(PlayerScript.Instance.Winnings(item, text));
        }
    }
}
