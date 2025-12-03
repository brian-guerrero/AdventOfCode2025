using System.IO;


var barcodeRanges = BarcodeParser.Parse("inputs/day2.txt");
List<Barcode> invalidBarcodes = [];
foreach (var range in barcodeRanges)
{
    //invalidBarcodes.AddRange(range.GetInvalidBarcodes(2));
    invalidBarcodes.AddRange(range.GetInvalidBarcodes());
    Console.WriteLine($"{range.Lower.Value}-{range.Higher.Value} has {invalidBarcodes.Count()} invalid barcodes: {string.Join(", ", invalidBarcodes.Select(b => b.Value))}");
}
Console.WriteLine($"Total invalid barcodes so far: {invalidBarcodes.Select(x => (long)x.Value).Sum(b => b)}");

public static class BarcodeParser
{
    public static BarcodeRange[] Parse(string path)
    {
        var barcodes = File.ReadAllText(path);
        var groups = barcodes.Split(',');
        BarcodeRange[] ranges = new BarcodeRange[groups.Length];
        int i = 0;
        foreach (var group in groups)
        {
            var range = group.Split('-');
            ranges[i] = new(new Barcode(long.Parse(range[0])), new Barcode(long.Parse(range[1])));
            i++;
        }
        return ranges;
    }
}

public record BarcodeRange(Barcode Lower, Barcode Higher);

public record Barcode(long Value)
{
    private string _stringValue => Value.ToString();
    public bool IsInvalid(int timesRepeatedDigit)
    {
        var midIndex = _stringValue.Length / timesRepeatedDigit;
        var parts = new HashSet<string>();
        for (int i = 0; i < timesRepeatedDigit; i++)
        {
            var part = _stringValue.Substring(i * midIndex, midIndex);
            var wasAdded = parts.Add(part);
            if (i > 0 && wasAdded)
            {
                return false;
            }
        }
        return parts.Count == 1;
    }

    public bool IsInvalid()
    {
        for (int repeatCount = 2; repeatCount <= _stringValue.Length; repeatCount++)
        {
            if (repeatCount == (_stringValue.Length - 1) || _stringValue.Length % repeatCount != 0)
            {
                continue;
            }
            if (IsInvalid(repeatCount))
            {
                return true;
            }
        }
        return false;
    }
}

public static class BarcodeExtensions
{
    public static IEnumerable<Barcode> GetFullRange(this BarcodeRange range)
    {
        List<Barcode> barcodes = [];
        for (long i = range.Lower.Value; i <= range.Higher.Value; i++)
        {
            yield return new Barcode(i);
        }
    }

    public static Barcode[] GetInvalidBarcodes(this BarcodeRange range) => range.GetFullRange().Where(b => b.IsInvalid()).ToArray();
    public static Barcode[] GetInvalidBarcodes(this BarcodeRange range, int timesRepeated) => range.GetFullRange().Where(b => b.IsInvalid(timesRepeatedDigit)).ToArray();
}
