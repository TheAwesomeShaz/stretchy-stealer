using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
using Obi;

public class GameController : MonoBehaviour
{
    public bool levelHasEnded;
    public bool gotTarget;
    public bool canInteractWithRope;
    public bool canPullTarget;
    public static GameController instance;

    [SerializeField] GameObject target;
    [SerializeField] GameObject[] setInactiveStuff;

    [Tooltip("Stuff to set inactive only when you win")]
    [SerializeField] GameObject[] setInactiveStuffOnWin;


    [SerializeField] GameObject winImage;
    [SerializeField] GameObject loseImage;
    [SerializeField] GameObject levelWinConfetti;
    [SerializeField] GameObject englishText;
    [SerializeField] GameObject hindiText;
    [SerializeField] GameObject tamilText;
    [SerializeField] GameObject teluguText;
    [SerializeField] GameObject SettingsCanvas;

    [SerializeField] SpriteRenderer handTargetSprite;
    SpriteRenderer playerSpriteRenderer;
    SpriteRenderer enemySpriteRenderer;

    [SerializeField] Material ropeNormalMaterial;
    [SerializeField] Material ropeSuccessMaterial;
    MeshRenderer ropeMeshRenderer;

    [SerializeField] Sprite[] idleSprite;
    [SerializeField] Sprite[] winSprite;
    [SerializeField] Sprite[] loseSprite;
    [SerializeField] Sprite[] enemyIdleSprite;
    [SerializeField] Sprite[] enemyWinSprite;
    [SerializeField] Sprite[] enemyLoseSprite;

    int currSceneIndex;

    //Audio Stuff 
    AudioSource audioSource;
    [SerializeField] AudioClip ropeStretchSFX;
    [SerializeField] AudioClip successSFX;
    [SerializeField] AudioClip failSFX;



    // Start is called before the first frame update
    void Start()
    {
        // GameAnalytics.Initialize();
        TinySauce.OnGameStarted(levelNumber: (currSceneIndex + 1).ToString());


        audioSource = Camera.main.GetComponent<AudioSource>();

        Time.timeScale = 1f;
        FindObjectOfType<RopeTenser>().force = 1f;

        Invoke(nameof(NoStretch), 1f);

        canInteractWithRope = true;
        instance = this;
        handTargetSprite.enabled = false;
        currSceneIndex = SceneManager.GetActiveScene().buildIndex;

        ropeMeshRenderer = FindObjectOfType<RopeWrap>().gameObject.GetComponent<MeshRenderer>();
        playerSpriteRenderer = FindObjectOfType<Player2D>().GetComponent<SpriteRenderer>();
        enemySpriteRenderer = FindObjectOfType<Enemy2D>().GetComponent<SpriteRenderer>();

        levelWinConfetti.SetActive(false);
        // levelLosePanel.SetActive(false);

        winImage.SetActive(false);
        loseImage.SetActive(false);
        englishText.SetActive(false);
        hindiText.SetActive(false);
        tamilText.SetActive(false);
        teluguText.SetActive(false);
        SettingsCanvas.SetActive(false);
        // ChooseLanguage(PlayerPrefs.GetInt("language", 1));
    }


