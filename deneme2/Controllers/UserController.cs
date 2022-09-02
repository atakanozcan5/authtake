using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using deneme2.Models;
using System.ComponentModel;

namespace deneme2.Controllers
{
    public class UserController : Controller
    {
        private AuthtakeEntities1 db = new AuthtakeEntities1();

        public async Task<ActionResult> Index()
        {
            return View(await db.User.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserName,Password,Mail,Name,Surname")] User user)
        {
            var userList = await db.User.ToListAsync();
            if (!userList.Any(u => u.UserName == user.UserName))
            {
                if (ModelState.IsValid)
                {
                    db.User.Add(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.message = "Böyle bir kullanıcı mevcut!";
            }
            return View(user);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,Password,Mail,Name,Surname")] User user)
        {
            var userList = await db.User.ToListAsync();
            List<User> userCopy = new List<User>(userList);
            var deletedList = userList.Remove(userList.Where(x=>x.Id == user.Id).FirstOrDefault());
            if(!userList.Any(u=>u.UserName == user.UserName))
            {
                if (ModelState.IsValid)
                {
                    var userFromDb = userCopy.Where(x => x.Id == user.Id).FirstOrDefault();
                    userFromDb.UserName = user.UserName;
                    userFromDb.Password = user.Password;
                    userFromDb.Mail = user.Mail;
                    userFromDb.Name = user.Name;
                    userFromDb.Surname = user.Surname;
                    db.Entry(userFromDb).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.message = "Böyle bir kullanıcı adı mevcut!";
            }
            return View(user);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.User.FindAsync(id);
            db.User.Remove(user);
            await db.SaveChangesAsync();
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
