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
            if (state.Goal > state.Chests.Count) return new List<Point>();
            var result = new List<Point>();
            var cheats = new HashSet<Point>(state.Chests);
            var energy = state.Energy;
            var goal = state.Goal;
            var last = state.Position;
            while (cheats.Count > 0)
            {
                var node = cheats
                    .Select(cheat => finder.GetPathsByDijkstra(state, last, new[] {cheat}).FirstOrDefault())
                    .OrderBy(x => x?.Cost)
                    .FirstOrDefault(x=> x?.Cost > 0);
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
