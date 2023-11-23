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
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace gender_bender
{
    public static class RedisToXML
    {
        public static (string[]? lines, string? filepath) Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            openFileDialog.Title = "Choose file with redis commands";

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);

                string line = System.Text.Encoding.Default.GetString(System.IO.File.ReadAllBytes(openFileDialog.FileName));

                string[] lines = line.Split(Environment.NewLine,
                            StringSplitOptions.RemoveEmptyEntries);

                return (lines, fileInfo.FullName);
            }
            return (null, null);
        }

        public static string[] OpenTEST()
        {
            string[] lines = Resources.TEST.Split(Environment.NewLine,
                            StringSplitOptions.RemoveEmptyEntries);

            return lines;
        }

        public static (bool IsGood, string message) CheckFileCompatibility(string[] lines) //only hash commands
        {
            int linecount = 0;
            foreach (string line in lines)
            {
                linecount++;
                string[]? parts = line.Split(' ');
                if (parts.Length > 0 && line != string.Empty && line != "\n")
                {
                    try
                    {
                        string command = parts[0];
                        string? nameOrKey = parts?[1];
                        string[]? argumentsOrFieldsOrValues = parts?[2..];

                        string pattern = @"^[a-zA-Z]+$";

                        if (line.Count() > 0 && command is null) return (false, $"Nie poprawna linia -  linia: {linecount}");

                        if (!Regex.IsMatch(command, pattern)) return (false, $"Nie poprawna komenda  - linia: {linecount}"); //return niepoprawna komenda

                        string lowercommand = command.ToLower();
                        bool error = false;

                        switch (lowercommand)
                        {
                            case "hset":
                                {
                                    // hset(command) ksiazka: scifi(name)(year 2022...)(arguments)
                                    if (nameOrKey is null || argumentsOrFieldsOrValues is null)
                                    {
                                        error = true;
                                        break;
                                    }
                                    if (argumentsOrFieldsOrValues.Count() % 2 != 0)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hget":
                                {
                                    // hget key field
                                    if (nameOrKey is null || argumentsOrFieldsOrValues is null)
                                    {
                                        error = true;
                                        break;
                                    }
                                    if (argumentsOrFieldsOrValues.Count() != 1)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hdel":
                                {
                                    // HDEL key field [field ...]
                                    if (nameOrKey is null || argumentsOrFieldsOrValues is null)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hexists":
                                {
                                    // HEXISTS key field
                                    if (nameOrKey is null || argumentsOrFieldsOrValues is null)
                                    {
                                        error = true;
                                        break;
                                    }
                                    if (argumentsOrFieldsOrValues.Count() != 1)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hgetall":
                                {
                                    //HGETALL key
                                    if (nameOrKey is null || argumentsOrFieldsOrValues.Count() != 0)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hincrby":
                                {
                                    //INCRBY key field increment
                                    if (nameOrKey is null || argumentsOrFieldsOrValues is null)
                                    {
                                        error = true;
                                        break;
                                    }
                                    if (argumentsOrFieldsOrValues.Count() != 2)
                                    {
                                        error = true;
                                        break;
                                    }

                                }
                                break;
                            case "hincrbyfloat":
                                {
                                    //HINCRBYFLOAT key field increment
                                    if (nameOrKey is null || argumentsOrFieldsOrValues is null)
                                    {
                                        error = true;
                                        break;
                                    }
                                    if (argumentsOrFieldsOrValues.Count() != 2)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hkeys":
                                {
                                    //HKEYS key
                                    if (nameOrKey is null || argumentsOrFieldsOrValues.Count() != 0)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hlen":
                                {
                                    //HLEN key
                                    if (nameOrKey is null || argumentsOrFieldsOrValues.Count() != 0)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hmget":
                                {
                                    //HMGET key field [field ...]
                                    if (nameOrKey is null || argumentsOrFieldsOrValues is null)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hmset":
                                {
                                    //HMSET key field value [field value ...]
                                    if (nameOrKey is null || argumentsOrFieldsOrValues is null)
                                    {
                                        error = true;
                                        break;
                                    }
                                    if (argumentsOrFieldsOrValues.Count() % 2 != 0)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hrandfield":
                                {
                                    //HRANDFIELD key [count [WITHVALUES]]
                                    //?
                                }
                                break;
                            case "hscan":
                                {
                                    //HSCAN key cursor [MATCH pattern] [COUNT count]
                                    //?
                                }
                                break;
                            case "hsetnx":
                                {
                                    //HSETNX key field value
                                    if (nameOrKey is null || argumentsOrFieldsOrValues is null)
                                    {
                                        error = true;
                                        break;
                                    }
                                    if (argumentsOrFieldsOrValues.Count() != 2)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hstrlen":
                                {
                                    //HSTRLEN key field
                                    if (nameOrKey is null || argumentsOrFieldsOrValues is null)
                                    {
                                        error = true;
                                        break;
                                    }
                                    if (argumentsOrFieldsOrValues.Count() != 1)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            case "hvals":
                                {
                                    //HVALS key
                                    if (nameOrKey is null || argumentsOrFieldsOrValues.Count() != 0)
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                break;
                            default:
                                return (false, $"Brak wsparcia dla komendy w linii: {linecount}");

                        }

                        if (error == true)
                        {
                            return (false, $"Błąd w linii - {linecount}");
                        }
                    }
                    catch
                    {
                        return (false, "Błędny plik - sprawdź poprawność komendy w linii -> " + linecount);
                    }
                    //arguments = arguments.Select(item => item.Replace('\"', ' ')).ToArray();
                }
            }

            return (true, "GOOD");
        }

        public static XDocument ConvertRedisTXT_To_XMLTXT(string[] lines)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            XElement mainElement = new XElement("RedisCommands");

            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');
                string command = parts[0];
                string nameOrKey = parts[1];
                string[] argumentsOrFieldsOrValues = parts[2..];

                //arguments = arguments.Select(item => item.Replace("\"", "")).ToArray();

                XElement rootElement = new XElement("RedisCommand");

                string lowercommand = command.ToLower();
                switch (lowercommand)
                {
                    case "hset":
                        {
                            // hset(command) ksiazka: scifi(name)(year 2022...)(arguments)
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            for (int i = 0; i < argumentsOrFieldsOrValues.Length; i += 2)
                            {
                                XElement key = new XElement(argumentsOrFieldsOrValues[i], argumentsOrFieldsOrValues[i + 1]);

                                rootElement.Add(key);
                            }
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hget":
                        {
                            // hget key field
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            XElement key = new XElement("Field", argumentsOrFieldsOrValues[0]);
                            rootElement.Add(key);
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hdel":
                        {
                            // HDEL key field [field ...]
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            for (int i = 0; i < argumentsOrFieldsOrValues.Count(); i++)
                            {
                                XElement key = new XElement("Field", argumentsOrFieldsOrValues[i]);
                                rootElement.Add(key);
                            }
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hexists":
                        {
                            // HEXISTS key field
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            XElement key = new XElement("Field", argumentsOrFieldsOrValues[0]);
                            rootElement.Add(key);
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hgetall":
                        {
                            //HGETALL key
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hincrby":
                        {
                            //INCRBY key field increment
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            XElement key = new XElement("Field", argumentsOrFieldsOrValues[0]);
                            XElement key2 = new XElement("Increment", argumentsOrFieldsOrValues[1]);
                            rootElement.Add(key);
                            rootElement.Add(key2);
                            mainElement.Add(rootElement);

                        }
                        break;
                    case "hincrbyfloat":
                        {
                            //HINCRBYFLOAT key field increment
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            XElement key = new XElement("Field", argumentsOrFieldsOrValues[0]);
                            XElement key2 = new XElement("Increment", argumentsOrFieldsOrValues[1]);
                            rootElement.Add(key);
                            rootElement.Add(key2);
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hkeys":
                        {
                            //HKEYS key
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hlen":
                        {
                            //HLEN key
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hmget":
                        {
                            //HMGET key field [field ...]
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            for (int i = 0; i < argumentsOrFieldsOrValues.Count(); i++)
                            {
                                XElement key = new XElement("Field", argumentsOrFieldsOrValues[i]);
                                rootElement.Add(key);
                            }
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hmset":
                        {
                            //HMSET key field value [field value ...]
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            for (int i = 0; i < argumentsOrFieldsOrValues.Length; i += 2)
                            {
                                XElement key = new XElement(argumentsOrFieldsOrValues[i], argumentsOrFieldsOrValues[i + 1]);

                                rootElement.Add(key);
                            }
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hrandfield":
                        {
                            //HRANDFIELD key [count [WITHVALUES]]
                            //?
                        }
                        break;
                    case "hscan":
                        {
                            //HSCAN key cursor [MATCH pattern] [COUNT count]
                            //?
                        }
                        break;
                    case "hsetnx":
                        {
                            //HSETNX key field value
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            XElement key = new XElement(argumentsOrFieldsOrValues[0], argumentsOrFieldsOrValues[1]);
                            rootElement.Add(key);
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hstrlen":
                        {
                            //HSTRLEN key field
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            XElement key = new XElement("Field", argumentsOrFieldsOrValues[0]);
                            rootElement.Add(key);
                            mainElement.Add(rootElement);
                        }
                        break;
                    case "hvals":
                        {
                            //HVALS key
                            rootElement.SetAttributeValue("Key", nameOrKey);
                            rootElement.SetAttributeValue("Name", command);
                            mainElement.Add(rootElement);
                        }
                        break;
                }
            }

            XDocument ConvertedFile = new XDocument(mainElement);
            return ConvertedFile;
        }
    }
}
