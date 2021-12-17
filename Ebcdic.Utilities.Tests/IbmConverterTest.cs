using System;
using System.Collections;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Ebcdic.Utilities.Tests;

public class IbmConverterTest
{
    #region GetBytes from Int16

    [Fact]
    public void ShouldConvertFromInt16()
    {
        // Arrange
        short value = -21555;

        // Act
        var result = IbmConverter.GetBytes(value);

        // Assert
        Assert.Equal(new byte[] {0xAB, 0xCD}, result);
    }

    #endregion

    #region GetBytes from Int32

    [Fact]
    public void ShouldConvertFromInt32()
    {
        // Arrange
        var value = -1985229329;

        // Act
        var result = IbmConverter.GetBytes(value);

        // Assert
        Assert.Equal(new byte[] {0x89, 0xAB, 0xCD, 0xEF}, result);
    }

    #endregion

    #region ToString()

    private const byte _comma = 0x6B;
    private const byte _bang = 0x5A;
    private const byte _H = 0xC8;
    private const byte _e = 0x85;
    private const byte _l = 0x93;
    private const byte _o = 0x96;

    [Fact]
    public void ShouldConvertFromEbcdicToUnicode()
    {
        // Arrange
        var bytes = new[] {_H, _e, _l, _l, _o, _comma};

        // Act
        var result = IbmConverter.ToString(bytes);

        // Assert
        Assert.Equal("Hello,", result);
    }

    [Fact]
    public void ShouldConvertFromEbcdicToUnicode2()
    {
        // Arrange
        var bytes = new[] {_l, _o, _l, _bang};

        // Act
        var result = IbmConverter.ToString(bytes);

        // Assert
        Assert.Equal("lol!", result);
    }

    [Fact]
    public void ShouldConvertToStringGivenStartingIndex()
    {
        // Arrange
        var bytes = new[] {_bang, _bang, _H, _e};
        var startingIndex = 2;

        // Act
        var result = IbmConverter.ToString(bytes, startingIndex);

        // Assert
        Assert.Equal("He", result);
    }

    [Fact]
    public void ShouldConvertToStringGivenStartingIndex2()
    {
        // Arrange
        var bytes = new[] {_H, _e, _l, _l, _o};
        var startingIndex = 1;

        // Act
        var result = IbmConverter.ToString(bytes, startingIndex);

        // Assert
        Assert.Equal("ello", result);
    }

    [Fact]
    public void ShouldConvertToStringGivenIndexAndLength()
    {
        // Arrange
        var bytes = new[] {_H, _o, _H, _o, _H, _o, _l, _e, _bang};
        var startingIndex = 4;
        var length = 4;

        // Act
        var result = IbmConverter.ToString(bytes, startingIndex, length);

        // Assert
        Assert.Equal("Hole", result);
    }

    [Fact]
    public void ShouldConvertToStringGivenIndexAndLength2()
    {
        // Arrange
        var bytes = new[] {_H, _o, _H, _o, _H, _o, _l, _e, _bang};
        var startingIndex = 0;
        var length = 6;

        // Act
        var result = IbmConverter.ToString(bytes, startingIndex, length);

        // Assert
        Assert.Equal("HoHoHo", result);
    }

    [Fact]
    public void ShouldThrowWhenArgumentNullForToStringGivenBytes()
    {
        Assert.Throws<ArgumentNullException>(() => IbmConverter.ToString(null));
    }

    [Fact]
    public void ShouldThrowWhenArgumentNullForToStringGivenBytesAndIndex()
    {
        Assert.Throws<ArgumentNullException>(() => IbmConverter.ToString(null, 1));
    }

    [Fact]
    public void ShouldThrowWhenArgumentNullForToStringGivenBytesAndIndexAndLength()
    {
        Assert.Throws<ArgumentNullException>(() => IbmConverter.ToString(null, 1, 1));
    }

    #endregion

    #region GetBytes from String

    [Fact]
    public void ShouldConvertFromUnicodeToEbcdic()
    {
        // Arrange
        var value = "Hello,";

        // Act
        var result = IbmConverter.GetBytes(value);

        // Assert
        Assert.Equal(new[] {_H, _e, _l, _l, _o, _comma}, result);
    }

    [Fact]
    public void ShouldConvertFromStringGivenStartingIndex()
    {
        // Arrange
        var value = "lol SHe!";
        var startingIndex = 5;

        // Act
        var result = IbmConverter.GetBytes(value, startingIndex);

        // Assert
        Assert.Equal(new[] {_H, _e, _bang}, result);
    }

