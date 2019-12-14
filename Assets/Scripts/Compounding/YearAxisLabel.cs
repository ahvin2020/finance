using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YearAxisLabel : PoolObject
{
	public Text labelText;

	public void Init(int year, int totalYears) {
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = Vector3.one;

		labelText.text = year.ToString();

		float maxWidth = transform.parent.GetComponent<RectTransform>().rect.size.x;
		float xPosition = 0;
		if (totalYears > 0) {
			xPosition = maxWidth * year / totalYears;
		}

		//Debug.Log(year + " " + totalYears + " " + maxWidth + " " + (maxWidth * year / totalYears));
		float width = maxWidth / totalYears;
		rectTransform.sizeDelta = new Vector2(width, rectTransform.rect.size.y);
		rectTransform.anchoredPosition = new Vector2(xPosition - width, 0);
	}
}
