﻿namespace UnitversalLibrary;

/// <summary>
/// Arbitrary precision decimal.
/// All operations are exact, except for division. Division never determines more digits than the given precision.
/// Source: https://gist.github.com/JcBernack/0b4eef59ca97ee931a2f45542b9ff06d
/// Based on https://stackoverflow.com/a/4524254
/// Author: Jan Christoph Bernack (contact: jc.bernack at gmail.com)
/// License: public domain
/// </summary>
public struct BigDecimal : IComparable, IComparable<BigDecimal>
{
    /// <summary>
    /// Specifies whether the significant digits should be truncated to the given precision after each operation.
    /// </summary>
    public static bool AlwaysTruncate = false;
    /// <summary>
    /// Sets the maximum precision of division operations.
    /// If AlwaysTruncate is set to true all operations are affected.
    /// </summary>
    public static int Precision = 150;
    public BigInteger Mantissa { get; set; }
    public int Exponent { get; set; }
    /// <summary>
    /// The string separator for separating the integer and decimal parts of the number.
    /// </summary>
    public static string DecimalSeparator = ".";
    /// <summary>
    /// The string separator for separating digits in the integer part of the number.
    /// </summary>
    public static string IntegerGroupSeparator = ",";
    /// <summary>
    /// The number of digits for separating integer groups.
    /// </summary>
    public static int IntegerGroupSize = 3;
    /// <summary>
    /// The string separator for separating digits in the decimal part of the number.
    /// </summary>
    public static string DecimalGroupSeparator = " ";
    /// <summary>
    /// The number of digits for separating decimal groups.
    /// </summary>
    public static int DecimalGroupSize = 5;
    /// <summary>
    /// The decimal and grouping separators as a <see cref="char"/>[].
    /// </summary>
    public static char[] Separators {
        get => (DecimalSeparator + IntegerGroupSeparator + DecimalGroupSeparator).ToCharArray();
    }
    public BigDecimal(BigInteger mantissa, int exponent)
        : this()
    {
        Mantissa = mantissa;
        Exponent = exponent;
        Normalize();
        if (AlwaysTruncate)
        {
            Truncate();
        }
    }
    /// <summary>
    /// Removes trailing zeros on the mantissa
    /// </summary>
    public void Normalize()
    {
        if (Mantissa.IsZero)
        {
            Exponent = 0;
        }
        else
        {
            BigInteger remainder = 0;
            while (remainder == 0)
            {
                var shortened = BigInteger.DivRem(Mantissa, 10, out remainder);
                if (remainder == 0)
                {
                    Mantissa = shortened;
                    Exponent++;
                }
            }
        }
    }
    /// <summary>
    /// Truncate the number to the given precision by removing the least significant digits.
    /// </summary>
    /// <returns>The truncated number</returns>
    public BigDecimal Truncate(int precision)
    {
        // copy this instance (remember it's a struct)
        BigDecimal shortened = this;
        int excessDigits = NumberOfDigits(shortened.Mantissa) - precision;
        if (excessDigits > 0)
        {
            shortened.Mantissa /= BigInteger.Pow(10, excessDigits);
            shortened.Exponent += excessDigits;
        }
        // normalize again to make sure there are no trailing zeros left
        shortened.Normalize();
        return shortened;
    }
    public BigDecimal Truncate()
    {
        return Truncate(Precision);
    }

    /// <summary>
    /// Round the number to the given precision where ending digits 0-4 are rounded down but 5-9 are rounded
    /// up.
    /// </summary>
    /// <returns>
    /// The rounded number.
    /// </returns>
    public BigDecimal Round(int precision)
    {
        BigDecimal shortened = this;
        BigInteger remainder;
        int excessDigits = NumberOfDigits(shortened.Mantissa) - precision - 1;
        if (excessDigits > -1)
        {
            if (excessDigits > 0)
            {
                shortened.Mantissa /= BigInteger.Pow(10, excessDigits);
                shortened.Exponent += excessDigits;
            }
            shortened.Mantissa = BigInteger.DivRem(shortened.Mantissa, 10, out remainder);
            shortened.Exponent += 1;
            if (remainder >= 5)
            {
                shortened.Mantissa += 1;
            }
        }
        shortened.Normalize();
        return shortened;
    }
    public BigDecimal Round()
    {
        return Round(Precision);
    }

    public BigDecimal Floor()
    {
        return Truncate(NumberOfDigits(Mantissa) + Exponent);
    }
    public static int NumberOfDigits(BigInteger value)
    {
        if (value.IsZero)
        {
            return 1;
        }
        else
        {
            return (int)Math.Floor(BigInteger.Log10(BigInteger.Abs(value))) + 1;
        }
    }

