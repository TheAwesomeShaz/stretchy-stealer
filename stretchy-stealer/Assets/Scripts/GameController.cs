using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    bool levelHasEnded;
    [SerializeField] GameObject target;

    [SerializeField] SpriteRenderer handTargetSprite;
    SpriteRenderer playerSprite;

    [SerializeField] Material ropeNormalMaterial;
    [SerializeField] Material ropeSuccessMaterial;
    Material ropeMaterial;



    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite winSprite;
    [SerializeField] Sprite loseSprite;
    [SerializeField] Sprite enemyIdleSprite;
    [SerializeField] Sprite enemyWinSprite;
    [SerializeField] Sprite enemyLoseSprite;
    public bool gotTarget;



    // Start is called before the first frame update
    void Start()
    {
        handTargetSprite.enabled = false;
        ropeMaterial = FindObjectOfType<RopeWrap>().ropeMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetMouseButton(0)) // holding mouse button
        {
            ChangeRopeColor();
        }

    }


    public void ChangeRopeColor()
    {
        if (gotTarget && !levelHasEnded)
        {
            ropeMaterial = ropeSuccessMaterial;
        }
        else
        {
            ropeMaterial = ropeNormalMaterial;
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
            Time.timeScale = 2;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
