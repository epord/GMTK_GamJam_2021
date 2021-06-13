using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Function { COS, SIN };

public class RotateLoop : MonoBehaviour
{
    public float speed = 1;
    public float amplitude = 1;
    public Function function = Function.SIN;


    // Update is called once per frame
    void Update()
    {
        if (function == Function.SIN)
            this.transform.Rotate(new Vector3(0, 0, Mathf.Sin(Time.time * speed) / amplitude));
        else
            this.transform.Rotate(new Vector3(0, 0, Mathf.Cos(Time.time * speed) / amplitude));
    }
}
