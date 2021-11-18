using System;

namespace DoaT
{
    [Serializable]
    public class AxisInput
    {
        public string axisName;
        public Action<float> callback;
        
        public AxisInput() {}

        public AxisInput(string axis)
        {
            axisName = axis;
        }
    }
}