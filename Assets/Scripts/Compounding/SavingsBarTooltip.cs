/******************************************************************************************
 * Name: UITooltip.cs
 * Created by: Jeremy Voss
 * Created on: 02/20/2019
 * Last Modified: 02/27/2019
 * Description:
 * Used to display tooltips while the cursor is hovered over an object.
 ******************************************************************************************/
using PixelsoftGames;
using PixelsoftGames.PixelUI;
using UnityEngine;
using UnityEngine.UI;

public class SavingsBarTooltip : MonoBehaviour
{
	#region Fields & Properties
	public Text yearText;
	public Text totalDepositsText;
	public Text totalInterestText;
	public Text totalText;
	#endregion

	public SavingsData savingsData;

	#region Monobehavior Callbacks

	void Start() {
		HideTooltip();
	}

	/*
	private void OnEnable() {
		transform.position = Input.mousePosition;
		if (Input.mousePosition.x > Screen.width / 2)
			GetComponent<RectTransform>().pivot = new Vector2(1f, 0.5f);
		else
			GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
	}
	*/

	// Update is called once per frame
	private void Update() {
		transform.position = Input.mousePosition;

		transform.position = Input.mousePosition;
		if (Input.mousePosition.x > Screen.width / 2)
			GetComponent<RectTransform>().pivot = new Vector2(1f, 0.5f);
		else
			GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Sets the text for the tooltip.
	/// </summary>
	/// <param name="text">The text to be displayed.</param>
	public void SetText(SavingsData data) {
		savingsData = data;

		yearText.text = string.Format("Year {0}", data.year + 1);
		totalDepositsText.text = string.Format("${0}", Util.FormatDouble(data.deposit));
		totalInterestText.text = string.Format("${0}", Util.FormatDouble(data.interest));
		totalText.text = string.Format("${0}", Util.FormatDouble(data.total));
	}

	/// <summary>
	/// Convenience method for showing the tooltip.
	/// </summary>
	public void ShowTooltip() {
		gameObject.SetActive(true);
	}

	/// <summary>
	/// Convenience method for hiding the tooltip.
	/// </summary>
	public void HideTooltip() {
		gameObject.SetActive(false);
	}

	#endregion
}
