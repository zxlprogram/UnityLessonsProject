using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Libra: MonoBehaviour
{
    public GameObject []plat=new GameObject[2];
    private Vector3[] platition=new Vector3[2];
    // Start is called before the first frame update
    void Start()
    {
        platition[0] = plat[0].transform.position;
        platition[1] = plat[1].transform.position;
    }

    void FixedUpdate()
    {
        float weight0 = plat[0].GetComponent<calcWeight>().weight;
        float weight1 = plat[1].GetComponent<calcWeight>().weight;

        // 计算位置偏移量
        Vector3 offset = new Vector3(0, 1, 0);

        if (plat[0].GetComponent<calcWeight>().weight > plat[1].GetComponent<calcWeight>().weight)
        {
            Debug.Log("Left side is heavier");
            transform.rotation = Quaternion.Euler(0, 0, 15);
            plat[0].transform.position = platition[0] - new Vector3(0, 1, 0);
            plat[1].transform.position = platition[1] + new Vector3(0, 1, 0);
        }
        else if (plat[0].GetComponent<calcWeight>().weight < plat[1].GetComponent<calcWeight>().weight)
        {
            Debug.Log("Right side is heavier");
            transform.rotation = Quaternion.Euler(0, 0, -15);
            plat[1].transform.position = platition[1] - new Vector3(0, 1, 0);
            plat[0].transform.position = platition[0] + new Vector3(0, 1, 0);
        }
        else
        {
            Debug.Log("Both sides are balanced");
            transform.rotation = Quaternion.Euler(0, 0, 0);
            plat[1].transform.position = platition[1];
            plat[0].transform.position = platition[0];

        }
        Debug.Log(plat[0].GetComponent<calcWeight>().weight +" "+ plat[1].GetComponent<calcWeight>().weight);
    }
}
    // Update is called once per frame
