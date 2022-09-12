namespace UniEyeController.Core.Status
{
    public struct UniEyeBlinkStatus : IEyeStatus
    {
        public bool ForceBlinkOff;
        public UniEyeBlinkStatus(bool forceBlinkOff)
        {
            ForceBlinkOff = forceBlinkOff;
        }
    }
}