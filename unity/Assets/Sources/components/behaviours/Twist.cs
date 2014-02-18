using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Sources.components.behaviours
{
    public class Twist : MonoBehaviour {

        public const float Epsilon = 0.001f;

        public Vector2 ChanceOfRandomTwist;
        public float TwistDuration;
        private float _randomTwistChance;
        private float _twistTime;
        private float _randomTwistChanceTime;
        private float strafeAngle;

        public void Start () {
            if (ChanceOfRandomTwist == Vector2.zero) ChanceOfRandomTwist = new Vector2(5f, 10f);
            if (Math.Abs(_randomTwistChance) < Epsilon) _randomTwistChance = Random.Range(ChanceOfRandomTwist.x, ChanceOfRandomTwist.y);
            if (Math.Abs(TwistDuration) < Epsilon) TwistDuration = 1.125f;
        }
	
        public void FixedUpdate () {
            _twistTime += Time.deltaTime;

            if (_twistTime <= TwistDuration)
            {
                strafeAngle = strafeAngle = Mathf.Lerp(0, 360, _twistTime / TwistDuration);
            }
            else
            {
                _randomTwistChanceTime += Time.deltaTime;
                if (_randomTwistChanceTime > _randomTwistChance)
                {
                    _randomTwistChanceTime = 0;
                    _twistTime = 0;
                    _randomTwistChance = Random.Range(ChanceOfRandomTwist.x, ChanceOfRandomTwist.y);
                }
            }

            transform.rotation *= Quaternion.Euler(0, 0, -strafeAngle);
        }
    }
}
