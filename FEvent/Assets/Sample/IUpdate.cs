using FEvent;

namespace FEvent.Sample
{
    public interface IUpdate : ISendEvent
    {
        void Update(float deltaTime);
    }


    
}