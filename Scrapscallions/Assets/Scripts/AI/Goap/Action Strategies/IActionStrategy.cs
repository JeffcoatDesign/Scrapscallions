namespace Scraps.AI.GOAP
{
    public interface IActionStrategy
    {
        bool CanPerform { get; }
        bool IsComplete { get; }

        void Begin()
        {
            // noop
        }

        void Tick(float deltaTime)
        {
            // noop
        }

        void Stop()
        {
            // noop
        }
    }
}