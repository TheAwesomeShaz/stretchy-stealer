using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HandController : MonoBehaviour
{
    public float speed = 1000;
    Rigidbody rb;
    private Camera.MonoOrStereoscopicEye hit;

    GameController gameController;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameController = FindObjectOfType<GameController>();
        speed = 10000;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GetComponentInChildren<BoxCollider2D>().enabled = true;


        if (Input.GetMouseButton(0) && GameController.instance.canInteractWithRope) // hold button
        {
            direction = (mousePosition - transform.position);
            rb.velocity = direction.normalized * speed * Time.deltaTime;
            if (gameController.gotTarget)
            {
                ResetRotation();
                return;
            }
            else
            {
                RotateArmToMouse();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            ResetRotation();

        }


        // rb.AddForce(direction.normalized * acceleration, ForceMode.Acceleration);
    }

    private void RotateArmToMouse()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}