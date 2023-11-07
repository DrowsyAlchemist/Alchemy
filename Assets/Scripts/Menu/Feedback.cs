using System.Runtime.InteropServices;

public static class Feedback
{
    public static void RequestFeedbackFromSDK()
    {
        RequestFeedback();
    }

    [DllImport("__Internal")]
    private static extern void RequestFeedback();
}