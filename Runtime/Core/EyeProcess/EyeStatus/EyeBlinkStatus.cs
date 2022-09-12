namespace UniEyeController.Core.EyeProcess.EyeStatus
{
    public struct EyeBlinkStatus : IEyeStatus
    {
        public bool ForceBlinkOff;
        public EyeBlinkStatus(bool forceBlinkOff)
        {
            ForceBlinkOff = forceBlinkOff;
        }
    }
}