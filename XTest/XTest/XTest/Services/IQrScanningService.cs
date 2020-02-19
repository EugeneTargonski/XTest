using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XTest.Services
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}
