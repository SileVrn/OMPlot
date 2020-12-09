using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot
{
    static class Accessories
    {
		static readonly char[] SIPrefixes = { 'y', 'z', 'a', 'f', 'p', 'n', 'µ', 'm', ' ', 'k', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' };

		internal static int Nearest2Pow(int a)
		{
			int i = 1;
			while (i < a)
				i <<= 1;
			return i;
		}

		internal static float Pow10(int a)
		{
			float b = 1;
			if (a > 0)
			{
				while (a-- > 0)
					b *= 10;
			}
			else if (a < 0)
			{
				while (a++ < 0)
					b /= 10;
			}
			return b;
		}

		internal static float Pow1000(int a)
		{
			float b = 1;
			if(a > 0)
			{
				while (a-- > 0)
					b *= 1000;
			}
			else if(a < 0)
			{
				while (a++ < 0)
					b /= 1000;
			}
			return b;
		}

		internal static string FloatFormat(int degree)
		{
			string format = "##0.";
			while (degree++ < 0)
				format += '#';
			return format;
		}

		internal static string FloatFormat(float value, double sigma)
		{
			int degree = (int)(Math.Ceiling(Math.Log10(Math.Abs(value / sigma))));
			string format = "###.";
			while (degree-- > 0)
				format += '#';
			return format;
		}

		internal static int Degree(float value)
		{
			if (value == 0.0f)
				return 0;
			if (float.IsPositiveInfinity(value))
				return int.MaxValue;
			if (float.IsNegativeInfinity(value))
				return int.MinValue;
			if (float.IsNaN(value))
				return 0;

			return (int)(Math.Floor(Math.Log10(Math.Abs(value)) / 3));
		}

		internal static string ToSI(float value, int degree, string format = "###.##")
		{
			if (value == 0.0f)
				return "0";
			if (float.IsPositiveInfinity(value))
				return "+∞";
			if (float.IsNegativeInfinity(value))
				return "-∞";
			if (float.IsNaN(value))
				return "NaN";

			if (degree == 0)
				return value.ToString(format);
			if (degree < 8 && degree > -8 && degree != 0)
				return (value * Pow1000(-degree)).ToString(format) + SIPrefixes[degree + 8];
			else
				return value.ToString();
		}

		internal static string ToSI(float value, string format = "###.##")
		{
			if (value == 0.0f)
				return "0";
			if (float.IsPositiveInfinity(value))
				return "+∞";
			if (float.IsNegativeInfinity(value))
				return "-∞";
			if (float.IsNaN(value))
				return "NaN";

			int degree = (int)(Math.Floor(Math.Log10(Math.Abs(value)) / 3));

			if(degree == 0)
			{
				return value.ToString(format);
			}
			if (degree < 8 && degree > -8 && degree != 0)
			{
				return (value * Pow1000(-degree)).ToString(format) + SIPrefixes[degree + 8];
			}
			else
			{
				return value.ToString();
			}
		}

		internal static float Round(float a, int sign)
		{
			if (sign > 0)
			{
				float b = Pow10(sign);
				return (float)Math.Round(a / b) * b;
			}
			else
			{
				return (float)Math.Round(a, -1 * sign);
			}
		}

		internal static int FirstSignRound(float a)
		{
			float b = Math.Abs(a);

			if (b > 10)
			{
				float c = 100;
				int i = 1;
				while (b > c)
				{
					i++;
					c *= 10;
				}
				return i--;
			}
			else if (b < 1)
			{
				float c = 0.1f;
				int i = -1;
				while (b < c)
				{
					i--;
					c /= 10;
				}
				return i++;
			}
			else
			{
				return 0;
			}
		}
	}
}
