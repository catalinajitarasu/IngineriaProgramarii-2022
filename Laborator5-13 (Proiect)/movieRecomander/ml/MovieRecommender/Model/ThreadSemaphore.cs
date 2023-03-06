public class ThreadSemaphore
{
    public bool isBeingUpdated { get; set; } // the model is being updated
    public bool isBeingLoaded { get; set; } //  the model is being loaded

    public ThreadSemaphore()
    {
        isBeingUpdated = false;
        isBeingLoaded = false;
    }
}
