using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private bool smash;
    private bool invincible;
    private float currentTime;
    public int currentBrokenStacks, totalStacks;

    public GameObject invincibleObj;
    public Image invincibleFill;
    public GameObject fireEffect, winEffect, splashEffect;

    [Header("Clips")]
    public AudioClip bounceOffClip, deadClip, winClip, destroyClip, iDestroyClip;

    private bool vibrateOff;
    private Material playerMat;
    private Animator anim;
    private AdController adController;

    public enum PlayerState
    {
        Prepeare,
        Play,
        Died,
        Finish
    }

    [HideInInspector]
    public PlayerState playerState = PlayerState.Prepeare;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentBrokenStacks = 0;
        StartUpPlayerAsset();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.SetBool("Scale", true);
        totalStacks = FindObjectsOfType<StackController>().Length;
        vibrateOff = EffectManager.instance.isNotVibrating;
        adController = GameObject.FindObjectOfType<AdController>();
    }

    void Update()
    {
        if (playerState == PlayerState.Play)
        {
            PlayerPointerCheck();
            FireEffect(invincible);
            InvincibleController(currentTime);
        }

        if (playerState == PlayerState.Finish)
        {
            FinishStateController();
        }
    }

    public void StartUpPlayerAsset()
    {
        foreach (Transform obj in transform)
        {
            obj.gameObject.SetActive(false);
        }
        GameObject child = transform.GetChild(PlayerPrefs.GetInt("PlayerAsset", 0)).gameObject;
        child.SetActive(true);

        fireEffect = child.transform.GetChild(0).gameObject;
        fireEffect.SetActive(false);

        ChangePlayerMaterial();
    }

    private void FixedUpdate()
    {
        if (playerState == PlayerState.Play)
        {
            if (Input.GetMouseButton(0))
            {
                anim.SetBool("Scale", false);
                smash = true;
                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
            else
                anim.SetBool("Scale", true);
        }

        if (rb.velocity.y > 5)
        {
            rb.velocity = new Vector3(rb.velocity.x, 7, rb.velocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!smash)
        {
            rb.velocity = new Vector3(0, 200 * Time.deltaTime * 5, 0);

            PlaySound(bounceOffClip, .15f);

            if (collision.gameObject.tag != "Finish")
            {
                SplashEffect(collision);
            }

        }
        else
        {
            if (invincible)
            {
                if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "plane")
                {
                    //Destroy(collision.transform.parent.gameObject);
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                    if(!vibrateOff)
                        Vibration.Vibrate(15);
                }
            }
            else
            {
                if (collision.gameObject.tag == "enemy")
                {
                    //Destroy(collision.transform.parent.gameObject);
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
                if (collision.gameObject.tag == "plane")
                {
                    rb.isKinematic = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    PlaySound(deadClip,.4f);
                    playerState = PlayerState.Died;
                }
            }
        }

        FindObjectOfType<GameUI>().LevelSliderFill(currentBrokenStacks / (float)totalStacks);

        if (collision.gameObject.tag == "Finish" && playerState == PlayerState.Play)
        {
            playerState = PlayerState.Finish;
            PlaySound(winClip, .3f);
            GameObject win = Instantiate(winEffect);
            win.transform.SetParent(Camera.main.transform);
            win.transform.localPosition = Vector3.up * 1.5f;
            win.transform.eulerAngles = Vector3.zero;
            PlayerPrefs.SetInt("BrokenStacks", PlayerPrefs.GetInt("BrokenStacks") + currentBrokenStacks);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!smash || collision.gameObject.tag == "Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
    }

    private void PlayerPointerCheck()
    {
        if (Input.GetMouseButtonDown(0))
            smash = true;

        if (Input.GetMouseButtonUp(0))
            smash = false;
    }

    private void InvincibleController(float currentTime)
    {
        if (this.currentTime >= 0.3f || invincibleFill.color == Color.red)
            invincibleObj.SetActive(true);
        else
            invincibleObj.SetActive(false);

        if (this.currentTime >= 1)
        {
            this.currentTime = 1;
            invincible = true;
            invincibleFill.color = Color.red;
        }
        else if (this.currentTime <= 0)
        {
            this.currentTime = 0;
            invincible = false;
            invincibleFill.color = Color.white;
        }

        if (invincibleObj.activeInHierarchy)
            invincibleFill.fillAmount = this.currentTime / 1;
    }

    private void FireEffect(bool invincible)
    {
        if (invincible)
        {
            CheckCurrentTime(-.45f);
            if (!fireEffect.activeInHierarchy)
                fireEffect.SetActive(true);
        }
        else
        {
            if (fireEffect.activeInHierarchy)
                fireEffect.SetActive(false);
            if (smash)
                CheckCurrentTime(.75f);

            else
                CheckCurrentTime(-.5f);
        }
    }

    private void FinishStateController()
    {
        if (Input.GetMouseButtonDown(0) && adController.adState == AdController.AdState.NotShowAdd)
        {
            FindObjectOfType<LevelSpawner>().NextLevel();
            //Time.timeScale = 1;
        }
    }

    private void CheckCurrentTime(float scaler)
    {
        currentTime += Time.deltaTime * scaler;
    }

    private void SplashEffect(Collision collision)
    {
        GameObject splash = Instantiate(splashEffect);
        splash.transform.SetParent(collision.transform);
        splash.transform.localEulerAngles = new Vector3(90, Random.Range(0, 359), 0);
        float randomScale = Random.Range(.18f, .25f);
        splash.transform.localScale = new Vector3(randomScale, randomScale, 1);
        splash.transform.position = new Vector3(transform.position.x, transform.position.y - .22f, transform.position.z);
        //splash.GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;

        splash.GetComponent<SpriteRenderer>().color = playerMat.color;
    }

    public void ChangeStartAssetColor(Color color1)
    {
        if (PlayerPrefs.GetInt("PlayerAsset") == 0)
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = color1;
    }

    public void ChangePlayerMaterial()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeSelf == true)
            {
                playerMat = gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material;
            }
        }
    }

    private void PlaySound(AudioClip audioClip, float volume = .5f)
    {
        SoundManager.instance.PlaySoundFX(audioClip, volume);
    }

    public void IncreaseBrokenStacks()
    {
        currentBrokenStacks++;
        if (!invincible)
        {
            ScoreManager.instance.AddScore(1);
            SoundManager.instance.PlaySoundFX(destroyClip,.5f);
        }
        else
        {
            ScoreManager.instance.AddScore(2);
            SoundManager.instance.PlaySoundFX(iDestroyClip, .5f);
        }
    }

}