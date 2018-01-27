using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    public AudioClip broadcastAudio;
    public Texture2D ghostPicture;

    public Vector3 leftFrequency, rightFrequency = new Vector3(0,0,0);

    public int channel;

	// Use this for initialization
	void Start () {
        RandomizeFrequency();
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
