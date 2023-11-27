using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalystDataImporter.Services
{
    public class MessageService : IMessageService
    {
        private readonly Dictionary<Type, List<Delegate>> _listeners = new Dictionary<Type, List<Delegate>>();

        public void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            var messageType = typeof(TMessage);
            if (!_listeners.ContainsKey(messageType))
            {
                _listeners[messageType] = new List<Delegate>();
            }

            _listeners[messageType].Add(action);
        }

        public void Unregister<TMessage>(object recipient)
        {
            var messageType = typeof(TMessage);
            if (_listeners.ContainsKey(messageType))
            {
                var delegatesToRemove = new List<Delegate>();

                foreach (Delegate del in _listeners[messageType])
                {
                    if (del.Target.Equals(recipient))
                    {
                        delegatesToRemove.Add(del);
                    }
                }

                foreach (var del in delegatesToRemove)
                {
                    _listeners[messageType].Remove(del);
                }
            }
        }

        public void Send<TMessage>(TMessage message)
        {
            var messageType = typeof(TMessage);
            if (!_listeners.TryGetValue(messageType, out var listener)) return;
            foreach (var action in listener.Select(del => del as Action<TMessage>))
            {
                action?.Invoke(message);
            }
        }
    }
}
