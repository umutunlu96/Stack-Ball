using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class GameUI : MonoBehaviour
{
    public GameObject homeUI, inGameUI, finishUI, gameOverUI, shopUI;
    public GameObject allBtns;
    private bool btns;
    [Header("Pre Game")]
    public Button soundBtn;
    public Sprite soundOnImg, soundOffImg;
    public Button vibrateBtn;
    public Sprite vibrateOnImg, vibrateOffImg;

    [Header("In Game")]
    public Image levelSlider;
    public Image currentLevelImg;
    public Image nextLevelImg;
    public Text currentLevelText, nextLevelText;

    [Header("Finish")]
    public Text finishLevelText;

    [Header("GameOver")]
    public Text gameOverScoreText;
    public Text gameOverBestText;

    private Player player;
    private Material playerMat;

    void Awake()
    {
        player = FindObjectOfType<Player>();

        GetPlayerMesh();

    }

    private void Start()
    {
        currentLevelText.text = FindObjectOfType<LevelSpawner>().level.ToString();
        nextLevelText.text = FindObjectOfType<LevelSpawner>().level + 1 + "";
        SetSliderColor();
    }

    public void SetSliderColor()
    {
        levelSlider.transform.parent.GetComponent<Image>().color = playerMat.color + Color.gray;
        levelSlider.color = playerMat.color;
        currentLevelImg.color = playerMat.color;
        nextLevelImg.color = playerMat.color;

        var tempColor1 = currentLevelImg.color;
        tempColor1.a = 1;
        currentLevelImg.color = tempColor1;


        var tempColor2 = nextLevelImg.color;
        tempColor2.a = 1;
        nextLevelImg.color = tempColor2;
    }


    void GetPlayerMesh()
    {
        for (int i = 0; i < FindObjectOfType<Player>().transform.childCount; i++)
        {
            if (FindObjectOfType<Player>().transform.GetChild(i).gameObject.activeSelf == true)
            {
                playerMat = FindObjectOfType<Player>().transform.GetChild(i).GetComponent<MeshRenderer>().material;
            }
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !IgnoreUI() && player.playerState == Player.PlayerState.Prepeare)
        {
            player.playerState = Player.PlayerState.Play;
            homeUI.SetActive(false);
            inGameUI.SetActive(true);
            shopUI.SetActive(false);
        }

        if (player.playerState == Player.PlayerState.Finish)
        {
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(true);
            gameOverUI.SetActive(false);

            finishLevelText.text = "Level " + FindObjectOfType<LevelSpawner>().level;
        }

        if (player.playerState == Player.PlayerState.Died)
        {
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(false);
            gameOverUI.SetActive(true);

            gameOverScoreText.text = ScoreManager.instance.score.ToString();
            gameOverBestText.text = PlayerPrefs.GetInt("HighScore").ToString();

            if (Input.GetMouseButtonDown(0))
            {
                PlayerPrefs.SetInt("AdShowCount", PlayerPrefs.GetInt("AdShowCount") + 1);
                ScoreManager.instance.ResetScore();
                SceneManager.LoadScene(0);
            }
        }
    }


    public void ToggleChangeCheck()
    {
        if (PlayerPrefs.GetInt("Muted") == 1)
            soundBtn.image.sprite = soundOffImg;
        else
            soundBtn.image.sprite = soundOnImg;

        if (PlayerPrefs.GetInt("Vibrate", 1) == 1)
            vibrateBtn.image.sprite = vibrateOffImg;
        else
            vibrateBtn.image.sprite = vibrateOnImg;
            
    }

    public void MuteToggle()
    {
        EffectManager.instance.MuteToggle();
        ToggleChangeCheck();
    }

    public void VibrationToggle()
    {
        EffectManager.instance.VibrationToggle();
        ToggleChangeCheck();
    }

    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }

    public void Settings()
    {
        btns = !btns;
        allBtns.SetActive(btns);
        if(btns)
            ToggleChangeCheck();
    }

    //OYUN ADRESINI EKLE
    public void RateUs()
    {
        Application.OpenURL("");
    }

    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        
        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (raycastResultList[i].gameObject.GetComponent<UIignore>() != null)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }
        return raycastResultList.Count > 0;
    }
}
