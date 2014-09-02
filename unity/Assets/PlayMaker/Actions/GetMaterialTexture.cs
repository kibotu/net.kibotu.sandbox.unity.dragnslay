// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
// Thanks to: Giyomu
// http://hutonggames.com/playmakerforum/index.php?topic=401.0

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Material)]
    [Tooltip("Get a texture from a material on a GameObject")]
    public class GetMaterialTexture : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(Renderer))]
        [Tooltip("The GameObject the Material is applied to.")]
        public FsmOwnerDefault gameObject;
        
        [Tooltip("The index of the Material in the Materials array.")]
        public FsmInt materialIndex;
        
        [UIHint(UIHint.NamedTexture)]
        [Tooltip("The texture to get. See Unity Shader docs for names.")]
        public FsmString namedTexture;
        
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Title("StoreTexture")] // fix parameter name
        [Tooltip("Store the texture in a variable.")]
        public FsmTexture storedTexture;

        [Tooltip("Get the shared version of the texture.")]
        public bool getFromSharedMaterial;

        public override void Reset()
        {
            gameObject = null;
            materialIndex = 0;
            namedTexture = "_MainTex";
            storedTexture = null;
            getFromSharedMaterial = false;
        }

        public override void OnEnter()
        {
            DoGetMaterialTexture();
            Finish();
        }

        void DoGetMaterialTexture()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return;
            }

            if (go.renderer == null)
            {
                LogError("Missing Renderer!");
                return;
            }

            string namedTex = namedTexture.Value;
            if (namedTex == "")
            {
                namedTex = "_MainTex";
            }

            if (materialIndex.Value == 0 && !getFromSharedMaterial)
            {
                storedTexture.Value = go.renderer.material.GetTexture(namedTex);
            }

            else if (materialIndex.Value == 0 && getFromSharedMaterial)
            {
                storedTexture.Value = go.renderer.sharedMaterial.GetTexture(namedTex);
            }

            else if (go.renderer.materials.Length > materialIndex.Value && !getFromSharedMaterial)
            {
                var materials = go.renderer.materials;
                storedTexture.Value = go.renderer.materials[materialIndex.Value].GetTexture(namedTex);
                go.renderer.materials = materials;
            }

            else if (go.renderer.materials.Length > materialIndex.Value && getFromSharedMaterial)
            {
                var materials = go.renderer.sharedMaterials;
                storedTexture.Value = go.renderer.sharedMaterials[materialIndex.Value].GetTexture(namedTex);
                go.renderer.materials = materials;
            }
        }
    }
}
