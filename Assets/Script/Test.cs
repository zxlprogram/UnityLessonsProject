using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Test : MonoBehaviour
{
    public Material matCross;
    public float moveSpeed = 10f;
    public float dropDistance = 2f;

    [Header("時間設定")]
    public float downTime = 0.1f;
    public float stayTime = 0.1f;
    public float upTime = 0.5f;

    private bool isSlicing = false;

    void Update()
    {
        if (isSlicing) return;

        float mx = Input.GetAxis("Mouse X");
        transform.Translate(mx * moveSpeed * Time.deltaTime, 0, 0);

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SliceRoutine());
        }
    }

    IEnumerator SliceRoutine()
    {
        isSlicing = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.down * dropDistance;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / downTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        ExecuteSliceLogic();

        yield return new WaitForSeconds(stayTime);

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / upTime;
            transform.position = Vector3.Lerp(endPos, startPos, t);
            yield return null;
        }

        transform.position = startPos;
        isSlicing = false;
    }

    void ExecuteSliceLogic()
    {
        Vector3 halfExtents = new Vector3(4f, 1f, 5f);
        Collider[] colliders = Physics.OverlapBox(transform.position, halfExtents, transform.rotation);

        // 如果連碰撞都沒偵測到，輸出 Log
        if (colliders.Length == 0) Debug.Log("未偵測到任何碰撞體");

        foreach (Collider c in colliders)
        {
            if (!c.CompareTag("slicedObj")) continue;

            // 取得原始重量
            float originalMass = 1.0f;
            Rigidbody rbOriginal = c.GetComponent<Rigidbody>();
            if (rbOriginal != null) originalMass = rbOriginal.mass;

            // 執行切割
            SlicedHull hull = c.gameObject.Slice(transform.position, transform.right);

            if (hull != null)
            {
                GameObject lower = hull.CreateLowerHull(c.gameObject, matCross);
                GameObject upper = hull.CreateUpperHull(c.gameObject, matCross);

                // 計算兩者體積比例來分配重量
                float vLower = GetVolume(lower);
                float vUpper = GetVolume(upper);
                float totalV = vLower + vUpper;

                // 設定物理屬性與重量
                Rigidbody rbL = SetupPiece(lower, originalMass * (vLower / totalV));
                Rigidbody rbU = SetupPiece(upper, originalMass * (vUpper / totalV));

                // *** 這裡就是你要的重量 Log ***
                Debug.Log($"<color=yellow>切割成功！</color>");
                Debug.Log($"下塊重量 (Lower): {rbL.mass} 份");
                Debug.Log($"上塊重量 (Upper): {rbU.mass} 份");

                Destroy(c.gameObject);

                if (GameManager.Instance != null)
                    GameManager.Instance.OnSliceFinished(rbL.mass + rbU.mass);
            }
            else
            {
                Debug.LogWarning("EzySlice: 切割失敗，可能是刀刃位置不在模型網格內");
            }
        }
    }

    // 輔助函式：設定碎片的物理組件
    Rigidbody SetupPiece(GameObject obj, float mass)
    {
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        obj.AddComponent<MeshCollider>().convex = true;
        rb.mass = mass;
        return rb;
    }

    // 輔助函式：估算網格體積
    float GetVolume(GameObject obj)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null) return 1f;
        Vector3 s = mesh.bounds.size;
        return s.x * s.y * s.z;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 gizmoPos = transform.position + (isSlicing ? Vector3.zero : Vector3.down * dropDistance);
        Gizmos.matrix = Matrix4x4.TRS(gizmoPos, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(8f, 2f, 10f));
    }
}