using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MyWeb.Models;
using MyWeb.Library;

namespace MyWeb.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private MyWebDBContext db = new MyWebDBContext();

        // GET: Admin/Category
        public ActionResult Index()
        {
            var list = db.Categorys
                .Where(m => m.Status != 0)
                .OrderByDescending(m => m.CreateAt)
                .ToList();
            return View(list);
        }

        public ActionResult Trash()
        {
            var list = db.Categorys
               .Where(m => m.Status == 0)
               .OrderByDescending(m => m.CreateAt)
               .ToList();
            return View(list);
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorys.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(db.Categorys.ToList(), "Orders", "Name", 0);
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Slug,ParentId,Orders,Metakey,Metadesc,CreateBy,CreateAt,UpdateBy,UpdateAt,Status")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentId == null)
                    category.ParentId = 0;
                string slug = XString.Str_Slug(category.Name);
                category.Slug = slug;
                category.CreateAt = DateTime.Now;
                category.CreateBy = int.Parse(Session["UserAdmin"].ToString());
                category.UpdateAt = DateTime.Now;
                category.UpdateBy = int.Parse(Session["UserAdmin"].ToString());
                db.Categorys.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(db.Categorys.ToList(), "Orders", "Name", 0);
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorys.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(db.Categorys.ToList(), "Orders", "Name", 0);
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Slug,ParentId,Orders,Metakey,Metadesc,CreateBy,CreateAt,UpdateBy,UpdateAt,Status")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentId == null)
                    category.ParentId = 0;
                string slug = XString.Str_Slug(category.Name);
                category.Slug = slug;
                category.UpdateAt = DateTime.Now;
                category.UpdateBy = int.Parse(Session["UserAdmin"].ToString());
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(db.Categorys.ToList(), "Orders", "Name", 0);
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorys.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categorys.Find(id);
            db.Categorys.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Trash");
        }

        // Thay đổi trạng thái 2->1, 1-> 2
        public ActionResult Status(int id)
        {
            Category category = db.Categorys.Find(id);
            category.Status = (category.Status == 1) ? 2 : 1;
            category.UpdateBy = 1;
            category.UpdateAt = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Thay đổi trạng thái=0 (Xóa vào thùng rác)
        public ActionResult DelTrash(int id)
        {
            Category category = db.Categorys.Find(id);
            category.Status = 0;
            category.UpdateBy = 1;
            category.UpdateAt = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Khôi phục mẫu tin (Trạng thái = 2)
        public ActionResult Restore(int id)
        {
            Category category = db.Categorys.Find(id);
            category.Status = 2;
            category.UpdateBy = 1;
            category.UpdateAt = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash");
        }        
    }
}
