using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Obi;

public class GameController : MonoBehaviour
{
    public bool levelHasEnded;
    public bool gotTarget;
    public bool canInteract;
    public bool canPullTarget;
    public static GameController instance;

    [SerializeField] GameObject target;
    [SerializeField] GameObject[] setInactiveStuff;

    [SerializeField] GameObject levelWinConfetti;
    [SerializeField] GameObject levelLosePanel;

    [SerializeField] SpriteRenderer handTargetSprite;
    SpriteRenderer playerSpriteRenderer;
    SpriteRenderer enemySpriteRenderer;

    [SerializeField] Material ropeNormalMaterial;
    [SerializeField] Material ropeSuccessMaterial;
    MeshRenderer ropeMeshRenderer;

    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite winSprite;
    [SerializeField] Sprite loseSprite;
    [SerializeField] Sprite enemyIdleSprite;
    [SerializeField] Sprite enemyWinSprite;
    [SerializeField] Sprite enemyLoseSprite;

    int currSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 2f;
        FindObjectOfType<RopeTenser>().force = 50f;

        Invoke(nameof(NoStretch), 1f);

        canInteract = true;
        instance = this;
        handTargetSprite.enabled = false;
        currSceneIndex = SceneManager.GetActiveScene().buildIndex;

        ropeMeshRenderer = FindObjectOfType<RopeWrap>().gameObject.GetComponent<MeshRenderer>();
        playerSpriteRenderer = FindObjectOfType<Player2D>().GetComponent<SpriteRenderer>();
        enemySpriteRenderer = FindObjectOfType<Enemy2D>().GetComponent<SpriteRenderer>();

        levelWinConfetti.SetActive(false);
        levelLosePanel.SetActive(false);
    }


    void NoStretch()
    {
        FindObjectOfType<ObiRope>().stretchingScale = 0.1f;
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        if (Input.GetMouseButtonUp(0) && canPullTarget && canInteract)
        {
            ChangeRopeColor(1);
            GotTarget();
            canInteract = false;
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
        playerSpriteRenderer.sprite = winSprite;
        enemySpriteRenderer.sprite = enemyWinSprite;
        Time.timeScale = 1f;

        target.SetActive(false);

        foreach (GameObject item in setInactiveStuff)
        {
            item.SetActive(false);
        }
        Invoke(nameof(SetWinConfettiActive), .5f);
        Invoke(nameof(RestartLevel), 2f);

    }
    public void LevelFail()
    {
        playerSpriteRenderer.sprite = loseSprite;
        enemySpriteRenderer.sprite = enemyLoseSprite;
        enemySpriteRenderer.flipX = false;

        target.SetActive(false);

        foreach (GameObject item in setInactiveStuff)
        {
            item.SetActive(false);
        }
        // Invoke(nameof(SetLosePanelActive), 1f);
        Invoke(nameof(RestartLevel), 2f);

    }

    public void SetWinConfettiActive()
    {
        levelWinConfetti.SetActive(true);
    }


    public void SetLosePanelActive()
    {
        levelLosePanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currSceneIndex);
    }
    public void LoadNextLevel()
    {
        if (currSceneIndex > 0 && currSceneIndex < 2)
        {
            SceneManager.LoadScene(currSceneIndex + 1);
        }
        if (currSceneIndex == 2)
        {
            SceneManager.LoadScene(0);
        }
    }
}
