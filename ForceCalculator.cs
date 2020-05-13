using CSgrapher;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows.Controls;
using Graph;
using System.Windows;
using System.Threading;

namespace ForceCalculator
{
    /// <summary>
    /// Class responsible for calculating forces applied to graph nodes.
    /// It also contains method for rearranging nodes on corresponding <see cref="Canvas"/>
    /// <see cref="CalculateForces"/> method ivokes only one "tick" of arrangement alghoritm, 
    /// so you should use it in loop or timer multiple times (more than 100 recommneded).
    /// </summary>
    class ForceCalculator
    {
        private float factor;
        private readonly Canvas mainCanvas;
        private Graph.Graph graph;

        /// <summary>
        /// Concstructor which targets current <see cref="MainWindow"/> and passes <see cref="Canvas"/>
        /// from it to <see cref="mainCanvas"/> field.
        /// </summary>
        /// <param name="mainWindow"></param>
        public ForceCalculator(MainWindow mainWindow)
        {
            mainCanvas = mainWindow.MainCanvas;
        }
        
        /// <summary>
        /// Method for calculating forces applied to passed <see cref="Graph"/>.
        /// Method iterates 10 times for faster results.
        /// </summary>
        /// <param name="graph"></param>
        public void CalculateForces(Graph.Graph graph)
        {
            this.graph = graph;

            float area = (float)mainCanvas.ActualWidth * (float)mainCanvas.ActualHeight * 0.6f;

            factor = (float)Math.Sqrt(area / graph.NodesCount);

            for (int n = 0; n < 10; n++)
            {
                foreach (Node currentNode in graph.Nodes)
                {
                    RepulsiveDisplacement(currentNode);
                }

                foreach (Edge currentEdge in graph.Edges)
                {
                    AttractiveDisplacement(currentEdge);
                }

                foreach (Node currentNode in graph.Nodes)
                {
                    DisplaceNode(currentNode);
                }
            }
        }

        /// <summary>
        /// Method for calculating ratio applied to <see cref="AttractiveDisplacement"/> displacement values.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns>Float ratio</returns>
        private float AttractiveRatio(float distance)
        {
            float force;

            force = (float)Math.Pow(distance, 2) / factor;

            return force;
        }

        /// <summary>
        /// Method which calculates new displacement based on attractive forces between connected <see cref="Node"/>s.
        /// </summary>
        /// <param name="edge"></param>
        private void AttractiveDisplacement(Edge edge)
        {
            Node firstNode = edge.Nodes.Item1;
            Node secondNode = edge.Nodes.Item2;

            Vector2 distanceVector = Vector2.Subtract(firstNode.Position, secondNode.Position);
            float distanceValue = Vector2.Distance(firstNode.Position, secondNode.Position);
            Vector2 ratio = Vector2.Divide(distanceVector, distanceValue);
            Vector2 displacment = Vector2.Multiply(ratio, AttractiveRatio(distanceValue));

            firstNode.Displacement = Vector2.Subtract(firstNode.Displacement, displacment);
            secondNode.Displacement = Vector2.Add(secondNode.Displacement, displacment);
        }

        /// <summary>
        /// Method for calculating ratio applied to <see cref="RepulsiveDisplacement"/> displacement values.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns>Float ratio</returns>
        private float RepulsiveRatio(float distance)
        {
            float force;

            force = (float)Math.Pow(factor, 2) / distance;

            return force;
        }

        /// <summary>
        /// Method which calcluates new bisplacement based on repulsive forces between every <see cref="Node"/> in current <see cref="Graph"/>.
        /// </summary>
        /// <param name="node"></param>
        private void RepulsiveDisplacement(Node node)
        {
            node.Displacement = new Vector2(0);

            foreach (Node currentRepulsor in graph.Nodes)
            {
                if (currentRepulsor.ID != node.ID)
                {
                    Vector2 distanceVector = Vector2.Subtract(node.Position, currentRepulsor.Position);

                    float distanceValue = Vector2.Distance(node.Position, currentRepulsor.Position);

                    Vector2 ratio = Vector2.Divide(distanceVector, distanceValue);
                    Vector2 displacment = Vector2.Multiply(ratio, RepulsiveRatio(distanceValue));

                    node.Displacement = Vector2.Add(node.Displacement, displacment);
                }
            }
        }

        /// <summary>
        /// Method which displace <see cref="Node"/> with new calculated position.
        /// </summary>
        /// <param name="node"></param>
        private void DisplaceNode(Node node)
        {
            Vector2 distanceVector = Vector2.Divide(node.Displacement, Vector2.Abs(node.Displacement));
            float distanceValue = Vector2.Distance(new Vector2(0), node.Displacement);
            Vector2 ratio = Vector2.Divide(distanceVector, distanceValue);
            Vector2 displacment = Vector2.Multiply(ratio, distanceValue);

            node.Position = Vector2.Add(node.Position, displacment);
            node.Position.X = (float)Math.Min(mainCanvas.ActualWidth - 20, Math.Max(0, node.Position.X));
            node.Position.Y = (float)Math.Min(mainCanvas.ActualHeight - 20, Math.Max(0, node.Position.Y));
        }
    }
}
