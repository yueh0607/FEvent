namespace FEvent.Sample
{
    public static class IUpdatePublishExtensions
    {
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Send(this IUpdate obj, float deltaTime)
        {
            obj.Update(deltaTime);
        }

        public static void Send<T>(this IEventPublisher publisher, float deltaTime) where T : IUpdate
        {
            DynamicQueue<IEventListener> list = publisher.GetPublishableEvents<T>(typeof(T));
            if (list != null)
            {
                list.StartEnum();
                while(list.MoveNext(out IEventListener obj))
                {
                    T eventObj = (T)obj;
                    eventObj.Send(deltaTime);
                    list.Return(obj);
                }
                list.EndEnum();
            }
        }
    }
}
