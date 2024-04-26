using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractTowerScript : TourBase
{
    [Header("Parameters")]
    public float radius;
    public float rotationSpeed;
    public float attractionSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectSlime();
        GetComponent<TourBase>().ApplyMovement();
    }

    void DetectSlime()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius );
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Blob") && hitCollider.gameObject.GetComponent<TargetScript>().propulsionDirection.magnitude > 0)
            {
                AttractSlime(hitCollider.gameObject);
            }
        }
    }

    private void AttractSlime(GameObject slime)
    {
        slime.transform.position = Vector3.MoveTowards(slime.transform.position, transform.position, attractionSpeed*Time.deltaTime);
        slime.GetComponent<TargetScript>().propulsionDirection = Vector3.RotateTowards(slime.GetComponent<TargetScript>().propulsionDirection, transform.position - slime.transform.position,rotationSpeed*Time.deltaTime,0);
    }
    

}
