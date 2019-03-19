using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greedy.Architecture;
using System.Drawing;

namespace Greedy
{  
    public class DijkstraPathFinder
    {
        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
        {
            var points = new List<Point>(targets);
            if(targets == null || !targets.Any()) yield break;
            var frontier = new Queue<Point>();
            frontier.Enqueue(start);
            var cameFrom = new Dictionary<Point, PathWithCost> {[start] = new PathWithCost(0, start)};
            var costSoFar = new Dictionary<Point, int> {[start] = 0};

            while (frontier.Any() && points.Any())
            {
                var current = frontier.Dequeue();

                foreach (Point next in Nodes(current))
                {
                    if (!state.InsideMap(next)) continue;
                    if (state.IsWallAt(next)) continue;
                    var newCost = costSoFar[current] + state.CellCost[next.X, next.Y];
                    if (costSoFar.ContainsKey(next) && newCost >= costSoFar[next]) continue;
                    costSoFar[next] = newCost;
                    frontier.Enqueue(next);
                    var paths = new List<Point>(cameFrom[current].Path){next};
                    cameFrom[next] = new PathWithCost(newCost, paths.ToArray());
                }
                if (!targets.Contains(current)) continue;
                yield return cameFrom[current];
                points.Remove(current);
            }
        }
        
        private IEnumerable<Point> Nodes(Point point)
        {
            for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
                if (dx != 0 && dy != 0)
                    continue;
                else
                {
                    yield return new Point
                    {
                        X = point.X + dx,
                        Y = point.Y + dy
                    };
                }
        }
    }
}