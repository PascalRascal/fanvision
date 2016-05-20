using UnityEngine;
using System.Collections;

public class fire_fireworks : MonoBehaviour {
    public GameObject fireworkPrefab;
    public GameObject cam;
    public float interval; //Interval between fireworks
    public float duration; //Duration of the fireworks display
    private float magnitude = 10;
    private float timePeriod;
    private float x, y, z;

	// Use this for initialization
	void Start () {
        timePeriod = interval;
	    
	}
	
	// Update is called once per frame
	void Update () {
        duration -= Time.deltaTime;
        if (duration > 0)
        {
            timePeriod -= Time.deltaTime;
            if (timePeriod < 0)
            {
                timePeriod = interval;
                fireFirework();
            }
        }
	}

    void fireFirework()
    {
        //Spawns a firework at a random coordinate at a certain magnitude away
        int bias = 0;
        float h2 = magnitude * magnitude;
        if (bias == 0)
        {
            x = Random.Range(-Mathf.Sqrt(h2), Mathf.Sqrt(h2));
            h2 = h2 - (x * x);
            y = Random.Range(-Mathf.Sqrt(h2), Mathf.Sqrt(h2));
            h2 = h2 - (y * y);
            z = Random.Range(-Mathf.Sqrt(h2), Mathf.Sqrt(h2));
        }
        //else if (bias == 1)
        {
            y = Random.Range(-Mathf.Sqrt(h2), Mathf.Sqrt(h2));
            h2 = h2 - (y * y);
            x = Random.Range(-Mathf.Sqrt(h2), Mathf.Sqrt(h2));
            h2 = h2 - (x * x);
            z = Random.Range(-Mathf.Sqrt(h2), Mathf.Sqrt(h2));
        }
        //else if (bias == 2)
        {
            z = Random.Range(-Mathf.Sqrt(h2), Mathf.Sqrt(h2));
            h2 = h2 - (z * z);
            y = Random.Range(-Mathf.Sqrt(h2), Mathf.Sqrt(h2));
            h2 = h2 - (y * y);
            x = Random.Range(-Mathf.Sqrt(h2), Mathf.Sqrt(h2));
        }
   


        Vector3 fireworkVector = new Vector3(x, y, z);

        StartCoroutine(fireworkRoutine(fireworkVector));
    }



    IEnumerator fireworkRoutine(Vector3 position)
    {
        Instantiate(fireworkPrefab, cam.transform.position + position, cam.transform.rotation);
        yield return true; 
    }

}
