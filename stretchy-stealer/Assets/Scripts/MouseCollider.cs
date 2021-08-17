using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCollider : MonoBehaviour
{
    Line line;
    Vector3 mousePos;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        line = FindObjectOfType<Line>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.transform.name);
        Debug.Log("it work?");
        if (other.tag == "wrappable")
        {
            RaycastDown();
            RaycastUp();
        }
    }

    private void RaycastUp()
    {
        RaycastHit2D hit2DUp = Physics2D.Raycast(mousePos, Vector2.up, 1f);
        if (hit2DUp)
        {
            Debug.Log(hit2DUp.transform.name);
            // Vector2 hitpointOnCollider = hit2D.collider.GetComponent<PolygonCollider2D>().ClosestPoint(hit2D.point);
            // AddPositionToRope(hitpointOnCollider);
            Vector2 closestPointOnCollider = line.GetClosestColliderPointFromRaycastHit(hit2DUp, hit2DUp.collider.GetComponent<PolygonCollider2D>());
            line.AddPositionToRope(closestPointOnCollider);
            line.SetLastPosToMousePos();
        }
    }

    private void RaycastDown()
    {
        RaycastHit2D hit2DDown = Physics2D.Raycast(mousePos, Vector2.down, 1f);
        if (hit2DDown)
        {
            Debug.Log(hit2DDown.transform.name);
            // Vector2 hitpointOnCollider = hit2D.collider.GetComponent<PolygonCollider2D>().ClosestPoint(hit2D.point);
            // AddPositionToRope(hitpointOnCollider);
            Vector2 closestPointOnCollider = line.GetClosestColliderPointFromRaycastHit(hit2DDown, hit2DDown.collider.GetComponent<PolygonCollider2D>());
            line.AddPositionToRope(closestPointOnCollider);
            line.SetLastPosToMousePos();
        }
    }
}
