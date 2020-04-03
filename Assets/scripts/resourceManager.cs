using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resourceManager : MonoBehaviour
{
    public int remainingResource;
    public bool isOccupied;

    public float prod;
    // Start is called before the first frame update
    void Start()
    {
        remainingResource = Random.Range(10,30);
        isOccupied = false;
        prod = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
