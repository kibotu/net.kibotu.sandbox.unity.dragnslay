// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends an Event in the next frame. Useful if you want to loop states every frame.")]
	public class NextFrameEvent : FsmStateAction
	{
		[RequiredField]
		public FsmEvent sendEvent;

		public override void Reset()
		{
			sendEvent = null;
		}

		public override void OnEnter()
		{
		}

		public override void OnUpdate()
		{
			Finish();

			Fsm.Event(sendEvent);
		}
	}
}