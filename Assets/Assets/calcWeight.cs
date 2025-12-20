using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calcWeight : MonoBehaviour
{
    public float weight;
    public GameObject libra;
    public int whichPlat;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        weight = CalculateMeshVolume(collision.gameObject.GetComponent<MeshFilter>().mesh);
        StartCoroutine(DelayMethod(collision));
    }
    IEnumerator DelayMethod(Collision collision)
    {
        yield return new WaitForSeconds(1f);
        Destroy(collision.gameObject); 
        weight = 0;
    }
    private void OnCollisionExit(Collision collision)
    {
    }
}
