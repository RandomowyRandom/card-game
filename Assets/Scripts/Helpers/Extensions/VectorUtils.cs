using UnityEngine;

namespace Helpers.Extensions
{
    public static class VectorUtils
    {
        public static bool IsAnyNaN(this Vector3 v)
        {
            return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);
        } 
    }
}