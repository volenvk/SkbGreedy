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
            var cheats = new HashSet<Point>(state.Chests.OrderBy(point => point.X).ThenBy(point => point.Y));
            var goal = state.Goal;

            if (cheats.Contains(state.Position))
            {
                cheats.Remove(state.Position);
                goal--;
            }
            if (goal > state.Chests.Count) return new List<Point>();

            return GetPaths(state, goal, cheats);
        }

        private List<Point> GetPaths(State state, int goal, ICollection<Point> cheats)
        {
            var energy = state.Energy;
            var result = new List<Point>();
            var last = state.Position;
                
            while (cheats.Count > 0)
            {
                Console.WriteLine("start search");
                var nodes = finder.GetPathsByDijkstra(state, last, cheats);
                var node = nodes.FirstOrDefault();
                Console.WriteLine(node);
                if (node == null) break;
                last = node.End;
                energy -= node.Cost;
                if (energy < 0) break;
                cheats.Remove(last);
                result.AddRange(node.Path.Skip(1));
                goal--;
                if (goal <= 0) break;
            }

            return result;
        }
    }
}
