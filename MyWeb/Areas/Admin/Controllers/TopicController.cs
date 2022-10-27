using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyWeb.Models;

namespace MyWeb.Areas.Admin.Controllers
{
    public class TopicController : Controller
    {
        private MyWebDBContext db = new MyWebDBContext();

        // GET: Admin/Topic
        public ActionResult Index()
        {
            var list = db.Topics
                .Where(m => m.Status != 0)
                .OrderByDescending(m => m.CreateAt)
                .ToList();
            return View(list);
        }

        public ActionResult Trash()
        {
            var list = db.Topics
               .Where(m => m.Status == 0)
               .OrderByDescending(m => m.CreateAt)
               .ToList();
            return View(list);
        }

        // GET: Admin/Topic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Admin/Topic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Topic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Slug,ParentId,Orders,Metakey,Metadesc,CreateBy,CreateAt,UpdateBy,UpdateAt,Status")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Topics.Add(topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(topic);
        }

        // GET: Admin/Topic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Admin/Topic/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Slug,ParentId,Orders,Metakey,Metadesc,CreateBy,CreateAt,UpdateBy,UpdateAt,Status")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        // GET: Admin/Topic/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Admin/Topic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // Thay đổi trạng thái 2->1, 1-> 2
        public ActionResult Status(int id)
        {
            Topic category = db.Topics.Find(id);
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
            Topic category = db.Topics.Find(id);
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
            Topic category = db.Topics.Find(id);
            category.Status = 2;
            category.UpdateBy = 1;
            category.UpdateAt = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash");
        }
    }
}
