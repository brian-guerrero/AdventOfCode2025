var (ranges, ingredients) = IngredientParser.Parse("inputs/Day5.txt");

Console.WriteLine($"There are {ingredients.Length} available ingredients.");
int freshAvalableCount = 0;
foreach (var ingredient in ingredients)
{
    if (ranges.HasIngredient(ingredient))
    {
        Console.WriteLine($"Ingredient {ingredient.IngredientId} is fresh.");
        freshAvalableCount++;
    }
}

Console.WriteLine($"There are {freshAvalableCount} fresh ingredients available.");

public static class IngredientParser
{
    public static (FreshIngredientRange[] Ranges, AvailableIngredient[] AvailableIngredients) Parse(string path)
    {
        string[] lines = File.ReadAllLines(path);
        var ranges = new List<FreshIngredientRange>();
        var availableIngredients = new List<AvailableIngredient>();

        var index = Array.FindIndex(lines, line => line.Equals(Environment.NewLine) || string.IsNullOrWhiteSpace(line));
        for (int i = 0; i < index; i++)
        {
            var parts = lines[i].Split('-');
            var minFresh = long.Parse(parts[0]);
            var maxFresh = long.Parse(parts[1]);
            ranges.Add(new FreshIngredientRange(minFresh, maxFresh));
        }
        for (int i = index + 1; i < lines.Length; i++)
        {
            var ingredientId = long.Parse(lines[i]);
            availableIngredients.Add(new AvailableIngredient(ingredientId));
        }

        return (ranges.ToArray(), availableIngredients.ToArray());
    }
}

public record FreshIngredientRange(long MinFreshId, long MaxFreshId);

public record AvailableIngredient(long IngredientId);


public static class IngredientExtensions
{
    extension(IEnumerable<FreshIngredientRange> ranges)
    {
        public bool HasIngredient(AvailableIngredient ingredient)
        {
            return ranges.Any(r => ingredient.IngredientId >= r.MinFreshId && ingredient.IngredientId <= r.MaxFreshId);
        }
    }
}