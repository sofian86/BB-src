using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BadmintonBuddy.Models
{
    public class BadmintonBuddyRepository
    {
        private BadmintonBuddyEntities entities;

        public BadmintonBuddyRepository()
        {
            entities = new BadmintonBuddyEntities();
        }

        public List<SearchClubViewModel> GetClubs(string searchterm)
        {
            List<SearchClubViewModel> searchClubList = new List<SearchClubViewModel>();
            var clubs = from c in entities.Clubs
                        where c.City.CityName.Contains(searchterm) || c.ClubName.Contains(searchterm) || c.Area.Contains(searchterm)
                        select c;
            foreach (var club in clubs)
            {
                SearchClubViewModel searchClub = new SearchClubViewModel();
                searchClub.club = club;
                var metadatas = from m in entities.Metadatas
                                where m.ClubID == club.ClubID
                                select m;
                searchClub.metaDataList = metadatas.ToList<Metadata>();
                searchClubList.Add(searchClub);
            }

            
            return searchClubList;           

        }

        public int AdditionalClub(AdditionalClubInfo info)
        {
            entities.AdditionalClubInfoes.AddObject(info);
            entities.SaveChanges();
            return info.ItemID;
        }

        public Club GetClub(int id)
        {
            var club = from c in entities.Clubs
                       where c.ClubID == id
                       select c;
            return club.First<Club>();
        }

        public string GetClubMapURL(int clubId)
        {
            var metadata = from m in entities.Metadatas
                           where m.ClubID == clubId && m.MetadataType == "Map"
                           select m;
            if (metadata != null)
            {
                return metadata.First<Metadata>().MetadataValue.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        public List<Metadata> GetClubImageURL(int clubId)
        {
            var imageList = from i in entities.Metadatas
                            where i.ClubID == clubId && i.MetadataType == "Image"
                            select i;
            return imageList.ToList<Metadata>();
        }

        public IQueryable<IGrouping<int,Club>> AllClubs()
        {
            var clubs = from c in entities.Clubs
                        orderby c.City.CityName
                        group c by c.City.CityID;
                        
            return clubs;
        }

        public Club AddNewClub(ClubViewModel newClubView)
        {
            int countryId = 0;
            if (!String.IsNullOrEmpty(newClubView.Country.Trim()))
            {
                countryId = AddCountry(newClubView.Country);
            }

            int stateId = 0;
            if (!String.IsNullOrEmpty(newClubView.State.Trim()))
            {
                stateId = AddState(newClubView.State, countryId);
            }

            int cityId = 0;
            if (!String.IsNullOrEmpty(newClubView.City.Trim()))
            {
                cityId = AddCity(newClubView.City, stateId);
            }
            newClubView.Club.CityID = cityId;
            entities.Clubs.AddObject(newClubView.Club);
            entities.SaveChanges();
            return newClubView.Club;
        }

        public Club EditClub(ClubViewModel editClubView)
        {
            int countryId = 0;
            if (!String.IsNullOrEmpty(editClubView.Country.Trim()))
            {
                countryId = AddCountry(editClubView.Country);
            }

            int stateId = 0;
            if (!String.IsNullOrEmpty(editClubView.State.Trim()))
            {
                stateId = AddState(editClubView.State, countryId);
            }

            int cityId = 0;
            if (!String.IsNullOrEmpty(editClubView.City.Trim()))
            {
                cityId = AddCity(editClubView.City, stateId);
            }

            editClubView.Club.CityID = cityId;
            entities.SaveChanges();
            return editClubView.Club;            
        }

        //public Club AddNewClub(ClubViewModel newClub)
        //{
        //    int countryId = 0;
        //    if (!String.IsNullOrEmpty(newClub.Country.Trim()))
        //    {
        //        countryId = AddCountry(newClub.Country);
        //    }

        //    int stateId = 0;
        //    if (!String.IsNullOrEmpty(newClub.State.Trim()))
        //    {
        //        stateId = AddState(newClub.State, countryId);
        //    }

        //    int cityId = 0;
        //    if (!String.IsNullOrEmpty(newClub.ClubName.Trim()))
        //    {
        //        cityId = AddCity(newClub.City, stateId);
        //    }

        //    Club newClubtoAdd = new Club();
        //    newClubtoAdd.CityID = cityId;

        //    if (!String.IsNullOrEmpty(newClub.ClubName))
        //    {
        //        newClubtoAdd.ClubName = newClub.ClubName.Trim();
        //    }
            
        //    if (!String.IsNullOrEmpty(newClub.Address))
        //    {
        //        newClubtoAdd.Address = newClub.Address.Trim();
        //    }
        //    if (!String.IsNullOrEmpty(newClub.AreaTag))
        //    {
        //        newClubtoAdd.Area = newClub.AreaTag.Trim();
        //    }
        //    if (!String.IsNullOrEmpty(newClub.Phone))
        //    {
        //        newClubtoAdd.ContactNo = newClub.Phone.Trim();
        //    }
        //    if (!String.IsNullOrEmpty(newClub.Fees))
        //    {
        //        newClubtoAdd.Fees = newClub.Fees.Trim();
        //    }
            
        //    newClubtoAdd.NoOfCourts = newClub.NoOfCourts;
        //    newClubtoAdd.SurfaceID = newClub.Surface;
        //    if (!String.IsNullOrEmpty(newClub.Timings))
        //    {
        //        newClubtoAdd.Timings = newClub.Timings.Trim();
        //    }
        //    if (!String.IsNullOrEmpty(newClub.Website))
        //    {
        //        newClubtoAdd.Website = newClub.Website.Trim();
        //    }           

        //    entities.Clubs.AddObject(newClubtoAdd);
        //    entities.SaveChanges();
        //    return newClubtoAdd;
            
        //}

        //public int EditClub(ClubViewModel editedClub)
        //{
            

        //}

        public Int32 AddMetadata(Metadata metaData)
        {
            entities.Metadatas.AddObject(metaData);
            entities.SaveChanges();
            return metaData.MetadataID;
        }

        public Int32 AddMessage(Message newMessage)
        {
            entities.Messages.AddObject(newMessage);
            entities.SaveChanges();
            return newMessage.MessageID;
        }

        private Int32 AddCountry(string countryName)
        {
            var countries = entities.Countries;
            foreach (var i in countries)
            {
                if(String.Equals(i.CountryName.ToLower(),countryName.Trim().ToLower()))
                {
                    return i.CountryID;
                }
            }
            Country newCountry = new Country();
            newCountry.CountryName = countryName.Trim();

            entities.Countries.AddObject(newCountry);
            entities.SaveChanges();
            return newCountry.CountryID;                
        }

        private Int32 AddState(string stateName,int countryId)
        {
            var states = entities.States;
            foreach (var i in states)
            {
                if (String.Equals(i.StateName.ToLower(), stateName.Trim().ToLower()))
                {
                    return i.StateID;
                }
            }
            State newState = new State();
            newState.StateName= stateName.Trim();
            newState.CountryID = countryId;

            entities.States.AddObject(newState);
            entities.SaveChanges();
            return newState.StateID;
        }

        private Int32 AddCity(string cityName, int stateId)
        {
            var cities = entities.Cities;
            foreach (var i in cities)
            {
                if (String.Equals(i.CityName.ToLower(), cityName.Trim().ToLower()))
                {
                    return i.CityID;
                }
            }
            City newCity = new City();
            newCity.CityName = cityName.Trim();
            newCity.StateID = stateId;

            entities.Cities.AddObject(newCity);
            entities.SaveChanges();
            return newCity.CityID;
        }
    }
}