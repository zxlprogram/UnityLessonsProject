using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using System.Diagnostics;
using JetBrains.Annotations;
public class Splitter : MonoBehaviour
{
    public Material matCross;
    public AudioSource sliceSE;

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
        //滑鼠控制刀
        float mx = Input.GetAxis("Mouse X");
        transform.Rotate(0, 0, -mx);

        if (Input.GetMouseButtonDown(0))
        {
            if (!GameManager.Instance.canSlice) return;
            DoSlice();
        }
    }
    public void DoSlice()  //切割功能
    {
        if (!GameManager.Instance.canSlice) return;

        Collider[] colliders = Physics.OverlapBox(
            transform.position,
            new Vector3(4, 0.005f, 5),
            transform.rotation
        );

        // 收集唯一目標（避免同一物件多個 collider）
        List<GameObject> targets = new List<GameObject>();
        HashSet<int> seen = new HashSet<int>();
        foreach (Collider c in colliders)
        {
            if (!c.CompareTag("slicedObj")) continue;

            GameObject target = c.attachedRigidbody ? c.attachedRigidbody.gameObject : c.transform.root.gameObject;
            if (target == null) continue;

            int id = target.GetInstanceID();
            if (seen.Contains(id)) continue;
            seen.Add(id);
            targets.Add(target);
        }

        if (targets.Count == 0) return;

        bool slicedAny = false;
        foreach (GameObject target in targets)
        {
            SlicedHull hull = target.Slice(transform.position, transform.up);
            if (hull == null) continue;

            // 播放音效
            if (!slicedAny && sliceSE != null)
            {
                sliceSE.Play();
            }

            GameObject lower = hull.CreateLowerHull(target, matCross);
            GameObject upper = hull.CreateUpperHull(target, matCross);

            Destroy(target);

            // 上半部留在原地，下半部掉到天平
            SetupPiece(upper, false);
            SetupPiece(lower, true);

            slicedAny = true;
        }

        if (slicedAny)
            GameManager.Instance.OnSliceFinished(0f);
    }
    void SetupPiece(GameObject obj, bool drop)  //設定切割後物件屬性
    {
        obj.tag = "slicedObj";

        MeshCollider mc = obj.AddComponent<MeshCollider>();
        mc.convex = true;

        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.useGravity = drop;
        rb.isKinematic = !drop;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }
}
