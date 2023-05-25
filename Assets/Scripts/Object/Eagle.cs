using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    [SerializeField, Range(0, 50)] float speed = 20;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);       
    }
}
