//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Data.Services.Providers;
using System.Web;
using System.Data.Linq;
using LuceneService;
using System.Net;

namespace SNWCF
{
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]

    public class SNDataService : EntityFrameworkDataService<SNDataModel>
    {

        public static SNDataModel de;
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
            config.UseVerboseErrors = true;
            de = new SNDataModel();
                       
        }

        [WebGet]
        public IQueryable<Item> GetLatestNews(int noOfItems) { 
        
        var latestitems = (from n in de.Items
                           select n).OrderByDescending(x => x.DateOfItem).Take(noOfItems);


        return latestitems;
        }

        [WebGet]

        public IQueryable<Item> SearchNews(string query , int limit) {

            string indexDir = "Index";
            
            //Utilities.IndexNewDocs();

            var resultsIDs = Searcher.Search(indexDir, query, limit);



            var results = from items in de.Items
                         where resultsIDs.Contains(items.ItemID)
                         select items;         

            return results;
        }

        [WebInvoke]
        public bool ResetPassword(string email , string newPass) { 
        
            var user = (from users in de.Users
               where users.Email == email
            select users).FirstOrDefault();
            if (user == null || newPass.Length ==0)
            {
                return false;
            }

            else {
                user.Password = newPass;
                de.SaveChanges();
                return true;
            }
        
        }

        [WebInvoke]
        public void ClassifyNewItems() {

            string ClassifierPath = "";
        // Calling Classifier
            WebClient client = new WebClient();
            client.DownloadString(ClassifierPath);
        }

        [WebInvoke]
        public void ClusterNewItems() {

        // Calling Cluster Module
            throw new NotImplementedException();
        
        }

        [WebGet]
        public IQueryable<Item> GetMostReadItems(int noOfItems) {

            var items = (from i in de.Items
                         select i).OrderByDescending(x=> x.ReadCount).Take(noOfItems);

            return items;
        }
        
        //Validations Constraints

        [ChangeInterceptor("Users")]
        public void UserUpdatedOrAdded(User u, UpdateOperations operation) {

            if (operation == UpdateOperations.Add || operation == UpdateOperations.Change)
            {

                var usersWithSameName = from users in de.Users
                                        where users.UserName == u.UserName
                                        select users;

                if (!Utilities.IsUNameUnique(u))
                    throw new DataServiceException(400, "UserName is taken");

                else if (!Utilities.IsEmailUnique(u))
                    throw new DataServiceException(400, "Email is taken");

                else if(!Utilities.IsEmailRight(u))
                    throw new DataServiceException(400, "Email is wrong");

                else if (!Utilities.IsUNameRight(u))
                    throw new DataServiceException(400, "UserName is wrong");
            
            }
        
        }


        public void AddItems(IEnumerable<Item> newItems) {

                string indexDir = "Index";
                List<NewsItem> inputData = Utilities.MapBetweenLuceneandSQL(newItems);
                Indexer luceneindexer = new Indexer(indexDir);
                luceneindexer.Index(inputData);
                luceneindexer.Close();
            
        
        
        }

    }
}
