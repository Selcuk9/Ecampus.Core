using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.Keyboard;

namespace VkApi.Group.Interfaces
{
    public interface IVKMessageManager
    {
        public event Action<Message> OnNewMessage;
        public void StartMessageHandling();
        public Task<bool> SendMessageAsync(string message, long? fromId);
        public Task<bool> SendMessageAsync(string message, long? fromId, MessageKeyboard keyboard);
    }
}
