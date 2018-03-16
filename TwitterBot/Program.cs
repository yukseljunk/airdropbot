using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace TwitterBot
{
    class Program
    {
        public static class MyCredentials
        {
            public static string CONSUMER_KEY = "cLe4R1zcvDjvzpdiIQitoF0fd";
            public static string CONSUMER_SECRET = "9ImYlxcmmt2IQA6UzI6fOIeKVHAP1lPuECIKxsPCRqbtBrJ7iT";
            public static string ACCESS_TOKEN = "708204944323649536-Js6oFonqxvTGjjrNOSUjCpcEova5aWz";
            public static string ACCESS_TOKEN_SECRET = "iKVoP9I9rJk3YLa4XEp4VnT8H71zSMvwOG7DLvpPOPtDc";

            public static ITwitterCredentials GenerateCredentials()
            {
                return new TwitterCredentials(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 6)
            {
                Console.WriteLine("consumerkey consumersecret accesstoken accesstokensecret command commandarg");
                return;
            }
            var consumerKey = args[0];
            var consumerSecret = args[1];
            var accessToken = args[2];
            var accessTokenSecret = args[3];
            var command = args[4];
            var commandArg = args[5];

            Auth.SetCredentials(new TwitterCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret));
            var user = User.GetAuthenticatedUser();
            long argLong;
            var longParse = long.TryParse(commandArg, out argLong);

            switch (command)
            {
                case "follow":
                    var followtask= Follow(user, commandArg);
                    followtask.Wait();
                    break;
                case "like":
                    if (!longParse)
                    {
                        Console.WriteLine("Longint command argument expected!");

                    }
                    if (Tweet.FavoriteTweet(argLong))
                    {
                        Console.WriteLine("Liked " + commandArg);
                    }
                    break;
                case "retweet":
                    if (!longParse)
                    {
                        Console.WriteLine("Longint command argument expected!");

                    }
                    var tw = Tweet.PublishRetweet(argLong);

                    if (tw != null)
                    {
                        Console.WriteLine("Retweeted " + commandArg);
                        Console.WriteLine("New tweet id " + tw.Id);
                    }
                    break;
                default:
                    Console.WriteLine("Unsupported command " + command);
                    break;
            }
            //Console.ReadLine();

        }

        public async static Task Follow(IAuthenticatedUser user, string twName)
        {
            var result = await user.FollowUserAsync(twName);
            Console.Write("Followed " + twName);
        }

    }
}
