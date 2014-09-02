// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Measures the Distance betweens 2 Game Objects and stores the result in a Float Variable.")]
	public class GetDistance : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmGameObject target;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			target = null;
			storeResult = null;
			everyFrame = true;
		}
		
		public override void OnEnter()
		{
			DoGetDistance();
			
			if (!everyFrame)
				Finish();
		}
		public override void OnUpdate()
		{
			DoGetDistance();
		}		
		
		void DoGetDistance()
		{
			GameObject go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;
			
			if (go == null || target.Value == null || storeResult == null)
				return;
					
			storeResult.Value = Vector3.Distance(go.transform.position, target.Value.transform.position);
		}

	}
}