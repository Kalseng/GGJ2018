using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class GameManager : MonoBehaviour {

    // Publicly assigned variables for game control.
    public Ghost g1;
    public AntennaRotator ar;

    // For managing interaction on/off state.
    private bool interactableTV = false;

    // Variables for storing ghost locations. Consider moving to Ghost script.
    public float leftDiff, rightDiff;

    private Quaternion ghostLeftQuat, ghostRightQuat;
    private Quaternion antennaLeftQuat, antennaRightQuat;

    // Variables for round timer
    public Image clock;
    public float roundLength = 30.0f;

    private float roundStartTime;

    // Lists of ghosts for rounds, denomonated by last number.
    private List<List<Ghost>> ghostRounds;
    public List<Ghost> ghostList1, ghostList2, ghostList3, ghostList4;

    // Round index
    public int roundIndex;

	// Use this for initialization
	void Start () {
        ghostRounds.Add(ghostList1);
        ghostRounds.Add(ghostList2);
        ghostRounds.Add(ghostList3);
        ghostRounds.Add(ghostList4);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Start"))
        {
            FreshStart();
        }

        if (interactableTV)
        {
            ProcessAntennae();
            ProcessTime();
        }
        else{
            GamePad.SetVibration(0, 0, 0);
        }
        
	}

    void ProcessTime()
    {
        if(Time.time - roundStartTime >= roundLength){
            clock.fillAmount = 0.0f;
            interactableTV = false;
        }
        clock.fillAmount = 1 - (Time.time - roundStartTime) / roundLength;
    }

    void ProcessAntennae ()
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

        if (leftDiff < 5.0f || rightDiff < 5.0f)
        {
            GamePad.SetVibration(0, 1.0f - leftDiff / 5.0f, 1.0f - rightDiff / 5.0f);
        }
        else
        {
            GamePad.SetVibration(0, 0, 0);
        }
    }

    void FreshStart()
    {
        interactableTV = true;

        g1.RandomizeFrequency();

        g1.rightFrequency += ar.rightAntennaRotationStart;
        g1.leftFrequency += ar.leftAntennaRotationStart;

        roundStartTime = Time.time;
    }
}
