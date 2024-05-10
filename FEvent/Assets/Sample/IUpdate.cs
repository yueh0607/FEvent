using FEvent;

namespace FEvent.Smaple
{
    public interface IUpdate : ISendEvent
    {
        void Update(float deltaTime);
    }


    
}