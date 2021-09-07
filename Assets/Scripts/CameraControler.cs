using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControler : MonoBehaviour
{
    public GameObject mouseOverObject;
    RaycastHit theObject;
    public float PanSpeed = 20f;
    public float BoarderThickness = 10f;

    public LayerMask layermask = 10;

    public float scrollSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        bool changed = false;

        if (Input.mousePosition.y >= (Screen.height - BoarderThickness))
        {
            pos.z += PanSpeed * Time.deltaTime;
            changed = true;

        };

        if (Input.mousePosition.y <= BoarderThickness)
        {
            pos.z -= PanSpeed * Time.deltaTime;
            changed = true;
        };

        if (Input.mousePosition.x >= (Screen.width - BoarderThickness))
        {
            pos.x += PanSpeed * Time.deltaTime;
            changed = true;
        };

        if (Input.mousePosition.x <= BoarderThickness)
        {
            pos.x -= PanSpeed * Time.deltaTime;
            changed = true;
        };
        //return if mouse is over UI or if a unit is moving
/*        if (!EventSystem.current.IsPointerOverGameObject())
        {*/
        float scroll = Input.GetAxis("Mouse ScrollWheel");        
        //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        pos.y -= scroll * 100f * scrollSpeed * Time.deltaTime;
        //}

        transform.position = pos;


        //selecting radar
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayLength = (mouseRay.origin.y / mouseRay.direction.y);

        Vector3 hitPos = mouseRay.origin - (mouseRay.direction * rayLength * 1000f);

        //finding which object mouse is hovering on
        if (Physics.Raycast(Camera.main.transform.position, hitPos, out theObject, Mathf.Infinity))
        {
            //Debug.Log("Hir objectt");
            mouseOverObject = theObject.transform.gameObject;
            //checking if hovering object has been changed o not

            //GameState.Instance.mouseOverObject = mouseOverObject.transform.parent.gameObject;
        }

        //when radar is selected 
        if (Input.GetMouseButtonDown(0) && mouseOverObject!=null)
        {
            //Debug.Log("Selected");
            Manager.Instance.selectRadar(mouseOverObject.GetComponent<Radar2>());
        }

        if (Input.GetMouseButtonDown(1) && mouseOverObject != null)
        {
            //Debug.Log("Selected");
            Manager.Instance.deselect();
        }

        //Debug.DrawRay(Camera.main.transform.position, hitPos);

    }
}
