using System;
using System.Collections;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Sources.components.behaviours
{
    public class Orbiting: MonoBehaviour
    {
        public const float Epsilon = 0.001f;

        public float radius;
        public float yoffset;
        public Transform center;
        public float circulationTerm;
        public Quaternion rotation;
        private float _time;

        public Vector2 heightvariance;
        public float heightchangetime;
        private float _randomHeight;
        private float _heighttime;
        private float _lastyoffset;
        public float height;
        public float strafeAngle;


        public void Start() {
            if (Math.Abs(yoffset) < Epsilon) yoffset = Random.Range(-2f, 2f);
            if(center == null) center = transform.parent;
            if (Math.Abs(radius) < Epsilon) radius = Random.Range(5f, 10f);
            if (Math.Abs(circulationTerm) < Epsilon) circulationTerm = Random.Range(4f, 12f);
            if(heightvariance == Vector2.zero) heightvariance = new Vector2(-2f, 2f);
            if (Math.Abs(heightchangetime) < Epsilon) heightchangetime = circulationTerm / 6f; // 4f;
            if (Math.Abs(strafeAngle) < Epsilon) strafeAngle = 10f;
            if(ChanceOfRandomTwist == Vector2.zero) ChanceOfRandomTwist = new Vector2(5f, 10f);
            if (Math.Abs(TwistDuration) < Epsilon) TwistDuration = 1.125f;
            _twistTime = TwistDuration;
            _randomTwistChance = Random.Range(ChanceOfRandomTwist.x, ChanceOfRandomTwist.y);
        }

        public Vector2 ChanceOfRandomTwist;
        public float TwistDuration;
        private float _randomTwistChance;
        private float _twistTime;
        private float _randomTwistChanceTime;

        public void FixedUpdate()
        {
            if (center == null) return;
            _time += Time.deltaTime;
            _heighttime += Time.deltaTime;
            _twistTime += Time.deltaTime;

            transform.position = GetFinalDestination();
            transform.rotation = GetFinalRotation();

            if (_twistTime <= TwistDuration)
            {
                strafeAngle = strafeAngle = Mathf.Lerp(10, 370, Easing.Sinusoidal.easeInOut(_twistTime / TwistDuration));
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
        }

        private static float GetStrafeAngle(float dt, float duration)
        {
            // http://www.wolframalpha.com/input/?i=360+*+sin+%28x*6%29
            return -180f * Mathf.Cos(dt*6f/duration) + 180f; 
        }

        public Vector3 GetFinalDestination()
        {
            height = ComputeHeightVariation();
            return RotateAroundCenterY(transform.position, _time, radius, center.position, circulationTerm, height);
        }

        private float ComputeHeightVariation()
        {
            if (_heighttime > heightchangetime)
            {
                _heighttime = 0;
                _lastyoffset = _randomHeight;
                _randomHeight = Random.Range(heightvariance.x, heightvariance.y) + yoffset;
            }
            return Mathf.Lerp(_lastyoffset, _randomHeight, _heighttime / heightchangetime);
        }

        public Quaternion GetFinalRotation()

        {
            // beware this is the most complicated way i could come up with to rotate the plane along the tangent of the circle
            var targetDir = RotateAroundCenterY(transform.position, (_time + circulationTerm / 360), radius, center.position, circulationTerm, height) - transform.position;
            var step = 1000 * Time.deltaTime;
            var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            //Debug.DrawRay(transform.position, newDir, Color.red);
            return Quaternion.LookRotation(newDir) * Quaternion.Euler(0, 0, -strafeAngle);//*Quaternion.Euler(-90f, 0, 0) * Quaternion.Euler(0,0,90) *;
        }

        public static Vector3 RotateAroundCenterY(Vector3 position, float time, float radius, Vector3 center, float circulationTerm, float yoffset)
        {
            var circle = RotoateAroundCenter(time, radius, new Vector2(center.x, center.z), circulationTerm);
            position.x = circle.x;
            position.y = center.y + yoffset;
            position.z = circle.y;
            return position;
        }

        public static Vector2 RotoateAroundCenter(float time, float radius, Vector2 center, float circulationTerm)
        {
            var dx = center.x + (-radius * Mathf.Cos(Phi(time, circulationTerm)));
            var dy = center.y + (radius * Mathf.Sin(Phi(time, circulationTerm)));
            return new Vector2(dx, dy);
        }

        public static float Phi(float t, float tu)
        {
            return (Mathf.Abs(tu) < Epsilon) ? 2f * Mathf.PI * t : 2f * Mathf.PI * t / tu;
        }
    }
}
