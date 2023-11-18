using System;

namespace AnalystDataImporter.Services
{
    public interface IMessageService
    {
        void Register<TMessage>(object recipient, Action<TMessage> action);

        void Unregister<TMessage>(object recipient);

        void Send<TMessage>(TMessage message);
    }
}