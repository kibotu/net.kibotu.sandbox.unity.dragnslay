using UnityEngine;

namespace Assets.Sources.utility
{
    public static class MathUtilityHelper
    {
	
	    public static Vector3 Direction(this Vector3 source, Vector3 target) 
	    {
		    return (target - source).normalized;
	    }
	
	    public static float Range(float min, float max, float excludeRangeMin, float excludeRangeMax)
	    {
		    return Random.Range(0,2) == 0 ? Random.Range(min, excludeRangeMin) : Random.Range(excludeRangeMax, max);
	    }

        public static float ComputeVelocity(float acceleration, float startVelocity = 0f)
        {
            return startVelocity + acceleration * Time.time;
        }
    }
}