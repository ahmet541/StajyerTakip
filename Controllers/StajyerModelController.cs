using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StajyerTakip.Data;
using StajyerTakip.Models;

namespace StajyerTakip.Controllers
{
    public class StajyerModelController : Controller
    {
        private readonly StajyerContext _context;
        private readonly UploadController _uploadController;
        public StajyerModelController(StajyerContext context, UploadController uploadController)
        {
            _context = context;
            _uploadController = uploadController;
        }

        // GET: StajyerModels
        public async Task<IActionResult> Index()
        {

            return _context.ListOfStajyers != null ? 
                          View(await _context.ListOfStajyers.ToListAsync()) :
                          Problem("Entity set 'StajyerContext.ListOfStajyers'  is null.");
        }

        // GET: StajyerModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ListOfStajyers == null)
            {
                return NotFound();
            }

            var stajyerModel = await _context.ListOfStajyers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stajyerModel == null)
            {
                return NotFound();
            }

            return View(stajyerModel);
        }

        // GET: StajyerModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StajyerModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IdentificationNumber,StartingDate,EndingDate,LocationOfCV")] StajyerModel stajyerModel)
        {
            if (ModelState.IsValid)
            {
               stajyerModel.LocationOfCV = "";
               stajyerModel.LocationOfPicture = "defaultPictureForStajyer.jpg";


                _context.Add(stajyerModel);
                await _context.SaveChangesAsync();

                if ((string)TempData["Picture"] != null)
                {
                    stajyerModel.LocationOfPicture = renamePicture((string)TempData["Picture"], stajyerModel.Id);
                }
                
                if((string) TempData["CV"] != null){
                    stajyerModel.LocationOfCV = renameCv((string)TempData["CV"],stajyerModel.Id);
                }


                _context.Update(stajyerModel);
                    _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(stajyerModel);
        }

        // GET: StajyerModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ListOfStajyers == null)
            {
                return NotFound();
            }

            var stajyerModel = await _context.ListOfStajyers.FindAsync(id);
            if (stajyerModel == null)
            {
                return NotFound();
            }
            return View(stajyerModel);
        }

        // POST: StajyerModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IdentificationNumber,StartingDate,EndingDate,LocationOfCV")] StajyerModel stajyerModel)
        {
            if (id != stajyerModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stajyerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StajyerModelExists(stajyerModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(stajyerModel);
        }

        // GET: StajyerModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ListOfStajyers == null)
            {
                return NotFound();
            }

            var stajyerModel = await _context.ListOfStajyers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stajyerModel == null)
            {
                return NotFound();
            }

            return View(stajyerModel);
        }

        // POST: StajyerModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ListOfStajyers == null)
            {
                return Problem("Entity set 'StajyerContext.ListOfStajyers'  is null.");
            }
            var stajyerModel = await _context.ListOfStajyers.FindAsync(id);
            if (stajyerModel != null)
            {
                _context.ListOfStajyers.Remove(stajyerModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StajyerModelExists(int id)
        {
          return (_context.ListOfStajyers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public string renamePicture(string fileName, int stajyerId)
        {
            var supportedTypes = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            var fileExt = System.IO.Path.GetExtension(fileName).Substring(1);
            var newName = "pictureOfStajyer-" + stajyerId + "." + fileExt;
            _uploadController.File_Move(fileName,newName,"Pictures","Pictures");

            return  newName;
        }
        public async Task<ActionResult> Picture_Save(IEnumerable<IFormFile> pictures, int stajyerId)
        {
            var stajyerModel = _context.ListOfStajyers.Find(stajyerId);
            string fileExt;
            string nameOfFile = "";
            foreach (var file in pictures)
            {
                var supportedTypes = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                nameOfFile = "pictureOfStajyer-" + stajyerId + "." + fileExt;
            }

            if( stajyerModel.LocationOfPicture != "defaultPictureForStajyer.jpg")
            {
                _uploadController.Single_File_Remove(stajyerModel.LocationOfPicture, "Pictures");
            }

            stajyerModel.LocationOfPicture = nameOfFile;
            _context.Update(stajyerModel);
            _context.SaveChanges();
            return await _uploadController.File_Save(pictures, "Pictures", nameOfFile, true);
        }
        public ActionResult Picture_Remove(string[] fileNames)
        {
            return _uploadController.File_Remove(fileNames, "Pictures");
        }

        [HttpPost]
        public JsonResult UpdatePicture([FromBody]int data) 
        {
            int UpdatePictureId = data;
            if (_context.ListOfStajyers == null)
            {
                return Json("");
            }
            var stajyerModel =  _context.ListOfStajyers.Find(UpdatePictureId);

           

            return Json(stajyerModel.LocationOfPicture);

        }





        public string renameCv(string fileName, int stajyerId)
        {
            var supportedTypes = new[] { ".pdf" };
            var fileExt = System.IO.Path.GetExtension(fileName).Substring(1);
            var newName = "CvOfStajyer-" + stajyerId + "." + fileExt;
            _uploadController.File_Move(fileName, newName, "CVs", "CVs");

            return newName;
        }
        public async Task<ActionResult> Cv_Save(IEnumerable<IFormFile> cvs, int stajyerId)
        {
            var stajyerModel = _context.ListOfStajyers.Find(stajyerId);
            string fileExt;
            string nameOfFile = "";
            foreach (var file in cvs)
            {
                var supportedTypes = new[] { ".pdf" };
                fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                nameOfFile = "cvOfStajyer-" + stajyerId + "." + fileExt;
            }

            if (stajyerModel.LocationOfPicture != "")
            {
                _uploadController.Single_File_Remove(stajyerModel.LocationOfCV, "CVs");
            }

            stajyerModel.LocationOfCV = nameOfFile;
            _context.Update(stajyerModel);
            _context.SaveChanges();
            return await _uploadController.File_Save(cvs, "CVs", nameOfFile, true);
        }
        public ActionResult Cv_Remove(string[] fileNames)
        {
            return _uploadController.File_Remove(fileNames, "CVs");
        }
        [HttpPost]
        public JsonResult UpdateCv([FromBody] int data)
        {
            int UpdateCvId = data;
            if (_context.ListOfStajyers == null)
            {
                return Json("");
            }
            var stajyerModel = _context.ListOfStajyers.Find(UpdateCvId);



            return Json(stajyerModel.LocationOfCV);
        }

    }
}
