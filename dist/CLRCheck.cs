/*
 Common Language Runtime (.NET/Mono) checker
 @author Sam Saint-Pettersen, 2011
 Released into the public domain.
	
 Originally written for use with 
 the NSIS installation script for nGaudi.
	
 But use as you like. No warranty.
	
 Usage: [mono] CLRCheck.exe
 Output: CLR version (e.g. 4.0.30319.1)
 Exit code: -1 (neither true or false)
	
 Usage: [mono] CLRCheck.exe <minimal version>
 Output: x.x.x_x (CLR version)
 Exit code: 0 (false) / 1 (true)(CLR installed, is minimal version or greater)
 */
 using System;
 using System.Text;
 using System.Text.RegularExpressions;
 
 namespace Stpettersens.CLRCheck
 {
    class CLRCheck
    {
        static void Main(string[] args)
        {  
            int returned = 1; // Return exit code -1 for neither true or false; default assumption
            string detectedVer = String.Format("{0}\n", Environment.Version);
              
            if(args.Length == 1)
            {
                // TODO
            }
            Console.Write(detectedVer);
            
            Environment.Exit(returned);
        }
    }
 }
 