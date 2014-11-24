namespace Assets.Scripts.network.googleplayservice
{
    public class PackageDameon 
    {
        public static volatile int CurrentPackageId;
        public static readonly ConcurrentArrayList Unverified = new ConcurrentArrayList();

        public void StartWatchingTimeout()
        {
            
        }
    }
}
