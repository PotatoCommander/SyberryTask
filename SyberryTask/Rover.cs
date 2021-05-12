using System;
using System.Collections.Generic;

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
        private int[,] weights;

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
        //TODO: make sure that this method calls only for neighbours
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
    public VertexPlace Location;
    public int Priority;
}
public class PriorityQueue
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
    public List<VertexPlace> cameFrom
        = new List<VertexPlace>();
    public Dictionary<VertexPlace, int> costSoFar
        = new Dictionary<VertexPlace, int>();
    

    public AStarSearch(LatticeGraph graph, VertexPlace start, VertexPlace goal)
    {
        var frontier = new PriorityQueue();
        frontier.Enqueue(start, 0);

        cameFrom.Add(start);
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.X == goal.X && current.Y == goal.Y)
            {
                cameFrom.Add(goal);
                break;
            }

            foreach (var next in graph.GetNeighbours(current))
            {
                int newCost = costSoFar[current] + graph.GetCost(current, next);
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost + Rover.Function(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(current);
                }
            }
        }
    }
}
    
