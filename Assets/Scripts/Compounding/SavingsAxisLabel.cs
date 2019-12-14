using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavingsAxisLabel : PoolObject
{
	public Text labelText;

	public void Init(double currentSavings, double totalSavings) {
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = Vector3.one;
		
		labelText.text = Util.ToSI(currentSavings);

		float maxHeight = transform.parent.GetComponent<RectTransform>().rect.size.y;
		float yPosition = 0;
		if (totalSavings > 0) {
			yPosition = (float)(currentSavings / totalSavings * maxHeight);
		}

		rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPosition);

		rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y);
		rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y);
	}
}
