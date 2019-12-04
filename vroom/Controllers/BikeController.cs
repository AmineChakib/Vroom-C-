using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vroom.AppDbContext;
using vroom.Models;
using vroom.Models.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using vroom.Helpers;

namespace vroom.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Executive)]
    public class BikeController : Controller
    {
        private readonly VroomDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;

        [BindProperty]
        public BikeViewModel BikeVM { get; set; }

        public BikeController(VroomDbContext db,HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            BikeVM = new BikeViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Bike=new Models.Bike(),
            };
        }
        public IActionResult Index2()
        {
            var Bikes = _db.Bikes.Include(m => m.Make).Include(m => m.Model);
            return View(Bikes.ToList());
        }

        public IActionResult Index(int pageNumber=1,int pageSize=3)
        {
            int ExcludeRecords = (pageNumber * pageSize) - pageSize;
            var Bikes = _db.Bikes.Include(m => m.Make).Include(m => m.Model)
                .Skip(ExcludeRecords)
                .Take(pageSize);

            var result = new PagedResult<Bike>
            {
                Data = Bikes.AsNoTracking().ToList(),
                TotalItems = _db.Bikes.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            return View(result);
        }

        //Get Method
        public IActionResult Create()
        {
            return View(BikeVM);
        }
        //Post Method
        [HttpPost, ActionName("Create")]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                BikeVM.Makes = _db.Makes.ToList();
                BikeVM.Models = _db.Models.ToList();
                return View(BikeVM);
            }
            if (BikeVM.Bike.ModelID == 0)
            {
                ViewBag.Message = "Please select a Model from the list";
                return View(BikeVM);
            }
            _db.Bikes.Add(BikeVM.Bike);
            UploadImage();
            _db.SaveChanges();   
            return RedirectToAction(nameof(Index));
        }
        //public IActionResult Edit(int id)
        //{
        //    ModelVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
        //    if (ModelVM.Model == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ModelVM);
        //}
        //[HttpPost, ActionName("Edit")]
        //public IActionResult EditPost()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(ModelVM);
        //    }
        //    _db.Update(ModelVM.Model);
        //    _db.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            BikeVM.Bike = _db.Bikes.SingleOrDefault(b => b.Id == id);
            //Filter the models associated to the selected make
            BikeVM.Models = _db.Models.Where(m => m.MakeID == BikeVM.Bike.MakeID);
            if (BikeVM.Bike == null)
            {
                return NotFound();
            }
            return View(BikeVM);
        }

        //Post Method
        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost()
        {
            if (!ModelState.IsValid)
            {
                BikeVM.Makes = _db.Makes.ToList();
                BikeVM.Models = _db.Models.ToList();
                return View(BikeVM);
            }
            if (BikeVM.Bike.ModelID == 0)
            {
                ViewBag.Message = "Please select a Model from the list";
                return View(BikeVM);
            }
            _db.Bikes.Update(BikeVM.Bike);
            UploadImage();
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Bike bike = _db.Bikes.Find(id);
            if (bike == null)
            {
                return NotFound();
            }
            _db.Bikes.Remove(bike);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private void UploadImage()
        {
            //Get BikeID I have saved in database
            var BikeID = BikeVM.Bike.Id;

            //Get wwwroot path to save the file on server 
            string wwwrootPath = _hostingEnvironment.WebRootPath;
            //Get the aploaded files
            var files = HttpContext.Request.Form.Files;

            var SavedBike = _db.Bikes.Find(BikeID);

            if (files.Count != 0)
            {
                var ImagePath = @"images\bike\";
                var Extension = Path.GetExtension(files[0].FileName);
                var RelativeImagePath = ImagePath + BikeID + Extension;
                var AbsImagePath = Path.Combine(wwwrootPath, RelativeImagePath);
                //Upload the file on server
                using (var fileStream = new FileStream(AbsImagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                //Set the image path on database
                SavedBike.ImagePath = RelativeImagePath;

            }
        }
    }
}