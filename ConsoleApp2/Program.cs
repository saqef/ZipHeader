using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    [StructLayout(LayoutKind.Sequential)]

    class LocalFileHeader
    {   
        // Обязательная сигнатура, равна 0x04034b50
        public UInt32 signature;
        // Минимальная версия для распаковки
        public UInt16 versionToExtract;
        // Битовый флаг
        public UInt16 generalPurposeBitFlag;
        // Метод сжатия (0 - без сжатия, 8 - deflate)
        public UInt16 compressionMethod;
        // Время модификации файла
        public UInt16 modificationTime;
        // Дата модификации файла
        public UInt16 modificationDate;
        // Контрольная сумма
        public UInt16 crc32;
        // Сжатый размер
        public UInt16 compressedSize;
        // Несжатый размер
        public UInt16 uncompressedSize;
        // Длина название файла
        public UInt16 filenameLength;
        // Длина поля с дополнительными данными
        public UInt16 extraFieldLength;
        // Название файла (размером filenameLength)
        public UInt16 filename;
        // Дополнительные данные (размером extraFieldLength)
        public UInt16 extraField;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var header = new LocalFileHeader();
            var headerSize = Marshal.SizeOf(header);
            string path = @"C:\test\";
            List<string> filesname = Directory.GetFiles(path, "*.zip").ToList<string>();
            FileStream fileStream;
            foreach (var file in filesname)
            { fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                var buffer = new byte[headerSize];
                fileStream.Read(buffer, 0, headerSize);
                var headerPtr = Marshal.AllocHGlobal(headerSize);
                Marshal.Copy(buffer, 0, headerPtr, headerSize);
                Marshal.PtrToStructure(headerPtr, header);
                Type fieldsType = typeof(LocalFileHeader);
                FieldInfo[] fields = fieldsType.GetFields(BindingFlags.Public | BindingFlags.Instance);
               // int k = Convert.ToInt32(fields[1].GetValue(header));
                //Console.WriteLine(Convert.ToString(fields[1].GetValue(header),16));
                Console.WriteLine("File:{0}",file);
                foreach (var field in fields)
                    Console.WriteLine("{0}:\t{1:x}", field.Name,field.GetValue(header));
                Console.WriteLine("\n");
                Marshal.FreeHGlobal(headerPtr);
            }
            Console.ReadKey();
        }

    }
    

}
