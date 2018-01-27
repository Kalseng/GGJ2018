using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class GameManager : MonoBehaviour {

    public Ghost g1;
    public AntennaRotator ar;

    private bool interactableTV = true;

    private Quaternion ghostLeftQuat, ghostRightQuat;
    private Quaternion antennaLeftQuat, antennaRightQuat;
    public float leftDiff, rightDiff;

	// Use this for initialization
	void Start () {

        g1.rightFrequency += ar.rightAntennaRotationStart;
        g1.leftFrequency += ar.leftAntennaRotationStart;

	}
	
	// Update is called once per frame
	void Update () {

        if (interactableTV)
        {
            {
                ar.ProessRotation();

                ghostLeftQuat = Quaternion.Euler(g1.leftFrequency);
                ghostRightQuat = Quaternion.Euler(g1.rightFrequency);

                antennaLeftQuat = Quaternion.Euler(ar.leftAntenna.eulerAngles);
                antennaRightQuat = Quaternion.Euler(ar.rightAntenna.eulerAngles);

                leftDiff = Quaternion.Angle(ghostLeftQuat, antennaLeftQuat);
                rightDiff = Quaternion.Angle(ghostRightQuat, antennaRightQuat);

                leftDiff = 180.0f - leftDiff;
                rightDiff = 180.0f - rightDiff;

                if (leftDiff < 5.0f || rightDiff < 5.0f )
                {
                    GamePad.SetVibration(0, 1.0f - leftDiff / 5.0f, 1.0f - rightDiff / 5.0f);
                }
                else
                {
                    GamePad.SetVibration(0, 0, 0);
                }
            }
            
        }
        
	}
}
