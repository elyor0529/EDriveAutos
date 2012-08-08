using System.Linq;
using System.Web.Mvc;
using $rootnamespace$.Core.Model;
using $rootnamespace$.Core.Interfaces.Service;

namespace $rootnamespace$.Controllers
{
    public abstract partial class BaseController<T> : Controller where T : PersistentEntity
    {
        protected IService<T> Service;

        #region JSON

        public virtual JsonResult GetAll()
        {
            var json = this.Service.GetAll().ToList().Select(o => o.Serialize()).ToList();
            return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public virtual JsonResult GetById(int id)
        {
            return new JsonResult { Data = this.Service.GetById(id).Serialize(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public virtual JsonResult SaveOrUpdate(T entity)
        {
            var result = this.Service.SaveOrUpdate(entity);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public virtual JsonResult Delete(int id)
        {
            var entity = this.Service.GetById(id);
            this.Service.Delete(entity);
            return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public virtual JsonResult Page(int page, int pageSize)
        {
            var p = this.Service.Page(page, pageSize);
            var jsonPage = new { p.Count, p.CurrentPage, p.PagesCount, p.PageSize, Entities = p.Entities.ToList().Select(o => o.Serialize()).ToList() };
            return new JsonResult { Data = jsonPage, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public virtual ViewResult SinglePage()
        {
            return View();
        }

        #endregion
    }
}