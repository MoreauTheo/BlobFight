using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public List<Vector3> allInitialDirection;
    public List<Vector3> allNormalContact;
    public List<TargetScript> allBlob;
    public List<Vector3> allCollisionDirection;
    public List<bool> allIsBlob;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        for(int i = 0; i < allInitialDirection.Count; i++)
        {
            MakeChange(allInitialDirection[i], allNormalContact[i], allBlob[i], allIsBlob[i], allCollisionDirection[i]);
        }
        allBlob.Clear();
        allInitialDirection.Clear();
        allNormalContact.Clear();
        allIsBlob.Clear();
        allCollisionDirection.Clear();
    }
    
    public void MakeChange(Vector3 initalDirection,Vector3 normalContact,TargetScript blob ,bool isBlob,Vector3 collisionDirection = default(Vector3))
    {
        if(isBlob)
        {
            Debug.Log(blob.name + " : " + initalDirection + " + " + collisionDirection);
            blob.propulsionDirection = Vector3.Reflect(initalDirection, normalContact.normalized);
        }
        else
        {
            blob.propulsionDirection = Vector3.Reflect(initalDirection, normalContact.normalized);

        }
    }
}
