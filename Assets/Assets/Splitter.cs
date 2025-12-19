using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using System.Diagnostics;
using JetBrains.Annotations;
public class Splitter : MonoBehaviour
{
    public Material matCross;
    public static float CalculateMeshVolume(Mesh mesh, Transform transform = null)
    {
        if (mesh == null)
            return 0f;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        float volume = 0f;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            if (transform != null)
            {
                p1 = transform.TransformPoint(p1);
                p2 = transform.TransformPoint(p2);
                p3 = transform.TransformPoint(p3);
            }

            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }

        return Mathf.Abs(volume);
    }

    private static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return Vector3.Dot(p1, Vector3.Cross(p2, p3)) / 6f;
    }
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
                    foreach (GameObject obj in objs)
                    {
                            obj.tag = "slicedObj";
                            obj.AddComponent<Rigidbody>();
                            obj.AddComponent<MeshCollider>().convex = true;
                            UnityEngine.Debug.Log("mesh: "+ CalculateMeshVolume(obj.GetComponent<MeshFilter>().mesh));
                        }
                    }
                }

            }
        }
    }
}
