using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingsData
{
	public int year;
	public double deposit;
	public double interest;
	public double total;

	public SavingsData(int year, double deposit, double interest) {
		this.year = year;
		this.deposit = deposit;
		this.interest = interest;
		total = deposit + interest;
	}
}