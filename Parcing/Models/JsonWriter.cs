﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Parcing.Models
{
    public class FileJsonWriter
    {
        List<Review> ToWrite;
        int filenumber;
        public string fileprefix = "";
        public string outdir;
        public int inOneFile;
        int k =0;

        public FileJsonWriter(int inone,string prefix,string outd)
        {
            outdir = outd;
            fileprefix = prefix;
            inOneFile = inone;
            ToWrite = new List<Review>();
            filenumber = 1;
        }

        public void Write(Review rev)
        {
            ToWrite.Add(rev);
            k++;
            Console.WriteLine(k);
            if (ToWrite.Count >= inOneFile)
            {
                WriteFile();
                ToWrite = new List<Review>();
                filenumber++;
            }
        }

        public void WriteAll()
        {
            WriteFile();
        }

        void WriteFile()
        {
            if (!Directory.Exists(outdir)) { Directory.CreateDirectory(outdir); }
            string fileName =outdir+"/"+ fileprefix + filenumber.ToString() + ".json";
            StreamWriter streamWriter =new StreamWriter(File.Create(fileName));
            streamWriter.Write( JsonConvert.SerializeObject(ToWrite));
            streamWriter.Close();
        }

    }
}
