// --------------------------------------------------------------------------
// <copyright file="ExcelUtils.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Helpers
{
    using System.Drawing;
    using ExcelUtil.Spreadsheet;

    /// <summary>
    /// Excel Utilities.
    /// </summary>
    public class ExcelUtils
    {
        /// <summary>
        /// Gets the style integer number.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="isBold">If set to <c>true</c> [is bold].</param>
        /// <param name="isPositiveColor">If set to <c>true</c> [is positive color].</param>
        /// <returns>The style integer number.</returns>
        public CellStyle GetStyleIntNumber(string symbol, Color backgroundColor, bool isBold, bool isPositiveColor)
        {
            var style = new CellStyle();
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            style.NumberFormat = string.Format("{0}#,##0{1}_);-#,##0{2}", isPositiveColor ? "[RED]" : string.Empty, symbol, symbol);
            style.FillPattern.SetSolid(backgroundColor);
            style.Font.Name = "Tahoma";
            style.Font.Size = 160;
            if (isBold)
            {
                style.Font.Weight = ExcelFont.BoldWeight;
            }

            return style;
        }

        /// <summary>
        /// Gets the style integer number.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="fontColor">Color of the font.</param>
        /// <returns>The style integer number.</returns>
        public CellStyle GetStyleIntNumber(string symbol, Color backgroundColor, Color fontColor)
        {
            var style = new CellStyle();
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            style.NumberFormat = "#,##0" + symbol + "_);(#,##0)" + symbol;
            style.FillPattern.SetSolid(backgroundColor);
            style.Font.Name = "Tahoma";
            style.Font.Size = 160;

            style.Font.Weight = ExcelFont.BoldWeight;
            style.Font.Color = fontColor;

            return style;
        }

        /// <summary>
        /// Gets the style number center.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="isEven">If set to <c>true</c> [is even].</param>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The style number center.</returns>
        public CellStyle GetStyleNumberCenter(decimal num, bool isEven, string symbol)
        {
            var style = new CellStyle();
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            style.Font.Name = "Tahoma";
            style.Font.Size = 160;
            if (isEven)
            {
                style.FillPattern.SetSolid(Color.FromArgb(223, 234, 255));
            }

            if (num < 0)
            {
                style.Font.Color = Color.Red;
            }

            return style;
        }

        /// <summary>
        /// Gets the style title.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The style title.</returns>
        public CellStyle GetStyleTitle(Color color)
        {
            var celltitle = new CellStyle();
            celltitle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            celltitle.VerticalAlignment = VerticalAlignmentStyle.Center;
            celltitle.FillPattern.SetSolid(color);
            celltitle.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            celltitle.Font.Color = Color.Black;
            celltitle.Font.Name = "Tahoma";
            celltitle.Font.Size = 160;
            celltitle.Font.Weight = ExcelFont.BoldWeight;
            celltitle.WrapText = true;
            return celltitle;
        }

        /// <summary>
        /// Gets the style title.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="horizontal">The horizontal.</param>
        /// <param name="fontBold">The font bold.</param>
        /// <returns>The style title.</returns>
        public CellStyle GetStyleTitle(Color color, HorizontalAlignmentStyle horizontal, int fontBold)
        {
            var celltitle = new CellStyle();
            celltitle.HorizontalAlignment = horizontal;
            celltitle.VerticalAlignment = VerticalAlignmentStyle.Center;
            celltitle.FillPattern.SetSolid(color);
            celltitle.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            celltitle.Font.Color = Color.Black;
            celltitle.Font.Name = "Tahoma";
            celltitle.Font.Size = 160;
            celltitle.Font.Weight = fontBold;
            celltitle.WrapText = true;
            return celltitle;
        }
    }
}
