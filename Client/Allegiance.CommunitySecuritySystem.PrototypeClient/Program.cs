using System;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.PrototypeClient.localhost;

namespace Allegiance.CommunitySecuritySystem.PrototypeClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            // for the prototype, we're accepting untrusted server certificates
            ServicePointManager.ServerCertificateValidationCallback
                = new RemoteCertificateValidationCallback(ValidateServerCertificate);

            AuthenticatedData.SetLogin("Orion", Hash("Test"));

            using (var auth = new ClientService())
            {
                //Retrieve AutoUpdate data
                AutoUpdateClient.Check(auth);


                //Login, and then perform initial check-in


                Console.WriteLine("Logging in. {0}", DateTime.Now);

                Assembly blackbox = null;

                var loginResult = auth.Login(new LoginData()
                {
                    Alias = "Orion"
                });

                Console.WriteLine("Login Status: {0} - {1}", loginResult.Status, DateTime.Now);

                if (loginResult.Status != LoginStatus.Authenticated)
                    return;

                //Retrieve all messages for this login
                ReceiveMessages();

                ReceivePolls();

                blackbox = Assembly.Load(loginResult.BlackboxData);

                var validatorType = blackbox.GetType("Allegiance.CommunitySecuritySystem.Blackbox.Validator");
                var machineInfoType = blackbox.GetType("Allegiance.CommunitySecuritySystem.Blackbox.MachineInformation");
                var machineInfo = Activator.CreateInstance(machineInfoType);

                GatherMachineInfo(machineInfoType, machineInfo);

                var method = validatorType.GetMethod("Check", BindingFlags.Static | BindingFlags.Public);
                var results = method.Invoke(null, new object[] { machineInfo }) as byte[];

                var checkInResult = auth.CheckIn(new CheckInData()
                {
                    SessionIdSpecified = false,
                    EncryptedData = results,
                });

                Console.WriteLine("Initial Check-In Status: {0} - {1}", checkInResult.Status, DateTime.Now);
            }
        }

        private static void ReceiveMessages()
        {
            Console.WriteLine("Receiving messages... - {0}", DateTime.Now);

            using (var messaging = new ClientService())
            {
                BaseMessage[] messages = messaging.ListMessages(new AuthenticatedData());

                if (messages.Length == 0)
                    Console.WriteLine("  No new messages on the server.");
                else
                    foreach (BaseMessage message in messages)
                    {
                        if (message is GroupMessage)
                        {
                            GroupMessage groupMessage = message as GroupMessage;
                            Console.WriteLine("  New group message: '{0}', sent {1}", groupMessage.Message, groupMessage.DateCreated);
                        }
                        else if (message is PersonalMessage)
                        {
                            PersonalMessage personalMessage = message as PersonalMessage;
                            Console.WriteLine("  New personal message: '{0}', sent {1}", personalMessage.Message, personalMessage.DateCreated);
                        }
                        else
                        {
                            // do proper error handling and display here
                            Console.WriteLine("error: received unexpected message type from server");
                        }
                    }
            }
        }

        private static void ReceivePolls()
        {
            Console.WriteLine("Receiving polls... - {0}", DateTime.Now);

            using (var messaging = new ClientService())
            {
                Poll[] polls = messaging.ListPolls(new AuthenticatedData());

                if (polls.Length == 0)
                    Console.WriteLine("  No new polls on the server.");
                else
                    foreach (Poll poll in polls)
                    {
                        Console.WriteLine("  New Poll: {0}", poll.Question);
                        Console.WriteLine("    Created: {0}, Expires: {1}", poll.DateCreated, poll.DateExpires);
                        foreach (PollOption po in poll.PollOptions)
                        {
                            Console.WriteLine("    Option: {0}", po.Option);
                        }
                        //Vote on a random option
                        var r = new Random();
                        PollData vote = new PollData()
                        {
                            OptionId = poll.PollOptions[r.Next(0,poll.PollOptions.Length)].Id,
                            OptionIdSpecified = true
                        };
                        messaging.ApplyVote(vote);
                    }
            }
        }

        private static void GatherMachineInfo(Type type, object machineInfo)
        {
            //Gather Hardware information
        }

        private static string Hash(string password)
        {
            using (var sha = new SHA256Managed())
                return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}