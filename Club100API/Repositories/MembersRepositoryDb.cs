using System;
using System.Collections.Generic;
using System.Linq;
using Club100API.Data;
using Club100API.Models;

namespace Club100API.Repositories
{
    public class MembersRepositoryDb : IMembersRepository
    {
        private readonly MembersDbContext _context;
    
            public MembersRepositoryDb(MembersDbContext context)
            {
                _context = context;
            }

        public IEnumerable<Member> GetAll()
        {            
            return _context.Members.ToList();
        }

        public Member Add(Member member)
        {
            if (member is null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            _context.Members.Add(member);
            _context.SaveChanges();
            return member;
        }
        public IEnumerable<Member> GetMembersByCountAndName(int? minCount, int? maxCount, string? nameFilter)
        {
            if (minCount > maxCount && minCount != null && maxCount != null)
            {
                throw new ArgumentException("minCount cannot be greater than maxCount.");
            }
            IEnumerable<Member> result = _context.Members.AsEnumerable();
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
            return _context.Members.Find(id);
        }
        public Member? Delete(int id)
        {
            var member = GetById(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                _context.SaveChanges();
                return member;
            }
            return null;
        }
        public Member? Update(int id, Member updatedMember)
        {
            var existingMember = GetById(id);
            if (existingMember != null)
            {
                existingMember.Name = updatedMember.Name;
                existingMember.Club = updatedMember.Club;
                existingMember.Count = updatedMember.Count;
                _context.SaveChanges();
                return existingMember;
            }
            return null;
        }

    }
}