    void NoStretch()
    {
        FindObjectOfType<ObiRope>().stretchingScale = 0.1f;
        canInteractWithRope = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(ropeStretchSFX, 1f);
        }
    }


    public void ChangeRopeColor(int i)
    {
        if (gotTarget && !levelHasEnded)
        {
            ropeMeshRenderer.material = ropeSuccessMaterial;
        }
        if (i == 1)
        {
            ropeMeshRenderer.material = ropeNormalMaterial;
        }
    }


    public void GotTarget()
    {
        gotTarget = true;
        if (!levelHasEnded)
        {
            handTargetSprite.enabled = true;
            target.SetActive(false);
            handTargetSprite.GetComponent<BoxCollider2D>().enabled = false;

            FindObjectOfType<RopeTenser>().force = 3000f;

        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void PullTarget()
    {

        if (Input.GetMouseButtonUp(0) && canPullTarget && canInteractWithRope)
        {
            ChangeRopeColor(1);
            GotTarget();
            canInteractWithRope = false;
            // Time.timeScale = 6f;
        }
        else
        {
            canPullTarget = true;
            // canInteract = true;
            ropeMeshRenderer.material = ropeSuccessMaterial;

        }
    }

    public void LevelWin()
    {
        playerSpriteRenderer.sprite = winSprite[currSceneIndex];
        enemySpriteRenderer.sprite = enemyWinSprite[currSceneIndex];
        Time.timeScale = 1f;

        foreach (GameObject stuff in setInactiveStuffOnWin)
        {
            stuff.SetActive(false);
        }

        if (currSceneIndex == 0)
            target.SetActive(false);

        foreach (GameObject item in setInactiveStuff)
        {
            item.SetActive(false);
        }
        Invoke(nameof(SetWinConfettiActive), .5f);
        Invoke(nameof(LoadNextLevel), 3f);
        audioSource.PlayOneShot(successSFX, 0.7f);

        // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level " + (currSceneIndex + 1).ToString(), 1);

        TinySauce.OnGameFinished(levelNumber: (currSceneIndex + 1).ToString(), true, 1);

        StartCoroutine(SetActiveAfterTime(winImage, 1f));
    }
    public void LevelFail()
    {
        playerSpriteRenderer.sprite = loseSprite[currSceneIndex];
        enemySpriteRenderer.sprite = enemyLoseSprite[currSceneIndex];
        enemySpriteRenderer.flipX = false;

        if (currSceneIndex == 0)
            target.SetActive(false);

        foreach (GameObject item in setInactiveStuff)
        {
            item.SetActive(false);
        }
        // Invoke(nameof(SetLosePanelActive), 1f);
        Invoke(nameof(RestartLevel), 2f);
        audioSource.PlayOneShot(failSFX, 0.7f);

        // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level " + (currSceneIndex + 1).ToString(), 0);
        TinySauce.OnGameFinished(levelNumber: (currSceneIndex + 1).ToString(), false, 0);

        StartCoroutine(SetActiveAfterTime(loseImage, 1f));
    }

    public void SetWinConfettiActive()
    {
        levelWinConfetti.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currSceneIndex);
    }

    public void LoadNextLevel()
    {
        if (currSceneIndex >= 0 && currSceneIndex < 2)
        {
            SceneManager.LoadScene(currSceneIndex + 1);
        }
        if (currSceneIndex == 2)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void ChooseLanguage(int language)
    {
        PlayerPrefs.SetInt("language", language);

        switch (language)
        {
            case 1: // if lang is English
                englishText.SetActive(true);
                hindiText.SetActive(false);
                tamilText.SetActive(false);
                teluguText.SetActive(false);
                break;
            case 2: // if lang is Hindi
                englishText.SetActive(false);
                hindiText.SetActive(true);
                tamilText.SetActive(false);
                teluguText.SetActive(false);
                break;
            case 3: // if lang is Tamil
                englishText.SetActive(false);
                hindiText.SetActive(false);
                teluguText.SetActive(true);
                tamilText.SetActive(false);
                break;
            case 4: // if lang is Telugu
                englishText.SetActive(false);
                hindiText.SetActive(false);
                teluguText.SetActive(false);
                tamilText.SetActive(true);
                break;
        }
        CloseSettings();
    }

    public void CloseSettings()
    {
        SettingsCanvas.SetActive(false);
        // canInteractWithRope = true;
    }

    public void OpenSettings()
    {
        SettingsCanvas.SetActive(true);
        // canInteractWithRope = false;
    }

    IEnumerator SetActiveAfterTime(GameObject go, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        go.SetActive(true);
    }

}
