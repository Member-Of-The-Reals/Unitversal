using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Reflection;

namespace Unitversal
{
    public class IniFile
    {
        //File path
        public string Path;
        //Name of executable
        string ProgramName = Assembly.GetExecutingAssembly().GetName().Name;
        //Windows API functions for reading and writing ini file
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
        //Create file and set file path
        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? ProgramName + ".ini").FullName;
        }
        //Read a key from specified section
        public string Read(string Section, string Key)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }
        //Write value to key in specified section
        public void Write(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }
        //Delete a key from specified section
        public void DeleteKey(string Section, string Key)
        {
            Write(Section, Key, null);
        }
        //Delete a section
        public void DeleteSection(string Section)
        {
            Write(Section, null, null);
        }
        //Check for existance of key
        public bool KeyExists(string Section, string Key)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}