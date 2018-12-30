using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{
    /////////////////////////////////////////////////
    /// Special tool to create PorterPipe sytems by drawing its path using the ChildOrder in the Hierarchy.
    /////////////////////////////////////////////////
    public class PorterPipeDrawer : MonoBehaviour
    {

        [SerializeField] GameObject linearPipe;
        [SerializeField] GameObject curvingPipe;


        List<GameObject> objects = new List<GameObject>();
        GameObject parent;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount - 1; i++)
            {
                Vector3 p1 = transform.GetChild(i).position;
                Vector3 p2 = transform.GetChild(i + 1).position;
                Dim.Visualize.GizmosUtils.DrawCone(p2, p1 - p2, (p1 - p2).magnitude, 0.5f);
            }
        }

        public void BuildPipes()
        {
            parent = new GameObject("NewPorterPipeSystem");

            List<Vector3> positions = new List<Vector3>();

            int currentTarget = 1;
            Vector3 currentPos;
            Direction currentDirection = Direction.Up;

            for (int i = 0; i < transform.childCount; i++)
            {
                positions.Add(Dim.GlobalMethods.RoundVector3(transform.GetChild(i).position));
            }

            currentPos = positions[0];
            //build starting pipe
            CreatePipeAndChangePosition(ref currentPos, ref currentDirection, Direction.Up);

            //build Pipe from a to b with the starting pipe
            try
            {
                while (currentTarget < positions.Count)
                {
                    int counter = 0;
                    while (currentPos != positions[currentTarget])
                    {
                        Direction nextDir = Direction.Inwards;
                        try
                        {
                            nextDir = GetNextDirection(currentPos, positions[currentTarget], currentDirection);
                        }
                        catch (PorterPipeException)
                        {
                            break;
                        }

                        try
                        {
                            CreatePipeAndChangePosition(ref currentPos, ref currentDirection, nextDir);
                        }
                        catch (PorterPipeException)
                        {

                        }
                        finally
                        {
                            counter++;
                            if (counter > 300)
                            {
                                throw new PorterPipeException("BuildingFailed. Unable to reach target #" + currentTarget);
                            }
                        }


                    }
                    currentTarget++;
                }
            }
            catch (PorterPipeException e)
            {
                Debug.LogException(e);
            }
            finally
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    if (objects[i] == null)
                    {
                        objects.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        objects[i].name = "#" + i;
                    }
                }

                for (int i = 0; i < objects.Count; i++)
                {
                    if (i > 0)
                    {
                        objects[i].GetComponent<PorterPipeSection>().previousSection = objects[i - 1].GetComponent<PorterPipeSection>();

                    }

                    if (i < objects.Count - 1)
                    {
                        objects[i].GetComponent<PorterPipeSection>().nextSection = objects[i + 1].GetComponent<PorterPipeSection>();
                    }
                }
            }


        }

        private Direction GetNextDirection(Vector3 currentPos, Vector3 target, Direction currentDir)
        {
            float xDiff = target.x - currentPos.x;
            float yDiff = target.y - currentPos.y;
            float zDiff = target.z - currentPos.z;

            Direction desiredDir;

            List<Direction> candidates = new List<Direction>();

            if (zDiff > 1)
            {
                candidates.Add(Direction.Outwards);
            }
            else if (zDiff < -1)
            {
                candidates.Add(Direction.Inwards);
            }

            if (yDiff > 1)
            {
                candidates.Add(Direction.Up);
            }
            else if (yDiff < -1)
            {
                candidates.Add(Direction.Down);
            }


            if (xDiff > 1)
            {
                candidates.Add(Direction.Right);
            }
            else if (xDiff < -1)
            {
                candidates.Add(Direction.Left);
            }

            if (candidates.Count > 0)
            {

                desiredDir = candidates[Random.Range(0, candidates.Count)];

                if (desiredDir == GetOppositeOf(currentDir))
                {
                    return PickRandomDirection();
                }
                else
                {
                    return desiredDir;
                }
            }


            throw new PorterPipeException("BuildingFailed. Unable to reach target at" + target);

        }

        private Direction PickRandomDirection()
        {
            return (Direction)Random.Range(0, 6);
        }

        private Direction GetOppositeOf(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;

                case Direction.Down:
                    return Direction.Up;

                case Direction.Right:
                    return Direction.Left;

                case Direction.Left:
                    return Direction.Right;

                case Direction.Inwards:
                    return Direction.Outwards;

                case Direction.Outwards:
                    return Direction.Inwards;
            }

            throw new PorterPipeException("Error. Check Direction Enum.");
        }


        private void CreatePipeAndChangePosition(ref Vector3 position, ref Direction startDir, Direction endDir)
        {
            GameObject g;
            Vector3 rot;
            Vector3 objectDisp;
            Vector3 endPosDisp;

            GetRightPrefabWithPositionAndRotation(startDir, endDir, out g, out rot, out objectDisp, out endPosDisp);
            objects.Add(Instantiate(g, position + objectDisp, Quaternion.Euler(rot), parent.transform));

            position = position + endPosDisp;
            startDir = endDir;
        }

        private void GetRightPrefabWithPositionAndRotation(Direction startDir, Direction endDir, out GameObject g, out Vector3 rot, out Vector3 objectDisp, out Vector3 endPosDisp)
        {
            if (startDir == endDir)
            {
                g = linearPipe;
            }
            else
            {
                g = curvingPipe;
            }

            if (g == linearPipe)
            {
                rot = GetRotForLinearPipe(endDir);
                endPosDisp = GetEndPosForLinearPipe(endDir);
                objectDisp = endPosDisp / 2;
            }
            else
            {
                rot = GetRotForCurvedPipe(startDir, endDir);
                endPosDisp = GetEndPosForCurvedPipe(startDir, endDir);
                objectDisp = GetDispForPipeWith(startDir);
            }

        }

        private Vector3 GetRotForLinearPipe(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Vector3.zero;
                case Direction.Down:
                    return new Vector3(0, 0, 180);
                case Direction.Right:
                    return new Vector3(0, 0, 90);
                case Direction.Left:
                    return new Vector3(0, 0, -90);
                case Direction.Inwards:
                    return new Vector3(90, 0, 0);
                case Direction.Outwards:
                    return new Vector3(-90, 0, 0);
            }

            throw new PorterPipeException("Code should not reach here. Check Direction Enum");
        }

        private Vector3 GetEndPosForLinearPipe(Direction dir)
        {
            return GetDispForPipeWith(dir) * 2;

        }

        private Vector3 GetEndPosForCurvedPipe(Direction start, Direction end)
        {
            return GetDispForPipeWith(start) + GetDispForPipeWith(end);
        }

        private Vector3 GetRotForCurvedPipe(Direction start, Direction end)
        {

            switch (start)
            {
                case Direction.Up:
                    return GetRotForUp(end);

                case Direction.Down:
                    return GetRotForDown(end);

                case Direction.Right:
                    return GetRotForRight(end);

                case Direction.Left:
                    return GetRotForLeft(end);

                case Direction.Inwards:
                    return GetRotForInward(end);

                case Direction.Outwards:
                    return GetRotForOutward(end);

            }

            throw new PorterPipeException("How did you end up here?! Check Direction Enum.");
        }

        private Vector3 GetRotForUp(Direction end)
        {
            switch (end)
            {
                case Direction.Right:
                    return new Vector3(0, 0, 0);
                case Direction.Left:
                    return new Vector3(0, 180, 0);
                case Direction.Inwards:
                    return new Vector3(0, 90, 0);
                case Direction.Outwards:
                    return new Vector3(0, -90, 0);
            }

            throw new PorterPipeException("Illegal Direction Requested: " + end.ToString());
        }

        private Vector3 GetRotForDown(Direction end)
        {
            switch (end)
            {
                case Direction.Right:
                    return new Vector3(180, 0, 0);
                case Direction.Left:
                    return new Vector3(180, 180, 0);
                case Direction.Inwards:
                    return new Vector3(180, 90, 0);
                case Direction.Outwards:
                    return new Vector3(180, -90, 0);
            }

            throw new PorterPipeException("Illegal Direction Requested: " + end.ToString());
        }

        private Vector3 GetRotForRight(Direction end)
        {
            switch (end)
            {
                case Direction.Up:
                    return new Vector3(180, 0, -90);
                case Direction.Down:
                    return new Vector3(0, 0, -90);
                case Direction.Inwards:
                    return new Vector3(90, 0, -90);
                case Direction.Outwards:
                    return new Vector3(-90, 0, -90);
            }

            throw new PorterPipeException("Illegal Direction Requested: " + end.ToString());
        }

        private Vector3 GetRotForLeft(Direction end)
        {
            switch (end)
            {
                case Direction.Up:
                    return new Vector3(0, 0, 90);
                case Direction.Down:
                    return new Vector3(180, 0, 90);
                case Direction.Inwards:
                    return new Vector3(-90, 0, 90);
                case Direction.Outwards:
                    return new Vector3(90, 0, 90);
            }

            throw new PorterPipeException("Illegal Direction Requested: " + end.ToString());
        }

        private Vector3 GetRotForInward(Direction end)
        {
            switch (end)
            {
                case Direction.Right:
                    return new Vector3(-90, -90, 90);
                case Direction.Left:
                    return new Vector3(90, -90, 90);
                case Direction.Up:
                    return new Vector3(-0, -90, 90);
                case Direction.Down:
                    return new Vector3(-180, -90, 90);
            }

            throw new PorterPipeException("Illegal Direction Requested: " + end.ToString());
        }

        private Vector3 GetRotForOutward(Direction end)
        {
            switch (end)
            {
                case Direction.Right:
                    return new Vector3(90, 0, 0);
                case Direction.Left:
                    return new Vector3(-90, -90, -90);
                case Direction.Up:
                    return new Vector3(180, -90, -90);
                case Direction.Down:
                    return new Vector3(0, -90, -90);
            }

            throw new PorterPipeException("Illegal Direction Requested: " + end.ToString());
        }






        private Vector3 GetDispForPipeWith(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return new Vector3(0, 1, 0);
                case Direction.Down:
                    return new Vector3(0, -1, 0);
                case Direction.Right:
                    return new Vector3(1, 0, 0);
                case Direction.Left:
                    return new Vector3(-1, 0, 0);
                case Direction.Inwards:
                    return new Vector3(0, 0, -1);
                case Direction.Outwards:
                    return new Vector3(0, 0, 1);
            }

            throw new PorterPipeException("Code should not reach here. Check Direction Enum");
        }


        private enum Type { Linear, Curved };
        private enum Direction { Up, Right, Down, Left, Outwards, Inwards };


    }
    /////////////////////////////////////////////////
    ///  Exception thrown by PorterPipe Classes, usually symbolizes a broken PipeSystem.
    /////////////////////////////////////////////////
    public class PorterPipeException : System.Exception
    {
        public PorterPipeException(string message) : base(message) { }
    }
}