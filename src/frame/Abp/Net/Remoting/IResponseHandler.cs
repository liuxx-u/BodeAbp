using Abp.Net.Remoting.Args;

namespace Abp.Net.Remoting
{
    public interface IResponseHandler
    {
        void HandleResponse(RemotingResponse remotingResponse);
    }
}
