using UnityEngine;

namespace Interfaces
{
    public interface INetworkView
    {
        void SetNativeNetworkView(NetworkView nativeNetworkView);
        NetworkViewID GetNetworkViewId();
        void SetNetworkViewId(NetworkViewID id);

        void RPC(string name, NetworkPlayer target, params object[] args);
        void RPC(string name, RPCMode mode, params object[] args);
    }
}