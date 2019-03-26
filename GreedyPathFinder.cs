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
            var points = new HashSet<Point>(state.Chests);
            if (state.Goal > points.Count) return new List<Point>();
            
            var visit = new Stack<PathWithCost>(new []
            {
                new PathWithCost(0, state.Position)
            });
            var finished = new Dictionary<Point, PathWithCost>();
            
            while (visit.Count > 0)
            {
                var node = visit.Pop();
                if (points.Remove(node.End) && finished.ContainsKey(node.Start)) 
                    energy -= finished[node.Start].Cost;
                
                if (energy <= 0) break; 
                
                foreach (var next in GetSortPaths(state, node.End, points))
                {
                    if (finished.ContainsKey(next.Start) 
                        && next.Cost >= finished[next.Start].Cost) continue;
                    visit.Push(next);
                    if (energy - next.Cost < 0) continue;
                    if (finished.ContainsKey(next.Start) 
                        && next.Path.Count > finished[next.Start].Path.Count) continue;
                    finished[next.Start] = next;
                }
            }
            
            var result = new List<Point>();
            foreach (var edge in finished)
                result.AddRange(edge.Value.Path.Skip(1));

            return result;
        }

        private IEnumerable<PathWithCost> GetSortPaths(State state, Point start, IEnumerable<Point> cheats)
        {
            return cheats.Select(cheat => finder.GetPathsByDijkstra(state, start, new []{cheat}).First());
        }
    }
}
