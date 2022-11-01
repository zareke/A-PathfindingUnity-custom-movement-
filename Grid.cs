using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Grid : MonoBehaviour
{
   public bool displayGridGizmos;
   public LayerMask unwalkableMask;
   public Vector2 gridWorldSize;
   public float nodeRadius;
   private Node[,] grid;


   private float nodeDiameter;
   private int gridSizeX, gridSizeY;
   private void Awake()
   {
      nodeDiameter = nodeRadius * 2;
      gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
      gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

      CreateGrid();


   }

   public int MaxSize
   {
      get
      {
         return gridSizeX * gridSizeY;
      }
   }

   void CreateGrid()
   {


      grid = new Node[gridSizeX, gridSizeY];
      Vector3 wordlBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y/2;
      for (int x = 0; x < gridSizeX; x++)
      {

         for (int y = 0; y < gridSizeY; y++)
         {
            Vector3 worldPoint = wordlBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                 Vector3.up * (y * nodeDiameter + nodeRadius);  
            bool walkable = true;
            if (Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask) != null)
            {
               
               walkable = false;
               
            }
            grid[x, y] = new Node(walkable, worldPoint, x,y);

         }
         
      }

   }

   public List<Node> GetNeighbours(Node node)
   {


      List<Node> neighbours = new List<Node>();

      for (int x = -1; x <= 1; x++)
      {
         
         for (int y = -1; y <= 1; y++)
         {

            if (x == 0 && y == 0)
            {
               continue;
            }

            int checkX = node.gridX + x;
            int checkY = node.gridY + y;
            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
            {
               neighbours.Add(grid[checkX,checkY]);
            }

         }
         
      }

      return neighbours;
   }
   public Node NodeFromWorldPoint(Vector3 _worldPosition)
   {
      float percentX = (_worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
      float percentY = (_worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
      percentX = Mathf.Clamp01(percentX);
      percentY = Mathf.Clamp01(percentY);

      int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
      int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
      return grid[x, y];


   }

   public List<Node> path;
   private void OnDrawGizmos()
   {
      
     
      
      
      Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

   
         
         if (grid != null && displayGridGizmos)
      {
         foreach(Node n in grid)
         {
            Gizmos.color = (n.walkable) ? Color.white : Color.red;
           
            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            
         }
         
         
         
         
      }
      
      
   }
}
