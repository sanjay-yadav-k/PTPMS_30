using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineClass.Controllers
{
    public class OnlineclassesController : Controller
    {
        // GET: Onlineclasses
        public ActionResult Index()
        {
            return View();
        }

        // GET: Onlineclasses/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Onlineclasses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Onlineclasses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Onlineclasses/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Onlineclasses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Onlineclasses/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Onlineclasses/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}