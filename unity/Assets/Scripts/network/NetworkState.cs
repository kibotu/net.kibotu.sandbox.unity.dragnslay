namespace Assets.Sources.states
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