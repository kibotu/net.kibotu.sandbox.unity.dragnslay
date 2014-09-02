// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

// EditorWindow classes can't be called from a dll 
// so create a thin wrapper class as a workaround
// TODO: move to dll when Unity supports it

class FsmSelectorWindow : HutongGames.PlayMakerEditor.FsmSelector
{
}
