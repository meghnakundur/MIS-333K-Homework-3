//Name: Meghna Kundur
//Date: 10/15/2022
//Description: HW3 – Seeding and Search

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kundur_Meghna_HW3.DAL;
using Kundur_Meghna_HW3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Globalization;

namespace Kundur_Meghna_HW3.Controllers

{ 
    public class HomeController : Controller
    {
        //create an instance of the db_context
        private AppDbContext _context;

        //create the constructor so that we get an instance of AppDbContext
        public HomeController(AppDbContext dbContext)
        {
            _context = dbContext;
        }
        // GET: Home
        // query the database for all the repositories and pass the results of the query to the Index view
        public IActionResult Index(string SearchString)
        {
            var query = from r in _context.Repositories
                        select r;

            if (SearchString != null)
                { 
            query = query.Where(r => r.RepositoryName.Contains(SearchString) ||
                                     r.Description.Contains(SearchString));
                }

            List<Repository> SelectedRepositories = query.Include(r => r.Language).ToList();

            //Populate the view bag with a count of all repositories 
            ViewBag.AllRepositories = _context.Repositories.Count();

            //Populate the view bag with a count of selected repositories 
            ViewBag.SelectedRepositories = SelectedRepositories.Count;

            return View(SelectedRepositories.OrderByDescending(r => r.StarCount));

        }

        //detail action that will pull the information for a single repository from the database and pass it to the view
        public IActionResult Details(int? id)//id is the id of the repo you want to see 
        {
            if (id == null) //RepositoryID not specified 
            {
                //user did not specify a RepositoryID – take them to the error view 
                return View("Error", new String[] { "RepositoryID not specified - which repository do you want to view ? " }); 
            }

            //look up the repo in the database based on the id; 
            // be sure to include the language 
            Repository repository = _context.Repositories.Include(b => b.Language)
                                            .FirstOrDefault(b => b.RepositoryID == id);

            if (repository == null) //No repository with this id exists in the database 
            {
                //there is not a repository with this id in the database – show an error view
                return View("Error", new String[] { "Repository not found in database" });
            }

            //if code gets this far, all is well – display the details 
            return View(repository);
        }

        public IActionResult DetailedSearch()
        {
            ViewBag.AllLanguages = GetAllLanguagesSelectList();

            SearchViewModel svm = new SearchViewModel();

            return View();

        }

        public IActionResult DisplaySearchResults(SearchViewModel svm)
            {
                var query = from r in _context.Repositories
                            select r;

            //query for title search
                if (svm.RepositoryName != null && svm.RepositoryName != "")
                {
                    query = query.Where(r => r.RepositoryName.Contains(svm.RepositoryName));
                }

            //query for description search
                  if (svm.Description != null && svm.Description != "")
                   {
                       query = query.Where(r => r.Description.Contains(svm.Description));
                   }

           //query for category search
                 if (svm.Category != null)
                 {
                     query = query.Where(r => r.Category == svm.Category);
                 }

          //query for language search
            if (svm.Language != 0)
            {
                Language LanguageDisplay = _context.Languages.Find(svm.Language);
                query = query.Where(r => r.Language.Equals(LanguageDisplay));
            }

           //query for star count search
                 if (svm.StarCount != null)
                   {
                       switch (svm.StarOrder)
                      {
                          case StarOrder.greaterThan:
                              query = query.Where(r => r.StarCount >= svm.StarCount);
                              break;
                          case StarOrder.lessThan:
                              query = query.Where(r => r.StarCount <= svm.StarCount);
                              break;

                    } 

                  }


            //query for date search
              if (svm.UpdatedAfter != null)
                     {

                         query = query.Where(r => r.LastUpdate >= svm.UpdatedAfter);
                     }
 
            List<Repository> SelectedRepositories = query.Include(r => r.Language).ToList();

            //Populate the view bag with a count of all repositories 
            ViewBag.AllRepositories = _context.Repositories.Count();

            //Populate the view bag with a count of selected repositories 
            ViewBag.SelectedRepositories = SelectedRepositories.Count;

            return View("Index", SelectedRepositories.OrderByDescending(r => r.StarCount));
        }

        private SelectList GetAllLanguagesSelectList()
        {
            //Get the list of languages from the database
            List<Language> languageList = _context.Languages.ToList();

            //add a dummy entry so the user can select all months
            Language SelectNone = new Language() { LanguageID = 0, LanguageName = "All Languages" };
            languageList.Add(SelectNone);

            //convert the list to a SelectList by calling SelectList constructor
            SelectList languageSelectList = new SelectList(languageList.OrderBy(m => m.LanguageID), "LanguageID", "LanguageName");

            //return the selectList
            return languageSelectList;
        }
    }
}

