public class FeedbackButton : UIButton
{
    private void Awake() 
    => base.AssignOnClickAction(Feedback.RequestFeedbackFromSDK);
}