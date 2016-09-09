namespace BodeAbp.Queue.Utils
{
    public interface IRTStatisticService
    {
        void AddRT(double rtTime);
        double ResetAndGetRTStatisticInfo();
    }
}
