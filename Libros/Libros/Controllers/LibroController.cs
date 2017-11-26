using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Libros.Models;

namespace Libros.Controllers
{
    public class LibroController : Controller
    {
        private LibrosEntities db = new LibrosEntities();

        // GET: /Libro/
        public ActionResult Index()
        {
            return View(db.Libro.ToList());
        }

        // GET: /Libro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libro.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // GET: /Libro/Create
        public ActionResult Create()
        {
            List<Autor> autores = new List<Autor>();
            autores = db.Autor.ToList();
            ViewBag.AllAutores = autores;
            return View();
        }

        // POST: /Libro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Titulo,FechaEdicion")] Libro libro, int[] ids)
        {
            if (ModelState.IsValid)
            {
                db.Libro.Add(libro);
                db.SaveChanges();
                int idLibro = libro.IdLibro;

                foreach (var autorId in ids)
                {
                    Autor autor = new Autor();
                    autor = db.Autor.FirstOrDefault(a => a.IdAutor == autorId);
                    AutorLibro autorLibro = new AutorLibro();
                    autorLibro.Autor = autor;
                    autorLibro.Libro = libro;
                    db.AutorLibro.Add(autorLibro);
                    db.SaveChanges();
                }

                
                return RedirectToAction("Index");
            }

            else
            {
                List<Autor> autores = new List<Autor>();
                autores = db.Autor.ToList();
                ViewBag.AllAutores = autores;
                return View(libro);
            }
        }

        // GET: /Libro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libro.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // POST: /Libro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="IdLibro,Titulo,FechaEdicion")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(libro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(libro);
        }

        // GET: /Libro/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libro.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // POST: /Libro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Libro libro = db.Libro.Find(id);
            db.Libro.Remove(libro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
