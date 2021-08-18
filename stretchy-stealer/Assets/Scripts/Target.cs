using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
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
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Hand")
        {
            // gameController.GotTarget();
            gameController.PullTarget();
            if (Input.GetMouseButtonUp(0))
            {
                gameController.GotTarget();
            }
            // Debug.Log("STAYING TRIGGER");

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Hand")
        {
            gameController.canPullTarget = false;
            gameController.ChangeRopeColor(1);
        }
    }
}
