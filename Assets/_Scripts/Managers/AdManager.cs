using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    private BannerView bannerAd;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private BannerView bannerView;

    public static AdManager instance;
    private AdController adController;
    private ShopUI shopUi;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void Start()
    {
        MobileAds.Initialize(InitializationStatus => { });
        adController = GameObject.FindObjectOfType<AdController>();
        shopUi = GameObject.FindObjectOfType<ShopUI>();
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();

    }

    /*BANNER*/
    #region BANNER

    public void RequestBanner()
    {
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";         //Test ID
        //string adUnitId = "ca-app-pub-4762392528800851/9637389084";       //Gercek ID

        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        this.bannerView.LoadAd(this.CreateAdRequest());

        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
    }

    public void DestroyBanner()
    {
        bannerView.Destroy();
    }

    private void HandleOnAdLoaded(object sender, EventArgs e)
    {
        
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        RequestBanner();
    }
    #endregion


    /*INTERSTIAL ADS*/
    #region INTERSTIAL
    public void RequestIntertial()
    {
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";     //Test ID
        //string adUnitId = "ca-app-pub-4762392528800851/6503905381";     //Gercek ID


        if (this.interstitialAd != null)
            this.interstitialAd.Destroy();

        this.interstitialAd = new InterstitialAd(adUnitId);

        this.interstitialAd.LoadAd(this.CreateAdRequest());

        this.interstitialAd.OnAdFailedToLoad += InterstitialAd_OnAdFailedToLoad;

        this.interstitialAd.OnAdClosed += InterstitialAd_OnAdClosed;
    }

    private void InterstitialAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        RequestIntertial();
    }

    private void InterstitialAd_OnAdClosed(object sender, EventArgs e)
    {
        PlayerPrefs.SetInt("AdShowCount", 0);
        adController.adState = AdController.AdState.NotShowAdd;
    }

    public void ShowIntertial()
    {
        if (this.interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }

        else
        {
            print("interstitialAd not loaded yet.");
        }
    }
    #endregion


    /*REWARDED ADS*/
    #region REWARDED
    public void RequestRewarded()
    {
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";         //Test ID
        //string adUnitId = "ca-app-pub-4762392528800851/7206149945";         //Gercek ID

        if (this.rewardedAd != null)
            this.rewardedAd.Destroy();

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.LoadAd(this.CreateAdRequest());

        this.rewardedAd.OnAdLoaded += RewardedAd_OnAdLoaded; ;

        this.rewardedAd.OnAdFailedToLoad += RewardedAd_OnAdFailedToLoad;

        this.rewardedAd.OnUserEarnedReward += RewardedAd_OnUserEarnedReward;
        print("Reward requested");
    }

    private void RewardedAd_OnAdLoaded(object sender, EventArgs e)
    {
        print("Reward onloaded");
    }
    private void RewardedAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        print("Reward failed to load");
        RequestRewarded();
    }
    private void RewardedAd_OnUserEarnedReward(object sender, Reward e)
    {
        print("Rewarded player");
        shopUi.OpenNextBoxReward();
    }

    public void ShowRewarded()
    {
        if (this.rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }

        else
        {
            print("rewardedAd not loaded yet.");
        }
    }
    #endregion
}
