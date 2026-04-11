namespace Club100API.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Club { get; set; }= string.Empty;

        private string _name;

        public string Name
        {
            get {return _name; } 
            set { if(string.IsNullOrEmpty(value)|| value.Trim().Length < 2)

                {
                    throw new ArgumentOutOfRangeException("Name must be at least 2 characters"); 
                }
                _name = value; 
            }
        }
        private int _count;

        public int Count
        {
            get { return _count; }
            set
            {
                if (value < 100 || value > 199)
                {
                    throw new ArgumentException("Count must be between 100 and 199.");
                }
                _count = value;
            }
        }
        public override string ToString()
        {
            return $"Id: {Id}, ClubName: {Club}, Name: {Name}, Count: {Count}";
        }
    }
}
