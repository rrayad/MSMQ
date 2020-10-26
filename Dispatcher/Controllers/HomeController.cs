
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using System.Web.Mvc;


namespace api.Controllers
{
    public class HomeController : Controller
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


        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        // api Send
        public dynamic Send(string url, string label)
        {
            MessageQueue _messageQueue;
            var queue = ReadQueueNames();
            MessageResponse messageResponses = new MessageResponse();
            List<InnerMessages> listMessages = new List<InnerMessages>();

            try
            {

                _messageQueue = new MessageQueue(queue.ReadFromQueueName) { Formatter = new BinaryMessageFormatter() };
                var message = new Message() { Formatter = new BinaryMessageFormatter() };
                message.Body = url;
                message.Label = label;
                _messageQueue.Send(message);

                messageResponses.Error = false;
                messageResponses.Status = "El mensaje se envío correctamente";
                listMessages.Add(new InnerMessages() { body = url, label = label, id = "" });
                messageResponses.Messages = listMessages;
                return JsonConvert.SerializeObject(messageResponses);

            }
            catch (MessageQueueException me)
            {
                messageResponses.Error = true;
                messageResponses.Status = me.MessageQueueErrorCode.ToString() + " -> " + me.Message;
                messageResponses.Messages = new List<InnerMessages>();

                return JsonConvert.SerializeObject(messageResponses);
            }
        }

        [HttpGet]
        public dynamic GetMessages()
        {
            MessageResponse messageResponses = new MessageResponse();
            List<InnerMessages> listMessages = new List<InnerMessages>();

            var queue = ReadQueueNames();

            try
            {
                MessageQueue _messageQueue;
                _messageQueue = new MessageQueue(queue.ReadFromQueueName) { Formatter = new BinaryMessageFormatter() };

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


        public QueueSettings ReadQueueNames()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"appsettings.json");
            using (var sr = new StreamReader(path))
            {
                string conf = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<QueueSettings>(conf);
            }
        }

        public class QueueSettings
        {
            public string ReadFromQueueName { get; set; }
            public string WriteToQueueName { get; set; }
        }
    }

}

