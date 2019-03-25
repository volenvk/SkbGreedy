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
                foreach (var edge in GetPaths(state, last, points).OrderBy(x=>x.Cost))
                {
                    energy -= edge.Cost;
                    if (energy < 0) break;
                    tree.Add(edge);
                    if (HasCycle(tree))
                        tree.Remove(edge);
                    
                    last = edge.End;
                    points.Remove(last);
                    break;
                }
            }
            
            var result = new List<Point>();
            foreach (var edge in tree)
                result.AddRange(edge.Path.Skip(1));

            return result;
        }

        private static bool HasCycle(List<PathWithCost> edges)
        {
            return edges.All(x => edges.All(y => y.Path.SequenceEqual(x.Endpoints.Reverse())));
        }

        private IEnumerable<PathWithCost> GetPaths(State state, Point start, IEnumerable<Point> cheats)
        {
            return cheats.Select(cheat => finder.GetPathsByDijkstra(state, start, new []{cheat})
                .First());
        }
    }
}
