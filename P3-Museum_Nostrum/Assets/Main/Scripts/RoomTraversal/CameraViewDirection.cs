using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraViewDirection {

    //eager initialization of singleton.
    private static CameraViewDirection cvd = new CameraViewDirection();
    //singleton access from outside (read-only)
    public static CameraViewDirection Instance
    {
        get
        {
            return cvd;
        }
    }

    private CameraViewDirection()
    {
        //prevents multiple instance.
    }

    //---------------------------- StateMachine -------------------------------

    private static DirectionState north = new North();
    private static DirectionState east = new East();
    private static DirectionState south = new South();
    private static DirectionState west = new West();

    [SerializeField]
    private static DirectionState CurrentState = north;
    public DirectionState GetCurrentState()
    {
        return CurrentState;
    }


    public abstract class DirectionState
    {
        public abstract float HandleDragDistanceCalculation(Vector3 toPos, Vector3 fromPos);
        public abstract void TransitionRight();
        public abstract void TransitionLeft();
        public abstract void PrintState();
        public abstract Direction GetDirectionIdentifier();
    }

    public class North : DirectionState
    {
        public override void TransitionRight()
        {
            CurrentState = east;
        }
        public override void TransitionLeft()
        {
            CurrentState = west;
        }
        public override float HandleDragDistanceCalculation(Vector3 toPos, Vector3 fromPos)
        {
            return toPos.z - fromPos.z;
        }
        public override void PrintState()
        {
            Debug.Log("CurrentState = North");
        }
        public override Direction GetDirectionIdentifier()
        {
            return Direction.North;
        }
    }

    public class East : DirectionState
    {
        public override void TransitionRight()
        {
            CurrentState = south;
        }
        public override void TransitionLeft()
        {
            CurrentState = north;
        }
        public override float HandleDragDistanceCalculation(Vector3 toPos, Vector3 fromPos)
        {
            return toPos.x - fromPos.x; //Todo: figure out which axis is required for the calculation
        }
        public override void PrintState()
        {
            Debug.Log("CurrentState = East");
        }
        public override Direction GetDirectionIdentifier()
        {
            return Direction.East;
        }
    }

    public class South : DirectionState
    {
        public override void TransitionRight()
        {
            CurrentState = west;
        }
        public override void TransitionLeft()
        {
            CurrentState = east;
        }
        public override float HandleDragDistanceCalculation(Vector3 toPos, Vector3 fromPos)
        {
            return (-1) * (toPos.z - fromPos.z);    //inverted direction
        }
        public override void PrintState()
        {
            Debug.Log("CurrentState = South");
        }
        public override Direction GetDirectionIdentifier()
        {
            return Direction.South;
        }
    }

    public class West : DirectionState
    {
        public override void TransitionRight()
        {
            CurrentState = north;
        }
        public override void TransitionLeft()
        {
            CurrentState = south;
        }
        public override float HandleDragDistanceCalculation(Vector3 toPos, Vector3 fromPos)
        {
            return (-1) * toPos.x - fromPos.x; //XXX: -2 is a magic number that I just picked by visual testing.
        }
        public override void PrintState()
        {
            Debug.Log("CurrentState = West");
        }
        public override Direction GetDirectionIdentifier()
        {
            return Direction.West;
        }
    }
}

public enum Direction
{
    North,
    East,
    South,
    West
}