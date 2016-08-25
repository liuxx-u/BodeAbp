using System.Threading.Tasks;

namespace Abp.Rpc.Transport.Simple.Tcp
{
    internal static class TplExtensions
    {
        public static void Forget(this Task task) { }
    }
}
