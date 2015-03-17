using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using StackExchange.StacMan;

namespace GitHubUsers.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var github = new GitHubClient(new ProductHeaderValue("MyAmazingApp"));
            //var result = github.Search.SearchUsers(new SearchUsersRequest("")
            //    {
            //        AccountType = AccountSearchType.User,
            //        PerPage = 100,
            //        SortField = 
            //    }).Result;
            //foreach (var item in result.Items)
            //{
            //    github.PullRequest.
            //    System.Console.WriteLine(
            //        string.Format("{0} @ {1} {2} {3}", item.Name, item.Location, item.Hireable, item.Blog));
            //}
            var client = new StacManClient();
            var excludedLocations = File.ReadAllLines("Locations.txt").Select(l => l.ToLower()).ToArray();
            foreach (var page in Enumerable.Range(1, Int32.MaxValue))
            {
                var res = client.Users.GetAll("stackoverflow", page: page, pagesize: 100, sort: StackExchange.StacMan.Users.Sort.Reputation, order: Order.Desc).Result;
                var users = res.Data.Items.Where(u => u.Location != null && !excludedLocations.Any(location => u.Location.ToLower().Contains(location))).ToArray();
                foreach (var user in users)
                {
                    System.Console.WriteLine("{0} @ {1}: {2}", user.DisplayName, user.Location, user.Reputation);
                }
                System.Console.WriteLine("Found {0} users", users.Length);
                System.Console.WriteLine();
                System.Console.ReadKey();
                System.Console.WriteLine("...");
            }
        }
    }
}
