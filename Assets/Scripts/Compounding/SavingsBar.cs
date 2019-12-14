using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SavingsBar : PoolObject, IPointerEnterHandler, IPointerExitHandler
{
	[Header("GameObjects")]
	public RectTransform depositBar;
	public RectTransform interestBar;

	[Header("Events")]
	public IntGameEvent pointerEnterSavingsBarEvent;
	public IntGameEvent pointerExitSavingsBarEvent;

	private int index;

	public void Init(SavingsData savingsData, double totalSavings) {
		index = savingsData.year;
		GetComponent<RectTransform>().localScale = Vector3.one;

		float maxHeight = transform.parent.GetComponent<RectTransform>().rect.size.y;
		depositBar.sizeDelta = new Vector2(depositBar.sizeDelta.x, (float)(savingsData.deposit / totalSavings * maxHeight));
		interestBar.sizeDelta = new Vector2(interestBar.sizeDelta.x, (float)(savingsData.interest / totalSavings * maxHeight));
	}

	public void OnPointerEnter(PointerEventData eventData) {
		pointerEnterSavingsBarEvent.Raise(index);
	}

	public void OnPointerExit(PointerEventData eventData) {
		pointerExitSavingsBarEvent.Raise(index);
	}
}
