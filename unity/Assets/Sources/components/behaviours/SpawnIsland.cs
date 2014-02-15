using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    public class SpawnIsland : MonoBehaviour
    {
        private ParticleEmitter _waterfall;

        /*[ToolTip("This is a tooltip of String Field")]
        [Range(0,1)]
        [SerializeField]
        public float randomValue */

        public void Start()
        {
            _waterfall = transform.FindChild("Water Fountain").particleEmitter;
            _waterfall.enabled = false;

            var fsm = gameObject.GetComponent<PlayMakerFSM>();
            fsm.SendEvent("spawn_island");
            _waterfall.enabled = true;

            //StartCoroutine(animation.Play("SpawnIsland", false, () => Debug.Log("onComplete")));
        }
    }
}
