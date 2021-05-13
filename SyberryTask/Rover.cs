using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Rover 
{
    static public int Function(VertexPlace a, VertexPlace b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
    
    
    public static void CalculateRoverPath(int[,] map)
    {
        
    }
}
public class LatticeGraph
    {
        public int[,] weights;

        private int width;
        private int height;

        private int[][] directions = 
        {
            //Left
            new int[] { 1, 0 },
            //Right
            new int[] { -1, 0 },
            //Down
            new int[] { 0, 1},
            //Up
            new int[] { 0, -1}
        };

        public LatticeGraph(int[,] array)
        {
            weights = array;
            width = array.GetLength(0);
            height = array.GetLength(1);
        }
        //returns: weight of edge between two neighbour vertexes in graph
        public int GetCost(VertexPlace a, VertexPlace b)
        {
            var x1 = a.X;
            var y1 = a.Y;
            var x2 = b.X;
            var y2 = b.Y;
            return Math.Abs(weights[x1, y1] - weights[x2, y2]) + 1;
        }

        private bool IsInMatrix(VertexPlace coords)
        {
            return (0 <= coords.X && coords.X < width && 0 <= coords.Y && coords.Y < height);
        }
        public IEnumerable<VertexPlace> GetNeighbours(VertexPlace coords)
        {
            var listOfNeighbours = new List<VertexPlace>();
            foreach (var dir in directions) 
            {
                var neighbour = new VertexPlace(coords.X + dir[0], coords.Y + dir[1]);
                if (IsInMatrix(neighbour)) 
                {
                    listOfNeighbours.Add(neighbour);
                }
            }

            return listOfNeighbours;
        }
    }
public class VertexPlace
    {
        public int X;
        public int Y;
        public VertexPlace(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object? obj)
        {
             return Equals(obj as VertexPlace);
        }

        protected bool Equals(VertexPlace other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

public class Vertex
{
    public VertexPlace From;
    public VertexPlace To;
}
public class QueueWithPriority
    {
        private List<Tuple<VertexPlace, double>> elements = new();

        public int Count => elements.Count;

        public void Enqueue(VertexPlace item, double priority)
        {
            elements.Add(Tuple.Create(item, priority));
        }

        public VertexPlace Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++) 
            {
                if (elements[i].Item2 < elements[bestIndex].Item2) 
                {
                    bestIndex = i;
                }
            }

            var bestItem = elements[bestIndex].Item1;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
        }
public class AStarSearch
{
    private List<Vertex> cameFrom = new();
    private Dictionary<VertexPlace, int> costSoFar = new();
    public LatticeGraph graph;
    
    public AStarSearch(LatticeGraph g)
    {
        graph = g;
        var start = new VertexPlace(0, 0);
        var goal = new VertexPlace(graph.weights.GetLength(0) - 1, graph.weights.GetLength(1) - 1);
        var frontier = new QueueWithPriority();
        frontier.Enqueue(start, 0);

        cameFrom.Add(new Vertex(){From = null, To = start});
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.X == goal.X && current.Y == goal.Y)
            {
                break;
            }

            foreach (var next in graph.GetNeighbours(current))
            {
                int newCost = costSoFar[current]
                                 + graph.GetCost(current, next);
                if (!costSoFar.ContainsKey(next)
                    || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost + Rover.Function(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(new Vertex(){From = current, To = next});
                }
            }
        }
    }

    public List<VertexPlace> ExtractPath()
    {
        var path = new List<VertexPlace>();
        var elem = cameFrom.Find(x => Equals(x.To, new VertexPlace(graph.weights.GetLength(0) - 1,graph.weights.GetLength(1) - 1)));
        path.Add(elem.To);
        var fromelem = elem.From;
        while (fromelem != null && !fromelem.Equals(new VertexPlace(0,0)))
        {
            path.Add(fromelem);
            fromelem = cameFrom.Find(x => Equals(x.To, fromelem))?.From;
        }
        path.Add(cameFrom[0].To);

        return path;
    }
}

