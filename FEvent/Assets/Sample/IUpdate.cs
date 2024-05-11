using FEvent;

namespace FEvent.Sample
{


    public interface IUpdate2 : ISendEvent<int,string>
    {
        void Update2(int deltaTime0,string a);
    }



    public interface IUpdate : ISendEvent<float>
    {
        void Update(float deltaTime);
    }

    public interface IMyCallEvent : ICallEvent<int, string>
    {
        string OnCallEvent(int a);
    }
    
}