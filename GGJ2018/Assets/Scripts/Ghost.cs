using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    public AudioSource ghostSource;
    public Texture2D ghostPicture;
    public Transform vacTube;

    public Vector3 leftFrequency, rightFrequency = new Vector3(0,0,0);

    public int channel;

    private float maxVacCharge = 5.0f;
    public float vacCharge = 0.0f;

    public bool charging = false;

	// Use this for initialization
	void Start () {

        ghostSource = this.gameObject.GetComponent<AudioSource>();
        if (vacTube == null)
        {
            vacTube = GameObject.Find("GhostBody").transform;
        }
        RandomizeFrequency();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (vacCharge > 0 && !charging)
        {
            vacCharge -= Time.deltaTime / 2;
        }
	}

    public void RandomizeFrequency()
    {
        leftFrequency.x = Random.value * -30;
        leftFrequency.y = Random.value * -30;
        leftFrequency.z = 0;

        rightFrequency.x = Random.value * 30;
        rightFrequency.y = Random.value * 30;
        rightFrequency.z = 0;
    }

    public void FillVacTube()
    {
        vacCharge += Time.deltaTime;
        vacTube.localScale = new Vector3(1.0f, 1.0f, vacCharge / maxVacCharge);
    }

    public void CheckVacTube()
    {
        vacTube.localScale = new Vector3(1.0f, 1.0f, vacCharge / maxVacCharge);
    }
}
