namespace WorkflowBuilder.Services
{
    public class WorkflowEventService
    {
        public event EventHandler? LoadSampleRequested;

        public void RequestLoadSample()
        {
            LoadSampleRequested?.Invoke(this, EventArgs.Empty);
        }
    }
} 