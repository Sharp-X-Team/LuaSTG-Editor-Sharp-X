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
    public class ErrorMessage : MessageBase<ErrorDisplayPart>
    {
        public ErrorMessage(string message) : this(message, new MessageOptions())
        {
        }

        public ErrorMessage(string message, MessageOptions options) : base(message, options)
        {
        }

        protected override ErrorDisplayPart CreateDisplayPart()
        {
            return new ErrorDisplayPart(this);
        }

        protected override void UpdateDisplayOptions(ErrorDisplayPart displayPart, MessageOptions options)
        {
            return;
        }
    }

    public static class ErrorExtensions
    {
        public static void ShowError(this Notifier notifier, string message)
        {
            notifier.Notify<ErrorMessage>(() => new ErrorMessage(message));
        }

        public static void ShowError(this Notifier notifier, string message, MessageOptions displayOptions)
        {
            notifier.Notify<ErrorMessage>(() => new ErrorMessage(message, displayOptions));
        }
    }
}
