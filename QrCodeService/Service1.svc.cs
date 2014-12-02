using OnBarcode.Barcode.BarcodeScanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace QrCodeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string ReadQRCode(string Path)
        {
            string errorMessage = "Read QR Code Faild";

            string[] datas = BarcodeScanner.Scan(Path, BarcodeType.QRCode);
            try
            {
                if (!string.IsNullOrWhiteSpace(datas[0]))
                    return datas[0];
            }
            catch (IndexOutOfRangeException ex)
            {
                return errorMessage;
            }
            catch (NullReferenceException ex)
            {
                return errorMessage;
            }
            return errorMessage;
        }
    }
}
