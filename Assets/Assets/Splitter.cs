using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using System.Diagnostics;
using JetBrains.Annotations;
public class Splitter : MonoBehaviour
{
    public Material matCross;

    void Start()
    {

    }


    void Update()
    {
        float mx = Input.GetAxis("Mouse X");

        transform.Rotate(0, 0, -mx);

        if (Input.GetMouseButtonDown(0))
        {
            Collider[] colliders = Physics.OverlapBox(transform.position,
            new Vector3(4, 0.005f, 5), transform.rotation, ~LayerMask.GetMask("Solid"));
            Collider collider=transform.GetComponent<Collider>();
            collider = null;
            foreach (Collider c in colliders)
            {
                if (c.CompareTag("slicedObj"))
                {
                    Destroy(c.gameObject);
                //GameObject[] objs = c.gameObject.SliceInstantiate(transform.position, transform.up);
                SlicedHull hull = c.gameObject.Slice(transform.position, transform.up);
                if (hull != null)
                {
                    GameObject lower = hull.CreateLowerHull(c.gameObject, matCross);
                    GameObject upper = hull.CreateUpperHull(c.gameObject, matCross);
                    GameObject[] objs = new GameObject[] { lower, upper };
                    lower.AddComponent<Rigidbody>();
                    foreach (GameObject obj in objs)
                    {
                            obj.tag = "slicedObj";
                            obj.AddComponent<MeshCollider>().convex = true ;
                        }
                    }
                }

            }
        }
    }
}
