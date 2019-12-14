using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompoundMenu : MonoBehaviour
{
	enum FrequencyEnum
	{
		Monthly,
		Quarterly,
		Annually
	}

	const double DEFAULT_INTEREST_RATE = 4;
	const double DEFAULT_YEARS = 10;

	[Header("SO")]
	public DynamicPoolSO dynamicPoolSO;

	[Header("Inputs")]
	public InputField initialDepositInput;
	public InputField regularDepositInput;
	public InputField numberOfYearsInput;
	public Dropdown depositFrequencyDropdown;
	public Dropdown compoundFrequencyDropdown;
	public InputField interestRateInput;

	[Header("Graph")]
	public Transform savingsAxisLabelContainer;
	public SavingsAxisLabel savingsAxisLabelPrefab;

	public Transform yearAxisLabelContainer;
	public YearAxisLabel yearAxisLabelPrefab;

	public Transform savingsBarContainer;
	public SavingsBar savingsBarPrefab;

	public SavingsBarTooltip savingsBarTooltip;

	[Header("Result Texts")]
	public Text totalDepositsAmountText;
	public Text totalInterestAmountText;
	public Text totalSavingsAmountText;

	// results
	private double initialDeposit;
	private double regularDeposit;
	private int numberOfYears;
	private FrequencyEnum depositFrequency;
	private FrequencyEnum compoundFrequency;
	private double interestRate;
	private List<SavingsData> savingDatas;
	private double totalDeposits;
	private double totalInterests;
	private double totalSavings;

	void Start() {
		savingDatas = new List<SavingsData>();

		savingsBarPrefab.gameObject.SetActive(false);
		savingsAxisLabelPrefab.gameObject.SetActive(false);
		yearAxisLabelPrefab.gameObject.SetActive(false);

		dynamicPoolSO.ResetState();

		OnInterestRateEndEdit(DEFAULT_INTEREST_RATE.ToString());
		OnNumberOfYearsEndEdit(DEFAULT_YEARS.ToString());
	}

	#region Input
	public void OnInitialDepositEndEdit(string value) {
		initialDeposit = Util.ConvertStringToDouble(value);
		//initialDepositInput.text = Util.FormatDouble(initialDeposit);
		CalculateResult();
		DisplayResult();
	}

	public void OnRegularDepositEndEdit(string value) {
		regularDeposit = Util.ConvertStringToDouble(value);
		//regularDepositInput.text = Util.FormatDouble(regularDeposit);
		CalculateResult();
		DisplayResult();
	}

	public void OnNumberOfYearsEndEdit(string value) {
		numberOfYears = Util.ConvertStringToInt(value);
		//numberOfYears = Math.Min(numberOfYears, 50); // limit to 50 years
		numberOfYearsInput.text = Util.FormatInteger(numberOfYears);
		CalculateResult();
		DisplayResult();
	}

	public void OnDepositFrequencyValueChanged(int value) {
		depositFrequency = ConvertValueToFrequencyEnum(value);
		CalculateResult();
		DisplayResult();
	}

	public void OnCompoundFrequencyValueChanged(int value) {
		compoundFrequency = ConvertValueToFrequencyEnum(value);
		CalculateResult();
		DisplayResult();
	}

	public void OnInterestRateEndEdit(string value) {
		interestRate = Util.ConvertStringToDouble(value);
		interestRate = Math.Min(interestRate, 20); // limit to 20%
		interestRateInput.text = Util.FormatDouble(interestRate);
		CalculateResult();
		DisplayResult();
	}

	private FrequencyEnum ConvertValueToFrequencyEnum(int value) {
		FrequencyEnum frequencyEnum;

		switch (value) {
			case 0:
				frequencyEnum = FrequencyEnum.Monthly;
				break;
			case 1:
				frequencyEnum = FrequencyEnum.Quarterly;
				break;
			case 2:
				frequencyEnum = FrequencyEnum.Annually;
				break;
			default:
				frequencyEnum = FrequencyEnum.Monthly;
				break;
		}

		return frequencyEnum;
	}
	#endregion

	#region Calculate Result
	private void CalculateResult() {
		// reset
		savingDatas.Clear();
		totalDeposits = initialDeposit;
		totalInterests = 0;
		totalSavings = initialDeposit;

		// calculate savings for every year
		for (int i = 0; i<numberOfYears; i++) {
			totalDeposits += regularDeposit;
			totalInterests += (totalSavings + regularDeposit) * interestRate * 0.01f;
			totalSavings = totalDeposits + totalInterests;
			SavingsData savingData = new SavingsData(i, totalDeposits, totalInterests);
			savingDatas.Add(savingData);
		}
	}
	#endregion

	#region Result
	private void DisplayResult() {
		// update bar
		dynamicPoolSO.ReturnAllPoolObjects(savingsBarPrefab.PoolObjectId);
		foreach (SavingsData savingData in savingDatas) {
			SavingsBar savingsBar = dynamicPoolSO.GetPoolObject<SavingsBar>(savingsBarPrefab, savingsBarContainer);
			savingsBar.Init(savingData, totalSavings);
		}

		SetupSavingsAxisLabels();
		SetupYearAxisLabels();

		totalDepositsAmountText.text = string.Format("${0}", Util.FormatDouble(totalDeposits));
		totalInterestAmountText.text = string.Format("${0}", Util.FormatDouble(totalInterests));
		totalSavingsAmountText.text = string.Format("${0}", Util.FormatDouble(totalSavings));
	}

	private void SetupSavingsAxisLabels() {
		// round to nearest 5
		long roundedValue = (long)Util.Round(totalSavings, 5);
		int labelCount = 1;
		double interval = 0;

		// can be splitted to how many non zero labels?
		if (roundedValue > 0) {
			if (roundedValue % 3 == 0) {
				labelCount += 3;
				interval = roundedValue / 3;
			} else {
				labelCount += 2;
				interval = roundedValue / 2;
			}
		}

		dynamicPoolSO.ReturnAllPoolObjects(savingsAxisLabelPrefab.PoolObjectId);
		for (int i=0; i<labelCount; i++) {
			SavingsAxisLabel savingsAxisLabel = dynamicPoolSO.GetPoolObject<SavingsAxisLabel>(savingsAxisLabelPrefab, savingsAxisLabelContainer);
			savingsAxisLabel.Init(i * interval, totalSavings);
		}
	}

	private void SetupYearAxisLabels() {
		int totalYearsDisplay;
		int interval;

		if (numberOfYears > 5) {
			totalYearsDisplay = 5;
			interval = numberOfYears / 5;
		} else {
			totalYearsDisplay = numberOfYears;
			interval = 1;
		}

		dynamicPoolSO.ReturnAllPoolObjects(yearAxisLabelPrefab.PoolObjectId);

		// get the savings bar width
		for (int i = 1; i <= totalYearsDisplay; i++) {
			YearAxisLabel yearAxisLabel = dynamicPoolSO.GetPoolObject<YearAxisLabel>(yearAxisLabelPrefab, yearAxisLabelContainer);
			yearAxisLabel.Init(i * interval, numberOfYears);
		}
	}
	#endregion

	#region Savings Bar Tooltip
	public void OnPointerEnterSavingsBar(int index) {
		savingsBarTooltip.SetText(savingDatas[index]);
		savingsBarTooltip.ShowTooltip();
	}

	public void OnPointerExitSavingsBar(int index) {
		StartCoroutine(DoHideTooltip(index));
	}

	IEnumerator DoHideTooltip(int index) {
		yield return new WaitForEndOfFrame();

		if (savingsBarTooltip.savingsData.year == index) {
			savingsBarTooltip.HideTooltip();
		}
	}
	#endregion
}