    /// <summary>
    /// Tries to convert the string representation of a number to its <see cref="BigDecimal"/>
    /// equivalent, and returns a value that indicates whether the conversion succeeded.
    /// Also recognizes scientific notation.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="value"/> was converted successfully; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown when the value is null.
    /// </exception>
    public static bool TryParse(string value, out BigDecimal result, char separator = '.')
    {
        result = 0;
        BigInteger mantissa;
        int exponent = 0;
        string numberString = value.ToUpper();
        int separatorIndex = numberString.IndexOf(separator);
        //Check for duplicate decimals
        for (int i = separatorIndex; i > -1; i = numberString.IndexOf(separator, i + 1))
        {
            if (i != separatorIndex)
            {
                return false;
            }
        }
        //Scientific notation
        if (numberString.Contains("E"))
        {
            string[] tokenizedNumber = numberString.Split("E");
            numberString = tokenizedNumber[0];
            if (!int.TryParse(tokenizedNumber[1], out exponent))
            {
                return false;
            }
        }
        //If real number
        if (separatorIndex > -1)
        {
            numberString = numberString.Replace(separator.ToString(), "");
            exponent += separatorIndex - numberString.Length;
        }
        if (BigInteger.TryParse(numberString, out mantissa))
        {
            result = new BigDecimal(mantissa, exponent);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Converts the representation of a number, contained in the specified read-only
    /// span of characters, in a specified style to its <see cref="BigDecimal"/> equivalent.
    /// </summary>
    /// <returns>
    /// A value that is equivalent to the number specified in the <paramref name="value"/> parameter.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown when the value is null.
    /// </exception>
    /// <exception cref="System.FormatException">
    /// Thrown when the value is not properly formatted.
    /// </exception>
    public static BigDecimal Parse(string value, char separator = '.')
    {
        if (value == null)
        {
            throw new ArgumentNullException("value");
        }
        BigDecimal result;
        if (!TryParse(value, out result, separator))
        {
            throw new FormatException();
        }
        return result;
    }

    #region Conversions

    public static implicit operator BigDecimal(sbyte value)
    {
        return new BigDecimal(value, 0);
    }

    public static implicit operator BigDecimal(byte value)
    {
        return new BigDecimal(value, 0);
    }

    public static implicit operator BigDecimal(short value)
    {
        return new BigDecimal(value, 0);
    }

    public static implicit operator BigDecimal(ushort value)
    {
        return new BigDecimal(value, 0);
    }

    public static implicit operator BigDecimal(int value)
    {
        return new BigDecimal(value, 0);
    }

    public static implicit operator BigDecimal(uint value)
    {
        return new BigDecimal(value, 0);
    }

    public static implicit operator BigDecimal(long value)
    {
        return new BigDecimal(value, 0);
    }

    public static implicit operator BigDecimal(ulong value)
    {
        return new BigDecimal(value, 0);
    }

    public static implicit operator BigDecimal(double value)
    {
        var mantissa = (BigInteger)value;
        var exponent = 0;
        double scaleFactor = 1;
        while (Math.Abs(value * scaleFactor - (double)mantissa) > 0)
        {
            exponent -= 1;
            scaleFactor *= 10;
            mantissa = (BigInteger)(value * scaleFactor);
        }
        return new BigDecimal(mantissa, exponent);
    }

    public static implicit operator BigDecimal(decimal value)
    {
        var mantissa = (BigInteger)value;
        var exponent = 0;
        decimal scaleFactor = 1;
        while ((decimal)mantissa != value * scaleFactor)
        {
            exponent -= 1;
            scaleFactor *= 10;
            mantissa = (BigInteger)(value * scaleFactor);
        }
        return new BigDecimal(mantissa, exponent);
    }

    public static explicit operator double(BigDecimal value)
    {
        return (double)value.Mantissa * Math.Pow(10, value.Exponent);
    }

    public static explicit operator float(BigDecimal value)
    {
        return Convert.ToSingle((double)value);
    }

    public static explicit operator decimal(BigDecimal value)
    {
        return (decimal)value.Mantissa * (decimal)Math.Pow(10, value.Exponent);
    }

    public static explicit operator int(BigDecimal value)
    {
        return (int)(value.Mantissa * BigInteger.Pow(10, value.Exponent));
    }

    public static explicit operator uint(BigDecimal value)
    {
        return (uint)(value.Mantissa * BigInteger.Pow(10, value.Exponent));
    }

    #endregion

    #region Operators

    public static BigDecimal operator +(BigDecimal value)
    {
        return value;
    }

    public static BigDecimal operator -(BigDecimal value)
    {
        value.Mantissa *= -1;
        return value;
    }

    public static BigDecimal operator ++(BigDecimal value)
    {
        return value + 1;
    }

    public static BigDecimal operator --(BigDecimal value)
    {
        return value - 1;
    }

    public static BigDecimal operator +(BigDecimal left, BigDecimal right)
    {
        return Add(left, right);
    }

    public static BigDecimal operator -(BigDecimal left, BigDecimal right)
    {
        return Add(left, -right);
    }

    private static BigDecimal Add(BigDecimal left, BigDecimal right)
    {
        return left.Exponent > right.Exponent
            ? new BigDecimal(AlignExponent(left, right) + right.Mantissa, right.Exponent)
            : new BigDecimal(AlignExponent(right, left) + left.Mantissa, left.Exponent);
    }

    public static BigDecimal operator *(BigDecimal left, BigDecimal right)
    {
        return new BigDecimal(left.Mantissa * right.Mantissa, left.Exponent + right.Exponent);
    }

    public static BigDecimal operator /(BigDecimal dividend, BigDecimal divisor)
    {
        var exponentChange = Precision - (NumberOfDigits(dividend.Mantissa) - NumberOfDigits(divisor.Mantissa));
        if (exponentChange < 0)
        {
            exponentChange = 0;
        }
        dividend.Mantissa *= BigInteger.Pow(10, exponentChange);
        return new BigDecimal(dividend.Mantissa / divisor.Mantissa, dividend.Exponent - divisor.Exponent - exponentChange);
    }

    public static BigDecimal operator %(BigDecimal left, BigDecimal right)
    {
        return left - right * (left / right).Floor();
    }

    public static bool operator ==(BigDecimal left, BigDecimal right)
    {
        return left.Exponent == right.Exponent && left.Mantissa == right.Mantissa;
    }

    public static bool operator !=(BigDecimal left, BigDecimal right)
    {
        return left.Exponent != right.Exponent || left.Mantissa != right.Mantissa;
    }

    public static bool operator <(BigDecimal left, BigDecimal right)
    {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) < right.Mantissa : left.Mantissa < AlignExponent(right, left);
    }

    public static bool operator >(BigDecimal left, BigDecimal right)
    {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) > right.Mantissa : left.Mantissa > AlignExponent(right, left);
    }

