using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boids : MonoBehaviour
{
    /*
     * separation
     * alignment
     * cohesion
     * boundary
     * obstacle
     */

     public enum EBoidType
    {
        Fish1,
        Fish2,
        Fish3,
        Fish4,
    }
    public EBoidType type = EBoidType.Fish1;

    public BoidsSpawner spawn;

    public List<Boids> neighbours = new List<Boids>();
    Coroutine findBoidCoroutineDis;
    Coroutine findBoidCoroutine;
    Coroutine aiMoveCoroutine;

    [Space(10)]
    [Header("Option")]
    public Vector3 velocity;

    public float FOV = 120;           //시야
    public float speed = 1f;         //이동속도
    public float additionalSpeed;   //가속도
    public int maxNeighbour = 5;    //최대 무리 갯수

    [Header("추격 이동")]
    public Vector3 alignmentVec;
    public float alignmentWeight = 1f;

    [Header("회피 이동")]
    public Vector3 separationVec;
    public float separationWeight = 1f;

    [Header("지역 중심 이동")]
    public Vector3 boundaryVec;
    public float boundaryWeigth = 1f;

    [Header("무리 중심 이동")]
    public Vector3 cohesionVec;
    public float cohesionWeight = 1f;

    [Header("장애물 회피 이동")]
    public Vector3 obstacleVec;
    public float obstacleWeight = 1f;
    public LayerMask obstacleLayer;

    [Header("랜덤 이동")]
    public Vector3 aiVec;
    public float aiWeight = 1f;


    public float findDistance = 10;



    private void Start()
    {
        findBoidCoroutineDis = StartCoroutine("FindBoidCoroutineDis");
        aiMoveCoroutine = StartCoroutine("AIMoveCoroutine");
    }

    int idx = 0;
    IEnumerator FindBoidCoroutineDis()
    {
        neighbours.Clear();

        idx = 0;
        foreach (Boids boid in spawn.allList.Where(b => b != this &&
        Vector3.Angle(transform.forward, b.transform.position - transform.position) <= FOV &&
        (b.transform.position - transform.position).magnitude <= findDistance && b.type == type))
        {
            if (neighbours.Count > maxNeighbour) break;
            idx++;
            neighbours.Add(boid);
        }
        separationWeight = 2;
        yield return YieldCache.WaitForSeconds(2f);
        separationWeight++;

        findBoidCoroutineDis = StartCoroutine("FindBoidCoroutineDis");
    }

    //IEnumerator FindBoidCoroutine()
    //{
    //    neighbours.Clear();

    //    Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, _saveArea);
    //    for (int i = 0; i < cols.Length; i++)
    //    {
    //        if (Vector3.Angle(-transform.right, cols[i].transform.position - transform.position) <= FOV)
    //        {
    //            if (cols[i].gameObject == gameObject || cols[i].GetComponent<Boid2D>() == null) continue;
    //            neighbours.Add(cols[i].GetComponent<Boids>());
    //        }
    //        if (i > maxNeighbour) break;
    //    }

    //    if (neighbours.Count == 0)
    //    {
    //        _saveArea += 10;
    //        if (_saveArea >= _maxNeighbourArea) _saveArea = _maxNeighbourArea;
    //    }
    //    else _saveArea = _neighbourArea;

    //    yield return YieldCache.WaitForSeconds(Random.Range(0.2f, 1f));

    //    findBoidCoroutine = StartCoroutine(FindBoidCoroutine());
    //}


    // Update is called once per frame
    void Update()
    {
        if (additionalSpeed > 0)
        {
            additionalSpeed -= Time.deltaTime;
        }

        alignmentVec = AlignmentCalc().normalized * alignmentWeight;
        separationVec = SeparationCalc().normalized * separationWeight;
        boundaryVec = BoundaryCalc().normalized * boundaryWeigth;
        cohesionVec = CohesionCalc().normalized * cohesionWeight;
        obstacleVec = ObstacleCalc().normalized * obstacleWeight;

        velocity = alignmentVec + separationVec + boundaryVec + cohesionVec + obstacleVec;
        //velocity = alignmentVec + separationVec + boundaryVec + cohesionVec + obstacleVec + aiVec;
        //velocity = boundaryVec;
        //velocity = velocity.normalized;

        velocity = Vector3.Lerp(transform.forward, velocity, Time.deltaTime);
        velocity.Normalize();

        transform.rotation = Quaternion.LookRotation(velocity);

        transform.position += velocity * speed * Time.deltaTime;
    }

    public void WeightSetting(int alignment = 1, int separation = 1, int boundary = 1, int cohesion = 1, int obstacle = 1)
    {
        alignmentWeight = alignment;
        separationWeight = separation;
        boundaryWeigth = boundary;
        cohesionWeight = cohesion;
        obstacleWeight = obstacle;
    }

    #region Calculate
    
     IEnumerator AIMoveCoroutine()
    {
        speed = Random.Range(3, 10);
        aiVec = Random.insideUnitSphere;

        yield return YieldCache.WaitForSeconds(Random.Range(2, 4));

        aiMoveCoroutine = StartCoroutine("AIMoveCoroutine");
    }

    //추격
    Vector3 AlignmentCalc()
    {
        Vector3 velo = Vector3.forward;

        if (neighbours.Count > 0)
        {

            for (int i = 0; i < neighbours.Count; i++)
            {
                velo += neighbours[i].transform.forward;
            }
        }
        else return velo;

        velo /= neighbours.Count;

        return velo;
    }

    //회피
    Vector3 SeparationCalc()
    {
        Vector3 velo = Vector3.zero;

        if (neighbours.Count > 0)
        {

            for (int i = 0; i < neighbours.Count; i++)
            {
                velo += (transform.position - neighbours[i].transform.position);
            }
        }
        else return Vector3.zero;

        velo /= neighbours.Count;
        return velo;
    }


    //중심으로 이동
    Vector3 CohesionCalc()
    {
        Vector3 velo = Vector3.zero;

        if (neighbours.Count > 0)
        {
            for (int i = 0; i < neighbours.Count; i++)
            {
                velo += neighbours[i].transform.position;
            }
            velo /= neighbours.Count;
            velo -= transform.position;
        }

        return velo;
    }

    public bool recall = false;
    public float centerDis;
    Vector3 BoundaryCalc()
    {
        Vector3 offset = (spawn.transform.position - transform.position);

        if (offset.magnitude >= spawn.radius)
        {
            return offset;
        }
        else
        {
            return Vector3.zero;
        }
    }

    Vector3 ObstacleCalc()
    {
        Vector3 velo = Vector3.zero;

        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, findDistance*2, obstacleLayer);
        if (hit.collider != null)
        {
            velo = hit.normal;
            additionalSpeed = 10f;
        }

        return velo;
    }
    #endregion

    //public float raydis = 5f;
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawRay(transform.position, velocity.normalized * raydis);
    //}
}
