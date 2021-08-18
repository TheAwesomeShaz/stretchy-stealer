using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //the target has been acquired
        if (other.tag == "Hand" && gameController.gotTarget)
        {
            gameController.levelHasEnded = true;
            gameController.LevelWin();
        }
        // Debug.Log("EnteredTrigger");
    }
}
