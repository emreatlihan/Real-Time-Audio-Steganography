namespace RTASS.Common.Enums
{
    /// <summary>
    /// Echo hiding işlem durumlarını temsil eder
    /// </summary>
    public enum ProcessingStatus
    {
        Idle,
        Processing,
        Encoding,
        Decoding,
        Completed,
        Error,
        Cancelled
    }
}
