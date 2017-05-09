using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 生成身份证
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            test();
        }
        private void test()
        {
            IDNumber gen = new IDNumber("310113198207081423");
            List<string> cityCodes = new List<string>
            {
                //"310101","310103","310104","310105","310106","310107","310108","310109","310110",
                //"310112","310114","310116","310117","310118","310119","310120","310230"
                "310104"
            };
            List<string> codes = new List<string>();
            foreach (string city in cityCodes)
            {
                for (int i = 0; i <= 9; i++)
                {
                    for (int j = 0; j <= 9; j++)
                    {

                        for (int y = 0; y <= 9; y++)
                        {
                            string code = gen.Generate(city, "19910829", i.ToString() + j.ToString() + y.ToString(), true);
                           
                            string sex = code.Substring(12, 3);
                            if (int.Parse(sex) % 2 == 0)
                            {
                               // System.Diagnostics.Trace.WriteLine(code + "女");
                            }

                            else
                            {
                                codes.Add(code);
                                System.Diagnostics.Trace.WriteLine(code);
                            }
                         
                        }
                    }
                }
            }
            MessageBox.Show("ok");
        }
        private void test1()
        {
            IDNumber gen = new IDNumber("310113198207081423");
            string code = gen.Generate("310101", "19910829", "000", false);
        }

    }
    public class IDNumber

    {

        private readonly int[] CheckCodes = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };

        private readonly string ValidateCodes = "10X98765432";

        public string _provinceCode { get; set; }

        public string _cityCode { get; set; }

        public string _countryCode { get; set; }

        public string _birthday { get; set; }

        public string _policeStationCode { get; set; } 

        public bool _sex { get; set; } 

        private string _validateCode;

        public bool Accuracy { get; set; }

        public string Number { get; set; }

        public IDNumber(string idNumber)

        {

            Parse(idNumber);

            Accuracy = Validate(idNumber);

        }

        private void Parse(string idNumber)

        {

            if (idNumber.Length != 18 && idNumber.Length != 15)

            {

                throw new Exception(string.Format("{0}长度错误。", idNumber));

            }
            //1-2位省、自治区、直辖市代码；
            _provinceCode = idNumber.Substring(0, 2);
            //3-4位地级市、盟、自治州代码；
            _cityCode = idNumber.Substring(2, 2);
            //5-6位县、县级市、区代码；
            _countryCode = idNumber.Substring(4, 2);
            //7-14位出生年月日，比如19670401代表1967年4月1日； 
            _birthday = idNumber.Substring(6, 8);
            //15-17位为顺序号                        
            _policeStationCode = idNumber.Substring(14, 2);
            //17位（倒数第二位）男为单数，女为双数； 
            _sex = Convert.ToInt32(idNumber.Substring(16, 1)) % 2 == 1;
            //18位为校验码，0 - 9和X
             _validateCode = idNumber.Substring(17, 1);

        }

        private bool Validate(string idNumber)

        {

            bool accuracy = false;

            if (idNumber.Length == 18)

            {

                accuracy = string.Compare(GeneratorValidateCode(idNumber), _validateCode) == 0;

            }

            return accuracy;

        }

        private string GeneratorValidateCode(string idNumber)

        {

            string validateCode = string.Empty;

            if (idNumber.Length == 18)

            {

                int sum = 0;

                for (int i = 0; i <= 16; i++)

                {

                    sum += Convert.ToInt32(idNumber[i].ToString()) * Convert.ToInt32(CheckCodes[i]);

                }

                int validateCodeIndex = (sum % 11);

                validateCode = ValidateCodes[validateCodeIndex].ToString();

            }

            return validateCode;

        }

        public  string Generate(string birthday, bool sex)

        {

            StringBuilder sb = new StringBuilder();

            sb.Append(_provinceCode);

            sb.Append(_cityCode);

            sb.Append(_countryCode);

            sb.Append(birthday);

            sb.Append(_policeStationCode);

            if (sex)

            {

                sb.Append("1");

            }

            else

            {

                sb.Append("2");

            }

            sb.Append(GeneratorValidateCode(sb.ToString() + "0"));

            return sb.ToString();

        }
        public string Generate(string cityCode,string birthday,string codeNum, bool sex)

        {

            StringBuilder sb = new StringBuilder();
            sb.Append(cityCode);
            sb.Append(birthday);
            sb.Append(codeNum);
            sb.Append(GeneratorValidateCode(sb.ToString()+"0"));

            return sb.ToString();

        }
    }
}
