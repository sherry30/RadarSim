using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar2 : MonoBehaviour
{
    [SerializeField]
    private RectTransform radarPing;

    public Transform radar;
    public Transform sweepTransform;
    public float rotateSpeed;
    public float radius = 20f;    
    public LayerMask layerMask;
    public bool isActive = false;

    private List<string> colliders;
    

    private void Awake()
    {
        sweepTransform = radar.transform.Find("sweep");
        colliders = new List<string>();
    }

    private void Update()
    {
        if (!isActive)
            return;
        //float previousRotation = (sweepTransform.eulerAngles.z % 360) - 90;
        //rotating in radar UI
        //sweepTransform.eulerAngles -= new Vector3(0, 0, rotateSpeed* Time.deltaTime);

        /*float currentRotation = (sweepTransform.eulerAngles.z % 360 ) - 90;

        if(previousRotation<0 && currentRotation >= 0)
        {
            colliders.Clear();
            Debug.Log("List cleared");
        }*/


        //rotating in world
        Vector3 origin1 = transform.position;
        RaycastHit hit;
        Quaternion q = Quaternion.AngleAxis(rotateSpeed * Time.time, Vector3.up);
        //Debug.Log("Time: " + Time.time);
        Vector3 d = transform.forward * radius;
        Vector3 current = q * d;
        //current = new Vector3(current.x, 3f, current.z);

        /*currentRotation = (current.z % 360) - 90;
        if (previousRotation < 0 && currentRotation >= 0)
        {
            colliders.Clear();
            Debug.Log("List cleared");
        }
        previousRotation = currentRotation; */        
        if (Physics.Raycast(origin1, current, out hit))
        {
            /*if (!colliders.Contains(hit.collider.name))
            {
                colliders.Add(hit.collider.name);
                float xDist = hit.transform.position.x - origin1.x;
                float yDist = hit.transform.position.z - origin1.z;

                //for displaying on screen
                float xDisplay = (xDist / radius) * 250f;
                float yDisplay = (yDist / radius) * 250f;
                *//*Debug.Log("HIYT!!: " + hit.collider.name);
                Debug.Log("x Distance: " + (xDisplay));
                Debug.Log("y Distance: " + (yDisplay));*//*
                StartCoroutine(clearName(hit.collider.name));

                if (hit.distance <= radius)
                {
                    //ping
                    //RectTransform ping = Instantiate(radarPing, new Vector3(xDisplay, yDisplay, 0), Quaternion.identity);
                    RectTransform ping = Instantiate(radarPing, new Vector3(0, 0, 0), Quaternion.identity, radarUI.transform);
                    ping.localPosition = new Vector3(xDisplay, yDisplay, 0);
                    //ping.SetPositionAndRotation(new Vector3(xDisplay, yDisplay, 0), ping.rotation);
                }
                
            }*/
            
        }
        /*if(Physics.CheckSphere(origin1,radius,out hit, layerMask,QueryTriggerInteraction.UseGlobal ))
        {
            if (true)//!colliders.Contains(hit.collider.name))
            {
                colliders.Add(hit.collider.name);
                float xDist = hit.transform.position.x - origin1.x;
                float yDist = hit.transform.position.z - origin1.z;

                //for displaying on screen
                float xDisplay = (xDist / radius) * 250f;
                float yDisplay = (yDist / radius) * 250f;
                Debug.Log("HIYT!!: " + hit.collider.name);  
                *//*Debug.Log("x Distance: " + (xDisplay));
                Debug.Log("y Distance: " + (yDisplay));*//*
                StartCoroutine(clearName(hit.collider.name));

                if (hit.distance <= radius)
                {
                    //ping
                    //RectTransform ping = Instantiate(radarPing, new Vector3(xDisplay, yDisplay, 0), Quaternion.identity);
                    RectTransform ping = Instantiate(radarPing, new Vector3(0, 0, 0), Quaternion.identity, radar);
                    ping.localPosition = new Vector3(xDisplay, yDisplay, 0);
                    //ping.SetPositionAndRotation(new Vector3(xDisplay, yDisplay, 0), ping.rotation);
                }

            }
        }*/

        Collider[] hits = Physics.OverlapSphere(origin1, radius);
        foreach(var h in hits)
        {
            if (h.transform == transform || colliders.Contains(h.name))
            {
                continue;
            }
            if (!h.gameObject.CompareTag("Enemy"))
            {
                continue;
            }
            colliders.Add(h.name);
            StartCoroutine(clearName(h.name));
            //Debug.Log("HIYT!!: " + h.name);

            //displaying
            float xDist = h.transform.position.x - origin1.x;
            float yDist = h.transform.position.z - origin1.z;

            //for displaying on screen
            float xDisplay = (xDist / radius) * 250f;
            float yDisplay = (yDist / radius) * 250f;

            //ping
            //RectTransform ping = Instantiate(radarPing, new Vector3(xDisplay, yDisplay, 0), Quaternion.identity);
            if (Vector3.Distance(h.transform.position, transform.position) <= radius)
            {
                RectTransform ping = Instantiate(radarPing, new Vector3(0, 0, 0), Quaternion.identity, radar);
                ping.localPosition = new Vector3(xDisplay, yDisplay, 0);
            }

        }
        
        
        //Debug.DrawRay(origin1, new Vector3(0,0,0), Color.green);
    }
    IEnumerator clearName(string name)
    {
        yield return new WaitForSeconds(1f);
        colliders.Remove(name);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void activate()
    {
        isActive = true;
    }
    public void deactivate()
    {
        isActive = false;
    }

}
