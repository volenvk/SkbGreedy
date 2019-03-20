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
			if(state.Chests.Count < 2) return result;
			var points = new HashSet<Point>(state.Chests);
			var last = state.Position;
			while (points.Count > 0)
			{
				var paths = finder.GetPathsByDijkstra(state, last, points);
				if(paths == null && !paths.Any()) return result;
				var path = paths.OrderBy(x => x.Path.Count).First();
				var steps = state.Energy - path.Cost;
				if (steps <= 0) return result;
				last = path.End;
				points.Remove(last);
				result.AddRange(path.Path.Skip(1));
			}
			
			return result;
		}
	}
}