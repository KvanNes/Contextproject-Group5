using System.Collections.Generic;

namespace GraphicalUI
{
    public class PartsConfiguration
    {
        public HashSet<GraphicalUIPart> Parts = new HashSet<GraphicalUIPart>();

        public PartsConfiguration(params GraphicalUIPart[] parts)
        {
            foreach (GraphicalUIPart part in parts)
            {
                Parts.Add(part);
            }
        }
    }
}
