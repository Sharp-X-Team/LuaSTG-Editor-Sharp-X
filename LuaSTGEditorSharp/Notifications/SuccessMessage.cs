using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Core;

namespace LuaSTGEditorSharp.Notifications
{
    public class SuccessMessage : MessageBase<SuccessDisplayPart>
    {
        public SuccessMessage(string message) : this(message, new MessageOptions())
        {
        }

        public SuccessMessage(string message, MessageOptions options) : base(message, options)
        {
        }

        protected override SuccessDisplayPart CreateDisplayPart()
        {
            return new SuccessDisplayPart(this);
        }

        protected override void UpdateDisplayOptions(SuccessDisplayPart displayPart, MessageOptions options)
        {
            return;
        }
    }

    public static class SuccessExtensions
    {
        public static void ShowSuccess(this Notifier notifier, string message)
        {
            notifier.Notify<SuccessMessage>(() => new SuccessMessage(message));
        }

        public static void ShowSuccess(this Notifier notifier, string message, MessageOptions displayOptions)
        {
            notifier.Notify<SuccessMessage>(() => new SuccessMessage(message, displayOptions));
        }
    }
}
