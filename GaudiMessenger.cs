/*
nGaudi platform agnostic build tool on .NET
Copyright (c) 2011 Sam Saint-Pettersen.

nGaudi is a .NET rewrite of the original Gaudi tool which was written for
the Java Virtual Machine (JVM).

Licensed under the MIT/X11 License.
For dependencies, please see LICENSE file.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Stpettersens.nGaudi
{
    /// <summary>
    /// The GaudiMessenger class adds a network
    /// messaging service to Gaudi.
    /// </summary>
    class GaudiMessenger : GaudiBase
    {
        IPAddress ip;
        TcpListener serverSocket;
        TcpClient clientSocket;
        byte[] localhost = { 127, 0, 0, 1 };
        int port;

        /// <summary>
        /// Constructor for GaudiMessenger class.
        /// </summary>
        /// <param name="port">Port to operate messenger service.</param>
        public GaudiMessenger(int port)
        {
            ip = new IPAddress(localhost);
            this.port = port;
            serverSocket = new TcpListener(ip, port);
        }

        /// <summary>
        /// Start listening for clients.
        /// </summary>
        public void Start()
        {
            LogDump(String.Format("Server started on port {0}.", port));
            serverSocket.Start();
            clientSocket = serverSocket.AcceptTcpClient();
        }
    }
}