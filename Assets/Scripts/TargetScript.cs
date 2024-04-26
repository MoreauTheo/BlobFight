using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetScript : MonoBehaviour
{
    //[HideInInspector]
    public Vector3 propulsionDirection;
    public float propulsionAttenuation;
    [HideInInspector]
    public List<GameObject> hitted;
    private float hue;
    private Vector3 rotationSlime;
    private Quaternion targetRotation = Quaternion.identity;
    public bool RGB;
    private enum Type { Feu,Eau,Vent,Terre} ;
    private Type typeSlime;
    public CollisionManager collisionManager;
    void Start()
    {
        if (RGB)
        {
            hue = Random.Range(0f, 1f);
            transform.GetChild(0).GetComponent<Renderer>().materials[1].color = Color.HSVToRGB(hue, 1, 1);
        }
        else
        {
            typeSlime = (Type)Random.Range(0,4);
        }

        collisionManager = GameObject.Find("GameManager").GetComponent<CollisionManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(RGB)
        {
            if(hue>=1)
            { hue = 0; }
            hue += Time.deltaTime/2;
            transform.GetChild(0).GetComponent<Renderer>().materials[1].color = Color.HSVToRGB(hue, 1, 1);
        }

        propulsionDirection.y = 0;
        transform.position += propulsionDirection * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
        propulsionDirection -= propulsionDirection.normalized * propulsionAttenuation * Time.deltaTime;
        
        if(propulsionDirection.magnitude > 0.1f)
        {
            transform.LookAt(transform.position+propulsionDirection);
        }
        else
        {
            propulsionDirection = Vector3.zero;
            SetBlendedEulerAngles();
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 150 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
       
           if(!collisionManager.allBlob.Contains(this))
           {
                collisionManager.allNormalContact.Add(other.GetContact(0).normal);
                collisionManager.allBlob.Add(this);
                if(other.collider.gameObject.layer == LayerMask.NameToLayer("Blob"))
                {
                    TargetScript targetScript = other.collider.GetComponent<TargetScript>(); 
                    if(propulsionDirection.magnitude==0)
                    {
                        collisionManager.allInitialDirection.Add(-other.GetContact(0).normal * targetScript.propulsionDirection.magnitude);
                        Debug.Log(targetScript.propulsionDirection + " : " + targetScript.propulsionDirection.magnitude);
                    }
                    else
                    {
                        collisionManager.allInitialDirection.Add(propulsionDirection);

                    }
                    collisionManager.allIsBlob.Add(true);
                    collisionManager.allCollisionDirection.Add(targetScript.propulsionDirection);
                }
                else
                {
                    collisionManager.allIsBlob.Add(false);
                    collisionManager.allInitialDirection.Add(propulsionDirection);
                    collisionManager.allCollisionDirection.Add(Vector3.zero);
                }
            
           }

        //            propulsionDirection = propulsionDirection - 2 * (Vector3.Dot(propulsionDirection, other.GetContact(0).normal.normalized)) * other.GetContact(0).normal.normalized;

        /*
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Blob"))
        {
            if (propulsionDirection.magnitude > 0.1f)
            {

                    other.collider.gameObject.GetComponent<TargetScript>().propulsionDirection = -other.GetContact(0).normal * propulsionDirection.magnitude;

            }
        }*/

    }

    //Direction de base - 2(direction de base . vecteur normal du plan de contact normalisé) * vecteur normal du plan de contact normalisé

    public void SetBlendedEulerAngles()
    {
        targetRotation = Quaternion.LookRotation(GameObject.Find("Player").transform.position - transform.position);
    }




}
