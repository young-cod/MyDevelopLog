using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager GM;

    public UIManager UM;


    private void Awake()
    {
        if (GM != null)
        {
            Destroy(gameObject);
        }
        else
        {
            GM = this;
            DontDestroyOnLoad(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UM = transform.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
