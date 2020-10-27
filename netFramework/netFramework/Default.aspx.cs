using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace netFramework
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MessageQueue _messageQueue;
                _messageQueue = new MessageQueue("FORMATNAME:DIRECT=TCP:10.211.55.6\\private$\\yobiss") { Formatter = new BinaryMessageFormatter() };
                
                
                var message = new Message() { Formatter = new BinaryMessageFormatter() };
                message.Body = "nueva URL from docker";
                message.Label = "label from docker";
                _messageQueue.Send(message);

                _messageQueue.Close();
            }
            catch
            {

            }
        }
    }
}