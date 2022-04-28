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
    private AdManager adManager;

    public enum AdState
    {
        NotShowAdd,
        ShowAd
    }

    public enum BannerState
    {
        NotShowBanner,
        ShowBanner
    }

    public AdState adState = AdState.NotShowAdd;
    public BannerState bannerState = BannerState.NotShowBanner;

    private void Awake()
    {
        adShowCount = PlayerPrefs.GetInt("AdShowCount", 0);
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        adManager = GameObject.FindObjectOfType<AdManager>();
        InstertialAdCheck(SHOWADS);
    }

    private void Update()
    {
        if ((player.playerState == Player.PlayerState.Finish || player.playerState == Player.PlayerState.Died) && adState == AdState.ShowAd)
        {
            ShowInstertialAd(adShow);
        }

        if (player.playerState == Player.PlayerState.Play && bannerState == BannerState.NotShowBanner)
        {
            adManager.RequestBanner();
            bannerState = BannerState.ShowBanner;
            print("Show Banner");
        }

        else if (player.playerState != Player.PlayerState.Play && bannerState == BannerState.ShowBanner)
        {
            adManager.DestroyBanner();
            bannerState = BannerState.NotShowBanner;
            print("Destroy Banner");
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
                adState = AdState.ShowAd;
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

    public void RewardRequest()
    {
        AdManager.instance.RequestRewarded();
    }

    public void RewardShow()
    {
        AdManager.instance.ShowRewarded();
    }
}
