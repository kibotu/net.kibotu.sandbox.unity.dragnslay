using HutongGames.PlayMaker;

namespace Assets.Sources.playmaker.actions
{
    [ActionCategory(ActionCategory.ScriptControl)]
    public class ToggleEnable : FsmStateAction {

        public override void Reset()
        {
            
        }

        public override void OnEnter()
        {
            Finish();
        }
    }
}
