using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeWrap : MonoBehaviour
{
    public Material ropeMaterial;
    // Start is called before the first frame update
    void Start()
    {
        ropeMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
