using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;
    public GameObject Content;
    public GameObject Error;
    public GameObject Wheel;
    public Transform WinListingsMenuContent;
    public GameObject WinListing;

    public List<ItemModel> listItems;
    public List<GameWinningsModel> listWinnings;

    private int ID;
    public Text Nickname;
    public Text Description;
    public Text Jeton;
    public float reducer;
    public float multiplier = 1;
    bool round1 = false;
    public bool isStopped = false;
    private bool isPlay = false;
    private string closeUrl;
    private int jeton = 0;

    private void Awake()
    {
        Instance = this;
        var query = new UrlEncodingParser(Application.absoluteURL);
        string token = query.Get("token");
        if (!string.IsNullOrEmpty(token))
        {
            StartCoroutine(Connect(token));
        }
        else
        {
            ShowError("Invalid token.");
        }
    }

    private IEnumerator Connect(string token)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(string.Format("http://earnapi.com/Game/Connect?token={0}", token)))
        {
            yield return webRequest.SendWebRequest();

            if (!webRequest.isNetworkError)
            {
                var model = JsonConvert.DeserializeObject<GameConnectResultModel>(webRequest.downloadHandler.text);
                if (model.Status)
                {
                    ID = model.Data.ID;
                    Nickname.text = model.Data.Nickname;
                    Description.text = model.Data.Description;
                    jeton = model.Data.Jeton;
                    closeUrl = model.Data.CloseUrl;
                    LoadItems(model.Data.Type, model.Data.Items);
                    LoadWinnings(model.Winnings);
                    ShowContent();
                }
                else
                {
                    ShowError(model.Message);
                }
            }
            else
            {
                ShowError("Network error.");
            }
        }
    }

    public IEnumerator Winnings(string item, string text)
    {
        item = item.Replace("Item", string.Empty);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(string.Format("http://earnapi.com/Game/Winnings?token={0}&item={1}&rotation={2}", ID, item, Wheel.transform.localRotation.z)))
        {
            yield return webRequest.SendWebRequest();

            if (!webRequest.isNetworkError)
            {
                var model = JsonConvert.DeserializeObject<GameWinningsResultModel>(webRequest.downloadHandler.text);
                if (model.Status)
                {
                    AddWinningsToMenu(text);
                }
                else
                {
                    ShowError(model.Message);
                }
            }
            else
            {
                ShowError("Network error, please try again later.");
            }
        }
    }

    private void AddWinningsToMenu(string text)
    {
        var listing = Instantiate(WinListing, WinListingsMenuContent);
        if (listing != null)
        {
            listing.GetComponent<WinListing>().SetWinInfo(text);
        }
    }

    private void LoadItems(int Type, List<ItemModel> Items)
    {
        listItems = Items;
        if (Type == 1)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Wheel.transform.GetChild(0).GetChild(Items[i].index).GetChild(0).GetComponent<Text>();
                item.text = Items[i].value = Items[i].value;
            }
        }
    }

    private void LoadWinnings(List<GameWinningsModel> Winnings)
    {
        listWinnings = Winnings;
        for (int i = 0; i < listWinnings.Count; i++)
        {
            var item = listItems.Find(x => x.index == listWinnings[i].Item);
            if (item != null)
            {
                AddWinningsToMenu(item.value);
            }
        }
    }

    private void ShowContent()
    {
        Error.active = false;
        Content.active = true;
    }

    private void ShowError(string text)
    {
        Content.active = false;
        Error.active = true;
        Error.transform.GetChild(1).GetComponent<Text>().text = text;
    }

    public void OnClick_GoHome()
    {
        Application.ExternalEval(string.Format("window.location.href = '{0}'", closeUrl));
    }

    public void InitializeWheel()
    {
        SelectorScript.Instance.collected = false;
        multiplier = 1;
        reducer = Random.RandomRange(0.01f, 0.5f);
        round1 = false;
        isStopped = false;
    }

    public void OnClick_Play()
    {
        if (!isPlay && jeton > 0)
        {
            InitializeWheel();
            jeton--;
            isPlay = true;
        }
    }

    private void FixedUpdate()
    {
        Jeton.text = jeton.ToString();
        if (isPlay)
        {
            if (multiplier > 0)
            {
                Wheel.transform.Rotate(Vector3.forward, 1 * multiplier);
            }
            else
            {
                isStopped = true;
                isPlay = false;
            }

            if (multiplier < 20 && !round1)
            {
                multiplier += 0.1f;
            }
            else
            {
                round1 = true;
            }

            if (round1 && multiplier > 0)
            {
                multiplier -= reducer;
            }
        }
    }
}