    public static bool operator <=(BigDecimal left, BigDecimal right)
    {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) <= right.Mantissa : left.Mantissa <= AlignExponent(right, left);
    }

    public static bool operator >=(BigDecimal left, BigDecimal right)
    {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) >= right.Mantissa : left.Mantissa >= AlignExponent(right, left);
    }

    /// <summary>
    /// Returns the mantissa of value, aligned to the exponent of reference.
    /// Assumes the exponent of value is larger than of reference.
    /// </summary>
    private static BigInteger AlignExponent(BigDecimal value, BigDecimal reference)
    {
        return value.Mantissa * BigInteger.Pow(10, value.Exponent - reference.Exponent);
    }

    #endregion

    #region Additional mathematical functions

    public static BigDecimal Exp(double exponent)
    {
        var tmp = (BigDecimal)1;
        while (Math.Abs(exponent) > 100)
        {
            var diff = exponent > 0 ? 100 : -100;
            tmp *= Math.Exp(diff);
            exponent -= diff;
        }
        return tmp * Math.Exp(exponent);
    }

    public static BigDecimal Pow(double basis, double exponent)
    {
        var tmp = (BigDecimal)1;
        while (Math.Abs(exponent) > 100)
        {
            var diff = exponent > 0 ? 100 : -100;
            tmp *= Math.Pow(basis, diff);
            exponent -= diff;
        }
        return tmp * Math.Pow(basis, exponent);
    }

    #endregion

    /// <summary>
    /// Convert to a string formatted in scientific notation according to specified magnitude limits and
    /// specified decimal and grouping separators.
    /// </summary>
    public string ToFormattedString(BigDecimal SmallMagnitude, BigDecimal LargeMagnitude)
    {
        if (this.Mantissa.IsZero)
        {
            return "0";
        }
        BigDecimal Number = this;
        bool Negative = false;
        bool ScientificNotation = false;
        int ScientificNotationOffset = 0;
        string NumberString = Number.Mantissa.ToString();
        BigDecimal AbsoluteNumber = Number;
        //Temporarily remove negative sign
        if (NumberString[0] == '-')
        {
            AbsoluteNumber = Number * -1;
            NumberString = NumberString.Remove(0, 1);
            Negative = true;
        }
        //Scientific notation
        if (AbsoluteNumber > LargeMagnitude || AbsoluteNumber < SmallMagnitude)
        {
            ScientificNotationOffset = NumberString.Length - 1;
            if (NumberString.Length > 1)
            {
                NumberString = NumberString.Insert(1, DecimalSeparator);
            }
            ScientificNotation = true;
        }
        //Natural formatting
        else
        {
            if (Number.Exponent < 0)
            {
                int Exponent = Number.Exponent + NumberString.Length;
                if (Exponent <= 0)
                {
                    Exponent = Exponent * -1;
                    NumberString = $"0{DecimalSeparator}" + NumberString.PadLeft(NumberString.Length + Exponent, '0');
                }
                else
                {
                    NumberString = NumberString.Insert(Exponent, DecimalSeparator);
                }
            }
            else if (Number.Exponent >= 0)
            {
                NumberString = NumberString.PadRight(NumberString.Length + Number.Exponent, '0');
            }
        }
        //Digit grouping for decimal part
        if (DecimalGroupSize > 0)
        {
            int Separator = NumberString.IndexOf(DecimalSeparator) != -1 ? NumberString.IndexOf(DecimalSeparator) : NumberString.Length;
            for (int i = Separator + DecimalGroupSize + 1; i < NumberString.Length; i = i + DecimalGroupSize + 1)
            {
                NumberString = NumberString.Insert(i, DecimalGroupSeparator);
            }
        }
        //Digit grouping for integer part
        if (IntegerGroupSize > 0)
        {
            int Separator = NumberString.IndexOf(DecimalSeparator) != -1 ? NumberString.IndexOf(DecimalSeparator) : NumberString.Length;
            for (int i = Separator - IntegerGroupSize; i >= 1; i = i - IntegerGroupSize)
            {
                NumberString = NumberString.Insert(i, IntegerGroupSeparator);
            }
        }
        //Add negative sign back
        if (Negative)
        {
            NumberString = "-" + NumberString;
        }
        //Add exponent back
        if (ScientificNotation)
        {
            NumberString += $"E{Number.Exponent + ScientificNotationOffset}";
        }
        return NumberString;
    }
    /// <summary>
    /// Convert a <see cref="BigDecimal"/> string formatted with <see cref="ToFormattedString"/> to a <see cref="BigDecimal"/>.
    /// </summary>
    public static BigDecimal ReverseFormat(string Number, string DecimalSeparator, string IntegerGroupSeparator, string DecimalGroupSeparator)
    {
        //Remove integer and decimal grouping separators
        Number = Number.Replace(IntegerGroupSeparator, "");
        Number = Number.Replace(DecimalGroupSeparator, "");
        //Check if number is in scientific notation
        if (Number.Contains("E"))
        {
            //Split by E into mantissa and exponent
            string[] LargeNumber = Number.Split("E");
            string MantissaString = LargeNumber[0].Replace(DecimalSeparator, "");
            //Find decimal index
            int Offset = 1 - MantissaString.Length;
            //Get mantissa and exponent
            BigInteger Mantissa = BigInteger.Parse(MantissaString);
            int Exponent = int.Parse(LargeNumber[1]);
            return new BigDecimal(Mantissa, Exponent + Offset);
        }
        else
        {
            if (Number.Contains(DecimalSeparator))
            {
                string MantissaString = Number.Replace(DecimalSeparator, "");
                BigInteger Mantissa = BigInteger.Parse(MantissaString);
                int Exponent = Number.IndexOf(DecimalSeparator) - MantissaString.Length;
                return new BigDecimal(Mantissa, Exponent);
            }
            return new BigDecimal(BigInteger.Parse(Number), 0);
        }
    }
    /// <summary>
    /// Convert a <see cref="BigDecimal"/> to a plain decimal string.
    /// </summary>
    public static string PlainString(BigDecimal Number, string DecimalSeparator)
    {
        string NumberString = Number.Mantissa.ToString();
        if (Number.Exponent < 0)
        {
            int Exponent = Number.Exponent + NumberString.Length;
            if (Exponent <= 0)
            {
                Exponent = Exponent * -1;
                NumberString = $"0{DecimalSeparator}" + NumberString.PadLeft(NumberString.Length + Exponent, '0');
            }
            else
            {
                NumberString = NumberString.Insert(Exponent, DecimalSeparator);
            }
        }
        else if (Number.Exponent >= 0)
        {
            NumberString = NumberString.PadRight(NumberString.Length + Number.Exponent, '0');
        }
        return NumberString;
    }
    public string PlainString()
    {
        return PlainString(this, DecimalSeparator);
    }

    public override string ToString()
    {
        return string.Concat(Mantissa.ToString(), "E", Exponent);
    }

    public bool Equals(BigDecimal other)
    {
        return other.Mantissa.Equals(Mantissa) && other.Exponent == Exponent;
    }
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }
        return obj is BigDecimal && Equals((BigDecimal)obj);
    }
    public override int GetHashCode()
    {
        unchecked
        {
            return (Mantissa.GetHashCode() * 397) ^ Exponent;
        }
    }
    public int CompareTo(object obj)
    {
        if (ReferenceEquals(obj, null) || !(obj is BigDecimal))
        {
            throw new ArgumentException();
        }
        return CompareTo((BigDecimal)obj);
    }
    public int CompareTo(BigDecimal other)
    {
        return this < other ? -1 : (this > other ? 1 : 0);
    }
}