namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.States
{
    class GameState
    {
        public enum Type
        {
            Idle, Running, Pause, Resume, Stop
        }

        public Type State = Type.Idle;

    }
}