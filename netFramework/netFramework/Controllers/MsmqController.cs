using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Web;
using System.Web.Mvc;

namespace netFramework.Controllers
{
    public class MsmqController : Controller
    {

        public class MessageResponse
        {
            public bool Error { get; set; }
            public string Status { get; set; }
            public int Counter { get; set; }
            public List<InnerMessages> Messages { get; set; }
        };

        public class InnerMessages
        {
            public int position { get; set; }
            public string id { get; set; }
            public string label { get; set; }
            public string body { get; set; }



        };
        // GET: Msmq
        public dynamic Index()
        {

            MessageResponse messageResponses = new MessageResponse();
            List<InnerMessages> listMessages = new List<InnerMessages>();

            

            try
            {
                MessageQueue _messageQueue;
                _messageQueue = new MessageQueue("FORMATNAME:DIRECT=TCP:10.211.55.6\\private$\\yobiss") { Formatter = new BinaryMessageFormatter() };

                try
                {
                    Message[] mm = _messageQueue.GetAllMessages();
                    if (mm.Length > 0) { messageResponses.Error = false; messageResponses.Counter = mm.Length; messageResponses.Status = mm.Length + " Mensajes por leer"; }
                    else { messageResponses.Error = true; messageResponses.Counter = 0; messageResponses.Status = "No hay mensajes por leer"; }
                    int i = 0;
                    foreach (Message m in mm)
                    {
                        listMessages.Add(new InnerMessages() { position = i++, body = m.Body.ToString(), label = m.Label, id = m.Id }); ;
                    }
                }
                catch (MessageQueueException me)
                {
                    messageResponses.Error = true;
                    messageResponses.Status = me.MessageQueueErrorCode.ToString() + " -> " + me.Message;
                    messageResponses.Messages = new List<InnerMessages>();

                    return JsonConvert.SerializeObject(messageResponses);

                }
                finally
                {

                    messageResponses.Messages = listMessages;
                }

            }
            catch
            {
                messageResponses.Error = true;
                messageResponses.Status = "No hay mensajes";
                messageResponses.Messages = listMessages;

                return JsonConvert.SerializeObject(messageResponses);
            }

            return JsonConvert.SerializeObject(messageResponses);


        }

        
    }
}
