using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace gender_bender
{
    public class RedisToXML
    {
        public string FileName { get; set; }
        public RedisToXML() { }

        public static string[]? open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            openFileDialog.Title = "Choose file with redis commands";


            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);

                string line = System.Text.Encoding.Default.GetString(System.IO.File.ReadAllBytes(openFileDialog.FileName));

                string[] lines = line.Split(Environment.NewLine,
                            StringSplitOptions.RemoveEmptyEntries);

                return lines;
            }
            return null;
        }
    }
}
