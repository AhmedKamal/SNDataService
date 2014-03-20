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

namespace SNWCF
{
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]

    public class SNDataService : EntityFrameworkDataService<SmartNewsEntities>
    {

        public static SmartNewsEntities de;
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
            config.UseVerboseErrors = true;
            de= new SmartNewsEntities();
                       
        }

        [WebGet]
        public IQueryable<Item> GetLatestNews(int noOfItems) { 
        
        var latestitems = (from n in de.Items
                           select n).OrderByDescending(x => x.DateOfItem).Take(noOfItems);


        return latestitems;
        }

        [WebGet]

        public IQueryable<Item> SearchNews(string query) {

            var result = (from items in de.Items
                          where items.Content.Contains(query)
                          select items);

            return result;
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
        
        // Calling Classifier
            throw new NotImplementedException();

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
        //Get Other data can be done using filtering methods

    }
}
