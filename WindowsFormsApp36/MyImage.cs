using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp36
{
    public class MyImage
    {
        public MyImage(int id, string filename, byte[] data)
        {
            Id = id;
            FileName = filename;
            Data = data;
        }
        public int Id { get; private set; }
        public string FileName { get; private set; }
        public byte[] Data { get; private set; }
    }
}
