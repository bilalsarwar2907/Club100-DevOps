using Club100API.Models;
using Club100API.Repositories;
using System.Collections.ObjectModel;

public class MembersRepositoryTests
{
    [Fact]
    public void Add_Member_AssignsIdAndStores()
    {
        IMembersRepository repo = new MembersRepositoryList(includeData: false);
        Member newMember = new Member { Name = "TestMember", Club = "TestClub", Count = 110 };

        Member added = repo.Add(newMember);

        int firstId = added.Id;
        Assert.Equal(1, firstId);
        Assert.Equal("TestMember", added.Name);
        Assert.Equal(110, added.Count);

        IEnumerable<Member> all = repo.GetAll();
        int countAfterAdd = all.Count();
        Assert.Equal(1, countAfterAdd);

        Member secondMember = new Member { Name = "Second", Club = "TestClub", Count = 120 };
        Member addedSecond = repo.Add(secondMember);
        Assert.Equal(2, addedSecond.Id);

        Assert.Equal(1, newMember.Id);
        Assert.Equal(2, secondMember.Id);
    }

    [Fact]
    public void AddMember_Null_ThrowsArgumentNullException()
    {
        IMembersRepository repo = new MembersRepositoryList(includeData: false);

        ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => repo.Add(null!));
        Assert.Equal("member", ex.ParamName);
    }

    [Fact]
    public void GetAllMembers_ReturnsReadOnlyCollection()
    {
        IMembersRepository repo = new MembersRepositoryList(includeData: true);

        IEnumerable<Member> all = repo.GetAll();

        ReadOnlyCollection<Member> readOnly = Assert.IsType<ReadOnlyCollection<Member>>(all);
        IList<Member> asList = (IList<Member>)readOnly;

        // FIXED: Use valid Member data so that the exception comes from the read-only collection, not from validation
        NotSupportedException ex = Assert.Throws<NotSupportedException>(() => asList.Add(new Member { Name = "XX", Club = "AB", Count = 150 }));
    }

    [Fact]
    public void GetMemberById_ReturnsCorrectMemberOrNull()
    {
        IMembersRepository repo = new MembersRepositoryList(includeData: false);

        repo.Add(new Member { Name = "Member1", Club = "Club1", Count = 140 });

        Member? first = repo.GetById(1);
        Assert.NotNull(first);
        Assert.Equal(1, first!.Id);

        Member? notFound = repo.GetById(999);
        Assert.Null(notFound);
    }

    [Fact]
    public void RemoveMember_RemovesAndReturns()
    {
        IMembersRepository repo = new MembersRepositoryList(includeData: false);
        Member member = new Member { Name = "ToRemove", Club = "Club", Count = 150 };
        Member added = repo.Add(member);

        Member? removed = repo.Delete(added.Id);
        Assert.NotNull(removed);
        Assert.Equal(added.Id, removed!.Id);

        Member? postLookup = repo.GetById(added.Id);
        Assert.Null(postLookup);

        int remaining = repo.GetAll().Count();
        Assert.Equal(0, remaining);
    }

    [Fact]
    public void RemoveMember_NonExistent_ReturnsNull()
    {
        IMembersRepository repo = new MembersRepositoryList(includeData: false);

        Member? removed = repo.Delete(12345);
        Assert.Null(removed);
    }

    [Fact]
    public void UpdateMember_UpdatesExisting_ReturnsUpdated()
    {
        IMembersRepository repo = new MembersRepositoryList(includeData: false);
        Member original = new Member { Name = "Old", Club = "OldClub", Count = 160 };
        Member added = repo.Add(original);

        Member updatedPayload = new Member { Name = "NewName", Club = "NewClub", Count = 170 };
        Member? updated = repo.Update(added.Id, updatedPayload);

        Assert.NotNull(updated);
        Assert.Equal(added.Id, updated!.Id);
        Assert.Equal("NewName", updated.Name);
        Assert.Equal(170, updated.Count);

        Member? stored = repo.GetById(added.Id);
        Assert.NotNull(stored);
        Assert.Equal("NewName", stored!.Name);
        Assert.Equal(170, stored.Count);
    }

    [Fact]
    public void UpdateMember_NonExistent_ReturnsNull()
    {
        IMembersRepository repo = new MembersRepositoryList(includeData: false);
        Member payload = new Member { Name = "Whatever", Club = "Whatever", Count = 180 };

        Member? updated = repo.Update(42, payload);
        Assert.Null(updated);
    }
}