using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsSpawner : MonoBehaviour
{
    [Header("Craete Option")]

    [Header("1. Boundary Shape")]
    public bool circle;
    public bool rectangle;
    public float radius;

    [Header("2. MaxCreateCount")]
    public int createCount = 10;


    public GameObject prefabs;
    public List<Boids> allList = new List<Boids>();

    // Start is called before the first frame update
    void Start()
    {
        InitializeSetting();
    }

    void InitializeSetting()
    {
        for (int i = 0; i < createCount; i++)
        {
            if (circle)
            {
                Vector3 randPos = Random.insideUnitSphere;
                randPos *= radius;
                GameObject go = Instantiate(prefabs, transform.position + randPos, Quaternion.identity);
                go.transform.SetParent(transform);
                go.GetComponent<Boids>().spawn = this;
                allList.Add(go.GetComponent<Boids>());
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
