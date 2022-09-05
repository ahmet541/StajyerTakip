using Microsoft.AspNetCore.Mvc;
using StajyerTakip.Models;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using MailKit.Net.Smtp;
using SmtpClient = System.Net.Mail.SmtpClient;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace StajyerTakip.Controllers
{
    public class MailMessageModelController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly UploadController _uploadController;
        public ActionResult Index()
        {
           return View();
        }


        public MailMessageModelController(IEmailService emailService, UploadController uploadController)
        {
            _emailService = emailService;
            _uploadController = uploadController;
        }

        public ActionResult Send([Bind("To,Subject,Body,FromEmail,FromPassword")] MailMessageModel mail)
        {
            _emailService.Send(mail);
            ModelState.Clear();
            return View("Index");
        }

        // Basvuru formu process
        public async Task<ActionResult> BasvuruFormu_Save(IEnumerable<IFormFile> basvuruFormu)
        {
            return await _uploadController.File_Save(basvuruFormu, "BasvuruFiles", "BasvuruFormu.pdf", true);
        }
        public ActionResult BasvuruFormu_Remove()
        {
            // The parameter of the Remove action must be called "fileNames"
            return _uploadController.File_Remove(new string[] { "StajSözlesmesiYeni.pdf" }, "BasvuruFiles");

        }
        [HttpPost]
        public JsonResult UpdateBasvuruFormu()
        {
             // No need to use
            return Json("");
        }



        /// <summary>
        /// Staj sözlesmesi process
        /// </summary>
        /// <param name="files"> input files</param>
        /// <returns>File_Save method of UploadController object</returns>
        public async Task<ActionResult> StajSözlesmesi_Save(IEnumerable<IFormFile> stajSözlesmesi) // files are input, we can change its type to single file since we always get 1 file from kendo upload.
        {
            return await _uploadController.File_Save(stajSözlesmesi, "BasvuruFiles", "StajSözlesmesi.pdf",true);
        }
        public ActionResult StajSözlesmesi_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"  // This is what kendo documents says

            // In my code I always set yp name of stajsözlesmesi and basvuru formu. So I know what are their name right now which ar stajSözlesmesiYeni.pdf
            // and similar for basvuru formu.  So I do not use input fileNames
            // The Question arises then, Why I do not delete parameter, 

            // EDIT: I removed it and it works fine
            // EDIT: we will not use this method, I write just incase;
            return _uploadController.File_Remove(new string[] { "StajSözlesmesiYeni.pdf" },"BasvuruFiles");
        }
        [HttpPost]
        public JsonResult UpdateStajSözlesmesi()
        {

            //_uploadController.Single_File_Remove("StajSözlesmesi.pdf", "BasvuruFiles");
            return Json("");
        }



        [HttpPost]
        public JsonResult Clear_FilesToDeleteFolder()
        {

            _uploadController.Clear_FilesToDeleteFolder();
            return Json("");
        }





    }

}
