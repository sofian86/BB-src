using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BadmintonBuddy.Models;

namespace BadmintonBuddy.Controllers
{
    public class BadmintonController : Controller
    {
        BadmintonBuddyRepository repository = new BadmintonBuddyRepository();

        [OutputCache(CacheProfile = "CacheSearch")]
        [HttpGet]
        public ActionResult Search(string q)
        {
            List<SearchClubViewModel> clubList = null;
            string title = String.Empty;
            string searchFilters = String.Empty;
            string cityName = String.Empty;
            if (!String.IsNullOrEmpty(q))
            {
                clubList = repository.GetClubs(q.Trim());
                if (clubList != null && clubList.Count > 0)
                {
                    foreach (var searchClub in clubList)
                    {
                        if (!title.Contains(searchClub.club.City.CityName))
                        {
                            title = title + searchClub.club.City.CityName;
                        }
                        string[] areaSplit = searchClub.club.Area.Split(',');
                        foreach (var area in areaSplit)
                        {
                            if (!title.Contains(area))
                            {
                                title = title + " ," + area;
                            }

                            if (!searchFilters.Contains(area))
                            {
                                searchFilters = searchFilters + " ," + area;
                            }
                        }
                    }
                    
                }
                cityName = clubList.Last().club.City.CityName;
            }
            
            ViewBag.Title = "Badminton Courts in " + title;
            ViewBag.Searchterm = q;
            ViewBag.SearchFilters = searchFilters;
            ViewBag.CityName = cityName;
            return View(clubList);
        }

        //[HttpPost]
        //public ActionResult Search()
        //{
            
        //}

        [HttpGet]
        public ActionResult AddClub()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddClub(ClubViewModel newClub)
        {
            Club newClubAdded = repository.AddNewClub(newClub);
            ViewBag.ClubId = newClubAdded.ClubID.ToString();
            return View("AddMeta");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">This is clubId</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditClub(string name)
        {
            Club club = repository.GetClub(Int32.Parse(name));            
            ClubViewModel viewModel = new ClubViewModel();
            viewModel = ClubToClubView(club);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditClub(ClubViewModel editedClub)
        {
            if (ModelState.IsValid)
            {
                repository.EditClub(editedClub);

            }
            ViewBag.Message = "Club Edited";
            return View("Message");
        }

        //public ActionResult DeleteClub(int name)
        //{
        //    repository.DeleteClub(name);
        //    ViewBag.Message = "Club Deleted with ID: " + name + " Successfully"; ;
        //    return View("Message");
        //}

        private ClubViewModel ClubToClubView(Club clubToConvert)
        {
            ClubViewModel viewModel = new ClubViewModel();
            viewModel.Club = clubToConvert;
            viewModel.City = clubToConvert.City.CityName;
            viewModel.State = clubToConvert.City.State.StateName;
            viewModel.Country = clubToConvert.City.State.Country.CountryName;
            return viewModel;
        }


        
        public int AdditionalClub(AdditionalClubInfo info)
        {
            if (ModelState.IsValid)
            {
                info.isRead = false;
                return repository.AdditionalClub(info);
            }
            return 0;
        }

        [HttpPost]
        public Int32 AddMessage(Message message)
        {
            if (ModelState.IsValid)
            {
                return repository.AddMessage(message);
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">This is clubid</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddMeta(string name)
        {
            ViewBag.ClubId = name;
            return View();
        }

        [HttpPost]
        public ActionResult AddMeta(Metadata metaData)
        {
            if (ModelState.IsValid)
            {
                repository.AddMetadata(metaData);
                ViewBag.ClubId = metaData.ClubID;                
                return View("AddMeta");
            }
            else
            {
                ViewBag.ClubId = "Error";
                return View("AddMeta");
            }            

        }

        //public ActionResult Courts()
        //{
        //    var clubs = repository.AllClubs();
        //    return View(clubs);
        //}
        [OutputCache(CacheProfile="CacheAll")]
        public ActionResult Courts()
        {
            Dictionary<string,int> countryToClubCount = repository.GetCountryofClubs();
            Dictionary<string, string> clubCities = repository.GetClubCity();
            ViewData["CountriesCount"] = countryToClubCount;
            ViewData["CountryCity"] = clubCities;
            return View();
        }

        [OutputCache(CacheProfile = "CacheCourt")]
        public ActionResult Court(string name,string id)
        {
            var club = new Club();
            if (id != null)
            {
                int clubId = Int32.Parse(id);
                club = repository.GetClub(clubId);

                ViewBag.MapURL = repository.GetClubMapURL(clubId);
                List<Metadata> imageList = repository.GetClubImageURL(clubId);
                for (int i = 0; i < imageList.Count; i++)
                {
                    string viewBagName = "ImageURL" + i.ToString();
                    //string imgPath = "/Images/Club/" + clubId.ToString() + "/" + imageList[i].MetadataValue;
                    ViewData.Add(viewBagName,imageList[i].MetadataValue);
                }
            }
            ViewBag.Title = club.ClubName + ", Badminton Court, " + club.Area + " ," + club.City.CityName;
            ViewBag.MetaDescription = club.ClubName + ", Badminton Court, " + club.Area + " ," + club.City.CityName;
            ViewBag.MetaKeywords = club.ClubName+" , " + "badminton court "+club.Area+" , "+ "badminton court "+club.City.CityName;

            return View(club);
        }

        [OutputCache(CacheProfile="CacheAll")]
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult News()
        {
            return View();
        }

    }
}
