// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of an Object Variable in the PlayMaker Log Window.")]
	public class DebugObject : FsmStateAction
	{
        [Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
        [Tooltip("Prints the value of an Object variable in the PlayMaker log window.")]
		public FsmObject fsmObject;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			fsmObject = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			
			if (!fsmObject.IsNone)
			{
				text = fsmObject.Name + ": " + fsmObject;
			}
			
			ActionHelpers.DebugLog(Fsm, logLevel, text);
			
			Finish();
		}
	}
}