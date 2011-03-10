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
    class GaudiMessenger : GaudiBase
    {
        IPAddress ip;
        TcpListener serverSocket;
        TcpClient clientSocket;
        byte[] localhost = { 127, 0, 0, 1 };
        int port;

        public GaudiMessenger(int port)
        {
            ip = new IPAddress(localhost);
            this.port = port;
            serverSocket = new TcpListener(ip, port);
        }
        public void Start()
        {
            LogDump(String.Format("Server started on port {0}.", port));
            serverSocket.Start();
            clientSocket = serverSocket.AcceptTcpClient();
        }
    }
}