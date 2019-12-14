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
	public Text[] savingsText;
	public List<Text> yearsText;
	public Transform savingsBarContainer;
	public SavingsBar savingsBarPrefab;

	[Header("Result Texts")]
	public Text totalDepositsAmountText;
	public Text totalInterestAmountText;
	public Text totalSavingsAmountText;

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
		dynamicPoolSO.ResetState();

		OnInterestRateEndEdit(DEFAULT_INTEREST_RATE.ToString());
		OnNumberOfYearsEndEdit(DEFAULT_YEARS.ToString());
	}

	#region Input
	public void OnInitialDepositEndEdit(string value) {
		initialDeposit = Util.ConvertStringToDouble(value);
		initialDepositInput.text = string.Format("${0}", Util.FormatDouble(initialDeposit));
		CalculateResult();
		DisplayResult();
	}

	public void OnRegularDepositEndEdit(string value) {
		regularDeposit = Util.ConvertStringToDouble(value);
		regularDepositInput.text = string.Format("${0}", Util.FormatDouble(regularDeposit));
		CalculateResult();
		DisplayResult();
	}

	public void OnNumberOfYearsEndEdit(string value) {
		numberOfYears = Util.ConvertStringToInt(value);
		numberOfYears = Math.Min(numberOfYears, 50); // limit to 50 years
		numberOfYearsInput.text = string.Format("${0}", Util.FormatInteger(numberOfYears));
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
		interestRateInput.text = string.Format("{0}%", Util.FormatDouble(interestRate));
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
			SavingsData savingData = new SavingsData(totalDeposits, totalInterests);
			savingDatas.Add(savingData);
		}
	}
	#endregion

	#region Result
	private void DisplayResult() {
		dynamicPoolSO.ReturnAllPoolObjects(savingsBarPrefab.PoolObjectId);

		// update bar
		foreach (SavingsData savingData in savingDatas) {
			SavingsBar savingsBar = dynamicPoolSO.GetPoolObject<SavingsBar>(savingsBarPrefab, savingsBarContainer);
			savingsBar.Init(savingData, totalSavings);
		}

		// update savings labels
		savingsText[0].text = "0";
		savingsText[1].text = Util.ToSI(totalSavings * 0.3);
		savingsText[2].text = Util.ToSI(totalSavings * 0.6);
		savingsText[3].text = Util.ToSI(totalSavings * 0.9);

		totalDepositsAmountText.text = string.Format("${0}", Util.FormatDouble(totalDeposits));
		totalInterestAmountText.text = string.Format("${0}", Util.FormatDouble(totalInterests));
		totalSavingsAmountText.text = string.Format("${0}", Util.FormatDouble(totalSavings));
	}
	#endregion
}
