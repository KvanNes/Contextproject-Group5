using UnityEngine;

namespace Interfaces
{
    public interface INetworkView
    {
        void SetNativeNetworkView(NetworkView nativeNetworkView);
        NetworkViewID GetNetworkViewId();
        void SetNetworkViewId(NetworkViewID id);

        //void RPC(string name, NetworkPlayer target);
        void RPC(string name, NetworkPlayer target, params object[] args);
        void RPC(string name, RPCMode mode, params object[] args);
//        void RPC(string name, NetworkPlayer target, Vector3 vector3, float speed, int carNumber);
//        void RPC(string name, NetworkPlayer target, Vector3 vector3, int carNumber);
//        void RPC(string name, NetworkPlayer target, Quaternion quaternion, int carNumber);
//
//        void RPC(string name, RPCMode mode);
//        void RPC(string name, RPCMode mode, int position);
//        void RPC(string name, RPCMode mode, Quaternion currentRotation, int carNumber);
//        void RPC(string name, RPCMode mode, Vector3 currentPosition, float speed, int carNumber);
//        void RPC(string name, RPCMode mode, string pendingType, int pendingCarNumber);
    }
}