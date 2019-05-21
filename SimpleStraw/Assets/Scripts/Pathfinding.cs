using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{

    public class PathFinder {

        public static Path GetPath(Vector2 start, Vector2 end, int maxChecks) {

            int totalChecks = 0;

            Vector3 endCell3D = WorldManager.grid.LocalToCell(new Vector3(end.x, end.y, 0));
            Vector2 endCell = new Vector2(endCell3D.x, endCell3D.y);
            Node endNode = new Node(endCell, null, 0, 0);

            //Validation
            if (endNode.isClosed) {

                Debug.Log("end closed");
                return null;

            }

            Vector3 startCell3D = WorldManager.grid.LocalToCell(new Vector3(start.x, start.y, 0));
            Vector2 startCell = new Vector2(startCell3D.x, startCell3D.y);
            Node startNode = new Node(startCell, null, endNode);

            //Validation
            if (startNode.isClosed) {

                Debug.Log("start closed");
                return null;

            }

            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();

            Node curNode = startNode;
            closedList.Add(curNode);

            while (!curNode.Equals(endNode)) {

                if (totalChecks >= maxChecks) { return null; }

                for (int xx = -1; xx <= 1; xx++) {

                    for (int yy = -1; yy <= 1; yy++) {

                        Vector2 targPos = new Vector2(curNode.position.x + xx,
                                                    curNode.position.y + yy);

                        Node targNode = new Node(targPos, curNode, endNode);

                        //Validation
                        if (targNode.isClosed || closedList.Contains(targNode)) { continue; }

                        //Extra validation if this node is a corner
                        if (Mathf.Abs(xx) == Mathf.Abs(yy)) {

                            Node adjX = new Node(new Vector2(targPos.x, curNode.position.y), null, 0, 0);
                            Node adjY = new Node(new Vector2(curNode.position.x, targPos.y), null, 0, 0);

                            //Checks if this should be considered closed
                            if (adjX.isClosed && adjY.isClosed) { continue; }

                        }

                        //Consider
                        if (openList.Contains(targNode)) {

                            Node actual = openList.Find(otherNode => otherNode.Equals(targNode));

                            if (targNode.fCost < actual.fCost) {

                                actual.Update(curNode);

                            }

                        } else {
                            
                            openList.Add(targNode);

                        }

                    }

                } //end for

                if (openList.Count == 0) { return null; }

                Node best = openList[0];

                foreach (Node openNode in openList) {

                    if (openNode.fCost < best.fCost) { best = openNode; }

                }

                curNode = best;
                openList.Remove(best);
                closedList.Add(best);

                totalChecks++;

            } //end while

            List<Vector2> points = new List<Vector2>();

            while (curNode != null) {

                points.Add(WorldManager.grid.CellToLocal(new Vector3Int((int)curNode.position.x,
                                                                        (int)curNode.position.y, 0)));
                curNode = curNode.parent;

            }

            points.Reverse();

            return new Path(points);

        }

    }

    public class Path {

        private List<Vector2> points;

        public int length {

            get {

                return points.Count;

            }

        }

        public Vector2 this[int index] {

            get {
                return points[index];
            }

        }

        public Path(List<Vector2> points) {

            this.points = points;

        }

    }

    ///<summary>A unit that represents a space within the world.</summary>
    public class Node {

        private int gCost, hCost;

        public int fCost {
            get {
                return gCost + hCost;
            }
        }

        public Node parent;

        public readonly Vector2 position;

        private Vector3 realPos {
            get {
                Vector3Int cellPos = new Vector3Int((int)position.x, (int)position.y, 0);
                Vector3 pos = WorldManager.grid.CellToLocal(cellPos);
                return pos;
            }
        }

        public int walkCost {
            get {

                Collider2D floor = Colliders.GetCollider(realPos, LayerMask.GetMask("floor"));
                return floor != null ? floor.GetComponent<Tile>().moveCost:0;
            
            }
        }

        private bool hasFloor {
            get {

                Collider2D floor = Colliders.GetCollider(realPos, LayerMask.GetMask("floor"));
                
                return floor != null;

            }
        }

        public bool isClosed {

            get {
                
                Collider2D wall = Colliders.GetCollider(realPos, LayerMask.GetMask("walls"));
                
                return wall != null || !hasFloor;

            }
        }

        public void Update(Node parent) {

            this.parent = parent;
            this.gCost = parent != null ? parent.walkCost + walkCost:0;

        }

        public Node(Vector2 position, Node parent, int gCost, int hCost) {
            this.position = position;
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
        }

        public Node(Vector2 position, Node parent, Node endNode) {

            this.position = position;
            this.parent = parent;
            this.gCost = parent != null ? parent.walkCost + walkCost:0;
            this.hCost = (int)Mathf.Sqrt(Mathf.Pow(endNode.position.x - position.x, 2) + Mathf.Pow(endNode.position.y - position.y, 2));

        }

        // override object.Equals
        public override bool Equals(object obj)
        {   
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Node other = (Node)obj;

            return other.position == this.position;
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    public class Colliders {

        public static Collider2D[] GetColliders(Vector2 point, int layerMask) {

            return Physics2D.OverlapPointAll(point, layerMask);

        }

        public static Collider2D GetCollider(Vector2 point, int layerMask) {

            return Physics2D.OverlapPoint(point, layerMask);

        }

    }

}