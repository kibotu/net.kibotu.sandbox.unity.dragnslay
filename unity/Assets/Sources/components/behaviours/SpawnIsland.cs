using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    public class SpawnIsland : MonoBehaviour
    {
        private ParticleEmitter _waterfall;
        private PlayMakerFSM _fsm;

        /*[ToolTip("This is a tooltip of String Field")]
        [Range(0,1)]
        [SerializeField]
        public float randomValue */

        public void Start()
        {
            _waterfall = transform.FindChild("Water Fountain").particleEmitter;
            StopWaterFall();
            _fsm = gameObject.GetComponent<PlayMakerFSM>();
            //StartCoroutine(animation.Play("SpawnIsland", false, () => Debug.Log("onComplete")));

            _fsm.SendEvent("spawn_island");
        }

        public void StartWaterFall()
        {
            _waterfall.enabled = true;
        }

        public void StopWaterFall()
        {
            _waterfall.enabled = false;
        }
    }
}
