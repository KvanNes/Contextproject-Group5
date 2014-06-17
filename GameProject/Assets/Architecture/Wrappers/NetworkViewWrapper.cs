using Interfaces;
using UnityEngine;

namespace Wrappers
{
    public class NetworkViewWrapper : INetworkView
    {

        private NetworkView _nativeNetworkView;

        public void SetNativeNetworkView(NetworkView nativeNetworkView)
        {
            _nativeNetworkView = nativeNetworkView;
        }

        public NetworkViewID GetNetworkViewId()
        {
            return _nativeNetworkView.viewID;
        }

        public void SetNetworkViewId(NetworkViewID id)
        {
            _nativeNetworkView.viewID = id;
        }

        public void RPC(string name, NetworkPlayer target, params object[] args)
        {
            _nativeNetworkView.RPC(name,target,args);
        }

        public void RPC(string name, RPCMode mode, params object[] args)
        {
            _nativeNetworkView.RPC(name, mode, args);
        }
    }
         
}