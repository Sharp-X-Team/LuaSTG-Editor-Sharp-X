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
    public class InfoMessage : MessageBase<InfoDisplayPart>
    {
        public InfoMessage(string message) : this(message, new MessageOptions())
        {
        }

        public InfoMessage(string message, MessageOptions options) : base(message, options)
        {
        }

        protected override InfoDisplayPart CreateDisplayPart()
        {
            return new InfoDisplayPart(this);
        }

        protected override void UpdateDisplayOptions(InfoDisplayPart displayPart, MessageOptions options)
        {
            return;
        }
    }

    public static class InfoExtensions
    {
        public static void ShowInfo(this Notifier notifier, string message)
        {
            notifier.Notify<InfoMessage>(() => new InfoMessage(message));
        }

        public static void ShowInfo(this Notifier notifier, string message, MessageOptions displayOptions)
        {
            notifier.Notify<InfoMessage>(() => new InfoMessage(message, displayOptions));
        }
    }
}
