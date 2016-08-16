using OfficeOpenXml.Table;

namespace Phoenix.Common.Excel
{
    public enum FSExcelTableTheme
    {
        None,
        TableStyleLight1,
        TableStyleLight2,
        TableStyleLight3,
        TableStyleLight4,
        TableStyleLight5,
        TableStyleLight6,
        TableStyleLight7,
        TableStyleLight8,
        TableStyleLight9,
        TableStyleLight10,
        TableStyleLight11,
        TableStyleLight12,
        TableStyleLight13,
        TableStyleLight14,
        TableStyleLight15,
        TableStyleLight16,
        TableStyleLight17,
        TableStyleLight18,
        TableStyleLight19,
        TableStyleLight20,
        TableStyleLight21,
        TableStyleMedium1,
        TableStyleMedium2,
        TableStyleMedium3,
        TableStyleMedium4,
        TableStyleMedium5,
        TableStyleMedium6,
        TableStyleMedium7,
        TableStyleMedium8,
        TableStyleMedium9,
        TableStyleMedium10,
        TableStyleMedium11,
        TableStyleMedium12,
        TableStyleMedium13,
        TableStyleMedium14,
        TableStyleMedium15,
        TableStyleMedium16,
        TableStyleMedium17,
        TableStyleMedium18,
        TableStyleMedium19,
        TableStyleMedium20,
        TableStyleMedium21,
        TableStyleDark1,
        TableStyleDark2,
        TableStyleDark3,
        TableStyleDark4,
        TableStyleDark5,
        TableStyleDark6,
        TableStyleDark7,
        TableStyleDark8,
        TableStyleDark9,
        TableStyleDark10,
        TableStyleDark11
    }

    public static class FSExcelTableThemeExtensions
    {
        public static TableStyles Theme(this FSExcelTableTheme self)
        {
            switch (self)
            {
                case FSExcelTableTheme.None:
                    return TableStyles.None;
                case FSExcelTableTheme.TableStyleLight1:
                    return TableStyles.Light1;
                case FSExcelTableTheme.TableStyleLight2:
                    return TableStyles.Light2;
                case FSExcelTableTheme.TableStyleLight3:
                    return TableStyles.Light3;
                case FSExcelTableTheme.TableStyleLight4:
                    return TableStyles.Light4;
                case FSExcelTableTheme.TableStyleLight5:
                    return TableStyles.Light5;
                case FSExcelTableTheme.TableStyleLight6:
                    return TableStyles.Light6;
                case FSExcelTableTheme.TableStyleLight7:
                    return TableStyles.Light7;
                case FSExcelTableTheme.TableStyleLight8:
                    return TableStyles.Light8;
                case FSExcelTableTheme.TableStyleLight9:
                    return TableStyles.Light9;
                case FSExcelTableTheme.TableStyleLight10:
                    return TableStyles.Light10;
                case FSExcelTableTheme.TableStyleLight11:
                    return TableStyles.Light11;
                case FSExcelTableTheme.TableStyleLight12:
                    return TableStyles.Light12;
                case FSExcelTableTheme.TableStyleLight13:
                    return TableStyles.Light13;
                case FSExcelTableTheme.TableStyleLight14:
                    return TableStyles.Light14;
                case FSExcelTableTheme.TableStyleLight15:
                    return TableStyles.Light15;
                case FSExcelTableTheme.TableStyleLight16:
                    return TableStyles.Light16;
                case FSExcelTableTheme.TableStyleLight17:
                    return TableStyles.Light17;
                case FSExcelTableTheme.TableStyleLight18:
                    return TableStyles.Light18;
                case FSExcelTableTheme.TableStyleLight19:
                    return TableStyles.Light19;
                case FSExcelTableTheme.TableStyleLight20:
                    return TableStyles.Light20;
                case FSExcelTableTheme.TableStyleLight21:
                    return TableStyles.Light21;
                case FSExcelTableTheme.TableStyleMedium1:
                    return TableStyles.Medium1;
                case FSExcelTableTheme.TableStyleMedium2:
                    return TableStyles.Medium2;
                case FSExcelTableTheme.TableStyleMedium3:
                    return TableStyles.Medium3;
                case FSExcelTableTheme.TableStyleMedium4:
                    return TableStyles.Medium4;
                case FSExcelTableTheme.TableStyleMedium5:
                    return TableStyles.Medium5;
                case FSExcelTableTheme.TableStyleMedium6:
                    return TableStyles.Medium6;
                case FSExcelTableTheme.TableStyleMedium7:
                    return TableStyles.Medium7;
                case FSExcelTableTheme.TableStyleMedium8:
                    return TableStyles.Medium8;
                case FSExcelTableTheme.TableStyleMedium9:
                    return TableStyles.Medium9;
                case FSExcelTableTheme.TableStyleMedium10:
                    return TableStyles.Medium10;
                case FSExcelTableTheme.TableStyleMedium11:
                    return TableStyles.Medium11;
                case FSExcelTableTheme.TableStyleMedium12:
                    return TableStyles.Medium12;
                case FSExcelTableTheme.TableStyleMedium13:
                    return TableStyles.Medium13;
                case FSExcelTableTheme.TableStyleMedium14:
                    return TableStyles.Medium14;
                case FSExcelTableTheme.TableStyleMedium15:
                    return TableStyles.Medium15;
                case FSExcelTableTheme.TableStyleMedium16:
                    return TableStyles.Medium16;
                case FSExcelTableTheme.TableStyleMedium17:
                    return TableStyles.Medium17;
                case FSExcelTableTheme.TableStyleMedium18:
                    return TableStyles.Medium18;
                case FSExcelTableTheme.TableStyleMedium19:
                    return TableStyles.Medium19;
                case FSExcelTableTheme.TableStyleMedium20:
                    return TableStyles.Medium20;
                case FSExcelTableTheme.TableStyleMedium21:
                    return TableStyles.Medium21;
                case FSExcelTableTheme.TableStyleDark1:
                    return TableStyles.Dark1;
                case FSExcelTableTheme.TableStyleDark2:
                    return TableStyles.Dark2;
                case FSExcelTableTheme.TableStyleDark3:
                    return TableStyles.Dark3;
                case FSExcelTableTheme.TableStyleDark4:
                    return TableStyles.Dark4;
                case FSExcelTableTheme.TableStyleDark5:
                    return TableStyles.Dark5;
                case FSExcelTableTheme.TableStyleDark6:
                    return TableStyles.Dark6;
                case FSExcelTableTheme.TableStyleDark7:
                    return TableStyles.Dark7;
                case FSExcelTableTheme.TableStyleDark8:
                    return TableStyles.Dark8;
                case FSExcelTableTheme.TableStyleDark9:
                    return TableStyles.Dark9;
                case FSExcelTableTheme.TableStyleDark10:
                    return TableStyles.Dark10;
                case FSExcelTableTheme.TableStyleDark11:
                    return TableStyles.Dark11;
                default:
                    return TableStyles.None;
            }
        }
    }
}
