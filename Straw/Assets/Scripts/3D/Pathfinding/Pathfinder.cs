using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PathFinding {

    public class Utils {

        public static WorldManager worldManager {

            get { return Camera.main.GetComponent<WorldManager>(); }

        }

        public static Tile GetTile(Vector3 position) {

                GameObject obj = GetGameObject(position);
                return obj != null ? obj.GetComponent<Tile>():null;

        }

        public static Stairs GetStairs(Vector3 position) {

                GameObject obj = GetGameObject(position);
                return obj != null ? obj.GetComponent<Stairs>():null;

        }

        public static GameObject GetGameObject(Vector3 position) {

                Vector3 localPos = worldManager.GridToLocal(position);
                Collider collider = Colliders3D.getCollider(localPos, Utils.worldManager.cellSize, LayerMask.GetMask("solid"));
                if (collider == null) { return null; }
                return collider.gameObject;

        }

        public static bool IsFree(Vector3 position) {

            Vector3 localPos = worldManager.GridToLocal(position);
            return Colliders3D.isFree(localPos, Utils.worldManager.cellSize, LayerMask.GetMask("solid"));

        }

        private static GameObject consideredMarker;
        private static GameObject curNodeMarker;

        public static void CreateConsideredMarker(Vector3 position) {

            if (consideredMarker == null) {
                consideredMarker = (GameObject)Resources.Load("Point_considered");
            }

            GameObject.Instantiate(consideredMarker, position, Quaternion.identity);

        }

        public static void CreateCurNodeMarker(Vector3 position) {
            
            if (curNodeMarker == null) {
                curNodeMarker = (GameObject)Resources.Load("Point_curnode");
            }

            GameObject.Instantiate(curNodeMarker, position, Quaternion.identity);

        }

    }

    public class PathFinder {

        public static Path GetPath(Vector3 start, Vector3 end) {

            Vector3 gridStart = Utils.worldManager.LocalToGrid(start);
            Vector3 gridEnd = Utils.worldManager.LocalToGrid(end);

            Node endNode = new Node(gridEnd, null, 0, 0);
            Node startNode = new Node(gridStart, null, endNode);

            Node curNode = startNode;

            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();

            closedList.Add(curNode);

            while (!curNode.Equals(endNode)) {

                //Consider surrounding nodes, and stairs if we are in their climbing zones.
                for (int i = -1; i <= 1; i++) {
                    for (int j = -1; j <= 1; j++) {

                        Node targetNode = new Node(new Vector3(curNode.position.x + i, curNode.position.y, curNode.position.z + j),
                                                    curNode, endNode);

                        if (targetNode.isStair) {

                            //a stair would be closed, so we need to consider other nodes.
                            Stairs stairs = Utils.GetStairs(targetNode.position);

                            bool stairsFree = true;

                            foreach (GameObject freeZone in stairs.freeZones) {

                                Node zoneNode = new Node(Utils.worldManager.LocalToGrid(freeZone.transform.position),
                                                        curNode, endNode);

                                if (zoneNode.closed) { stairsFree = false; break; }

                            }

                            if (!stairsFree) { continue; }

                            if (curNode.position == Utils.worldManager.LocalToGrid(stairs.lowerClimbZone.transform.position)) {

                                //consider upperClimbZone
                                Node upperNode = new Node(Utils.worldManager.LocalToGrid(stairs.upperClimbZone.transform.position),
                                                            curNode, endNode);

                                ConsiderNode(ref upperNode, ref curNode, ref endNode, ref openList, ref closedList);

                            }

                        } else if (Utils.GetStairs(targetNode.position - (Vector3.up * Utils.worldManager.cellSize.z)) != null) {

                            Stairs stairs = Utils.GetStairs(targetNode.position - (Vector3.up * Utils.worldManager.cellSize.z));

                            bool stairsFree = true;

                            foreach (GameObject freeZone in stairs.freeZones) {

                                Node zoneNode = new Node(Utils.worldManager.LocalToGrid(freeZone.transform.position),
                                                        curNode, endNode);

                                if (zoneNode.closed) { stairsFree = false; break; }

                            }

                            if (!stairsFree) { continue; }

                            if (curNode.position == Utils.worldManager.LocalToGrid(stairs.upperClimbZone.transform.position)) {

                                //consider lowerClimbZone
                                Node lowerNode = new Node(Utils.worldManager.LocalToGrid(stairs.lowerClimbZone.transform.position),
                                                            curNode, endNode);

                                ConsiderNode(ref lowerNode, ref curNode, ref endNode, ref openList, ref closedList);

                            }

                        } else if (!targetNode.closed) {

                            ConsiderNode(ref targetNode, ref curNode, ref endNode, ref openList, ref closedList);

                        }

                    } //end for j
                } //end for i

                if (openList.Count == 0) { return null; }

                Node best = openList[0];

                foreach (Node node in openList) {

                    if (node.fCost < best.fCost) { best = node; }

                }

                curNode = best;
                closedList.Add(curNode);
                openList.Remove(curNode);
                //Utils.CreateCurNodeMarker(Utils.worldManager.GridToLocal(curNode.position));

            } // end while

            List<Vector3> pathPoints = new List<Vector3>();

            while (curNode != null) {

                pathPoints.Add(Utils.worldManager.GridToLocal(curNode.position));
                curNode = curNode.parent;

            }

            pathPoints.Reverse();

            return new Path(pathPoints);

        } //end getpath

        public static void ConsiderNode(ref Node targetNode, ref Node curNode, ref Node endNode, ref List<Node> openList, ref List<Node> closedList) {

            if (!targetNode.closed && !closedList.Contains(targetNode)) {

                if (openList.Contains(targetNode)) {

                    Node diffNode = new Node(targetNode.position, curNode, endNode);

                    if (diffNode.fCost < targetNode.fCost) {

                        targetNode.UpdateParent(curNode);

                    }

                } else {

                    openList.Add(targetNode);
                   // Utils.CreateConsideredMarker(Utils.worldManager.GridToLocal(targetNode.position));

                }

            }

        }

    }

    //Represents a space in the world.
    public class Node {

        private float gCost;
        private float hCost;
        public float fCost {
            get { return gCost + hCost; }
        }

        public Node parent;

        public Vector3 position;

        public Node(Vector3 position, Node parent, Node endNode) {

            gCost = walkCost + (parent != null ? parent.gCost:0);
            hCost = Mathf.Sqrt(Mathf.Pow(endNode.position.x - position.x, 2) + Mathf.Pow(endNode.position.y - position.y, 2) + Mathf.Pow(endNode.position.z - position.z, 2));
            this.parent = parent;
            this.position = position;

        }

        public Node(Vector3 position, Node parent, float gCost, float hCost) {

            this.parent = parent;
            this.position = position;
            this.gCost = gCost;
            this.hCost = hCost;

        }

        public void UpdateParent(Node parent) {

            gCost = parent.gCost + parent.walkCost;
            this.parent = parent;

        }

        public bool isStair {
            get {
                return Utils.GetStairs(position) != null;
            }
        }

        public float walkCost {
            get {
                Vector3 underPos = position - (Vector3.up * Utils.worldManager.cellSize.z);
                Tile myTile = Utils.GetTile(underPos);
                return myTile != null ? myTile.moveCost:0;
            }
        }

        public GameObject floor {
            get {
                Vector3 underPos = position - (Vector3.up * Utils.worldManager.cellSize.z);
                return Utils.GetGameObject(underPos);
            }
        }

        public bool closed {
            get {
                Vector3 underPos = position - (Vector3.up * Utils.worldManager.cellSize.z);
                return !Utils.IsFree(position) || Utils.IsFree(underPos);
            }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            Node other = (Node)obj;

            return other.position == position;

        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

}