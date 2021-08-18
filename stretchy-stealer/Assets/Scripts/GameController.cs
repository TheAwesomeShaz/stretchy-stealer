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



    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 2f;
        FindObjectOfType<RopeTenser>().force = 200f;
        canInteract = false;

        Invoke(nameof(NoStretch), 1f);

        canInteract = true;
        instance = this;
        handTargetSprite.enabled = false;
        ropeMeshRenderer = FindObjectOfType<RopeWrap>().gameObject.GetComponent<MeshRenderer>();
        playerSpriteRenderer = FindObjectOfType<Player2D>().GetComponent<SpriteRenderer>();
        enemySpriteRenderer = FindObjectOfType<Enemy2D>().GetComponent<SpriteRenderer>();
    }


    void NoStretch()
    {
        FindObjectOfType<ObiRope>().stretchingScale = 0;
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

        target.SetActive(false);

        foreach (GameObject item in setInactiveStuff)
        {
            item.SetActive(false);
        }

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
    }
}
