using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Sources.utility
{
    public static class GameObjectHelper
    {
        /*public static Component GetComponentInChild(GameObject parent, string name, Component type)
        {
            return parent.transform.FindChild(name).GetComponent<type>();
        }*/
		
		public static float Range2(this Random random, float min, float max, float excludeMin, float excludeMax) {
			return 0f;
		}

        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count == 0;
        }

		public static T GetRandom<T>(this List<T> list) 
		{
			return list[Random.Range(0,list.Count)];
		}

        public static T Add<T>(this T[] array, T addItem)
        {
            int i;
            for(i = 0; i < array.Length; ++i)
                if (Equals(null, array[i]))
                    break;

            Debug.Log(i);
            array[i] = addItem;

            return addItem;
        }

        // @see http://answers.unity3d.com/questions/217351/animations-ignore-timescale.html
        public static IEnumerator Play(this Animation animation, string clipName, bool useTimeScale, Action onComplete)
        {
            Debug.Log("Overwritten Play animation, useTimeScale? " + useTimeScale);
            //We Don't want to use timeScale, so we have to animate by frame..
            if (!useTimeScale)
            {
                Debug.Log("Started this animation! ( " + clipName + " ) ");
                var _currState = animation[clipName];
                var isPlaying = true;
                //var _startTime = 0F;
                var _progressTime = 0F;
                var _timeAtLastFrame = 0F;
                var _timeAtCurrentFrame = 0F;
                var deltaTime = 0F;


                animation.Play(clipName);

                _timeAtLastFrame = Time.realtimeSinceStartup;
                while (isPlaying)
                {
                    _timeAtCurrentFrame = Time.realtimeSinceStartup;
                    deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                    _timeAtLastFrame = _timeAtCurrentFrame;

                    _progressTime += deltaTime;
                    _currState.normalizedTime = _progressTime / _currState.length;
                    animation.Sample();

                    //Debug.Log(_progressTime);

                    if (_progressTime >= _currState.length)
                    {
                        //Debug.Log("Bam! Done animating");
                        if (_currState.wrapMode != WrapMode.Loop)
                        {
                            //Debug.Log("Animation is not a loop anim, kill it.");
                            //_currState.enabled = false;
                            isPlaying = false;
                        }
                        else
                        {
                            //Debug.Log("Loop anim, continue.");
                            _progressTime = 0.0f;
                        }
                    }

                    yield return new WaitForEndOfFrame();
                }
                yield return null;
                if (onComplete != null)
                {
                    Debug.Log("Start onComplete");
                    onComplete();
                }
            }
            else
            {
                animation.Play(clipName);
            }
        }
    }
}
