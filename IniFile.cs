using System;
using System.IO;
using System.Collections.Generic;
namespace PlayGround_c_
{
    class IniFile
    {
        private Dictionary <string,Dictionary<string,string>> _iniFile = new Dictionary<string,Dictionary<string,string>>();
        private string _name = "";
        //----------------------------------------------------------
        public IniFile(string fileName)
        {
            if(File.Exists(fileName))
            {
                _iniFile.Clear();
                Console.WriteLine($"INI found : {fileName}");
                _name = fileName;
                
                Load();
            }
        }
        //----------------------------------------------------------
        private void Load()
        {
            using (StreamReader reader = new StreamReader(File.OpenRead(_name)))
            {
                string curHeader = "";
                int counter = 0;
                int numberOfMissings = 0;
                while(!reader.EndOfStream)
                {
                    var curLine = reader.ReadLine();
                    
                    if(!string.IsNullOrEmpty(curLine))
                        if(curLine.StartsWith("["))
                        {
                            curHeader = curLine;
                            _iniFile.Add(curHeader, new Dictionary<string, string>());
                        }
                        else
                        {
                            string[] cfgs = curLine.Split("=",2);
                            if(_iniFile[curHeader].TryAdd(cfgs[0],cfgs[1]))
                            counter ++;
                            else
                            numberOfMissings++;
                        }
                }
                Console.WriteLine($"{_iniFile.Keys.Count} Headers with total of {counter} configs were found. {numberOfMissings} were missed");
            }            
        }
        //----------------------------------------------------------
        public void Display()
        {
            Console.Clear();
            int counter = 0;
            foreach (var kvp in _iniFile)
            {
                Console.WriteLine(kvp.Key);
                foreach (var kvp2 in kvp.Value)
                {
                    Console.WriteLine("".PadRight(4)+$"{kvp2.Key}={kvp2.Value}");   
                    counter ++;
                }
            }
            Console.WriteLine($"{_iniFile.Keys.Count} Headers with total of {counter} configs were found");
        }
        //----------------------------------------------------------
        public void DisplayHeader(string header)
        {
            Console.Clear();
            if(_iniFile.ContainsKey(header))
            {
                foreach (var kvp in _iniFile[header])
                {
                    Console.WriteLine($"{kvp.Key}={kvp.Value}");
                }
                Console.WriteLine($"{_iniFile[header].Values.Count} configs were found");
            }
        }
        //-----------------------------------------------------------
        public string GetValue(string header, string key)
        {
            Console.Clear();
            if(_iniFile.ContainsKey(header))
            {
                if(_iniFile[header].ContainsKey(key))
                {
                    return _iniFile[header][key];
                }
                else
                {
                    return $"{key} not found";
                }
            }
            else
            {
                return $"{header} not found";
            }
        }
        //-------------------------------------------------------------
        public void SetValue(string value, string header, string key)
        {
            Console.Clear();
            if(_iniFile.ContainsKey(header))
            {
                if(_iniFile[header].ContainsKey(key))
                {
                    _iniFile[header][key] = value;
                    DisplayHeader(header);
                }
                else
                {
                    Console.WriteLine($"{key} not found");
                }
            }
            else
            {
                Console.WriteLine($"{header} not found");
            }
        }
        //-------------------------------------------------------------
        public void Save()
        {
            using(StreamWriter writer = new StreamWriter(File.OpenWrite(_name)))
            {
                foreach (var kvp in _iniFile)
                {
                    writer.WriteLine(kvp.Key);
                    foreach (var kvp2 in kvp.Value)
                    { 
                        if(kvp2.Key != "")
                        writer.WriteLine(kvp2.Key+"="+kvp2.Value);
                    }
                    writer.WriteLine();
                }       
            }
        }
    }
}