    [Fact]
    public void ShouldConvertFromStringGivenStartingIndexAndLength()
    {
        // Arrange
        var value = "Hole in the ground";
        var startingIndex = 1;
        var length = 3;

        // Act
        var result = IbmConverter.GetBytes(value, startingIndex, length);

        // Assert
        Assert.Equal(new[] {_o, _l, _e}, result);
    }

    [Fact]
    public void ShouldThrowWhenArgumentNullForGetBytesFromString()
    {
        Assert.Throws<ArgumentNullException>(() => IbmConverter.GetBytes(null, 1, 1));
    }

    #endregion

    #region ToInt16()

    [Fact]
    public void ShouldConvertZeroInt16()
    {
        // Arrange
        var value = new byte[2];

        // Act
        var result = IbmConverter.ToInt16(value);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void ShouldConvertNegativeInt16()
    {
        // Arrange
        var value = new byte[] {0xAB, 0xCD};

        // Act
        var result = IbmConverter.ToInt16(value);

        // Assert
        Assert.Equal(-21555, result);
    }

    [Fact]
    public void ShouldIgnoreTrailingBytesInt16()
    {
        // Arrange
        var value = new byte[] {0, 1, 99, 99};

        // Act
        var result = IbmConverter.ToInt16(value);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ShouldConvertInt16WithStartIndex()
    {
        // Arrange
        var value = new byte[] {99, 99, 99, 0, 1, 99};
        var startIndex = 3;

        // Act
        var result = IbmConverter.ToInt16(value, startIndex);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ShouldConvertAnotherInt16WithStartIndex()
    {
        // Arrange
        var value = new byte[] {99, 99, 2, 0, 99};
        var startIndex = 2;

        // Act
        var result = IbmConverter.ToInt16(value, startIndex);

        // Assert
        Assert.Equal(512, result);
    }

    [Fact]
    public void ShouldThrowWhenArgumentNullForToInt16()
    {
        Assert.Throws<ArgumentNullException>(() => IbmConverter.ToInt16(null));
    }

    [Fact]
    public void ShouldThrowWhenArgumentNullForToInt16WithStartIndex()
    {
        Assert.Throws<ArgumentNullException>(() => IbmConverter.ToInt16(null, 1));
    }

    #endregion

    #region ToInt32()

    [Fact]
    public void ShouldConvertZeroInt32()
    {
        // Arrange
        var bytes = new byte[4];

        // Act
        var result = IbmConverter.ToInt32(bytes);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void ShouldConvertNegativeInt32()
    {
        // Arrange
        var bytes = new byte[] {0x89, 0xAB, 0xCD, 0xEF};

        // Act
        var result = IbmConverter.ToInt32(bytes);

        // Assert
        Assert.Equal(-1985229329, result);
    }

    [Fact]
    public void ShouldIgnoreTrailingBytesInt32()
    {
        // Arrange
        var bytes = new byte[] {0, 0, 0, 1, 99, 99};

        // Act
        var result = IbmConverter.ToInt32(bytes);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ShouldConvertInt32WithStartIndex()
    {
        // Arrange
        var value = new byte[] {99, 99, 99, 0, 0, 0, 1, 99};
        var startIndex = 3;

        // Act
        var result = IbmConverter.ToInt32(value, startIndex);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ShouldConvertAnotherInt32WithStartIndex()
    {
        // Arrange
        var value = new byte[] {99, 99, 1, 2, 4, 0, 99};
        var startIndex = 2;

        // Act
        var result = IbmConverter.ToInt32(value, startIndex);

        // Assert
        Assert.Equal(16909312, result);
    }

    [Fact]
    public void ShouldThrowWhenArgumentNullForToInt32()
    {
        Assert.Throws<ArgumentNullException>(() => IbmConverter.ToInt32(null));
    }

    [Fact]
    public void ShouldThrowWhenArgumentNullForToInt32WithStartIndex()
    {
        Assert.Throws<ArgumentNullException>(() => IbmConverter.ToInt32(null, 1));
    }

    #endregion

    #region ToSingle()

    private void VerifyToSingleReturns(float expected, byte[] value)
    {
        // Act
        var result = IbmConverter.ToSingle(value);

        // Assert
        Assert.InRange(result, expected - 0.0001f, expected + 0.0001f);
    }

    [Fact]
    public void ZeroShouldBeTheSame()
    {
        var expected = 0.0f;
        var bytes = BitConverter.GetBytes(expected);
        VerifyToSingleReturns(expected, bytes);
    }

    [Fact]
    public void One()
    {
        var expected = 1f;
        var bytes = new byte[4];
        bytes[0] = 64 + 1; // 16^1 with bias of 64
        bytes[1] = 16; // 16 to the right of the decimal 
        VerifyToSingleReturns(expected, bytes);
    }

    [Fact]
    public void NegativeOne()
    {
        var expected = -1f;
        var bytes = new byte[4];
        bytes[0] = 128 + 64 + 1; // +128 for negative sign in first bit
        bytes[1] = 16; // 16 to the right of the decimal 
        VerifyToSingleReturns(expected, bytes);
    }

    [Fact]
    public void SampleValueFromSegy()
    {
        var bytes = new byte[] {0xc0, 0x1f, 0xf4, 0x62};
        VerifyToSingleReturns(-0.1248f, bytes);
    }

    [Fact]
    public void SampleValueFromWikipediaToSingle()
    {
        VerifyToSingleReturns(_wikipediaSingle, GetWikipediaSampleBytes());
    }

    [Fact]
    public void ShouldThrowWhenArgumentNullForToSingle()
    {
        Assert.Throws<ArgumentNullException>(() => IbmConverter.ToSingle(null));
    }

    [Fact]
    public void VerifyFractionBytesCanBeConvertedToInt32()
    {
        var fractionBytes = new byte[] {255, 255, 255, 0};
        var i = BitConverter.ToInt32(fractionBytes, 0);
        var u = BitConverter.ToUInt32(fractionBytes, 0);
        Assert.Equal((int) u, i);
    }

    #endregion

    #region GetBytes from Single

    [Fact]
    public void BytesFromZero()
    {
        VerifySingleConversion(0);
    }

    [Fact]
    public void BytesFromOne()
    {
        VerifySingleConversion(1f);
    }

    [Fact]
    public void BytesFromNegativeOne()
    {
        VerifySingleConversion(-1f);
    }

    [Fact]
    public void SampleValueToSegy()
    {
        VerifySingleConversion(-0.1248f);
    }

    [Fact]
    public void SampleValueFromWikipediaToBytes()
    {
        Assert.Equal(GetWikipediaSampleBytes(), IbmConverter.GetBytes(_wikipediaSingle));
    }

    [Fact]
    public void SingleConversionForRandomNumbers()
    {
        var random = new Random(51293);
        for (var i = 0; i < 1000; i++)
        {
            var value = (float) (random.NextDouble() * 100);
            VerifySingleConversion(value);
            VerifySingleConversion(-value);
        }
    }

    private void VerifySingleConversion(float value)
    {
        var result = IbmConverter.GetBytes(value);
        var reverseValue = IbmConverter.ToSingle(result);
        var epsilon = 0.0001f;
        Assert.InRange(reverseValue, value - epsilon, value + epsilon);
    }

    private readonly float _wikipediaSingle = -118.625f;

    private byte[] GetWikipediaSampleBytes()
    {
        // This test comes from the example described here: http://en.wikipedia.org/wiki/IBM_Floating_Point_Architecture#An_Example
        // The difference is the bits have to be reversed per byte because the highest order bit is on the right
        // 0100 0011 0110 1110 0000 0101 0000 0000
        var bools = new[]
        {
            false, true, false, false, false, false, true, true,
            false, true, true, false, true, true, true, false,
            false, false, false, false, false, true, false, true,
            false, false, false, false, false, false, false, false
        };
        var bits = new BitArray(bools);
        var bytes = new byte[4];
        bits.CopyTo(bytes, 0);
        return bytes;
    }

    #endregion

    // TODO: Support for running on Big Endian architecture

    #region ToDecimal

    [Fact]
    public void DecimalZeroShouldBeTheSame()
    {
        var expected = (decimal) 0;
        var bytes = IbmConverter.GetBytes(expected);
        var result = IbmConverter.ToUnpackedDecimal(bytes, 2);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void DecimalShouldBeTheSame()
    {
        var expected = (decimal) 123.45;
        var bytes = IbmConverter.GetBytes(expected);
        var result = IbmConverter.ToUnpackedDecimal(bytes, 2);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void PackedDecimalConversion()
    {
        decimal expected = 12345.6789m;
        byte[] bytes;
        using (var stream = new MemoryStream())
        {
            using (var writer = new BinaryWriter(stream))
            {
                writer.WriteIbmPackedDecimal(expected);
            }
            bytes = stream.ToArray();
        }
        using (var stream = new MemoryStream(bytes))
        {
            using (var reader = new BinaryReader(stream))
            {
                var result = reader.ReadPackedDecimalIbm((byte)bytes.Length,4);
                result.Should().Be(expected);
            }
        }

    }

    #endregion
}