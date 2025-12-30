using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using System.Diagnostics;
using JetBrains.Annotations;
public class Splitter : MonoBehaviour
{
    public Material matCross;

    public enum OwnerPlayer
    {
        Player1,
        Player2
    }
    public OwnerPlayer ownerPlayer;
    void Start()
    {

    }


    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        transform.Rotate(0, 0, -mx);

        if (Input.GetMouseButtonDown(0))
        {
            if (!GameManager.Instance.canSlice) return;
            DoSlice();
            /*Collider[] colliders = Physics.OverlapBox(transform.position,
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

            }*/
        }
    }
    public void DoSlice()  //切割功能
    {
        Collider[] colliders = Physics.OverlapBox(
            transform.position,
            new Vector3(4, 0.005f, 5),
            transform.rotation
        );

        foreach (Collider c in colliders)
        {
            if (!c.CompareTag("slicedObj")) continue;

            SlicedHull hull = c.gameObject.Slice(transform.position, transform.up);
            if (hull == null) continue;

            GameObject lower = hull.CreateLowerHull(c.gameObject, matCross);
            GameObject upper = hull.CreateUpperHull(c.gameObject, matCross);

            Destroy(c.gameObject);

            SetupPiece(upper, false); //上面不掉下面掉
            SetupPiece(lower, true);

            GameManager.Instance.OnSliceFinished(0f);
        }
    }
    void SetupPiece(GameObject obj, bool drop)  //設定切割後物件屬性
    {
        obj.tag = "slicedObj";

        MeshCollider mc = obj.AddComponent<MeshCollider>();
        mc.convex = true;

        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.useGravity = drop;
        rb.isKinematic = !drop;
    }
}
