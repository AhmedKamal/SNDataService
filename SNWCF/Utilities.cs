using LuceneService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SNWCF
{
	public class Utilities
	{
        static SmartNewsEntities de = SNDataService.de;
        public static bool IsEmailUnique(User u) {

            var usersWithSameEmail = from users in de.Users
                                     where users.Email == u.Email
                                     select users;
            if (usersWithSameEmail.Count() != 0) return false;

            return true;
        }


        public static bool IsUNameUnique(User u)
        {

            var usersWithSameUName = from users in de.Users
                                     where users.UserName == u.UserName
                                     select users;
            if (usersWithSameUName.Count() != 0) return false;

            return true;

        }

        public static bool IsUNameRight(User u) {

            string UnamePattern = "^[a-z0-9_-]{3,15}$";
             return Regex.IsMatch(u.UserName, UnamePattern);
        }

        public static bool IsEmailRight(User u) {
            string EmailPattern = "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";
            return Regex.IsMatch(u.Email, EmailPattern);
        }

        public static bool IndexNewDocs() {

            var newsItems = from items in de.Items
                            where items.DateOfItem > DateTime.Today
                            select new { items.Content, items.Title, items.ItemID };

            List<NewsItem> inputData = new List<NewsItem>();
            foreach (var item in newsItems)
            {
                inputData.Add(new NewsItem { Title = item.Title, Content = item.Content, ID = item.ItemID });
            }

            string indexDir = "Index";


            //Indexing Documents *** Updating the index should be done when new items are added , this code shouldn't be here.
            Indexer luceneindexer = new Indexer(indexDir);
            luceneindexer.Index(inputData);
            luceneindexer.Close();
        
			return true;
        }
	}
}