var problems = WorksheetParser.ParseCephalopodProblems("inputs/Day6.txt");
Console.WriteLine($"There are {problems.Length} cephalopod problems.");
foreach (var problem in problems)
{
    Console.WriteLine(problem);
}
var grandTotal = problems.Sum(p => p.Solve());
Console.WriteLine("The total solution of all cephalopod problems is " + grandTotal);
public static class WorksheetParser
{
    public static CephalopodProblem[] ParseCephalopodProblems(string path)
    {
        string[] lines = File.ReadAllLines(path);
        var problems = new Dictionary<int, (List<long> Values, OperationType Operation)>();
        var operations = lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(op => op.Equals("+") ? OperationType.Addition : OperationType.Multiplication)
            .ToArray();
        for (int i = 0; i < lines.Length - 1; i++)
        {
            var parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < parts.Length; j++)
            {
                if (!problems.ContainsKey(j))
                {
                    problems[j] = (new List<long>(), operations[j]);
                }
                if (long.TryParse(parts[j], out long value))
                {
                    problems[j].Values.Add(value);
                }
            }
        }
        return problems.Values
            .Select(p => new CephalopodProblem(p.Values, p.Operation))
            .ToArray();
    }
}

public record CephalopodProblem(IEnumerable<long> Values, OperationType Operation)
{
    public override string ToString()
    {
        var opSymbol = Operation == OperationType.Addition ? "+" : "*";
        return string.Join($" {opSymbol} ", Values) + $" = {Solve()}";
    }

    public long Solve()
    {
        return Operation switch
        {
            OperationType.Addition => Values.Sum(),
            OperationType.Multiplication => Values.Aggregate(1L, (acc, val) => acc * val),
            _ => throw new NotSupportedException($"Operation {Operation} is not supported.")
        };
    }
};

public enum OperationType
{
    Addition,
    Multiplication
}