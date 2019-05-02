using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Node represents some space of some size in the world.
/// <remarks>Used for pathfinding. Only recognizes colliders in the "solid" layer.</remarks>
/// </summary>
public class Node
{
    //Cost to move to this node
    public int gCost;
    //Distance to end node
    public int hCost;
    //evaluation score
    public int fCost {
        get {
            return gCost + hCost;
        }
    }
    //cost to walk to this node
    public int moveCost {
        get {
            Tile floor = getFloor();
            if (floor != null) {
                return floor.moveCost;
            } else {
                return 99999999;
            }
        }
    }
    //What node was chosen before considering this one
    public Node parent;

    //position in grid cells
    public readonly Vector3Int position;
    
    /// <summary>
    /// Creates a node with manual values.
    /// </summary>
    /// <param name="position">Node position in terms of grid cells.</param>
    /// <param name="size">Node size in terms of grid cells.</param>
    /// <param name="gCost">Cost to move here (parent gCost + moveCost)</param>
    /// <param name="hCost">Distance to end Node</param>
    /// <param name="parent">The Node from which this Node was traveled to</param>
    public Node(Vector3Int position, int gCost, int hCost, Node parent) {
        this.position = position;
        this.gCost = gCost;
        this.hCost = hCost;
        this.parent = parent;
    }

    /// <summary>
    /// Creates a Node calculating values using given parent Node and destination Node.
    /// </summary>
    /// <param name="position">Node position in terms of grid cells.</param>
    /// <param name="size">Node size in terms of grid cells.</param>
    /// <param name="parent">The Node from which this Node was traveled to</param>
    /// <param name="endNode">The destination node</param>
    public Node(Vector3Int position, Node parent, Node endNode) {
        this.position = position;
        if (parent != null) { this.gCost = parent.gCost + moveCost; }
        this.hCost = (int)Mathf.Sqrt(Mathf.Pow(endNode.position.x * 10 - position.x * 10, 2) + Mathf.Pow(endNode.position.y * 10 - position.y * 10, 2) + Mathf.Pow(endNode.position.y * 10 - position.y * 10, 2));
        this.parent = parent;
    }

    /// <summary>
    /// Gets the floor Tile below the given space.
    /// </summary>
    /// <returns>Returns the Tile component in the GameObject below the node.</returns>
    public Tile getFloor() {
        Vector3 cellSize = Camera.main.GetComponent<Grid>().cellSize;
        Vector3 local = Camera.main.GetComponent<Grid>().CellToLocal(position);
        Vector3 floorLocal = new Vector3(local.x, local.y, local.z - 1);
        Collider2D floorCollider = Colliders.getCollider(floorLocal,
                                                        new Vector2(cellSize.x, cellSize.y),
                                                        LayerMask.GetMask("solid"));
        if (floorCollider != null) {
            Tile tile = floorCollider.GetComponent<Tile>();
            if (tile == null) { throw new MissingComponentException(string.Format("Solid '{1}' does not have the Tile script component!", floorCollider.gameObject.name)); }
            return tile;
        }
        return null;
    }

    private bool hasCheckedClosed = false;
    private bool closed_iternal = false;
    public bool closed {
        get {

            if (!hasCheckedClosed) {

                Grid grid = Camera.main.GetComponent<Grid>();
                Vector3 local = grid.CellToLocal(position);
                Collider2D[] colliders = Colliders.getColliders(local, new Vector2(grid.cellSize.x, grid.cellSize.y),
                                                                LayerMask.GetMask("solid"));
                bool free = true;
                foreach(Collider2D collider in colliders) {
                    Tile tile = collider.GetComponent<Tile>();
                    if (tile == null) { throw new MissingComponentException(string.Format("Solid '{1}' does not have the Tile script component!", collider.gameObject.name)); }
                    if (tile.isStair == false) {
                        free = false;
                        break;
                    }
                }

                bool result = !free
                                ||
                                getFloor() == null;

                closed_iternal = result;
                hasCheckedClosed = true;

            }

            return closed_iternal;
            
        }
    }

    public Tile getSpace() {
        Vector3 cellSize = Camera.main.GetComponent<Grid>().cellSize;
        Vector3 local = Camera.main.GetComponent<Grid>().CellToLocal(position);
        Collider2D collider = Colliders.getCollider(local,
                                                        new Vector2(cellSize.x, cellSize.y),
                                                        LayerMask.GetMask("solid"));
        if (collider != null) {
            Tile tile = collider.GetComponent<Tile>();
            if (tile == null) { throw new MissingComponentException(string.Format("Solid '{1}' does not have the Tile script component!", collider.gameObject.name)); }
            return tile;
        }
        return null;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        Node other = (Node)obj;

        return position.x == other.position.x && position.y == other.position.y && position.z == other.position.z;

    }
    
    // override object.GetHashCode for Equals
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}