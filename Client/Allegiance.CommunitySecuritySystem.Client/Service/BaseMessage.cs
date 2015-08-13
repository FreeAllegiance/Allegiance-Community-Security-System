using System;
using System.Collections.Generic;
using Allegiance.CommunitySecuritySystem.Client.ClientService;
using Allegiance.CommunitySecuritySystem.Client.Utility;

namespace Allegiance.CommunitySecuritySystem.Client.Service
{
    [Serializable]
    class BaseMessage
    {
        #region Fields

        private const string MessageDataStore   = "inbox.ds";
        private const string MessageStorePass   = "gyN8:!aq&p)-XxH5";
        private const string MessageKey         = "Messages";

        private static DataStore _messageStore         = null;

        #endregion

        #region Properties

        public static DataStore MessageStore
        {
            get
            {
                if(_messageStore == null)
                    _messageStore = DataStore.Open(MessageDataStore, MessageStorePass);
                return _messageStore;
            }
        }

        public static List<BaseMessage> Messages
        {
            get { return MessageStore[MessageKey] as List<BaseMessage>; }
        }

        public int Id { get; set; }

        public string Sender { get; set; }

        public string Message { get; set; }

        public string Subject { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateToSend { get; set; }

        public DateTime? DateExpires { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieve messages from server if messages are not stored locally
        /// </summary>
        public static void RetrieveMessages(TaskDelegate onCompleteDelegate)
        {
            //Check the message datastore to see if messages have already been loaded
            var messages = Messages;

            if (messages == null)
                messages = new List<BaseMessage>();

            //If not, retrieve the message list from the server
            TaskHandler.RunTask(delegate(object data)
            {
                var parameters  = data as object[];
                var signal      = parameters[0] as TaskDelegate;
                var dataStore   = parameters[1] as DataStore;
                var list        = parameters[2] as List<BaseMessage>;
                var newItems    = 0;

                try
                {
                    ListMessageResult listMessageResult     = ServiceHandler.Service.ListMessages(new AuthenticatedData());
					var insertionList = new List<BaseMessage>();
					newItems = listMessageResult.Messages.Length;

					foreach (var message in listMessageResult.Messages)
                    {
						insertionList.Add(new BaseMessage()
						{
						    Id          = message.Id,
						    Subject     = message.Subject,
						    Message     = message.Message,
						    Sender      = message.Sender,
						    DateToSend  = message.DateToSend,
						    DateExpires = message.DateExpires,
						    DateCreated = message.DateCreated
						});
                    }

                    list.InsertRange(0, insertionList);

                    //Store callsign list to datastore
                    dataStore[MessageKey] = list;
                    dataStore.Save();
                }
                catch (Exception error)
                {
                    Log.Write(error);
                }

                //Signal to the calling thread that the operation is complete
                signal(new object[] { list, newItems });

            }, onCompleteDelegate, MessageStore, messages);
        }

        #endregion
    }
}