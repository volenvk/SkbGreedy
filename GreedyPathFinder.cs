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
            var result = new List<Point>();
            var last = state.Position;
            var energy = state.Energy;
            var points = new HashSet<Point>(state.Chests);
            while (points.Count > 0)
            {
                var path = GetMinTrack(GetPaths(state, last, points));
                energy -= path.Cost;
                if (energy < 0) break;
                result.AddRange(path.Path.Skip(1));
                last = path.End;
                points.Remove(last);
            }

            return result;
        }

        private IEnumerable<PathWithCost> GetPaths(State state, Point start, IEnumerable<Point> cheats)
        {
            var points = new Queue<Point>(cheats);
            while (points.Count > 0)
            {
                var point = points.Dequeue();
                var path = finder.GetPathsByDijkstra(state, start, new []{point}).FirstOrDefault();
                if (path == null) continue;
                yield return path;
            }
        }


        private static PathWithCost GetMinTrack(IEnumerable<PathWithCost> tracks)
        {
            if (tracks == null) return null;
            PathWithCost result = null;
            var min = int.MaxValue;
            foreach (var track in tracks)
            {
                if (track.Cost >= min) continue;
                result = track;
                min = track.Cost;
            }

            return result;
        }
    }
}
