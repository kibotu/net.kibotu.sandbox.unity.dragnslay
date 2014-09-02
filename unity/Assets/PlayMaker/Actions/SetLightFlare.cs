// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Sets the Flare effect used by a Light.")]
	public class SetLightFlare : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;
		public Flare lightFlare;

		public override void Reset()
		{
			gameObject = null;
			lightFlare = null;
		}

		public override void OnEnter()
		{
			DoSetLightRange();
			Finish();
		}
		
		void DoSetLightRange()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			
			Light light = go.light;
			if (light == null)
			{
				LogError("Missing Light Component!");
				return;
			}
			
			light.flare = lightFlare;
		}
	}
}