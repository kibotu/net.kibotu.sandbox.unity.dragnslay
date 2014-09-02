// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Sends an Event when the specified Mouse Button is pressed. Optionally store the button state in a bool variable.")]
	public class GetMouseButtonDown : FsmStateAction
	{
		[RequiredField]
		public MouseButton button;
		public FsmEvent sendEvent;
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
		
		public override void Reset()
		{
			button = MouseButton.Left;
			sendEvent = null;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			bool buttonDown = Input.GetMouseButtonDown((int)button);
			
			if (buttonDown)
				Fsm.Event(sendEvent);
			
			storeResult.Value = buttonDown;
		}
	}
}