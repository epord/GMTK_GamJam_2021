using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    public float speed = 1;

    private void Update()
    {
        Vector3 direction = new Vector3(-16, -1, 0);
        transform.position = transform.position + direction * Time.deltaTime * speed;
    }
}
