var map = MapParser.ParseMap("inputs/Day4.txt");

var papers = map
    .GetPapersWithAtMostXAdjacentPaper(4);
Console.WriteLine($"There are {papers.Count()} accessible papers in the warehouse.");

var removablePaperCount = papers.Count();
int paperRemoved = 0;
while (removablePaperCount > 0)
{
    var positions = papers
        .Select(p => (p.X, p.Y))
        .ToArray();
    paperRemoved += positions.Length;
    Console.WriteLine($"Removing {positions.Length} papers...");
    map = map.RemovePapers(positions);

    removablePaperCount = map
        .GetPapersWithAtMostXAdjacentPaper(4)
        .Count();
}
Console.WriteLine($"Removed a total of {paperRemoved} papers from the warehouse.");
Console.WriteLine($"After removing papers, there are {map.WithCoordinates().Count(c => c.Item is Paper)} papers left in the warehouse.");


public interface GridObject
{
}

public record Paper() : GridObject;
public record Space() : GridObject;

public static class MapParser
{
    public static GridObject[,] ParseMap(string path)
    {
        string[] lines = File.ReadAllLines(path);
        int height = lines.Length;
        int width = lines[0].Length;
        GridObject[,] map = new GridObject[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char c = lines[y][x];
                map[x, y] = c switch
                {
                    '@' => new Paper(),
                    '.' => new Space(),
                    _ => throw new ArgumentException($"Invalid character '{c}' in map.")
                };
            }
        }

        return map;
    }
}

public static class WarehouseMapExtensions
{

    extension(GridObject[,] map)
    {
        public GridObject[,] RemovePapers((int X, int Y)[] positions)
        {
            foreach (var (x, y) in positions)
            {
                map[x, y] = new Space();
            }
            return map;
        }

        public IEnumerable<(int X, int Y, GridObject Item)> GetPapersWithAtMostXAdjacentPaper(int x)
        {
            var papers = map.WithCoordinates()
                .Where(cell => cell.Item is Paper);
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            var validPapers = 0;
            var directions = new (int dx, int dy)[]
            {
                (-1, 0), // left
                (-1, -1), // up-left
                (0, -1), // up
                (1, -1), // up-right
                (1, 0),  // right
                (1, 1),  // down-right
                (0, 1),  // down
                (-1, 1)   // down-left
            };
            foreach (var paper in papers)
            {
                int adjacentPaperCount = 0;
                foreach (var (dx, dy) in directions)
                {
                    int newX = paper.X + dx;
                    int newY = paper.Y + dy;

                    if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                    {
                        if (map[newX, newY] is Paper)
                        {
                            adjacentPaperCount++;
                        }
                    }
                }
                if (adjacentPaperCount < x)
                {
                    yield return paper;
                }
            }
        }
        public IEnumerable<(int X, int Y, GridObject Item)> WithCoordinates()
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    yield return (x, y, map[x, y]);
                }
            }
        }
    }
}