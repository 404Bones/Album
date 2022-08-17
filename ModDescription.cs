using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using System.IO;

namespace Album
{
    public class ModDescription
    {
        public ModDescription(string modname, string desc)
        {
            string albumdescpath = Paths.BepInExRootPath + "/albumdesc/";
            Directory.CreateDirectory(albumdescpath);
            if (File.Exists(albumdescpath + modname.ToLower() + ".txt")) {
                return;
            } else {
                var descfile = File.CreateText(albumdescpath + modname.ToLower() + ".txt");
                descfile.Write(desc);
                descfile.Close();
            }  
        }
    }
}
