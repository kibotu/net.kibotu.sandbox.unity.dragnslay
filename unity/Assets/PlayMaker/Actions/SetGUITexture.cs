// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUIElement)]
	[Tooltip("Sets the Texture used by the GUITexture attached to a Game Object.")]
	public class SetGUITexture : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(GUITexture))]
		public FsmOwnerDefault gameObject;
		public FsmTexture texture;
		
		public override void Reset()
		{
			gameObject = null;
			texture = null;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go != null && go.guiTexture != null)
			{
				go.guiTexture.texture = texture.Value;
			}
			
			Finish();
		}
	}
}