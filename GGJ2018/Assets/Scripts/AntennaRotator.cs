using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class AntennaRotator : MonoBehaviour {

    public Transform leftAntenna, rightAntenna;
    public Vector3 leftAntennaRotationStart, rightAntennaRotationStart;
    public float sensitivityModifier = 30;

	// Use this for initialization
	void Start () {
        leftAntennaRotationStart = leftAntenna.localEulerAngles;
        rightAntennaRotationStart = rightAntenna.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ProessRotation()
    {
        float lhDegree = Input.GetAxis("HorizontalLeft") * -1 * sensitivityModifier;
        float lvDegree = Input.GetAxis("VerticalLeft") * -1 * sensitivityModifier;

        float rhDegree = Input.GetAxis("HorizontalRight") * sensitivityModifier;
        float rvDegree = Input.GetAxis("VerticalRight") * sensitivityModifier;

        leftAntenna.localRotation = Quaternion.Euler(leftAntennaRotationStart + new Vector3(lhDegree, lvDegree, 0));
        rightAntenna.localRotation = Quaternion.Euler(rightAntennaRotationStart + new Vector3(rhDegree, rvDegree, 0));
    }
}