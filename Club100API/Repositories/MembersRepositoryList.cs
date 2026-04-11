using Club100API.Models;
namespace Club100API.Repositories
{
    public class MembersRepositoryList : IMembersRepository
    {
        private readonly List<Member> _members = new List<Member>();

        private int _nextId = 1;

        public MembersRepositoryList(bool includeData = false)
        {
            if (includeData)
            {

                // Initialize with some sample data
                Add(new Member { Club = "Club A", Name = "Alice", Count = 100 });
                Add(new Member { Club = "Club B", Name = "Bob", Count = 101 });
                Add(new Member { Club = "Club C", Name = "Charlie", Count = 102 });
                Add(new Member { Club = "Club D", Name = "David", Count = 103 });
                Add(new Member { Club = "Club E", Name = "Eve", Count = 104 });
            }
        }
        public IEnumerable<Member> GetAll()
        {
            return _members;
        }

        public IEnumerable<Member> GetMembersByCountAndName(int? minCount, int? maxCount, string? nameFilter)
        {
            if (minCount > maxCount && minCount != null && maxCount != null)
            {
                throw new ArgumentException("minCount cannot be greater than maxCount.");
            }
            IEnumerable<Member> result = _members.AsEnumerable();
            if (minCount != null)
            {
                result = result.Where(m => m.Count >= minCount);
            }
            if (maxCount != null)
            {
                result = result.Where(m => m.Count <= maxCount);
            }
            if (nameFilter != null)
            {
                result = result.Where(m => m.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase));
            }
            return result;

        }
        public Member? GetById(int id)
        {
            return _members.FirstOrDefault(m => m.Id == id);
        }

        public Member Add(Member member)
        {
            if (member is null)
            {
                throw new ArgumentNullException(nameof(member));

            }
            member.Id = _nextId++;
            _members.Add(member);
            return member;
        }
        public Member? Delete(int id)
        {
            var existingMember = GetById(id);
            if (existingMember != null)
            {
                _members.Remove(existingMember);
                return existingMember;
            }
            return null;
        }

        public Member? Update(Member member)
        {
            var existingMember = GetById(member.Id);
            if (existingMember != null)
            {
                existingMember.Club = member.Club;
                existingMember.Name = member.Name;
                existingMember.Count = member.Count;
                return existingMember;
            }
            return null;
        }
    }

}
