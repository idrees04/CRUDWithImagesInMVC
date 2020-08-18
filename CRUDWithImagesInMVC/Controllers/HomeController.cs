using CRUDWithImagesInMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUDWithImagesInMVC.Controllers
{
    public class HomeController : Controller
    {
        CRUDImagesEntities db = new CRUDImagesEntities();
        // GET: Home
        public ActionResult Index()
        {
            var data = db.employees.ToList();
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(employee e)
        {
            if (ModelState.IsValid == true)
            {
                string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                string extension = Path.GetExtension(e.ImageFile.FileName);

                // when i will click to submit (after choose file) which file will be post HttpPstedFileBase class will handle that file
                HttpPostedFileBase postedFile = e.ImageFile;
                //now i am saving size of file in byte in varyable lenght
                int length = postedFile.ContentLength;
                if (extension.ToLower() == ".jpg"|| extension.ToLower() == ".png"||extension.ToLower() == ".jpeg")
                {
                    if(length<=1000000)
                    {
                        fileName = fileName + extension;
                        e.image_path = "~/images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/images/"),fileName);
                        e.ImageFile.SaveAs(fileName);
                        db.employees.Add(e);

                        int a = db.SaveChanges();
                        if(a>0)
                        {
                            TempData["CreateMessage"] = "<script>alert('Data Inserted Successfully')</script>";
                            ModelState.Clear();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["CreateMessage"] = "<script>alert('Data Not Inserted')</script>";
                        }
                    }
                    else
                    {
                        TempData["SizeMessage"] = "<script>alert('Image Size should be Less then 1MB')</script>";
                    }
                }
                else
                {
                    TempData["ExtensionMessage"] = "<script>alert('Format Not Supported')</script>";
                }
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var EmployeeRow = db.employees.Where(model => model.id == id).FirstOrDefault();
            Session["Image"] = EmployeeRow.image_path;
            return View(EmployeeRow);
        }
        [HttpPost]
        public ActionResult Edit(employee e)
        {
            if(ModelState.IsValid==true)
            {
                if(e.ImageFile!=null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                    string extension = Path.GetExtension(e.ImageFile.FileName);

                    // when i will click to submit (after choose file) which file will be post HttpPstedFileBase class will handle that file
                    HttpPostedFileBase postedFile = e.ImageFile;
                    //now i am saving size of file in byte in varyable lenght
                    int length = postedFile.ContentLength;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                    {
                        if (length <= 1000000)
                        {
                            fileName = fileName + extension;
                            e.image_path = "~/images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                            e.ImageFile.SaveAs(fileName);

                            db.Entry(e).State = EntityState.Modified;

                            int a = db.SaveChanges();
                            if (a > 0)
                            {
                                TempData["UpdateMessage"] = "<script>alert('Data Updated Successfully')</script>";
                                ModelState.Clear();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["UpdateMessage"] = "<script>alert('Data Not Updated')</script>";
                            }
                        }
                        else
                        {
                            TempData["SizeMessage"] = "<script>alert('Image Size should be Less then 1MB')</script>";
                        }
                    }
                    else
                    {
                        TempData["ExtensionMessage"] = "<script>alert('Format Not Supported')</script>";
                    }
                }
                else
                {
                    e.image_path = Session["Image"].ToString();
                    db.Entry(e).State = EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["UpdateMessage"] = "<script>alert('Data Updated Successfully')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["UpdateMessage"] = "<script>alert('Data Not Updated')</script>";
                    }
                }
            }
            
            return View();
        }
    }
}