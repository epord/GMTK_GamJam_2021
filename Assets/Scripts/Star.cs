using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float sizeVariation = Random.Range(0.90f, 1.1f);
        this.transform.localScale = new Vector3(sizeVariation, sizeVariation, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
