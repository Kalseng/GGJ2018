using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVScreenAgitator : MonoBehaviour {

	public GameObject	titleText,
						titleTextContainer,
						scanlinePrefab,
						staticImage;
	public float 	offset,
					overshootMaximum,
					minTweenTime,
					maxTweenTime,
					minWaitTime,
					maxWaitTime,
					shiftXMaxAmount,
					shiftYMaxAmount,
					scanlineTweenTime,
					scanlineTweenAmount,
					staticXMaxAmount,
					staticYMaxAmount,
					staticMinTweenTime,
					staticMaxTweenTime;

	private bool swapped;

	private void Start() {
		StartCoroutine(swap());
		StartCoroutine(shift());
		StartCoroutine(staticShift());
		StartCoroutine(scan());
	}

	private IEnumerator swap() {

		float	tweenTime = Random.Range(minTweenTime, maxTweenTime),
				waitTime = Random.Range(minWaitTime, maxWaitTime);

		LeanTween.moveY(titleText, swapped ? offset : -offset, tweenTime).setEaseInOutBounce();
		swapped = !swapped;

		yield return new WaitForSeconds(tweenTime + waitTime);
		StartCoroutine(swap());
	}
	
	private IEnumerator shift() {

		float	tweenTime = Random.Range(minTweenTime, maxTweenTime),
				waitTime = Random.Range(minWaitTime, maxWaitTime),
				shiftYAmount = Random.Range(-shiftYMaxAmount, shiftYMaxAmount),
				shiftXAmount = Random.Range(-shiftXMaxAmount, shiftXMaxAmount);

		LeanTween.moveY(titleTextContainer, shiftYAmount, tweenTime).setEaseInOutBounce();
		LeanTween.moveX(titleTextContainer, shiftXAmount, tweenTime).setEaseInOutBounce();

		yield return new WaitForSeconds(tweenTime + waitTime);
		StartCoroutine(shift());
	}
	
	private IEnumerator staticShift() {

		float	tweenTime = Random.Range(staticMinTweenTime, staticMaxTweenTime),
				shiftYAmount = Random.Range(-staticYMaxAmount, staticYMaxAmount),
				shiftXAmount = Random.Range(-staticXMaxAmount, staticXMaxAmount);

		LeanTween.moveY(staticImage, shiftYAmount, tweenTime).setEaseInOutBounce();
		LeanTween.moveX(staticImage, shiftXAmount, tweenTime).setEaseInOutBounce();

		yield return new WaitForSeconds(tweenTime);
		StartCoroutine(staticShift());
	}

	private IEnumerator scan() {

		GameObject o = Instantiate(scanlinePrefab, Vector3.up * (scanlineTweenAmount / 2), Quaternion.identity);
		LeanTween.moveY(o, -scanlineTweenAmount, scanlineTweenTime);
		Destroy(o, scanlineTweenTime);

		yield return new WaitForSeconds(Random.Range(0, scanlineTweenTime * 2));
		StartCoroutine(scan());
	}
}
