using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourBase : MonoBehaviour
{
    public Vector3 propulsionDirection;
    public float propulsionAttenuation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void ApplyMovement()
    {
        propulsionDirection.y = 0;
        transform.position += propulsionDirection * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
        propulsionDirection -= propulsionDirection.normalized * propulsionAttenuation * Time.deltaTime;

        if (propulsionDirection.magnitude <= 0.1f)
        {
            propulsionDirection = Vector3.zero;
        }
    }
}
