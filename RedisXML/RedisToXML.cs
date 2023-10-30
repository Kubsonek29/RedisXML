using Microsoft.Win32;
using RedisXML;
using RedisXML.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace gender_bender
{
    public static class RedisToXML
    {
        public static string[]? Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            openFileDialog.Title = "Choose file with redis commands";


            if (openFileDialog.ShowDialog() == true)
            {
                //FileInfo fileInfo = new FileInfo(openFileDialog.FileName);

                string line = System.Text.Encoding.Default.GetString(System.IO.File.ReadAllBytes(openFileDialog.FileName));

                string[] lines = line.Split(Environment.NewLine,
                            StringSplitOptions.RemoveEmptyEntries);

                return lines;
            }
            return null;
        }

        public static string[] OpenTEST()
        {
            string[] lines = Resources.TEST.Split(Environment.NewLine,
                            StringSplitOptions.RemoveEmptyEntries);

            return lines;
        }

        public static bool CheckFileCompatibility(string[] lines)
        {
            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');
                if (parts.Length > 0)
                {
                    string command = parts[0];
                    string name = parts[1];
                    string[] arguments = parts[2..];

                    string pattern = @"^[a-zA-Z]+$";


                    if (line.Count() > 0 && (command == null || name == null || arguments == null)) return false;

                    if (!Regex.IsMatch(command, pattern)) return false; //return niepoprawna komenda
                    //TODO add other checks for command Compatibility

                    if (arguments.Count() % 2 != 0) return false; //nie poprawna ilosc argumentow - musi byc parzysta
                    //hset(command) ksiazka:scifi(name) (year 2022 liga trash)(arguments)

                    //arguments = arguments.Select(item => item.Replace('\"', ' ')).ToArray();


                }
            }

            return true;
        }

        public static void ConvertRedisTXT_To_XMLTXT(string[] lines)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            XElement mainElement = new XElement("RedisCommands");

            foreach(string line in lines)
            {
                string[] parts = line.Split(' ');
                string command = parts[0];
                string name = parts[1];
                string[] arguments = parts[2..];

                arguments = arguments.Select(item => item.Replace("\"", "")).ToArray();

                XElement rootElement = new XElement("RedisCommand");
                rootElement.SetAttributeValue("Name", command);
                for (int i = 0; i < arguments.Length; i+=2)
                {
                    XElement key = new XElement(arguments[i], arguments[i+1]);

                    rootElement.Add(key);
                }
                mainElement.Add(rootElement);
            }

            XDocument ConvertedFile = new XDocument(mainElement);
            ConvertedFile.Save("redis.xml");


        }
    }
}
