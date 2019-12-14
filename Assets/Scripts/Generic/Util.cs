using System;
using System.Text;
using UnityEngine;

public static class Util
{
    private static StringBuilder stringBuilder = new StringBuilder();

    public static string FormatInteger(int numberToFormat) {
		return string.Format("{0:n0}", numberToFormat);
	}

	public static string FormatDouble(double num) {
		return num.ToString("0.00");
	}

	public static string ToSI(double value, string unit = "") {
		if (value == 0) {
			return value.ToString();
		}

		string[] superSuffix = new string[] { "K", "M", "G", "T", "P", "A", };
		string[] subSuffix = new string[] { "m", "u", "n", "p", "f", "a" };
		double v = value;
		int exp = 0;
		while (v - Math.Floor(v) > 0) {
			if (exp >= 18)
				break;
			exp += 3;
			v *= 1000;
			v = Math.Round(v, 12);
		}

		while (Math.Floor(v).ToString().Length > 3) {
			if (exp <= -18) {
				break;
			}
			exp -= 3;
			v /= 1000;
			v = Math.Round(v, 12);
		}
		if (exp > 0) {
			return v.ToString() + subSuffix[exp / 3 - 1] + unit;
		} else if (exp < 0) {
			return v.ToString() + superSuffix[-exp / 3 - 1] + unit;
		} else {
			return v.ToString() + unit;
		}
	}

	public static double Round(double i, int v) {
		return Math.Round(i / v) * v;
	}

	public static double ConvertStringToDouble(string value) {
		double result;
		result = double.TryParse(value, out result) ? result : 0;
		result = Math.Max(result, 0);
		result = Math.Round(result, 2);
		return result;
	}

	public static int ConvertStringToInt(string value) {
		int result;
		result = int.TryParse(value, out result) ? result : 0;
		result = Mathf.Max(result, 0);
		return result;
	}
}
