using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyWeb.Models;
using MyWeb.Library;
using System.IO;

namespace MyWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private MyWebDBContext db = new MyWebDBContext();

        // GET: Admin/Product
        public ActionResult Index()
        {
            var list = db.Products
                .Join(
                db.Categorys,
                p => p.CatId,
                c => c.Id,
                (p,c) => new ProductCategory
                {
                    Id = p.Id,
                    CatId = p.CatId,
                    Name = p.Name,
                    Slug = p.Slug,
                    Detail = p.Detail,
                    Metakey = p.Metakey,
                    Metadesc = p.Metadesc,
                    Img = p.Img,
                    Number = p.Number,
                    Price = p.Price,
                    Pricesale = p.Pricesale,
                    CreateAt = p.CreateAt,
                    CreateBy = p.CreateBy,
                    UpdateAt = p.UpdateAt,
                    UpdateBy = p.UpdateBy,
                    Status = p.Status,
                    CatName = c.Name
                })
                .Where(m => m.Status != 0)
                .OrderByDescending(m => m.CreateAt)
                .ToList();
            return View(list);
        }

        public ActionResult Trash()
        {
            var list = db.Products
               .Where(m => m.Status == 0)
               .OrderByDescending(m => m.CreateAt)
               .ToList();
            return View(list);
        }
        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CatId,Name,Slug,Detail,Metakey,Metadesc,Img,Number,Price,Pricesale,CreateBy,CreateAt,UpdateBy,UpdateAt,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                string slug = XString.Str_Slug(product.Name);
                product.Slug = slug;
                product.CreateAt = DateTime.Now;
                product.CreateBy = int.Parse(Session["UserAdmin"].ToString());
                product.UpdateAt = DateTime.Now;
                product.UpdateBy = int.Parse(Session["UserAdmin"].ToString());
                // Hình ảnh
                var img = Request.Files["fileImg"];
                string[] fileExtention = { ".jpg", ".png", ".gif" };
                if(img.ContentLength != 0)
                {
                    if (fileExtention.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        // Upload file <slug>.<file_type>
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        // Lưu vào CSDL
                        product.Img = imgName;
                        string pathImg = Path.Combine(Server.MapPath("~/Public/images/Product/"), imgName);
                        // Lưu vào file lên Server
                        img.SaveAs(pathImg);
                    }
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CatId,Name,Slug,Detail,Metakey,Metadesc,Img,Number,Price,Pricesale,CreateBy,CreateAt,UpdateBy,UpdateAt,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                string slug = XString.Str_Slug(product.Name);
                product.Slug = slug;
                product.UpdateAt = DateTime.Now;
                product.UpdateBy = int.Parse(Session["UserAdmin"].ToString());
                // Hình ảnh
                var img = Request.Files["fileImg"];
                string[] fileExtention = { ".jpg", ".png", ".gif" };
                if (img.ContentLength != 0)
                {
                    if (fileExtention.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        // Upload file <slug>.<file_type>
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        // Xoá hình
                        if(product.Img != null)
                        {
                            string deletePath = Path.Combine(Server.MapPath("~/Public/images/Product/"), product.Img);
                            if (System.IO.File.Exists(deletePath))
                            {
                                System.IO.File.Delete(deletePath);
                            }
                        }
                        // Lưu vào CSDL
                        product.Img = imgName;
                        string pathImg = Path.Combine(Server.MapPath("~/Public/images/Product/"), imgName);
                        // Lưu vào file lên Server
                        img.SaveAs(pathImg);
                    }
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Thay đổi trạng thái 2->1, 1-> 2
        public ActionResult Status(int id)
        {
            Product product = db.Products.Find(id);
            product.Status = (product.Status == 1) ? 2 : 1;
            product.UpdateBy = int.Parse(Session["UserAdmin"].ToString());
            product.UpdateAt = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Thay đổi trạng thái=0 (Xóa vào thùng rác)
        public ActionResult DelTrash(int id)
        {
            Product product = db.Products.Find(id);
            product.Status = 0;
            product.UpdateBy = int.Parse(Session["UserAdmin"].ToString());
            product.UpdateAt = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Khôi phục mẫu tin (Trạng thái = 2)
        public ActionResult Restore(int id)
        {
            Product product = db.Products.Find(id);
            product.Status = 2;
            product.UpdateBy = int.Parse(Session["UserAdmin"].ToString());
            product.UpdateAt = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash");
        }
    }
}
