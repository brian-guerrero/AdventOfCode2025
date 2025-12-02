using System.Text;

var instructions = InstructionParser.GetInstructions("Day1.txt");
// instructions = [new Right(70)];
var safe = new Safe(50);
safe.ApplyInstructions(instructions);

public class Safe
{
    private int _zeroCounter = 0;
    private int _dialPlacement;

    private const int MAX_SAFE_DIAL_VALUE = 99;
    private const int MIN_SAFE_DIAL_VALUE = 0;
    private const int FULL_ROTATION = 100;
    public Safe(int initialDialPlacement)
    {
        _dialPlacement = initialDialPlacement;
    }

    public void ApplyInstructions(List<Instruction> instructions)
    {
        Console.WriteLine($"The dial starts by pointing at {_dialPlacement}.");
        foreach (var instruction in instructions)
        {
            _dialPlacement = instruction switch
            {
                Right r => (_dialPlacement + r.Value) % FULL_ROTATION,
                Left l when _dialPlacement < l.Value => MAX_SAFE_DIAL_VALUE + ((_dialPlacement - l.Value + 1) % FULL_ROTATION),
                Left l => (_dialPlacement - l.Value) % FULL_ROTATION,
                _ => throw new Exception("Should not hit")
            }
        ;
            Console.WriteLine($"The dial is rotated {instruction} to point at {_dialPlacement}.");
            if (_dialPlacement.Equals(MIN_SAFE_DIAL_VALUE))
            {
                _zeroCounter++;
            }
        }
        Console.WriteLine($"The password to this puzzle is {_zeroCounter}");
    }

}

public abstract record Instruction(int Value);

public record Right(int Value) : Instruction(Value)
{
    public override global::System.String ToString()
    {
        return $"R{Value}";
    }
};

public record Left(int Value) : Instruction(Value)
{
    public override global::System.String ToString()
    {
        return $"L{Value}";
    }
}

public static class InstructionParser
{
    public static List<Instruction> GetInstructions(string path)
    {
        List<Instruction> instructions = [];
        string[] lines = File.ReadAllLines(path, Encoding.UTF8);
        foreach (var line in lines)
        {
            var instructionType = line.AsSpan().Slice(start: 0, length: 1);
            var value = line.AsSpan().Slice(start: 1);
            if (instructionType.Contains('L'))
            {
                instructions.Add(new Left(int.Parse(value)));
            }
            if (instructionType.Contains('R'))
            {
                instructions.Add(new Right(int.Parse(value)));
            }

        }
        return instructions;
    }
}
