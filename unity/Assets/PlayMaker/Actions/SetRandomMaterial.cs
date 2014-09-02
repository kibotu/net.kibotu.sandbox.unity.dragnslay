// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets a Game Object's material randomly from an array of Materials.")]
	public class SetRandomMaterial : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;
		public FsmInt materialIndex;
		public FsmMaterial[] materials;
		
		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			materials = new FsmMaterial[3];
		}

		public override void OnEnter()
		{
			DoSetRandomMaterial();			
			Finish();
		}
		
		void DoSetRandomMaterial()
		{
			if (materials == null) return;
			if (materials.Length == 0) return;

			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

			if (go.renderer == null)
			{
				LogError("SetMaterial: Missing Renderer!");
				return;
			}

			if (materialIndex.Value == 0)
			{
				go.renderer.material = materials[Random.Range(0, materials.Length)].Value;
			}
			else if (go.renderer.materials.Length > materialIndex.Value)
			{
				var newMaterials = go.renderer.materials;
				newMaterials[materialIndex.Value] = materials[Random.Range(0, materials.Length)].Value;
				go.renderer.materials = newMaterials;
			}		
		}
	}
}