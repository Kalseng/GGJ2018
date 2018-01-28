using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleWordSwapper : MonoBehaviour {

	public GameObject	titleText,
						titleTextContainer,
						scanlinePrefab;
	public float 	offset,
					overshootMaximum,
					minTweenTime,
					maxTweenTime,
					minWaitTime,
					maxWaitTime,
					shiftXMaxAmount,
					shiftYMaxAmount,
					scanlineTweenTime,
					scanlineTweenAmount;

	private bool swapped;

	private void Start() {
		StartCoroutine(swap());
		StartCoroutine(shift());
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

	private IEnumerator scan() {

		GameObject o = Instantiate(scanlinePrefab, Vector3.up * (scanlineTweenAmount / 2), Quaternion.identity);
		LeanTween.moveY(o, -scanlineTweenAmount, scanlineTweenTime);
		Destroy(o, scanlineTweenTime);

		yield return new WaitForSeconds(Random.Range(0, scanlineTweenTime * 2));
		StartCoroutine(scan());
	}
}
