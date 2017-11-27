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
            List<Libro> libros = new List<Libro>();
            libros = db.Libro.ToList();

            foreach(Libro libro in libros)
            {
                libro.Autores = db.AutorLibro.Where(al => al.IdLibro == libro.IdLibro).Count();
            }

            return View(libros);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Titulo,FechaEdicion")] Libro libro, int[] idsAutores)
        {
            if (ModelState.IsValid && idsAutores != null)
            {
                db.Libro.Add(libro);
                db.SaveChanges();
                int idLibro = libro.IdLibro;

                foreach (var autorId in idsAutores)
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

            List<Autor> autores = new List<Autor>();
            autores = db.Autor.ToList();
            foreach (Autor autor in autores)
            {
                bool selected = db.AutorLibro.Any(a => a.IdAutor == autor.IdAutor && a.IdLibro == id);
                autor.Selected = selected;
            }
            ViewBag.AllAutores = autores;


            return View(libro);
        }

        // POST: /Libro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdLibro,Titulo,FechaEdicion")] Libro libro, int[] idsAutores)
        {
            if (ModelState.IsValid && idsAutores != null)
            {
                db.Entry(libro).State = EntityState.Modified;
                db.SaveChanges();

                List<AutorLibro> actualList = new List<AutorLibro>();
                actualList = db.AutorLibro.Where(a => a.IdLibro == libro.IdLibro).Select(al => al).ToList();

                foreach (AutorLibro actualLibro in actualList)
                {
                    db.AutorLibro.Remove(actualLibro);
                }

                foreach (var autorId in idsAutores)
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

        
        public ActionResult Search(string titulo, DateTime? edicion, string autor)
        {
            int numAutores = autor!=""? Convert.ToInt32(autor) : 0;
            List<Libro> libros = new List<Libro>();


            libros = db.Libro.Where(l => l.Titulo.Contains(titulo) || l.FechaEdicion == edicion || l.AutorLibro.Count() == numAutores).ToList();

            foreach (Libro libro in libros)
            {
                libro.Autores = db.AutorLibro.Where(al => al.IdLibro == libro.IdLibro).Count();
            }

            return View("Index",libros);
        }
    }
}
