using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class GameManager : MonoBehaviour {

    // Publicly assigned variables for game control.
    public Ghost g1;
    public AntennaRotator ar;
    public Transform dial; // Rotates on x axis
    public Transform vt;
    public GameObject pressToStart, controls;
    public float maxVacCharge; // In seconds

    // For channel/gamepad management
    public GamePadState gpState;
    public int currentChannel = 0;

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

    public AudioSource clientSource;

    // Lists of ghosts for rounds, denomonated by last number.
    // In the inspector, when assigning ghosts to spaces in the list, keep
    // in mind the ghost in index 0 is the "main ghost", and subsequent ghosts
    // are "sidequest ghosts".
    private List<List<Ghost>> ghostRounds;

    public List<AudioClip> clientClips;
    public List<Ghost> ghostList1, ghostList2, ghostList3, ghostList4;

    // Round index
    public int roundIndex = -1;

	// Use this for initialization
	void Start () {

        

        // Set gpState to the gamepad state of the first player.
        gpState = GamePad.GetState(0);


        // Instantiates ghostRounds list and stores other ghost lists
        ghostRounds = new List<List<Ghost>>() { ghostList1, ghostList2, ghostList3, ghostList4 };

        //
        
        foreach (List<Ghost> l in ghostRounds)
        {
            List<int> nums = new List<int>() { 0, 1, 2, 3 };
            foreach(Ghost g in l)
            {
                int c = Random.Range(0, nums.Count-1);
                g.channel = nums[c];
                nums.RemoveAt(c);

                g.RandomizeFrequency();

                g.rightFrequency += ar.rightAntennaRotationStart;
                g.leftFrequency += ar.leftAntennaRotationStart;
                
                if (ghostRounds.IndexOf(l) == 0)
                {
                }

            }
            nums.Clear();
        }

        // Don't show controls at start
        controls.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // Gamepad state needs updated every frame.
        gpState = GamePad.GetState(0);

        // Debugging tool for initiating a dummie round
        if (Input.GetButtonDown("Start"))
        {

            // FreshStart();
            StartCoroutine(NextRound());
        }

        // Controls TV interaction. Deactivates sex toy mode on 
        // controller if not in use.
        if (interactableTV)
        {
            CheckGhosts();
            ProcessTime();
            ProcessChannel();
        }
        else{
            GamePad.SetVibration(0, 0, 0);
        }
        
	}

    void ProcessTime()
    {
        // Manage round timer via "clock" fill percentage
        if(Time.time - roundStartTime >= roundLength){
            clock.fillAmount = 0.0f;
            interactableTV = false;
        }
        clock.fillAmount = 1 - (Time.time - roundStartTime) / roundLength;
    }

    void CheckGhosts ()
    {

        ar.ProessRotation();

        Ghost lastghost = new Ghost();

        foreach (Ghost g in ghostRounds[roundIndex])
        {

            ghostLeftQuat = Quaternion.Euler(g.leftFrequency);
            ghostRightQuat = Quaternion.Euler(g.rightFrequency);

            antennaLeftQuat = Quaternion.Euler(ar.leftAntenna.eulerAngles);
            antennaRightQuat = Quaternion.Euler(ar.rightAntenna.eulerAngles);

            leftDiff = Quaternion.Angle(ghostLeftQuat, antennaLeftQuat);
            rightDiff = Quaternion.Angle(ghostRightQuat, antennaRightQuat);

            leftDiff = 180.0f - leftDiff;
            rightDiff = 180.0f - rightDiff;
            
            // Very good code that changes the static intensity based on how close you are to succeeding
            TVScreenPictureController tvspc = FindObjectOfType<TVScreenPictureController>();
            float intensity = (leftDiff + rightDiff) / 36;
            tvspc.setStaticIntensity(intensity);

            Debug.Log(rightDiff);
            Debug.Log(leftDiff);
            Debug.Log(g.channel);

            if ((leftDiff < 5.0f || rightDiff < 5.0f) && (g.channel == currentChannel))
            {
                lastghost = g;
                GamePad.SetVibration(0, 1.0f - leftDiff / 5.0f, 1.0f - rightDiff / 5.0f);
                Debug.Log((10.0f - (leftDiff + rightDiff)) / 10.0f);
                if (leftDiff + rightDiff < 10)
                {
                    g.ghostSource.volume = ((10.0f - (leftDiff + rightDiff)) / 10.0f)/2 + 5;
                }
                if (leftDiff + rightDiff < 5)
                {
                    g.charging = true;
                    g.FillVacTube();
                    if (g.vacTube.localScale.z >= 1)
                    {
                        if(ghostRounds[roundIndex].IndexOf(g) == 0)
                        {
                            StartCoroutine(NextRound());
                        }
                        else
                        {
                            g.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    g.CheckVacTube();
                    g.charging = false;
                }
            }
            else
            {
                if (lastghost != null)
                {
                    lastghost.ghostSource.volume = 0;
                }
                GamePad.SetVibration(0, 0, 0);
            }
        }
    }

    void ProcessChannel()
    {
        int lastChannel = currentChannel;
        if(gpState.DPad.Up == ButtonState.Pressed)
        {
            currentChannel = 0;
        }
        else if(gpState.DPad.Right == ButtonState.Pressed)
        {
            currentChannel = 1;
        }else if(gpState.DPad.Down == ButtonState.Pressed)
        {
            currentChannel = 2;
        }else if(gpState.DPad.Left == ButtonState.Pressed)
        {
            currentChannel = 3;
        }

        if (lastChannel != currentChannel){
            dial.transform.localRotation = Quaternion.Euler(90 * currentChannel, 90, -90);
        }
        
    }

    void FreshStart()
    {
        interactableTV = true;

        roundStartTime = Time.time;

    }

    IEnumerator NextRound()
    {
        interactableTV = false;
        roundIndex++;

        if(roundIndex == 0)
        {

            // Switch from "press _ to start" to controls
            StartCoroutine(showControls());
        }

        foreach (List<Ghost> l in ghostRounds)
        {
            if (ghostRounds.IndexOf(l) != roundIndex)
            {
                foreach (Ghost g in l)
                {
                    g.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (Ghost g in l)
                {
                    g.gameObject.SetActive(true);
                }
            }
        }

        Debug.Log("finished activating/deactivating ghosts");

        if (roundIndex > 0)
        {
            clientSource.clip = clientClips[roundIndex - 1];
            Debug.Log("Set client clip");
            clientSource.Play();
            Debug.Log("Playing lient clip");
            yield return new WaitForSeconds(clientClips[roundIndex].length);
        }

        Debug.Log("Interaction possible");
        interactableTV = true;

        Debug.Log("Setting start time.");
        roundStartTime = Time.time;
    }

    // Show controls for a bit
    //TODO: alpha tween
    //TODO: go away faster if player moves sticks
    private IEnumerator showControls() {
        pressToStart.SetActive(false);
        controls.SetActive(true);
        yield return new WaitForSeconds(5f);
        controls.SetActive(false);
    }
}
