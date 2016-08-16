using System;

namespace Phoenix.Core
{
    public static class FSFormat
    {
        public const string BasicDateFormat = "dd/MM/yyyy";
        public const string ShortDateFormat = "dd-MMM-yyyy";
        public const string LongDateFormat = "dd MMMM yyyy";
        public const string BasicDateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        public const string ShortDateTimeFormat = "dd-MMM-yyyy HH:mm:ss";
        public const string LongDateTimeFormat = "dd MMMM yyyy HH:mm:ss";
        public const string DateStamp = "yyyyMMdd";
        public const string TimeStamp = "yyyyMMdd_HHmmss";
        public const string MoneyFormat = "#,##0.00";
        public const string BasicMoneyFormat = "#,##0";
        public const string RateFormat = "0.00000";
        public const string PercentFormat = "P2";

        /// <summary> Returns a String Representation of the Date in the Format:<br/>dd/MM/yyyy </summary>
        public static string BasicDate(DateTime dt) { return dt.ToString(BasicDateFormat); }
        
        /// <summary> Returns a String Representation of the Date in the Format:<br/>dd/MM/yyyy </summary>
        public static string BasicDate(string dt) { return DateTime.Parse(dt).ToString(BasicDateFormat); }

        /// <summary> Returns a String Representation of the Date in the Format:<br/>dd-MMM-yyyy </summary>
        public static string ShortDate(DateTime dt) { return dt.ToString(ShortDateFormat); }
        
        /// <summary> Returns a String Representation of the Date in the Format:<br/>dd-MMM-yyyy </summary>
        public static string ShortDate(string dt) { return DateTime.Parse(dt).ToString(ShortDateFormat); }

        /// <summary> Returns a String Representation of the Date in the Format:<br/>dd MMMM yyyy </summary>
        public static string LongDate(DateTime dt) { return dt.ToString(LongDateFormat); }

        /// <summary> Returns a String Representation of the Date in the Format:<br/>dd/MM/yyyy HH:mm:ss </summary>
        public static string BasicDateTime(DateTime dt) { return dt.ToString(BasicDateTimeFormat); }
        
        /// <summary> Returns a String Representation of the Date in the Format:<br/>dd/MM/yyyy HH:mm:ss </summary>
        public static string BasicDateTime(string dt) { return DateTime.Parse(dt).ToString(BasicDateTimeFormat); }

        /// <summary> Returns a String Representation of the Date in the Format:<br/>dd-MMM-yyyy HH:mm:ss </summary>
        public static string ShortDateTime(DateTime dt) { return dt.ToString(ShortDateTimeFormat); }

        /// <summary> Returns a String Representation of the Date in the Format:<br/>dd-MMM-yyyy HH:mm:ss </summary>
        public static string ShortDateTime(string dt) { return DateTime.Parse(dt).ToString(ShortDateTimeFormat); }

        /// <summary> Returns a String Representation of the Date in the Format:<br/>dd MMMM yyyy HH:mm:ss </summary>
        public static string LongDateTime(DateTime dt) { return dt.ToString(LongDateTimeFormat); }

        /// <summary> Returns a String Representation of the Amount in the Format:<br/>#,##0.00</summary>
        public static string Money(double amount) { return amount.ToString(MoneyFormat); }
       
        /// <summary> Returns a String Representation of the Amount in the Format:<br/>#,##0.00</summary>
        public static string Money(string amount) { return double.Parse(amount).ToString(MoneyFormat); }

        /// <summary> Returns a String Representation of the Amount in the Format:<br/>#,##0</summary>
        public static string BasicMoney(double amount) { return amount.ToString(BasicMoneyFormat); }
        
        /// <summary> Returns a String Representation of the Amount in the Format:<br/>#,##0</summary>
        public static string BasicMoney(string amount) { return double.Parse(amount).ToString(BasicMoneyFormat); }

        /// <summary> Returns a String Representation of the Amount in the Format:<br/>0.00000</summary>
        public static string Rate(double amount) { return amount.ToString(RateFormat); }
        
        /// <summary> Returns a String Representation of the Amount in the Format:<br/>0.00000</summary>
        public static string Rate(string amount) { return double.Parse(amount).ToString(RateFormat); }

        /// <summary> Returns a String Representation of the Amount in the Format:<br/>$#,##0.00</summary>
        public static string Currency(double amount, string symbol)
        {
            return amount.ToString(symbol + MoneyFormat);
        }
        
        /// <summary> Returns a String Representation of the Amount in the Format:<br/>$#,##0.00</summary>
        public static string Currency(string amount, string symbol)
        {
            return double.Parse(amount).ToString(symbol + MoneyFormat);
        }
        
        /// <summary> Returns blank if Amount is zero, or a String Representation of the Amount in the Format:<br/>#,##0.00</summary>
        public static string MoneyBlank(string amount)
        {
            return double.Parse(amount) == 0.0 ? "" : double.Parse(amount).ToString(BasicMoneyFormat);
        }
        
        /// <summary> Returns blank if Amount is zero, or a String Representation of the Amount in the Format:<br/>#,##0.00</summary>
        public static string MoneyBlank(double amount)
        {
            return amount == 0.0 ? "" : amount.ToString(BasicMoneyFormat);
        }
        
        /// <summary> Returns a String Representation of the Amount in the Format:<br/>0.00%</summary>
        public static string Percentage(double amount)
        {
            return amount.ToString(PercentFormat) + "%";
        }
    }
}
