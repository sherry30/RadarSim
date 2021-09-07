using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Radar2 currentRadar;
    public static Manager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update

    public void selectRadar(Radar2 radar)
    {
        deselect();
        for(int i=0; i< radar.radar.childCount; i++)
        {
            Transform t = radar.radar.GetChild(i);
            if (t.CompareTag("Ping"))
            {
                Destroy(t.gameObject);
            }
        }
        currentRadar = radar;
        currentRadar.radar.gameObject.SetActive(true);
        currentRadar.activate();
    }

    public void deselect()
    {
        if (currentRadar != null){
            currentRadar.radar.gameObject.SetActive(false);
            currentRadar.deactivate();
        }
    }
}
