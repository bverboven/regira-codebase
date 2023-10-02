﻿using Regira.Office.PDF.Abstractions;
using Regira.Utilities;

namespace Regira.Office.PDF.Models;

public class HtmlInput : PdfInputBase
{
    public string? HtmlContent { get; set; }
    public string? HeaderHtmlContent { get; set; }
    /// <summary>
    /// Height of header in mm
    /// </summary>
    public int? HeaderHeight { get; set; }
    public string? FooterHtmlContent { get; set; }
    /// <summary>
    /// Height of footer in mm
    /// </summary>
    public int? FooterHeight { get; set; }

    public HtmlInput(int dpi = DimensionsUtility.DPI.DEFAULT)
        : base(dpi)
    {
    }
}