using Natak.Domain.Enums;

namespace Natak.Domain.Managers;

public sealed class TileManager
{
    private const int Size = 5;
    
    private static readonly Random random = new();
    
    private readonly List<Tile> tiles = [];

    public TileManager(List<Tile> tiles)
    {
        this.tiles = tiles;
    }
    
    public TileManager()
    {
        GenerateTiles();
    }
    
    public List<Tile> GetTiles()
        => tiles;
    
    public List<Tile> GetTiles(int activationNumber)
        => tiles.Where(x => x.ActivationNumber == activationNumber).ToList();
    
    public List<Tile> GetTiles(ResourceType type)
        => tiles.Where(x => x.Type == type).ToList();
    
    public Tile? GetTile(Point point)
        => tiles.FirstOrDefault(x => x.Point.Equals(point));
    
    private void GenerateTiles()
    {
        var remainingResourceTileTypes = DomainConstants.GetTileResourceTypeTotals();
        var remainingActivationNumbers = DomainConstants.GetTileActivationNumberTotals();

        for (var x = 0; x < Size; x++)
        {
            for (var y = 0; y < Size; y++)
            {
                var point = new Point(x, y);
                
                if (!IsValidPoint(point))
                {
                    continue;
                }
                
                AddNewTile(
                    point,
                    remainingResourceTileTypes,
                    remainingActivationNumbers);
            }
        }
        /// Tentative. Cannot be placed until the location of the desert is decided.
        bool IsAbc = true;
        if (IsAbc)
        {
            int[] abc = { 5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11 };
            Point[] points = new Point[] {
                new Point(2, 0), new Point(1, 1), new Point(0, 2),
                new Point(0, 3),  new Point(0, 4),  new Point(1, 4),  new Point(2, 4),
                new Point(3, 3),  new Point(4, 2),  new Point(4, 1),  new Point(4, 0),
                new Point(3, 0),  new Point(2, 1),  new Point(1, 2),  new Point(1, 3),
                new Point(2, 3),  new Point(3, 2),  new Point(3, 1),  new Point(2, 2), };
            int n = 0;
            foreach (var point in points)
            {
                foreach (var tile in tiles)
                {
                    if (tile.Point.X == point.X && tile.Point.Y == point.Y)
                    {
                        if (tile.ActivationNumber != 0)
                        {
                            tile.ActivationNumber = abc[n++];
                        }
                        break;
                    }
                }
            }
        }

    }

    private void AddNewTile(
        Point point,
        Dictionary<ResourceType, int> remainingResourceTileTypes,
        Dictionary<int, int> remainingActivationNumbers)
    {
        if (remainingResourceTileTypes is null || remainingResourceTileTypes.Count == 0)
        {
            throw new ArgumentException($"{nameof(remainingResourceTileTypes)} must not be null or empty.");
        }

        if (remainingActivationNumbers is null || remainingActivationNumbers.Count == 0)
        {
            throw new ArgumentException($"{nameof(remainingActivationNumbers)} must not be null or empty.");
        }

        ResourceType type;
        var lowestTileTypeNum = (int)remainingResourceTileTypes.First().Key;
        var highestTileTypeNum = (int)remainingResourceTileTypes.Last().Key;

        do
        {
            type = (ResourceType)random.Next(lowestTileTypeNum, highestTileTypeNum + 1);
        }
        while (remainingResourceTileTypes[type] <= 0);

        remainingResourceTileTypes[type]--;

        if (type == ResourceType.None)
        {
            tiles.Add(new Tile(type, 0, point));
            return;
        }

        int activationNumber;
        var lowestActivationNum = remainingActivationNumbers.First().Key;
        var highestActivationNum = remainingActivationNumbers.Last().Key;

        do
        {
            activationNumber = random.Next(lowestActivationNum, highestActivationNum + 1);
        }
        while (remainingActivationNumbers[activationNumber] <= 0);

        remainingActivationNumbers[activationNumber]--;

        tiles.Add(new Tile(type, activationNumber, point));
    }

    private static bool IsValidPoint(Point point)
    {
        var x = point.X;
        var y = point.Y;

        return x + y >= 2 && x + y <= Size + 1;
    }
}