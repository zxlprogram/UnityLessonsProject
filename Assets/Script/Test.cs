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

    [Header("偵測設定")]
    // 建議數值：X=4, Y=1, Z=5 (Y 不需要太大，因為下落時會持續掃描)
    public Vector3 detectArea = new Vector3(4f, 1f, 5f);

    [Header("UI 顯示")]
    public GameObject resultUI;

    private bool isSlicing = false;

    void Start()
    {
        if (resultUI != null) resultUI.SetActive(false);
    }

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
        bool hasSliced = false; // 用來確保一次下刀只切一次

        // --- 1. 下落階段 ---
        while (t < 1)
        {
            t += Time.deltaTime / downTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);

            // 在下落的每一幀都進行偵測，直到切到東西為止
            if (!hasSliced)
            {
                if (ExecuteSliceLogic())
                {
                    hasSliced = true; // 成功切到，標記起來
                }
            }
            yield return null;
        }

        // --- 2. 停頓階段 ---
        yield return new WaitForSeconds(stayTime);

        // --- 3. 回彈階段 ---
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

    // 將回傳值改為 bool，讓協程知道是否切到了
    bool ExecuteSliceLogic()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, detectArea, transform.rotation);
        bool hitAnything = false;

        foreach (Collider c in colliders)
        {
            if (!c.CompareTag("slicedObj")) continue;

            float originalMass = 1.0f;
            Rigidbody rbOriginal = c.GetComponent<Rigidbody>();
            if (rbOriginal != null) originalMass = rbOriginal.mass;

            // 執行切割
            SlicedHull hull = c.gameObject.Slice(transform.position, transform.right);

            if (hull != null)
            {
                GameObject lower = hull.CreateLowerHull(c.gameObject, matCross);
                GameObject upper = hull.CreateUpperHull(c.gameObject, matCross);

                // --- 1. 計算比例並存儲到 GameData ---
                float vLower = GetVolume(lower);
                float vUpper = GetVolume(upper);
                float totalV = vLower + vUpper;

                if (totalV > 0)
                {
                    GameData.UpperPercent = (vUpper / totalV) * 100f;
                    GameData.LowerPercent = (vLower / totalV) * 100f;
                    Debug.Log($"成功切割並儲存: 上 {GameData.UpperPercent:F1}% : 下 {GameData.LowerPercent:F1}%");
                }

                // --- 2. 顯示 Next 按鈕 ---
                if (resultUI != null) resultUI.SetActive(true);

                // --- 3. 處理物理 (包含過於複雜模型的報錯修正) ---
                SetupPiece(lower, originalMass * (vLower / totalV));
                SetupPiece(upper, originalMass * (vUpper / totalV));

                Destroy(c.gameObject);
                hitAnything = true;

                if (GameManager.Instance != null)
                    GameManager.Instance.OnSliceFinished(originalMass);
            }
        }
        return hitAnything;
    }

    Rigidbody SetupPiece(GameObject obj, float mass)
    {
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.mass = mass;

        MeshFilter mf = obj.GetComponent<MeshFilter>();
        MeshCollider mc = obj.AddComponent<MeshCollider>();

        if (mf != null && mf.sharedMesh != null)
        {
            mc.sharedMesh = mf.sharedMesh;
            if (mf.sharedMesh.vertexCount < 250)
            {
                mc.convex = true;
            }
            else
            {
                DestroyImmediate(mc);
                obj.AddComponent<BoxCollider>();
            }
        }
        return rb;
    }

    float GetVolume(GameObject obj)
    {
        MeshFilter mf = obj.GetComponent<MeshFilter>();
        if (mf == null || mf.sharedMesh == null) return 0.1f;

        Vector3 s = mf.sharedMesh.bounds.size;
        return s.x * s.y * s.z;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Gizmos 會跟著刀子的當前位置畫出偵測範圍
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, detectArea * 2);
    }
}