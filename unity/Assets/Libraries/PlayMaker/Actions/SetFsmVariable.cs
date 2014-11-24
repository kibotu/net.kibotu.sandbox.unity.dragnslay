// (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.StateMachine)]
    [Tooltip("Set the value of a variable in another FSM.")]
    public class SetFsmVariable : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The GameObject that owns the FSM")]
        public FsmOwnerDefault gameObject;

        [UIHint(UIHint.FsmName)]
        [Tooltip("Optional name of FSM on Game Object")]
        public FsmString fsmName;

        public FsmString variableName;

        [RequiredField]
        [HideTypeFilter]
        public FsmVar setValue;

        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

        private GameObject cachedGO;
        private PlayMakerFSM sourceFsm;
        private INamedVariable sourceVariable;
        private NamedVariable targetVariable;

        public override void Reset()
        {
            gameObject = null;
            fsmName = "";
            setValue = new FsmVar();
        }

        public override void OnEnter()
        {
            InitFsmVar();

            DoGetFsmVariable();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoGetFsmVariable();
        }

        void InitFsmVar()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return;
            }

            if (go != cachedGO)
            {
                sourceFsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
                sourceVariable = sourceFsm.FsmVariables.GetVariable(setValue.variableName);
                targetVariable = Fsm.Variables.GetVariable(setValue.variableName);

                setValue.Type = FsmUtility.GetVariableType(targetVariable);

                if (!string.IsNullOrEmpty(setValue.variableName) && sourceVariable == null)
                {
                    LogWarning("Missing Variable: " + setValue.variableName);
                }

                cachedGO = go;
            }
        }

        void DoGetFsmVariable()
        {
            if (setValue.IsNone)
            {
                return;
            }

            InitFsmVar();

            setValue.GetValueFrom(sourceVariable);
            setValue.ApplyValueTo(targetVariable);
        }
    }
}
