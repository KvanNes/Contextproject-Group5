using UnityEngine;
using System.Collections.Generic;

namespace GraphicalUI
{
    public class GraphicalUIController : MonoBehaviour
    {
        public static readonly PartsConfiguration ServerConfiguration = new PartsConfiguration(new ServerPart(), new RestartButtonPart());
        public static readonly PartsConfiguration DriverConfiguration = new PartsConfiguration(new DriverPart(), new RestartButtonPart(), new SignControllerDriver());
        public static readonly PartsConfiguration ThrottlerConfiguration = new PartsConfiguration(new ThrottlerPart(), new RestartButtonPart(), new SignControllerThrottler());
        public static readonly PartsConfiguration MainConfiguration = new PartsConfiguration(new MainPart());
        public static readonly PartsConfiguration TutorialConfiguration = new PartsConfiguration(new TutorialPart());

        private Stack<PartsConfiguration> Configurations = new Stack<PartsConfiguration>();

        public void Add(PartsConfiguration configuration)
        {
            Configurations.Push(configuration);
        }

        public void Remove()
        {
            if (Configurations.Count > 0)
                Configurations.Pop();
        }

        public void Start()
        {
            Add(MainConfiguration);
        }

        public void OnGUI()
        {
            if (Configurations.Count == 0)
            {
                return;
            }

            PartsConfiguration configuration = Configurations.Peek();
            foreach (GraphicalUIPart part in configuration.Parts)
            {
                if (!part.Initialized)
                {
                    part.Initialized = true;
                    part.Initialize();
                }

                part.DrawGraphicalUI();
            }
        }
    }
}
