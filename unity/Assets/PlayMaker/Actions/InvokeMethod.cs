// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Invokes a Method in a Behaviour attached to a Game Object. See Unity InvokeMethod docs.")]
	public class InvokeMethod : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
	
		[RequiredField]
		[UIHint(UIHint.Script)]
		public FsmString behaviour;

		[RequiredField]
		[UIHint(UIHint.Method)]
		public FsmString methodName;
		
		[HasFloatSlider(0, 10)]
		public FsmFloat delay;
		
		public FsmBool repeating;
		
		[HasFloatSlider(0, 10)]
		public FsmFloat repeatDelay;
		
		public FsmBool cancelOnExit;

		public override void Reset()
		{
			gameObject = null;
			behaviour = null;
			methodName = "";
			delay = null;
			repeating = false;
			repeatDelay = 1;
			cancelOnExit = false;
		}

		MonoBehaviour component;

		public override void OnEnter()
		{
			DoInvokeMethod(Fsm.GetOwnerDefaultTarget(gameObject));

			Finish();
		}

		void DoInvokeMethod(GameObject go)
		{
			if (go == null)
			{
				return;
			}

			component = go.GetComponent(behaviour.Value) as MonoBehaviour;

			if (component == null)
			{
				LogWarning("InvokeMethod: " + go.name + " missing behaviour: " + behaviour.Value);
				return;
			}

			if (repeating.Value)
			{
				component.InvokeRepeating(methodName.Value, delay.Value, repeatDelay.Value);
			}
			else
			{
				component.Invoke(methodName.Value, delay.Value);
			}
		}

		public override void OnExit()
		{
			if (component == null)
			{
				return;
			}

			if (cancelOnExit.Value)
			{
				component.CancelInvoke(methodName.Value);
			}
		}

	}
}