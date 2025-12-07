var problems = WorksheetParser.ParseCephalopodProblems("inputs/Day6.txt");
Console.WriteLine($"There are {problems.Length} cephalopod problems.");
foreach (var problem in problems)
{
    Console.WriteLine(problem);
}
var grandTotal = problems.Sum(p => p.Solve());
Console.WriteLine("The total solution of all cephalopod problems is " + grandTotal);

var correctProblems = WorksheetParser.ParseCephalopodProblems("inputs/Day6.txt", TextDirection.RTL_IN_COLUMN);
Console.WriteLine($"There are {correctProblems.Length} cephalopod problems.");
foreach (var problem in correctProblems)
{
    Console.WriteLine(problem);
}
grandTotal = correctProblems.Sum(p => p.Solve());
Console.WriteLine("The total solution of all cephalopod problems is " + grandTotal);
public static class WorksheetParser
{
    public static CephalopodProblem[] ParseCephalopodProblems(string path, TextDirection direction = TextDirection.LTR)
    {
        string[] lines = File.ReadAllLines(path);
        var problems = new Dictionary<int, (List<long> Values, OperationType Operation)>();
        var operations = lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(op => op.Equals("+") ? OperationType.Addition : OperationType.Multiplication)
            .ToArray();
        if (direction == TextDirection.RTL_IN_COLUMN)
        {
            ParseRightToLeftInColumn(lines, problems, operations);
        }
        if (direction == TextDirection.LTR)
        {
            ParseLeftToRight(lines, problems, operations);
        }
        return problems.Values
            .Select(p => new CephalopodProblem(p.Values, p.Operation))
            .ToArray();
    }

    private static void ParseRightToLeftInColumn(string[] lines, Dictionary<int, (List<long> Values, OperationType Operation)> problems, OperationType[] operations)
    {
        Dictionary<int, List<char>> columnValues = new();
        for (int i = 0; i < lines.Length - 1; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (!columnValues.ContainsKey(j))
                {
                    columnValues[j] = new List<char>();
                }
                columnValues[j].Add(lines[i][j]);
            }
        }
        var skipColumns = columnValues.Where(column => column.Value.All(c => c == ' ')).Select(column => column.Key).ToList();
        var operationIndex = operations.Length - 1;
        foreach (var kvp in columnValues.OrderByDescending(k => k.Key))
        {
            if (skipColumns.Contains(kvp.Key))
            {
                operationIndex--;
                continue;
            }
            var charList = kvp.Value;
            var valueStr = new string(charList.ToArray());

            if (long.TryParse(valueStr, out long value))
            {
                if (!problems.ContainsKey(operationIndex))
                {
                    problems[operationIndex] = (new List<long>(), operations[operationIndex]);
                }
                problems[operationIndex].Values.Add(value);
            }
        }
    }

    private static void ParseLeftToRight(string[] lines, Dictionary<int, (List<long> Values, OperationType Operation)> problems, OperationType[] operations)
    {
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

public enum TextDirection
{
    RTL_IN_COLUMN,
    LTR
}