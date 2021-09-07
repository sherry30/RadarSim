using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Jet : MonoBehaviour
{
    public List<float> speed;
    public List<float> longitude;
    public List<float> latitude;
    public List<float> time;
    public List<float> altitude;
    public int testDist = 0;
    public bool inFLight = false;
    public float delayTime = 5f;

    private float sLongitude;
    private float sLatitude;
    private float sTimer;
    private int valueCount;
    private int currentValue=0;
    private float x2,y2,d;
    private float timer, seconds, percent;
    private Vector3 startPos, Dest;
    // Start is called before the first frame update
    private void Start()
    {
        
        //reading from file
        speed = new List<float>();
        longitude = new List<float>();
        latitude = new List<float>();
        time = new List<float>();
        altitude = new List<float>();

        readCSV();

        //setting up for moving
        sLongitude = longitude[0];
        sLatitude = latitude[0];
        valueCount = longitude.Count;
        sTimer = time[0];
        startPos = transform.position;


        //starting
        currentValue++;
        calculateDest(currentValue);
        inFLight = true;


    }

    public void readCSV()
    {
        StreamReader strReader = new StreamReader("Assets/Data/C130.csv");
        bool end = false;
        int i = 0;
        while (!end)
        {
            string data = strReader.ReadLine();
            if (data == null)
            {
                end = true;
                break;
            }
            var dataVal = data.Split(',');
            //Debug.Log(dataVal[0].ToString() + " " + dataVal[1].ToString() + " " + dataVal[2].ToString() + " " + dataVal[3].ToString() + " " + dataVal[4].ToString());
            if (i == 0)
            {
                i++;
                continue;
            }
            time.Add(float.Parse(dataVal[0]));
            longitude.Add(float.Parse(dataVal[1]));
            latitude.Add(float.Parse(dataVal[2]));
            altitude.Add(float.Parse(dataVal[3]));
            speed.Add(float.Parse(dataVal[5]));

        }
    }
    private void Update()
    {
        if (!inFLight)
            return;
        if (timer <= seconds)
        {
            timer += Time.deltaTime;

            percent = timer / seconds;
            transform.position = startPos + (Dest - startPos) * percent;

        }
        else if(currentValue<valueCount)
        {
            currentValue++;
            calculateDest(currentValue);
            timer = 0f;
        }
        else
        {
            inFLight = false;
            Debug.Log("all values used");
        }
    }

    public void calculateDest(int value)
    {
        //setting previous values
        sTimer = time[value-1];
        startPos = transform.position;
        sLatitude = latitude[value - 1];
        sLongitude = longitude[value - 1];

        //calc
        float phi1 = sLatitude * Mathf.PI / 180f;
        float phi2 = latitude[value] * Mathf.PI / 180f;
        float del = (sLongitude - longitude[value]) * Mathf.PI / 180f;
        float R = 6371e3f;
        //Debug.Log("R: " + ( Mathf.Cos(phi1) * Mathf.Cos(phi2) * Mathf.Cos(del)));
        //float distance = Mathf.Acos(Mathf.Sin(phi1) * Mathf.Sin(phi2) + Mathf.Cos(phi1) * Mathf.Cos(phi2) * Mathf.Cos(del)) * R;
        //Debug.Log("Distance " + distance);
        float x = del * Mathf.Cos(phi1 + phi2) / 2;
        float y = phi2 - phi1;
        d = Mathf.Sqrt(x * x + y * y) * R;
        float angle = Mathf.Atan(d);
        y2 = 111.320f * (latitude[value] - sLatitude) + Mathf.Sin(angle) * d;
        x2 = Mathf.Sqrt(d * d - y2 * y2) + Mathf.Cos(angle) * d;
        seconds = time[value] - sTimer;
        seconds *= delayTime;
        Dest = new Vector3(startPos.x+y2, startPos.y, startPos.z+x2);

        //output
        Debug.Log("CUrrent value: " + currentValue);
        //Debug.Log("startPos " + startPos);
        Debug.Log("Destination " + Dest);
        Debug.Log("seconds to finish " + seconds);
        Debug.Log("Distance " + d);
    }

}
