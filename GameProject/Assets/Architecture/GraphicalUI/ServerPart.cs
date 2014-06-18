using UnityEngine;
using NetworkManager;

namespace GraphicalUI
{
    public class ServerPart : GraphicalUIPart
    {

        private void DrawLightControl()
        {
            if (GUI.Button(new Rect(0, 50, 300, 50), new GUIContent("Toggle light")))
            {
                MainScript.NetworkController.NetworkView.RPC("ToggleLight", RPCMode.Others);
            }
        }

        private void DrawOverviewControl()
        {
            if (GUI.Button(new Rect(0, 100, 300, 50), new GUIContent("Toggle overview")))
            {
                MainScript.NetworkController.NetworkView.RPC("ToggleOverview", RPCMode.Others);
            }
        }

        public override void DrawGraphicalUI()
        {
            if (MainScript.Server.Game != null)
            {
                //Based on: http://answers.unity3d.com/questions/296204/gui-font-size.html
                GUI.skin.label.fontSize = 20;

                GUI.Label(new Rect(10, 10, 200, 50), new GUIContent("Server started"));

                DrawLightControl();
                DrawOverviewControl();
            }
        }
    }
}
