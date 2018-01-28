using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVScreenPictureController : MonoBehaviour {

	// Public
	public RawImage staticImage,
					ghostImage;
	public float	initialStaticIntensity,
					imageCrossfadeTime;

	// Private
	private Texture2D frame1, frame2;
	private int currentFrame = 1;

	private void Start() {
		setStaticIntensity(initialStaticIntensity);
		StartCoroutine(toggleFrame());
	}

	public void setStaticIntensity(float intensity) {
		staticImage.color = new Color(1, 1, 1, intensity);
	}

	public void setGhostImages(Texture2D frame1, Texture2D frame2) {
		this.frame1 = frame1;
		this.frame2 = frame2;
	}

	private IEnumerator toggleFrame() {
		if(frame1 != null && frame2 != null) {
			if(currentFrame == 1) {
				currentFrame = 2;
				ghostImage.texture = frame2;
			}
			else {
				currentFrame = 1;
				ghostImage.texture = frame1;
			}
		}
		yield return new WaitForSeconds(Random.Range(0f, 2f));
		StartCoroutine(toggleFrame());
	}
}
