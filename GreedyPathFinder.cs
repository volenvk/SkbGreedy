using System;
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
            var energy = state.Energy;
            var result = new List<Point>();
            var tree = new HashSet<PathWithCost>();
            var points = new HashSet<Point>(state.Chests);
            if (state.Goal > points.Count) return new List<Point>();
            if (points.Count < 2) return new List<Point>(state.Chests);
            
            points.Add(state.Position);
            foreach (var start in points)
            {
                foreach (var end in points)
                {
                    if(start == end) continue;
                    var edge = GetNode(state, start, end);
                    tree.Add(edge);
                }
            }
            var sort = tree.OrderBy(x => x.Cost);
            var kraskalTree = new LinkedList<PathWithCost>();
            kraskalTree.AddLast(sort.First(x => x.Start == state.Position));
            points.Remove(kraskalTree.Last.Value.Start);
            points.Remove(kraskalTree.Last.Value.End);
            foreach (var edge in sort)
            {
                if (kraskalTree.First?.Value.Start == edge.End 
                    || kraskalTree.Last?.Value.End != edge.Start) continue;
                kraskalTree.AddLast(edge);
                points.Remove(edge.End);
                if (points.Count == 0) break;
            }
            
            foreach (var edge in kraskalTree)
                result.AddRange(edge.Path.Skip(1));
            return result;
        }

        private PathWithCost GetNode(State state, Point start, Point end)
        {
            return finder.GetPathsByDijkstra(state, start, new[] {end}).First();
        }
    }
}
