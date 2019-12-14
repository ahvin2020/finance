using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavingsBar : PoolObject
{
	public RectTransform depositBar;
	public RectTransform interestBar;
	//private int maxHeight;

	public void Init(SavingsData savingsData, double totalSavings) {
		GetComponent<RectTransform>().localScale = Vector3.one;

		float maxHeight = GetComponentInParent<RectTransform>().sizeDelta.y;
		depositBar.sizeDelta = new Vector2(depositBar.sizeDelta.x, (float)(savingsData.deposit / totalSavings * maxHeight));
		interestBar.sizeDelta = new Vector2(interestBar.sizeDelta.x, (float)(savingsData.interest / totalSavings * maxHeight));
	}
}
