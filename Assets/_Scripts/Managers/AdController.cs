using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdController : MonoBehaviour
{
    [Header("ADS")]
    private bool SHOWADS = true;
    [SerializeField]private int adShowCount; //Ads
    public bool adShow; //Ads
    public bool rewardRequest;
    private Player player;

    private void Awake()
    {
        adShowCount = PlayerPrefs.GetInt("AdShowCount", 0);
        print(adShowCount);
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        InstertialAdCheck(SHOWADS);
    }

    private void Update()
    {
        if (player.playerState == Player.PlayerState.Finish)
        {
            ShowInstertialAd(adShow);
        }
    }

    private void InstertialAdCheck(bool showAds)
    {
        if (showAds)
        {
            adShowCount = PlayerPrefs.GetInt("AdShowCount");

            if (adShowCount >= 3)
            {
                AdManager.instance.RequestIntertial();
                adShow = true;
            }
        }
    }

    private void ShowInstertialAd(bool adShow)
    {
        if (adShow)
        {
            AdManager.instance.ShowIntertial();
            this.adShow = false;
            PlayerPrefs.SetInt("AdShowCount", 0);
        }
    }
    //private void RewardRequest(bool rewardRequest)
    //{
    //    if (rewardRequest)
    //    {
    //        AdManager.instance.RequestRewarded();
    //        this.rewardRequest = false;
    //    }
    //}

    //public void RewardShow()
    //{
    //    AdManager.instance.ShowRewarded();
    //}
}
