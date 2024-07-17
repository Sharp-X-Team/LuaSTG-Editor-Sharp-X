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
    public class WarningMessage : MessageBase<WarningDisplayPart>
    {
        public WarningMessage(string message) : this(message, new MessageOptions())
        {
        }

        public WarningMessage(string message, MessageOptions options) : base(message, options)
        {
        }

        protected override WarningDisplayPart CreateDisplayPart()
        {
            return new WarningDisplayPart(this);
        }

        protected override void UpdateDisplayOptions(WarningDisplayPart displayPart, MessageOptions options)
        {
            return;
        }
    }

    public static class WarningExtensions
    {
        public static void ShowWarning(this Notifier notifier, string message)
        {
            notifier.Notify<WarningMessage>(() => new WarningMessage(message));
        }

        public static void ShowWarning(this Notifier notifier, string message, MessageOptions displayOptions)
        {
            notifier.Notify<WarningMessage>(() => new WarningMessage(message, displayOptions));
        }
    }
}
