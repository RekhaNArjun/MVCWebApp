using System;
using System.Net;
using System.Net.Sockets;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class LocalMachineController : ApiController
    {
        public static int RequestNumber = 0;

        public MachineIp Get()
        {
            MachineIp mIp = new MachineIp();
            IPHostEntry host;
            LocalMachineController.RequestNumber++;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    mIp.ApiIp = ip.ToString();
                    mIp.ApiMachineName = Environment.MachineName;
                    mIp.ApiOS = Environment.OSVersion.VersionString;
                    mIp.RequestNumber = LocalMachineController.RequestNumber;
                }
            }

            return mIp;
        }
    }
}