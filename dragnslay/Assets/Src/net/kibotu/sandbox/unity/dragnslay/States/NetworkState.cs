namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.States
{
    class NetworkState
    {
        public enum Type
        {
            Connected, Connecting, Disconnected, Reconnecting
        }

        public Type State = Type.Disconnected;
    }
}