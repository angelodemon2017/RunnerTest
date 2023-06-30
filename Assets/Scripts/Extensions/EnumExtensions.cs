using UnityEngine;

public static class EnumExtensions
{
    public static Vector3 GetVector3Direction(this Direction dir)
    {
        switch (dir)
        {
            case Direction.Znegative:
                return Vector3.back;
            case Direction.Xpositive:
                return Vector3.right;
            case Direction.Xnegative:
                return Vector3.left;
            case Direction.Zpositive:
            default:
                return Vector3.forward;
        }
    }

    public static Quaternion GetQuaternionirection(this Direction dir)
    {
        switch (dir)
        {
            case Direction.Znegative:
                return Quaternion.Euler(0f, 180f, 0f);
            case Direction.Xpositive:
                return Quaternion.Euler(0f, 90f, 0f);
            case Direction.Xnegative:
                return Quaternion.Euler(0f, 270f, 0f);
            case Direction.Zpositive:
            default:
                return Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public static Vector3 GetEuler(this Direction dir)
    {
        switch (dir)
        {
            case Direction.Znegative:
                return new Vector3(0f, 180f, 0f);
            case Direction.Xpositive:
                return new Vector3(0f, 90f, 0f);
            case Direction.Xnegative:
                return new Vector3(0f, 270f, 0f);
            case Direction.Zpositive:
            default:
                return new Vector3(0f, 0f, 0f);
        }
    }
}