using PdfSharp.Pdf.IO;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static QR.Models.Connect;

namespace QR.Controllers
{
    public class HomeController : Controller
    {

        //********************* ОБНОВИТЬ РАСПИСАНИЕ **************************
        [HttpGet]
        public ViewResult Upload()=> View();
    
        
        [HttpPost]                  
        public ActionResult Upload(HttpPostedFileBase file, string password)
        {           

            if (file != null && file.ContentLength > 0 && password.Equals("$ecret") && file.ContentType == "application/pdf")
            {
           
                string fileName = "rasp.pdf";
                // сохраняем файл в папку Files в проекте
                file.SaveAs(Server.MapPath("~/Files/" + fileName));
                return View("Result", true);
            }
            return View("Result", false) ;  
            
        }

        //********************* СКАЧАТЬ РАСПИСАНИЕ **************************


        public async Task<FileResult> Download(int? page)
        {
            PdfSharp.Pdf.PdfDocument PDFDoc;
            try
            {
                
                DateTime strLastModified = System.IO.File.GetLastWriteTime(Server.MapPath("~/Files/rasp.pdf"));

                if ((DateTime.Now - strLastModified).TotalDays > 14.0 )
                        {//старое расписание
                    PDFDoc = PdfSharp.Pdf.IO.PdfReader.Open(Server.MapPath("~/Files/dummy.pdf"), PdfDocumentOpenMode.Import);
                } else { //скачиваем новое
                    PDFDoc = PdfSharp.Pdf.IO.PdfReader.Open(Server.MapPath("~/Files/rasp.pdf"), PdfDocumentOpenMode.Import);
                }

            }
            catch
            {
                PDFDoc = PdfSharp.Pdf.IO.PdfReader.Open(Server.MapPath("~/Files/dummy.pdf"), PdfDocumentOpenMode.Import);
            }

            PdfSharp.Pdf.PdfDocument PDFNewDoc = new PdfSharp.Pdf.PdfDocument();
            PDFNewDoc.Info.Author = "МОРУЦ - Лищук Сергей Дмитриевич";



            //********************
            try
            {
                BaseDbContext db = new BaseDbContext();
                int p;
                if (page == null) { p = 0; } else { p = (int)page; }    

                
                int h = await db.Database.SqlQuery<int>("INSERT INTO [dbo].[t_load] ([loaddate],[page]) VALUES('" + DateTime.Now +"'," +  p.ToString() + ") select 1").FirstOrDefaultAsync();           
                db.Dispose();
            }
            catch {
            }
            
             
         
            //*******************

            if (page == null || page > PDFDoc.Pages.Count)
            {
                for (int Pg = 0; Pg < PDFDoc.Pages.Count; Pg++)
                {
                    PDFNewDoc.AddPage(PDFDoc.Pages[Pg]);
                }
            }
            else { PDFNewDoc.AddPage(PDFDoc.Pages[(int)page-1]); }


            MemoryStream stream = new MemoryStream();
            PDFNewDoc.Save(stream, false);
            string file_type = "application/pdf";
            string file_name = "Расписание.pdf";            
            

            return File(stream, file_type, file_name);
        }
        

        //********************* СТРАНИЦА ПРИВЕТСТВИЯ **************************

        public ActionResult Index()
        {
            return View();
        }

       
    }
}