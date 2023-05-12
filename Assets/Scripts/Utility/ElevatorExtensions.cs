using UnityEngine;

public static class ElevatorExtensions
{
    public static bool DoesImplementInterface<T>(this RaycastHit _raycastHit, out T _foundInterface) where T : class
    {
        if (_raycastHit.collider == null)
        {
            _foundInterface = null;
            return false;
        }

        _foundInterface = _raycastHit.collider.GetComponent<T>();

        return _foundInterface != null;
    }

    public static bool IsArrayValid(this Object[] _targetArray)
    {
        if (_targetArray.Length <= 0)
        {
            return false;
        }

        for (int i = 0; i < _targetArray.Length; i++)
        {
            if (_targetArray[i] == null)
            {
                return false;
            }
        }

        return true;
    }
}
