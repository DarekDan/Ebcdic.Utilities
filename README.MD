# Ebcidic Utilities

A .Net 6 port of [Unplugged IBM bits](https://github.com/jfoshee/UnpluggedIbmBits)

## Sample usage

```c#
using var stream = File.OpenWrite("punchcard.bin");
using var writer = new BinaryWriter(stream);
writer.WriteEbcdic(randomizerText.Generate());
writer.WriteIbmSingle(randomizerFloat.Generate()!.Value);
writer.WriteBigEndian(randomizerInt16.Generate()!.Value);
writer.WriteBigEndian(randomizerInt32.Generate()!.Value);
```
