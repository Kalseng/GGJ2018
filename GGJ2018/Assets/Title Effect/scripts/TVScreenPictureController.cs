using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVScreenPictureController : MonoBehaviour {

	// Public
	public RawImage staticImage;
	public float	initialStaticIntensity,
					imageCrossfadeTime;

	private void Start() {
		setStaticIntensity(initialStaticIntensity);
	}

	private void setStaticIntensity(float intensity) {
		staticImage.color = new Color(1, 1, 1, intensity);
	}
}
