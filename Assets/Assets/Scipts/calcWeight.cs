using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calcWeight : MonoBehaviour
{
    public float weight;
    public GameObject libra;
    public int whichPlat;
    public Color color;
    private HashSet<int> countedObjects = new HashSet<int>(); // 避免同一物件重複計入
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

    // Start is called before the first frame update
    void Start()
    {
        libra.GetComponent<Libra>().plat[whichPlat] = this.gameObject;
        gameObject.GetComponent<Renderer>().material.color=color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var mf = collision.gameObject.GetComponent<MeshFilter>();
        if (mf == null) return;

        // 若已經計入重量，不再重算
        int id = collision.gameObject.GetInstanceID();
        if (countedObjects.Contains(id)) return;
        countedObjects.Add(id);

        // 計算重量並將物件固定在秤盤上，不讓它穿透掉到底下
        weight += CalculateMeshVolume(mf.mesh);

        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }

        SnapOnTop(collision);

        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    private void SnapOnTop(Collision collision)
    {
        Collider panCol = GetComponent<Collider>();
        if (panCol == null) return;

        Collider objCol = collision.collider;
        if (objCol == null) return;

        Bounds objBounds = objCol.bounds;
        float panTop = panCol.bounds.max.y;
        float objHalfHeight = objBounds.extents.y;

        Vector3 pos = collision.transform.position;
        pos.y = panTop + objHalfHeight + 0.005f;
        collision.transform.position = pos;
    }
    IEnumerator DelayMethod(GameObject collision)
    {
        yield return new WaitForSeconds(1f);
        Destroy(collision); 
        gameObject.GetComponent<Renderer>().material.color=color;
    }
    private void OnCollisionExit(Collision collision)
    {
    }
}
