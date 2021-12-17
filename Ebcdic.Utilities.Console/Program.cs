using System.Diagnostics;
using Ebcdic.Utilities;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

var sw = Stopwatch.StartNew();

var randomizerText =
    RandomizerFactory.GetRandomizer(new FieldOptionsTextWords { Min = 2, Max = 10, UseNullValues = false });
var randomizerFloat = new RandomizerNumber<float>(new FieldOptionsFloat { Min = -100, Max = 100, UseNullValues = false });
var randomizerInt16 = new RandomizerNumber<short>(new FieldOptionsShort { Min = -100, Max = 100, UseNullValues = false });
var randomizerInt32 = new RandomizerNumber<int>(new FieldOptionsInteger { Min = -100, Max = 100, UseNullValues = false });

using var stream = File.OpenWrite("punchcard.bin");
using var writer = new BinaryWriter(stream);
for (var i = 0; i < 1000000; i++)
{
    for (int j = 0; j < 10; j++)
    {
        writer.WriteEbcdic(randomizerText.Generate());
        writer.WriteIbmSingle(randomizerFloat.Generate()!.Value);
        writer.WriteBigEndian(randomizerInt16.Generate()!.Value);
        writer.WriteBigEndian(randomizerInt32.Generate()!.Value);
    }
}

sw.Stop();
Console.WriteLine(sw.Elapsed);