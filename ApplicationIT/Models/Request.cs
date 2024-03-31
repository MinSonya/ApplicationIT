using static System.Net.Mime.MediaTypeNames;

namespace ApplicationIT.Models
{
    //public enum ActivityType 
    //{
    //    Report,
    //    Masterclass,
    //    Discussion
    //}

    public class Request
    {
        public Guid id { get; set; }
        public Guid author { get; set; }
        public string? activity { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public string? outline { get; set; }
        public DateTime date { get; set; }
        public DateTime? submitDate { get; set; }
        public bool submit { get; set; }

        public Request()
        {
            this.id = new Guid();
            this.date = DateTime.Now.ToUniversalTime();
            this.submit = false;
            this.submitDate = null;
        }

        public RequestShortForm ToShortForm()
        {
            return new RequestShortForm
            {
                id = this.id,
                author = this.author,
                name = this.name,
                activity = this.activity,
                description = this.description,
                outline = this.outline
            };
        }
    }

    public class RequestShortForm
    {
        public Guid id { get; set; }
        public Guid author { get; set; }
        public string? name { get; set; }
        public string? activity { get; set; }
        public string? description { get; set; }
        public string? outline { get; set; }
    }
}
