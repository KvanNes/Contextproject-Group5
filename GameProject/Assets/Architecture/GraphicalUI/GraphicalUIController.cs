using UnityEngine;
using System.Collections.Generic;

namespace GraphicalUI
{
    public class GraphicalUIController : MonoBehaviour
    {
        public static readonly PartsConfiguration ServerConfiguration = new PartsConfiguration(new ServerPart(), new RestartButtonPart(), new TimerPart());
        public static readonly PartsConfiguration DriverConfiguration = new PartsConfiguration(new DriverPart(), new RestartButtonPart(), new SignPartDriver(), new FinishedPart(), new TimerPart(), new CountDownPart(), new StartPart());
        public static readonly PartsConfiguration ThrottlerConfiguration = new PartsConfiguration(new ThrottlerPart(), new RestartButtonPart(), new SignPartThrottler(), new FinishedPart(), new TimerPart(), new CountDownPart(), new StartPart());
        public static readonly PartsConfiguration MainConfiguration = new PartsConfiguration(new MainPart());
        public static readonly PartsConfiguration TutorialConfiguration = new PartsConfiguration(new TutorialPart());

        public Stack<PartsConfiguration> Configurations = new Stack<PartsConfiguration>();

        private void FireBecomeVisible()
        {
            foreach (GraphicalUIPart part in Configurations.Peek().Parts)
            {
                part.BecomeVisible();
            }
        }

        public void Add(PartsConfiguration configuration)
        {
            Configurations.Push(configuration);
            FireBecomeVisible();
        }

        public void Remove()
        {
            if (Configurations.Count > 0) {
                Configurations.Pop();
                if(Configurations.Count > 0) {
                    FireBecomeVisible();
                }
            }
        }

        public void Clear()
        {
            Configurations.Clear();
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
