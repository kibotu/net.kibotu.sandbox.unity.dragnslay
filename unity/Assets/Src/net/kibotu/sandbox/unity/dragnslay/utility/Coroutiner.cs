using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility
{
    /// <summary>
    /// Author: 		Sebastiaan Fehr (Seb@TheBinaryMill.com)
    /// Date: 			March 2013
    /// Summary:		Creates MonoBehaviour instance through which 
    ///                 static classes can call StartCoroutine.
    /// Description:    Classes that do not inherit from MonoBehaviour, or static 
    ///                 functions within MonoBehaviours are inertly unable to 
    ///                 call StartCoroutene, as this function is not static and 
    ///                 does not exist on Object. This Class creates a proxy though
    ///                 which StartCoroutene can be called, and destroys it when 
    ///                 no longer needed.
    /// @see http://forum.unity3d.com/threads/15524-Static-StartCoroutine
    /// </summary>
    public class Coroutiner
    {
        public static Coroutine StartCoroutine(IEnumerator iterationResult)
        {
            //Create GameObject with MonoBehaviour to handle task.
            var handlerGo = new GameObject("Coroutine");
            return handlerGo.AddComponent<CoroutinerInstance>().ProcessWork(iterationResult);
        }

        private static IEnumerator DelayAction(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        public static Coroutine StartDelayedAction(Action action, float delay)
        {
            return StartCoroutine(DelayAction(action, delay));
        }
    }

    public class CoroutinerInstance : MonoBehaviour
    {
        public Coroutine ProcessWork(IEnumerator iterationResult)
        {
            return StartCoroutine(DestroyWhenComplete(iterationResult));
        }

        public IEnumerator DestroyWhenComplete(IEnumerator iterationResult)
        {
            yield return StartCoroutine(iterationResult);
            Destroy(gameObject);
        }
    }
}
