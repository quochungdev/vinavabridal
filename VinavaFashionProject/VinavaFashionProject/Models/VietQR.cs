using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinavaFashionProject.Models
{
    public class VietQR
    {
        public string payloadFormatIndicator;
        public string consumerAccountInformation;
        public string guid;
        public string serviceCode;
        public string transactionCurrency;
        public string transactionAmount;
        public string countryCode;
        public string additionalDataFieldTemplate;
        public string QRCRC;
        public string crc;

        public VietQR()
        {
            this.payloadFormatIndicator = "000201010212";
            this.consumerAccountInformation = "";
            this.guid = "0010A000000727";
            this.serviceCode = "0208QRIBFTTA";
            this.transactionCurrency = "5303704";
            this.transactionAmount = "";
            this.countryCode = "5802VN";
            this.additionalDataFieldTemplate = "";
            this.QRCRC = "6304";
            this.crc = "";
        }

        private string ConvertLength(string str)
        {
            int num = int.Parse(str);
            return num < 10 ? $"0{num}" : num.ToString();
        }

        public VietQR SetTransactionAmount(decimal money)
        {
            string moneyString = money.ToString();
            string length = ConvertLength(moneyString.Length.ToString());
            this.transactionAmount = $"54{length}{money}";
            return this;
        }

        public VietQR SetBeneficiaryOrganization(string acquierID, string consumerID)
        {
            VietQR vqr = new VietQR();
            string acquierLength = ConvertLength(acquierID.Length.ToString());
            string acquier = $"00{acquierLength}{acquierID}";
            string consumerLength = ConvertLength(consumerID.Length.ToString());
            string consumer = $"01{consumerLength}{consumerID}";
            string beneficiaryOrganizationLength = ConvertLength(($"{acquier}{consumer}").Length.ToString());
            string consumerAccountInformationLength = ConvertLength(($"01{beneficiaryOrganizationLength}{acquier}{consumer}{vqr.serviceCode}{vqr.guid}").Length.ToString());
            this.consumerAccountInformation = $"38{consumerAccountInformationLength}{vqr.guid}01{beneficiaryOrganizationLength}{acquier}{consumer}0208QRIBFTTA";
            return this;
        }

        public VietQR SetAdditionalDataFieldTemplate(string content)
        {
            string contentLength = ConvertLength(content.Length.ToString());
            string additionalDataFieldTemplateLength = ConvertLength((content.Length + 4).ToString());
            this.additionalDataFieldTemplate = $"62{additionalDataFieldTemplateLength}08{contentLength}{content}";
            return this;
        }

        public string CreatePaymentCRC(string strInput)
        {
            return Crc16Ccitt(strInput).ToString("X");
        }

        public static ushort Crc16Ccitt(string strInput)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(strInput);
            const ushort poly = 4129;
            ushort[] table = new ushort[256];
            ushort initialValue = 0xffff;
            ushort temp, a;
            ushort crc = initialValue;
            for (int i = 0; i < table.Length; ++i)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                        temp = (ushort)((temp << 1) ^ poly);
                    else
                        temp <<= 1;
                    a <<= 1;
                }
                table[i] = temp;
            }
            for (int i = 0; i < bytes.Length; ++i)
            {
                crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }
            return crc;
        }

    }
}
