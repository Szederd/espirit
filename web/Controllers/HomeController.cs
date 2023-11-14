using SlowDataSource.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webapp.ToolBox;

namespace webapp.Controllers
{
    public class HomeController : Controller
    {
        private const int PAGESIZE = 10;
        private AdvancedCache _advancedCache = new AdvancedCache();

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Szűri és rendezi az elemeket a kapott paramétereknek megfelelően
        /// </summary>
        /// <param name="model">Szűrőparaméter input</param>
        /// <returns>A keresésnek megfelelő html</returns>
        public ActionResult LoadListPartial(Models.ListFilterModel model)
        {
            
            //dinamikus linq inicializálása
            var allItems = AdvancedCache.Current.Get<IEnumerable<SampleData>>("1", () => {
                SlowDataSource.DataSource src = new SlowDataSource.DataSource();
                return src.FetchAll().Where(x => true);
            }, 60);

            if (model.SearchVal != null) {
                allItems = allItems.Where(s => s.TextData.Contains(model.SearchVal));
            }

            if (model.SortOrder > 0)
            {
                if (model.SortOrder == Models.SortOrderEnum.Asc)
                {
                    if (model.SortCol == "text")
                    {
                        allItems = allItems.OrderBy(s => s.TextData);
                    }
                    else if (model.SortCol == "mgd")
                    {
                        allItems = allItems.OrderBy(s => s.MissingGroupingData);
                    }
                    else
                    {
                        allItems = allItems.OrderBy(s => s.NumericData);
                    }
                }
                else if (model.SortOrder == Models.SortOrderEnum.Desc)
                {
                    if (model.SortCol == "text")
                    {
                        allItems = allItems.OrderByDescending(s => s.TextData);
                    }
                    else if (model.SortCol == "mgd")
                    {
                        allItems = allItems.OrderByDescending(s => s.MissingGroupingData);
                    }
                    else
                    {
                        allItems = allItems.OrderByDescending(s => s.NumericData);
                    }
                }
            }

            //Fix
            model.PageSize = PAGESIZE;
            model.AllCount = allItems.Count();
            
            //Paging 'logic'
            model.Items = allItems.Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList();

            return PartialView(model);
        }

        
    }
}