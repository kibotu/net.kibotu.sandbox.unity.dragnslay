// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets the Scale of a named texture in a Game Object's Material. Useful for special effects.")]
	public class SetTextureScale : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;
		public FsmInt materialIndex;
		[UIHint(UIHint.NamedColor)]
		public FsmString namedTexture;
		[RequiredField]
		public FsmFloat scaleX;
		[RequiredField]
		public FsmFloat scaleY;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			namedTexture = "_MainTex";
			scaleX = 1;
			scaleY = 1;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetTextureScale();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoSetTextureScale();
		}

		void DoSetTextureScale()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

			if (go.renderer == null)
			{
				LogError("Missing Renderer!");
				return;
			}
			
			if (go.renderer.material == null)
			{
				LogError("Missing Material!");
				return;
			}
			
			if (materialIndex.Value == 0)
			{
				go.renderer.material.SetTextureScale(namedTexture.Value, new Vector2(scaleX.Value, scaleY.Value));
			}
			else if (go.renderer.materials.Length > materialIndex.Value)
			{
				var materials = go.renderer.materials;
				materials[materialIndex.Value].SetTextureScale(namedTexture.Value, new Vector2(scaleX.Value, scaleY.Value));
				go.renderer.materials = materials;
			}			
		}
	}
}