namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility
{
    class DisplayHelper
    {
        // static
        private DisplayHelper()
        {
        }

        public static int GetDisplayWidth()
        {
            #if UNITY_EDITOR
                
                return 480;
               
            #elif UNITY_ANDROID
            
                var displayHelper = new AndroidJavaClass("net.kibotu.sandbox.android.DisplayHelperClass");
                var width = displayHelper.CallStatic<int>("getDisplayWidth");
                var height = displayHelper.CallStatic<int>("getDisplaHeight");
                Debug.Log("Display [w=" + width + "|h=" + height + "]");
            
            #endif
        }
    }
}
