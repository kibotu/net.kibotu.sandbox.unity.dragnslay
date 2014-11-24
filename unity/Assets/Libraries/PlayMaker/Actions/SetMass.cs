// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Sets the Mass of a Game Object's Rigid Body.")]
	public class SetMass : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		[HasFloatSlider(0.1f,10f)]
		public FsmFloat mass;

		public override void Reset()
		{
			gameObject = null;
			mass = 1;
		}

		public override void OnEnter()
		{
			DoSetMass();
			
			Finish();
		}

		void DoSetMass()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			if (go.rigidbody == null) return;
			
			go.rigidbody.mass = mass.Value;
		}
	}
}