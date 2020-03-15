using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeSender.PipeServer
{
    public class PipeServer
    {
        private NamedPipeServerStream pipeServer;
        private StreamWriter streamWriter;

        public PipeServer()
        {
            var pipeSecurity = new PipeSecurity();
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            var idRef = windowsIdentity.Owner;
            PipeAccessRule accessRule = new PipeAccessRule(idRef, PipeAccessRights.ReadWrite, AccessControlType.Allow);
            pipeSecurity.AddAccessRule(accessRule);

            this.pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.Out, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous, 5000, 5000, pipeSecurity);
   
            streamWriter = new StreamWriter(pipeServer);
            Debug.WriteLine("NamedPipeServerStream object created.");
                
           
        }

       
        public void SendMessageToClients(string message)
        {

            if (! pipeServer.IsConnected)
            {
            // Wait for a client to connect
            Console.Write("Waiting for client connection...");
            pipeServer.WaitForConnection();
            Console.WriteLine("Client connected.");
            }

            try
            {
                // Read user input and send that to the client process.

                streamWriter.AutoFlush = true;

                streamWriter.WriteLine(message);
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }

        }

      


    }
}
