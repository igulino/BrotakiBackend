namespace skeleton
{
    using Google.Cloud.Firestore;
    using Google.Type;
    using Microsoft.VisualBasic;

    [FirestoreData]
    public class Followers
    {

        [FirestoreProperty]
        public int followedID { get; set; }
        [FirestoreProperty]
        public int followerID { get; set; }

    }

    [FirestoreData]

    public class Posts
    {
        [FirestoreProperty]
        public string desc { get; set; }
        [FirestoreProperty]
        public string Image { get; set; }
        [FirestoreProperty]
        public int PostID { get; set; }
        [FirestoreProperty]
        public int likes { get; set; }
        [FirestoreProperty]
        public string date { get; set; }

    }

    [FirestoreData]
    public class PostsUser
    {
        [FirestoreProperty]
        public int ID { get; set; }
        [FirestoreProperty]
        public Posts Posts { get; set; }
    }

    [FirestoreData]
    public class user
    {
        [FirestoreProperty]
        public string name { get; set; }
        [FirestoreProperty]
        public int points { get; set; }
        [FirestoreProperty]
        public string image { get; set; }
        [FirestoreProperty]
        public int ID { get; set; }
        [FirestoreProperty]
        public string Password { get; set; }

    }
    [FirestoreData]
    public class FeedItem
        {
            [FirestoreProperty]
            public skeleton.Posts Post { get; set; }
            [FirestoreProperty]
            public user User { get; set; }
        }



}