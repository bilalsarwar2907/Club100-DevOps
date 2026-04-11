using Club100API.Models;

namespace Club100API.Repositories
{
    public interface IMembersRepository
    {
        Member Add(Member member);
        Member? Delete(int id);
        IEnumerable<Member> GetAll();
        Member? GetById(int id);
        IEnumerable<Member> GetMembersByCountAndName(int? minCount, int? maxCount, string? nameFilter);
        Member? Update(Member member);
    }
}