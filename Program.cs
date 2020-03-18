using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net;

namespace Download_Photo_Facebook
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tool download full hinh anh ban be tren FB by talaai1312_ver2");
            Console.WriteLine("---------");
            Console.WriteLine("Bam phim 1, sau do Enter de tool bat dau chay");
            string checkConfirm = Console.ReadLine();
            Console.WriteLine("Bam cho vui thoi, chu bam phim gi cung chay ca ^_^");
            Thread.Sleep(1000);
            Console.WriteLine("Bat dau download hinh anh");
            Console.WriteLine("---------");
            
            List<string> listID = File.ReadAllLines("listFB.txt").ToList();
            string fbToken = File.ReadAllText("token.txt");
            if (fbToken == "" || listID.Count == 0)
            {
                Console.WriteLine("Ban hay nhap token/ listFB muon download hinh");
                Console.ReadKey();
                return;
            }
            FacebookRequest facebookRequest = new FacebookRequest();

            foreach(string id in listID)
            {
                Console.WriteLine("Bat dau download hinh anh cua FB {0}", id);
                
                List<string> listDirectLinkPhotos = facebookRequest.GetPhotosUploaded(fbToken, id).GetAwaiter().GetResult();
                if (listDirectLinkPhotos.Count == 0)
                {
                    Console.WriteLine("Loi get direct link photos. Vui long kiem tra lai id facebook");
                    continue;
                }
                Console.WriteLine("Da getlink xong cua FB {0}, bat dau tai hinh ve may", id);
                Console.WriteLine("---------");
                Thread.Sleep(1000);

                int count = 0;
                foreach (string directPhotoslink in listDirectLinkPhotos)
                {
                    string localFolder = @"Hinh_" + id;
                    Directory.CreateDirectory(localFolder);
                    string localFilename = localFolder + @"\" + count + ".jpg";
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(directPhotoslink, localFilename);
                    }
                    count++;
                    if (count % 10 == 0)
                    {
                        Console.WriteLine("Da tai duoc {0} hinh, vui long cho...", count);
                    }
                    Thread.Sleep(500);
                }
                Console.WriteLine("Da tai hinh xong cua FB {0}", id);
                Thread.Sleep(3000);
            }
            Console.WriteLine("Tool da chay xong");
            Console.WriteLine("Tool download full hinh anh ban be tren FB by talaai1312_ver2");
            Console.ReadKey();
        }
    }
}
