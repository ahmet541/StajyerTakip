using Microsoft.AspNetCore.Mvc;
using System.IO;
using ContentDispositionHeaderValue = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue;

namespace StajyerTakip.Controllers
{
    public partial class UploadController : Controller
    {
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public ActionResult CustomDropZone()
        {
            return View();
        }
        [HttpPost]
        public bool CheckExistanceOfPicture([FromBody] string data)
        {
            var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "Pictures", data);


            return System.IO.File.Exists(physicalPath);
        }
      
        public async Task<ActionResult> Picture_Save(IEnumerable<IFormFile> pictures)
        {
            // The Name of the Upload    component is "files"
            if (pictures != null)
            {
                foreach (var file in pictures)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                    var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "Pictures", fileName);
                    TempData["Picture"] = fileName;
                    Console.WriteLine(physicalPath);
                    using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    };
                }
            }

            // Return an empty string to signify success

            return Content("");
        }


        public ActionResult Picture_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "Pictures", fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult Single_Picture_Remove(string fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                
                
                var fileName = Path.GetFileName(fileNames);
                var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "Pictures", fileName);

                // TODO: Verify user permissions

                if (System.IO.File.Exists(physicalPath))
                {
                    // The files are not actually removed in this demo
                    System.IO.File.Delete(physicalPath);
                }
                
            }

            // Return an empty string to signify success
            return Content("");
        }

        public async Task<ActionResult> Cv_Save(IEnumerable<IFormFile> cvs)
        {
            // The Name of the Upload component is "files"
            if (cvs != null)
            {
                foreach (var file in cvs)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                    var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "CVs", fileName);
                    TempData["CV"] = fileName;
                    // The files are not actually saved in this demo
                    using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    };
                }
            }

            // Return an empty string to signify success
            
            return Content("");
        }

        public ActionResult Cv_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "CVs", fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult Single_Cv_Remove(string fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {


                var fileName = Path.GetFileName(fileNames);
                var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "CVs", fileName);

                // TODO: Verify user permissions

                if (System.IO.File.Exists(physicalPath))
                {
                    // The files are not actually removed in this demo
                    System.IO.File.Delete(physicalPath);
                }

            }

            // Return an empty string to signify success
            return Content("");
        }

        public bool CheckExistanceOfCv([FromBody] string data)
        {
            var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "CVs", data);


            return System.IO.File.Exists(physicalPath);
        }

        public bool File_Rename( string type, string newName) // I write this for my older code which requires to rename, but now I do not need to use it.
        {

            // Some browsers send file names with full path.
            // We are only interested in the file name.
            var fileName ="";
            if (type == "BasvuruFiles")
            {
                fileName = "BasvuruFormuYeni";
            }
            var oldPath = Path.Combine(WebHostEnvironment.WebRootPath, type + "s", fileName);
            var newPath = Path.Combine(WebHostEnvironment.WebRootPath, type + "s", newName);
            try
            {
                System.IO.File.Move(oldPath, newPath);
                return true;

            }
            catch
            {

            }
            return false;
        }
        public bool File_Move(string oldName, string newName,string fromFolder,string toFolder)
        {

            // Some browsers send file names with full path.
            // We are only interested in the file name.
           
            var oldPath = Path.Combine(WebHostEnvironment.WebRootPath,fromFolder, oldName);
            var newPath = Path.Combine(WebHostEnvironment.WebRootPath, toFolder, newName);
            try
            {
                System.IO.File.Move(oldPath, newPath);
                return true;

            }
            catch // TODO  I do not know what to do here
            {

            }
            return false;
        }
        /**
         * 
         */
        public async Task<ActionResult> File_Save(IEnumerable<IFormFile> files, string destinationFolder, string newName, bool changeName)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                    
                    if(changeName)
                    {
                        fileName = newName;
                    }
                   
                    var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, destinationFolder, fileName);
                   
                    //TempData[type] = fileName; // TODO
                    Console.WriteLine(physicalPath);
                    // The files are not actually saved in this demo
                    using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    };
                }
            }

            // Return an empty string to signify success

            return Content("");
        }

        public ActionResult File_Remove(string[] fileNames, string destinationFolder)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, destinationFolder, fileName); 

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult Single_File_Remove(string fileNames,string pathFolder) // instead of fileNames we can use any name. Since in code we have var fileName, I did not use fileName as a input parameter
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {


                var fileName = Path.GetFileName(fileNames);
                if(fileName == "defaultPictureForStajyer.jpg")
                {
                    return Content("");
                }
                var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, pathFolder, fileName);

                // TODO: Verify user permissions

                if (System.IO.File.Exists(physicalPath))
                {
                    // The files are not actually removed in this demo
                    System.IO.File.Delete(physicalPath);
                }

            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Clear_FilesToDeleteFolder()
        {
            // The parameter of the Remove action must be called "fileNames"

            System.IO.DirectoryInfo di = new DirectoryInfo("wwwroot\\FilesToDelete");

            foreach (FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }

            // Return an empty string to signify success
            return Content("");
        }
        public bool CheckExistanceOfFile([FromBody] string data,string type)
        {
            var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, type+"s", data);


            return System.IO.File.Exists(physicalPath);
        }





    }


}
