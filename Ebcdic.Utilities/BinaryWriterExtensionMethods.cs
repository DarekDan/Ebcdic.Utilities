﻿namespace Ebcdic.Utilities;

public static class BinaryWriterExtensionMethods
{
    /// <summary>
    ///     Writes the given Unicode string as an 8-bit EBCDIC encoded character string
    /// </summary>
    public static void WriteEbcdic(this BinaryWriter writer, string value)
    {
        var bytes = IbmConverter.GetBytes(value);
        writer.Write(bytes);
    }

    /// <summary>
    ///     Writes a big endian encoded Int16 to the stream
    /// </summary>
    public static void WriteBigEndian(this BinaryWriter writer, short value)
    {
        var bytes = IbmConverter.GetBytes(value);
        writer.Write(bytes);
    }

    /// <summary>
    ///     Writes a big endian encoded Int32 to the stream
    /// </summary>
    public static void WriteBigEndian(this BinaryWriter writer, int value)
    {
        var bytes = IbmConverter.GetBytes(value);
        writer.Write(bytes);
    }

    /// <summary>
    ///     Writes an IBM System/360 Floating Point encoded Single to the stream
    /// </summary>
    public static void WriteIbmSingle(this BinaryWriter writer, float value)
    {
        var bytes = IbmConverter.GetBytes(value);
        writer.Write(bytes);
    }

    /// <summary>
    ///     Writes a packed decimal to the stream
    /// </summary>
    public static void WriteIbmPackedDecimal(this BinaryWriter writer, decimal value)
    {
        var bytes = IbmConverter.GetBytes(value);
        writer.Write(bytes);
    }
}