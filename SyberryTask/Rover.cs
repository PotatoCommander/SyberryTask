using System;
using System.Collections.Generic;

public class Rover 
{
    public class LatticeGraph
    {
        public int[,] weights;
        
        public int width;
        public int height;
        public  int[][] directions = 
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
    }
    public class PriorityQueue
    {
        private List<Tuple<VertexPlace, int>> elements = new();
        
        public void Enqueue(VertexPlace item, int priority)
        {
            elements.Add(Tuple.Create(item, priority));
        }

        public VertexPlace Dequeue()
        {
            var returnable = elements[^1];
            elements.RemoveAt(elements.Count - 1);
            return returnable;
        }
    }
    
    public static void CalculateRoverPath(int[,] map)
    {
    }
    
}
