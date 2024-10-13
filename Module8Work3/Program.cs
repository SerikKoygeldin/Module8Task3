using System;
using System.IO;
using System.Runtime;
using System.Runtime.ConstrainedExecution;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Module8Work3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dirName = @"C:\CSharp\1";

            if (Directory.Exists(dirName)) // Проверим, что директория существует
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirName);

                long deleteSize = 0;

                try
                {
                    long dirSize = DirSize(dirInfo);
                    Console.WriteLine("Исходный размер папки"+ $" {dirSize} байт");

                    ClearDirectoryByTime(dirName, out deleteSize);

                    Console.WriteLine("Освобождено "+ $" {deleteSize} байт");

                    dirSize = DirSize(dirInfo);
                    Console.WriteLine("Текущий размер папки "+ $" {dirSize} байт");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            else
            {
                Console.WriteLine("Каталог " + dirName + " не существует!");
            }       
        }
        static void ClearDirectoryByTime(string dirName,out long deleteSize)
        {
            deleteSize = 0;
            TimeSpan interval = TimeSpan.FromMinutes(30);           
            try             
            {             
                string[] dirs = Directory.GetDirectories(dirName);
                   
                foreach (string dir in dirs)          
                {     
                    DateTime dt = Directory.GetLastWriteTime(dir);       
                    TimeSpan difference = DateTime.Now - dt;
          
                    if (difference > interval)         
                    {           
                        DirectoryInfo dirInfo = new DirectoryInfo(dir);

                        deleteSize += DirSize(dirInfo);
                        dirInfo.Delete(true); // Удаление со всем содержимым      
                    }     
                }
       
                string[] files = Directory.GetFiles(dirName);    
                foreach (string file in files)      
                {     
                    DateTime dt = File.GetLastWriteTime(file);      
                    TimeSpan difference = DateTime.Now - dt;
      
                    if (difference > interval)          
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        deleteSize += fileInfo.Length;
                        fileInfo.Delete();            
                    }     
                }      
            }
              
            catch (Exception ex)              
            {            
                Console.WriteLine(ex.Message);             
            }
        }
        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }

            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo Di in dis)
            {
                size += DirSize(d);
            }
            return size;
        }
    }
}

