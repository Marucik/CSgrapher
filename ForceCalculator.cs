using CSgrapher;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows.Controls;
using Graph;

namespace ForceCalculator
{
    class ForceCalculator
    {
        private float factor;
        private float temperature;
        private Canvas mainCanvas;
        public ForceCalculator(MainWindow mainWindow)
        {
            mainCanvas = mainWindow.MainCanvas;
        }

        public void CalculateForces(Graph.Graph graph)
        {
            float area = (float)mainCanvas.ActualWidth * (float)mainCanvas.ActualHeight;
            temperature = (float)Math.Max(mainCanvas.ActualWidth, mainCanvas.ActualHeight) / 10;

            factor = (float)Math.Sqrt(area / graph.NodesCount);

            for (int n = 0; n < 100; n++)
            {
                foreach (Node currentNode in graph.Nodes)
                {
                    currentNode.Displacement = new Vector2(0);

                    foreach (Node currentRepulsor in graph.Nodes)
                    {
                        if (currentRepulsor.ID != currentNode.ID)
                        {
                            Vector2 distanceVector = Vector2.Subtract(currentNode.Position, currentRepulsor.Position);

                            float distanceValue = Vector2.Distance(currentNode.Position, currentRepulsor.Position);

                            Vector2 ratio = Vector2.Divide(distanceVector, distanceValue);
                            Vector2 displacment = Vector2.Multiply(ratio, Repulsive(distanceValue));

                            currentNode.Displacement = Vector2.Add(currentNode.Displacement, displacment);
                        }
                    }
                }

                foreach (Edge currentEdge in graph.Edges)
                {
                    Node firstNode = currentEdge.Nodes[0];
                    Node secondNode = currentEdge.Nodes[1];

                    Vector2 distanceVector = Vector2.Subtract(firstNode.Position, secondNode.Position);
                    float distanceValue = Vector2.Distance(firstNode.Position, secondNode.Position);
                    Vector2 ratio = Vector2.Divide(distanceVector, distanceValue);
                    Vector2 displacment = Vector2.Multiply(ratio, Attractive(distanceValue));

                    firstNode.Displacement = Vector2.Subtract(firstNode.Displacement, displacment);
                    secondNode.Displacement = Vector2.Add(secondNode.Displacement, displacment);
                }

                foreach (Node currentNode in graph.Nodes)
                {
                    Vector2 distanceVector = Vector2.Divide(currentNode.Displacement, Vector2.Abs(currentNode.Displacement));
                    float distanceValue = Vector2.Distance(new Vector2(0), currentNode.Displacement);
                    Vector2 ratio = Vector2.Divide(distanceVector, distanceValue);
                    Vector2 displacment = Vector2.Multiply(ratio, distanceValue);

                    currentNode.Position = Vector2.Add(currentNode.Position, displacment);
                }

            }
        }

        private float Attractive(float distance)
        {
            float force;

            force = (float)Math.Pow(distance, 2) / factor;

            return force;
        }

        private float Repulsive(float distance)
        {
            float force;

            force = (float)Math.Pow(factor, 2) / distance;

            return force;
        }
    }
}
