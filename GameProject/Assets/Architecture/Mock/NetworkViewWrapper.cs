using UnityEngine;

namespace Mock
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

        public void RPC(string name, NetworkPlayer target)
        {
            _nativeNetworkView.RPC(name, target);
        }

        public void RPC(string name, NetworkPlayer target, object[] args)
        {
            _nativeNetworkView.RPC(name, target, args);
        }

        public void RPC(string name, NetworkPlayer target, Vector3 vector3, float speed, int carNumber)
        {
            _nativeNetworkView.RPC(name, target, vector3, speed, carNumber);
        }

        public void RPC(string name, NetworkPlayer target, Vector3 vector3, int carNumber)
        {
            _nativeNetworkView.RPC(name, target, vector3, carNumber);
        }

        public void RPC(string name, NetworkPlayer target, Quaternion quaternion, int carNumber)
        {
            _nativeNetworkView.RPC(name, target, quaternion, carNumber);
        }

        public void RPC(string name, RPCMode mode)
        {
            _nativeNetworkView.RPC(name, mode);
        }

        public void RPC(string name, RPCMode mode, int position)
        {
            _nativeNetworkView.RPC(name, mode, position);
        }

        public void RPC(string name, RPCMode mode, Quaternion currentRotation, int carNumber)
        {
            _nativeNetworkView.RPC(name, mode, currentRotation, carNumber);
        }

        public void RPC(string name, RPCMode mode, Vector3 currentPosition, float speed, int carNumber)
        {
            _nativeNetworkView.RPC(name, mode, currentPosition, speed, carNumber);
        }
    }
}