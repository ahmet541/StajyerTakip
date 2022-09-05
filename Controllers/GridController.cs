using Kendo.Mvc.UI;
using StajyerTakip.Data;
using StajyerTakip.Models;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.Extensions;
using Microsoft.EntityFrameworkCore;

namespace StajyerTakip.Controllers
{
    public class GridController : Controller
    {
        private readonly StajyerContext _context;
        private readonly UploadController _uploadController;
        public GridController(StajyerContext context, UploadController uploadController)
        {

            _context = context;
            _uploadController = uploadController;
        }


        public async Task<IActionResult> ReadOrdersAsync([DataSourceRequest] DataSourceRequest request)
        {


            List<StajyerModel> stajyers = await _context.ListOfStajyers.ToListAsync();
            await UpdateStatesOfAllStajyersAsync(stajyers);
            var dsResult = stajyers.ToDataSourceResult(request);
            return Json(dsResult);
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> CreateOrders([DataSourceRequest] DataSourceRequest request, [Bind("Name,IdentificationNumber,StartingDate,EndingDate")] StajyerModel userModel)
        {
            userModel.LocationOfCV = "";
            userModel.LocationOfPicture = "defaultPictureForStajyer.jpg";
            if (ModelState.IsValid) 
            {
                userModel.Id = 0;
                _context.Add(userModel);
                _context.ListOfStajyers.Add(userModel);

                await _context.SaveChangesAsync();

            }

            return Json(new[] { userModel }.ToDataSourceResult(request, ModelState));
        }


        public async Task<IActionResult> UpdateOrders([DataSourceRequest] DataSourceRequest request, StajyerModel userModel)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(_context.ListOfStajyers?.Any(e => e.Id == userModel.Id)).GetValueOrDefault())
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
                return Json(new[] { userModel }.ToDataSourceResult(request, ModelState));
        }

        // destroyorders


        public async Task<IActionResult> DestroyOrders([DataSourceRequest] DataSourceRequest request, StajyerModel userModel)
        {
            int id = userModel.Id;
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

            _uploadController.Single_File_Remove(stajyerModel.LocationOfPicture,"Pictures");
            _uploadController.Single_File_Remove(stajyerModel.LocationOfCV, "CVs");
            return Json(new[] { userModel }.ToDataSourceResult(request));

        }

      
        private bool StajyerModelExists(int id)
        {
            return (_context.ListOfStajyers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task UpdateStatesOfAllStajyersAsync(List<StajyerModel> stajyers)
        {
            DateTime today = DateTime.Now;
            DateTime start;
            DateTime end;
            foreach (var stajyer in stajyers)
            {
                start = stajyer.StartingDate;
                end = stajyer.EndingDate;
                if(stajyer.State == Status.ihlal)
                {
                    // do nothing // instead of adding everthing inside if clause, I think this is neat
                }
                else if (DateTime.Compare(today, start) < 0) // have not started yet
                {
                    stajyer.State = Status.başlamadı;
                }
                else if (DateTime.Compare(today, end) > 0) //  staj done
                {
                    stajyer.State = Status.bitirdi;
                }
                else
                {
                    stajyer.State = Status.devam_ediyor;
                }
                _context.Update(stajyer);
                await _context.SaveChangesAsync();
            }
        }

    }

}
