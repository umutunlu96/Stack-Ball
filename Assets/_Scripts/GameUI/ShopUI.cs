using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text brokenStacksInfo;
    public GameObject[] Assets;
    public int[] assetGoals;
    public Image[] assetSliderImg;
    private int totalBrokenStacks;
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        AssetSliderFillAmount();
    }

    public void AssetButtonPressed(int index)
    {
        if (assetSliderImg[index].fillAmount == 1)
            ChangePlayerAsset(index);

        BrokenStackInfoText(index);
    }

    private void ChangePlayerAsset(int index)
    {
        PlayerPrefs.SetInt("PlayerAsset", index);
        player.StartUpPlayerAsset();
    }

    private void BrokenStackInfoText(int index)
    {
        totalBrokenStacks = PlayerPrefs.GetInt("BrokenStacks", 1);

        if (index == 0)
            brokenStacksInfo.text = "Free";
        else
            brokenStacksInfo.text = "Broken Stacks: " + totalBrokenStacks + " / " + assetGoals[index];
    }

    public void AssetSliderFillAmount()
    {
        totalBrokenStacks = PlayerPrefs.GetInt("BrokenStacks", 1);

        for (int i = 0; i < assetSliderImg.Length; i++)
        {
            AssetSliderFill(i, (totalBrokenStacks / (float)assetGoals[i]));
        }
    }

    private void AssetSliderFill(int index, float fillAmount)
    {
        assetSliderImg[index].fillAmount = fillAmount;

        if (fillAmount >= 1)
        {
            assetSliderImg[index].color = Color.green;
            Assets[index].GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            assetSliderImg[index].color = Color.red;
            Assets[index].GetComponent<CanvasGroup>().alpha = .7f;
        }
    }
}
