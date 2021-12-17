using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Ebcdic.Utilities.Tests;

public class BinaryWriterExtensionMethodsTest
{
    [Fact]
    public void WriteEbcdicShouldWriteConvertedBytes()
    {
        var value = "Nope";
        var expected = new byte[] {0xD5, 0x96, 0x97, 0x85};
        VerifyBytesWritten(w => w.WriteEbcdic(value), expected);
    }

    [Fact]
    public void WriteInt16ShouldReverseBytes()
    {
        short value = 100 + 7 * 256;
        var expected = new byte[] {7, 100};
        VerifyBytesWritten(w => w.WriteBigEndian(value), expected);
    }

    [Fact]
    public void WriteInt32ShouldReverseBytes()
    {
        var value = 13 + (11 + (29 + 17 * 256) * 256) * 256;
        var expected = new byte[] {17, 29, 11, 13};
        VerifyBytesWritten(w => w.WriteBigEndian(value), expected);
    }

    [Fact]
    public void WriteSingleShouldConvertToIbmFormat()
    {
        var value = 64.125488f;
        var expected = new byte[] {66, 64, 32, 32};
        VerifyBytesWritten(w => w.WriteIbmSingle(value), expected);
    }

    private void VerifyBytesWritten(Action<BinaryWriter> act, byte[] expected)
    {
        var bytes = new byte[expected.Length];
        using (var stream = new MemoryStream(bytes))
        using (var writer = new BinaryWriter(stream))
        {
            // Act
            act(writer);

            // Assert
            stream.Position.Should().Be(expected.Length, "Wrong number of bytes were written.");
        }

        bytes.Should().Equal(expected);
    }
}