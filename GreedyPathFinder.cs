using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        private readonly DijkstraPathFinder finder = new DijkstraPathFinder();

        public List<Point> FindPathToCompleteGoal(State state)
        {
            var tree = new List<PathWithCost>();
            var last = state.Position;
            var energy = state.Energy;
            var points = new HashSet<Point>(state.Chests);
            if (state.Goal > points.Count) return new List<Point>();
            while (points.Count > 0)
            {
                foreach (var edge in GetSortPaths(state, last, points))
                {
                    energy -= edge.Cost;
                    tree.Add(edge);
                    if (HasCycle(tree))
                    {
                        tree.Remove(edge);
                        continue;
                    }
                    
                    last = edge.End;
                    points.Remove(last);
                    break;
                }
                if (energy < 0) break;
            }
            
            var result = new List<Point>();
            foreach (var edge in tree)
                result.AddRange(edge.Path.Skip(1));

            return result;
        }

        private static bool HasCycle(List<PathWithCost> edges)
        {
            var e = edges.GetEnumerator();
            if (!e.MoveNext()) return false;
            var last = e.Current;
            var result = false;
            while (e.MoveNext())
            {
                var prev = e.Current;
                var items = prev.Path.Except(prev.Path);
                result = items.Any();
            }

            return result;
        }

        private IEnumerable<PathWithCost> GetSortPaths(State state, Point start, IEnumerable<Point> cheats)
        {
            return cheats.Select(cheat => finder.GetPathsByDijkstra(state, start, new []{cheat})
                .First()).OrderBy(x=>x.Cost);
        }
    }
}
