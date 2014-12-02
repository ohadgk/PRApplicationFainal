using Cirrious.MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrApplication.Clients.Windows8.Core.Messeges
{
    public class ByteArrMessage : MvxMessage
    {
        public byte[] PicAsByteArray { get; set; }

        public ByteArrMessage(object sender,byte[] data) :base(sender)
        {
            PicAsByteArray = data;
        }
    }
}
