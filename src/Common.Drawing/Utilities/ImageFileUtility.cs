﻿using Regira.Drawing.Abstractions;
using Regira.Drawing.Core;
using Regira.IO.Abstractions;
using Regira.IO.Extensions;
using Regira.IO.Utilities;
using Regira.Utilities;

namespace Regira.Drawing.Utilities;

public static class ImageFileUtility
{
    public static IImageFile Load(this IImageFile img, string path)
    {
        var bytes = File.ReadAllBytes(path);
        img.Bytes = bytes;
        img.Length = bytes.Length;
        return img;
    }
    public static IImageFile ToImageFile(this IBinaryFile file)
    {
        if (!file.HasBytes() && !file.HasStream())
        {
            if (file.HasPath())
            {
                return new ImageFile().Load(file.Path!);
            }
        }
        return new ImageFile
        {
            Bytes = file.Bytes,
            Stream = file.Stream,
            Length = file.GetLength()
        };
    }


    public static string ToBase64(IMemoryFile file, string contentType = "image/png")
    {
        var bytes = file.GetBytes();
        if (bytes?.Any() != true)
        {
            throw new Exception("Could not get contents of file");
        }

        return $"data:{contentType};base64,{FileUtility.GetBase64String(bytes)}";
    }
    public static IImageFile FromBase64(string contents)
    {
        string? contentType = null;
        var firstChars = StringUtility.Truncate(contents, 64);
        if (firstChars.Contains(','))
        {
            contentType = firstChars.Split(';').FirstOrDefault()?.Split(':').LastOrDefault();
        }

        var bytes = FileUtility.GetBytes(contents);
        return new ImageFile
        {
            Bytes = bytes,
            Length = bytes.Length,
            ContentType = contentType
        };
    }
}