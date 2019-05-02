using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{

    /// <summary>
    /// Gets a grid-aligned path from the given start point to the given end point.
    /// </summary>
    /// <param name="start">World position of start point (Path start with be grid-aligned)</param>
    /// <param name="end">World position of end point (Path end with be grid-aligned)</param>
    /// <param name="maxChecks">Number of tiles that will be checked. Bigger the number, further the reach, slower the finding</param>
    public static Path getPath(Vector3 start, Vector3 end, int maxChecks) {

        //GameObject point = Resources.Load("Pather") as GameObject; //VISUAL DEBUG OBJECT

        //The amount of nodes travelled to
        int checks = 0;

        //The grid object to prevent many calls throughout the script.
        Grid grid = Camera.main.GetComponent<Grid>();

        //Destination node & starting point node
        Node endNode = new Node(grid.LocalToCell(end), 0, 0, null);
        Node startNode = new Node(grid.LocalToCell(start), null, endNode);

        //List of nodes to be traveled to
        LinkedList<Node> openList = new LinkedList<Node>();

        //List of nodes already traveled to
        LinkedList<Node> closedList = new LinkedList<Node>();

        //The node we are currently at
        Node curNode = startNode;

        //While we're not at the end...
        while ( !curNode.Equals(endNode) ) {
            
            //For knowing what corners can be checked
            bool[,] isOpen = new bool[3,3];

            //4 cardinal directions
            for (int xx = -1; xx <= 1; xx++) {
                for (int yy = -1; yy <= 1; yy++) {

                    //Ignore corners here
                    if (Mathf.Abs(xx) == Mathf.Abs(yy)) { continue; }

                    //Create the node object that we'll be looking at
                    Node nodeLook = new Node(new Vector3Int(curNode.position.x + xx, curNode.position.y + yy, curNode.position.z),
                                    curNode,
                                    endNode);

                    //if this node isn't blocked & we haven't already been there...
                    if (!nodeLook.closed && !closedList.Contains(nodeLook)) {

                        //Consider this node.
                        ConsiderNode(ref nodeLook,
                            ref curNode,
                            ref endNode,
                            ref openList,
                            ref closedList,
                            false);

                    }

                    //If this wasn't blocked, note that for when we check corners.
                    if (!nodeLook.closed) { isOpen[xx + 1, yy + 1] = true; }

                }
            }

            //Corners
            for (int xx = -1; xx <= 1; xx+=2) {
                for (int yy = -1; yy <= 1; yy+=2) {
                    
                    //Create the node object that we'll be looking at
                    Node nodeLook = new Node(new Vector3Int(curNode.position.x + xx, curNode.position.y + yy, curNode.position.z),
                                    curNode,
                                    endNode);

                    //If the two adjacent blocks are open and the node isnt' blocked and we haven't already been there...
                    if ((isOpen[xx + 1, 1] || isOpen[1, yy + 1]) && !nodeLook.closed && !closedList.Contains(nodeLook)) {
                        
                        //Consider this node.
                        ConsiderNode(ref nodeLook,
                            ref curNode,
                            ref endNode,
                            ref openList,
                            ref closedList,
                            true);

                    }

                }
            }
            
            //Gets the Tile info from the space represented by the current node
            Tile space = curNode.getSpace();

            //If there actually was a tile in the space and if there was a stair in that space...
            if (space != null && space.isStair) {
                
                //The node directly above the stairs
                Node nodeAbove = new Node(new Vector3Int(curNode.position.x, curNode.position.y, curNode.position.z + 1),
                                    curNode,
                                    endNode);

                //The node directly below the stairs
                Node nodeBelow = new Node(new Vector3Int(curNode.position.x, curNode.position.y, curNode.position.z - 1),
                                    curNode,
                                    endNode);

                //If the node above isn't blocked & we haven't already been there...
                if (!nodeAbove.closed && !closedList.Contains(nodeAbove)) {

                    ConsiderNode(ref nodeAbove,
                        ref curNode,
                        ref endNode,
                        ref openList,
                        ref closedList,
                        false);

                }

                //If the node above isn't blocked & we haven't already been there...
                if (!nodeBelow.closed && !closedList.Contains(nodeBelow)) {

                    ConsiderNode(ref nodeBelow,
                        ref curNode,
                        ref endNode,
                        ref openList,
                        ref closedList,
                        false);

                }

            } else {

                Tile floor = curNode.getFloor();

                //If there are stairs below me...
                if (floor != null && floor.isStair) {

                    Node nodeBelow = new Node(new Vector3Int(curNode.position.x, curNode.position.y, curNode.position.z - 1),
                                        curNode,
                                        endNode);

                    if (!nodeBelow.closed && !closedList.Contains(nodeBelow)) {

                        ConsiderNode(ref nodeBelow,
                            ref curNode,
                            ref endNode,
                            ref openList,
                            ref closedList,
                            false);

                    }

                }

            }

            //We've considered all surrounding nodes.

            //Nowhere to travel to
            if (openList.Count == 0) { return null; }

            //Finding node with best pathing score
            Node bestNode = openList.First.Value;

            foreach (Node node in openList) {

                if (node.fCost < bestNode.fCost) {

                    bestNode = node;

                }

            }

            //Travel there
            curNode = bestNode;
            closedList.AddLast(curNode);
            openList.Remove(curNode);
            checks++;

            //GameObject.Instantiate(point, grid.CellToLocal(curNode.position), Quaternion.identity);  //VISUAL DEBUG OBJECT

            //Prevents the algorithm from going too far.
            if (checks > maxChecks) { return null; }

        }

        GameObject shower = Resources.Load("Path_Shower") as GameObject;

        //Make path
        List<Vector3> pathPoints = new List<Vector3>();
        Vector3 cellSize = Camera.main.GetComponent<Grid>().cellSize;
        while(curNode != null) {
            
            Vector3 pathPos = Camera.main.GetComponent<Grid>().CellToLocal(curNode.position);

            pathPoints.Add(pathPos);

            //GameObject.Instantiate(shower, pathPos, Quaternion.identity);  //VISUAL DEBUG OBJECT

            curNode = curNode.parent;

        }

        pathPoints.Reverse();

        return new Path(pathPoints);

    }

    /// <summary>
    /// Considers a single node based on hueristics.
    /// </summary>
    private static void ConsiderNode(ref Node toConsider, ref Node curNode, ref Node endNode, ref LinkedList<Node> openList, ref LinkedList<Node> closedList, bool isCorner) {

        if (!toConsider.closed && !closedList.Contains(toConsider)) {

            if (openList.Contains(toConsider)) {

                //Reconsider

                //The node from the list, to ensure it's by reference
                Node lookNode = openList.Find(toConsider).Value;

                //The node it could be
                Node ifNode = new Node(lookNode.position, curNode, endNode);

                if (isCorner) { ifNode.gCost += 2; }

                if (ifNode.fCost < lookNode.fCost) {

                    //Adjusting values from the considered node if going there from here is better
                    lookNode.gCost = ifNode.gCost;
                    lookNode.parent = curNode;

                }

            } else {

                //First Consider
                openList.AddLast(toConsider);

                if (isCorner) { toConsider.gCost += 2; }

            }
        }

    }

}
