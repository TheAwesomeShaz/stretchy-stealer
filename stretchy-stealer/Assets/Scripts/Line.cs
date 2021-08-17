using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Line : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    Vector3 mousePos;
    Vector3 mouseDir;
    Camera cam;
    LineRenderer lr;
    LayerMask whatIsWrappable;
    bool positionAdded = false;

    RaycastHit2D hit2D;
    RaycastHit2D hit2Dup;

    public List<Vector3> ropePositions = new List<Vector3>();

    private Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        cam = Camera.main;
        ropePositions.Add(transform.position);
        ropePositions.Add(mousePos);

    }


    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseDir = mousePos - gameObject.transform.position;
        mouseDir.z = 0;
        mouseDir = mouseDir.normalized;

        if (Input.GetMouseButtonDown(0)) // oN CLICK
        {
            lr.enabled = true;

        }
        if (Input.GetMouseButton(0)) // on hold button
        {
            startPos = gameObject.transform.position;
            startPos.z = 0;
            lr.SetPosition(0, startPos);
            SetLastPosToMousePos();

            // hit2D = Physics2D.Raycast(transform.position, mouseDir, 100f); //raycast from line origin to mouse position


            // TODO: debug purposes
            RaycastDown();
            RaycastUp();

        }
        if (Input.GetMouseButtonUp(0))
        {

        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ShrinkRope();
        }


        Debug.DrawRay(mousePos, Vector2.down, Color.green);



    }

    public void RaycastUp()
    {
        hit2Dup = Physics2D.Raycast(mousePos, Vector2.up, 1f);
        if (hit2Dup)
        {
            Debug.Log(hit2Dup.transform.name);
            // Vector2 hitpointOnCollider = hit2D.collider.GetComponent<PolygonCollider2D>().ClosestPoint(hit2D.point);
            // AddPositionToRope(hitpointOnCollider);
            Vector2 closestPointOnCollider = GetClosestColliderPointFromRaycastHit(hit2Dup, hit2Dup.collider.GetComponent<PolygonCollider2D>());
            AddPositionToRope(closestPointOnCollider);
            SetLastPosToMousePos();
        }
    }

    public void RaycastDown()
    {
        hit2D = Physics2D.Raycast(mousePos, Vector2.down, 1f);
        if (hit2D)
        {
            Debug.Log(hit2D.transform.name);
            // Vector2 hitpointOnCollider = hit2D.collider.GetComponent<PolygonCollider2D>().ClosestPoint(hit2D.point);
            // AddPositionToRope(hitpointOnCollider);
            Vector2 closestPointOnCollider = GetClosestColliderPointFromRaycastHit(hit2D, hit2D.collider.GetComponent<PolygonCollider2D>());
            AddPositionToRope(closestPointOnCollider);
            SetLastPosToMousePos();
        }
    }

    public Vector2 GetClosestColliderPointFromRaycastHit(RaycastHit2D hit, PolygonCollider2D polyCollider)
    {
        // 2
        var distanceDictionary = polyCollider.points.ToDictionary<Vector2, float, Vector2>(
            position => Vector2.Distance(hit.point, polyCollider.transform.TransformPoint(position)),
            position => polyCollider.transform.TransformPoint(position));

        // 3
        var orderedDictionary = distanceDictionary.OrderBy(e => e.Key);
        return orderedDictionary.Any() ? orderedDictionary.First().Value : Vector2.zero;
    }


    public void SetLastPosToMousePos()
    {
        endPos = mousePos;
        endPos.z = 0;
        if (ropePositions.Count == 2)
        {
            lr.SetPosition(1, endPos);
            Debug.Log(ropePositions.Count);
        }
        else
        {
            lr.SetPosition(ropePositions.Count - 1, endPos);

        }
    }

    public void AddPositionToRope(Vector2 position)
    {
        // it is +1 than what indices we have so basically increasing the no of vertices
        lr.positionCount = ropePositions.Count + 1;
        ropePositions.Add(position);

        Debug.Log("New positions are " + ropePositions.Count);

        // we are setting the the position at the second last position (since index starts from 0)
        lr.SetPosition(ropePositions.Count - 2, position);

        SetLastPosToMousePos();
    }

    void ShrinkRope()
    {
        StartCoroutine(ShirnkRopeAfterTime());
    }

    IEnumerator ShirnkRopeAfterTime()
    {

        for (int i = ropePositions.Count - 1; i > 0; i--)
        {
            yield return new WaitForSeconds(.01f);
            lr.positionCount--;
        }
        // lr.positionCount = 2;
    }
}
