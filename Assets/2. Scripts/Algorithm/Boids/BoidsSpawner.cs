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

    [Header("3. boid °¡ÁßÄ¡")]
    public int alignmentWeight = 1;
    public int separationWeight = 1;
    public int boundaryWeight = 1;
    public int cohesionWeight = 1;
    public int obstacleWeight = 1;



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
                go.GetComponent<Boids>().type = (Boids.EBoidType)Random.Range(0, 4);
                go.GetComponent<Boids>().spawn = this;
                go.GetComponent<Boids>().WeightSetting(alignmentWeight, separationWeight, boundaryWeight, cohesionWeight, obstacleWeight);

                allList.Add(go.GetComponent<Boids>());
            }
        }
    }


    void WeightSetting(int alignment = 1, int separation = 1, int boundary = 1, int cohesion = 1, int obstacle = 1)
    {
        foreach (Boids boid in allList)
        {
            boid.WeightSetting(alignment, separation, boundary, cohesion, obstacle);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            foreach (Boids boid in allList)
            {
                boid.speed++;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (Boids boid in allList)
            {
                boid.speed--;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            WeightSetting(alignmentWeight, separationWeight, boundaryWeight, cohesionWeight, obstacleWeight);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
