using System;
using Nest;

namespace DemoApp
{
    class Program
    {
        public static Uri node;
        public static ConnectionSettings settings;
        public static ElasticClient client;

        static void Main(string[] args)
        {
            //CreateIndex();
            //InsertData();
            //PerformTermQuery();
            //PerformMatchPhrase();
            PerformFilter();
        }

        private static void PerformFilter()
        {
            client = GetClient();

            var result = client.Search<Post>(s => s
                .Index("my_blog")
                .Query(p => p.Term(q => q.PostText, "blog"))
                .PostFilter(f => f.DateRange(r => r.Field("postDate").GreaterThanOrEquals(Convert.ToDateTime("2017-07-03T03:16:30.2537291Z")))));
        }

        private static void PerformMatchPhrase()
        {
            client = GetClient();

            var result = client.Search<Post>(s => s
                .Index("my_blog")
                .Query(q => q.MatchPhrase(m => m.Field("postText").Query("third blog post"))));
        }

        private static void PerformTermQuery()
        {
            //https://hassantariqblog.wordpress.com/2016/09/22/elastic-search-search-document-using-nest-in-net/

            client = GetClient();

            var result = client.Search<Post>(s => s
                .Index("my_blog")
                .Query(p => p.Term(q => q.PostText, "blog")));
        }

        private static void InsertData()
        {
            client = GetClient();
            
            var newBlogPost = new Post
            {
                UserId = 1,
                PostDate = DateTime.UtcNow,
                PostText = "This is a blog post from NEST!"
            };

            client.Index(newBlogPost, i => i
                .Index("my_blog")
                .Id(1));

            newBlogPost = new Post
            {
                UserId = 1,
                PostDate = DateTime.UtcNow,
                PostText = "This is another blog post."
            };

            client.Index(newBlogPost, i => i
                .Index("my_blog")
                .Id(2));

            newBlogPost = new Post
            {
                UserId = 1,
                PostDate = DateTime.UtcNow,
                PostText = "This is a third blog post."
            };

            client.Index(newBlogPost, i => i
                .Index("my_blog")
                .Id(3));

            newBlogPost = new Post
            {
                UserId = 1,
                PostDate = DateTime.UtcNow,
                PostText = "This is a blog post from the future."
            };

            client.Index(newBlogPost, i => i
                .Index("my_blog")
                .Id(4));
        }

        private static void CreateIndex()
        {
            client = GetClient();

            var indexSettings = new IndexSettings();
            indexSettings.NumberOfReplicas = 1;
            indexSettings.NumberOfShards = 5;

            var indexConfig = new IndexState
            {
                Settings = indexSettings
            };

            if (!client.IndexExists("employee").Exists)
            {
                client.CreateIndex("my_blog", c => c
                .InitializeUsing(indexConfig)
                .Mappings(m => m.Map<Post>(mp => mp.AutoMap())));
            }
        }

        private static ElasticClient GetClient()
        {
            node = new Uri("http://localhost:9200");
            settings = new ConnectionSettings(node);
            client = new ElasticClient(settings);
            return client;
        }
    }

    public class Post
    {
        public int UserId { get; set; }
        public DateTime PostDate { get; set; }
        public string PostText { get; set; }
    }
}
