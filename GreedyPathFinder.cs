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
            Console.WriteLine($"new test {state.MazeName}");
            var result = new List<Point>();
            var cheats = new HashSet<Point>(state.Chests);
            var energy = state.Energy;
            var goal = state.Goal;
            var last = state.Position;
            if (cheats.Contains(last))
            {
                cheats.Remove(last);
                goal--;
            }
            if (goal > state.Chests.Count) return new List<Point>();
            
            while (cheats.Count > 0)
            {
                Console.WriteLine("start search");
                var node = finder.GetPathsByDijkstra(state, last, cheats)
                        .OrderBy(x => x?.Cost)
                        .FirstOrDefault();
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
