using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    public class DestroyGameObject : MonoBehaviour {

        public void Remove(GameObject go)
        {
            Destroy(go);
        }
    }
}
