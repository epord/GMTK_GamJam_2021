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
        float rotationVariation = Random.Range(0f, 360f);
        this.transform.Rotate(new Vector3(0, 0, 1), rotationVariation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
