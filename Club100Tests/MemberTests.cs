using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Club100API.Models;
using Xunit;    

namespace Club100Tests
{
    public class MemberTests
    {
        [Fact]
            public void Valid_Name_Sets_Correctly()


            {
            var member = new Member();

            member.Name = "John Doe";

            Assert.Equal("John Doe", member.Name);

            }
        [Fact]
        public void Name_OneCharacter_ThrowsException()
        {
            var member = new Member();
            Assert.Throws<ArgumentOutOfRangeException>(() => member.Name = "A");
        }
        [Fact]
        public void Name_ZeroCharacter_ThrowsException()
        {
            var member = new Member();
            Assert.Throws<ArgumentOutOfRangeException>(() => member.Name = "");
        }
        [Fact]
        public void Name_Null_ThrowsException()
        {
            var member = new Member();
            Assert.Throws<ArgumentOutOfRangeException>(() => member.Name = null!);
        }
        [Fact]
        public void Name_ExactlyTwoChars_SetsCorrectly()
        {
            var member = new Member();
            member.Name = "Jo";
            Assert.Equal("Jo", member.Name);
        }
            [Fact]
            public void Valid_Count_Sets_Correctly()
        {
            var member = new Member();
            member.Count = 150;
                Assert.Equal(150, member.Count);
        }
        [Fact]
        public void Count_Exactly100_SetsCorrectly()
        {
            var member = new Member();
            member.Count = 100;
            Assert.Equal(100, member.Count);
        }

        [Fact]
        public void Count_Exactly199_SetsCorrectly()
        {
            var member = new Member();
            member.Count = 199;
            Assert.Equal(199, member.Count);
        }
        [Fact]
        public void Count_Below100_ThrowsException()
        {
            var member = new Member();
            Assert.Throws<ArgumentException>(() => member.Count = 99);
        }

        [Fact]
        public void Count_Above199_ThrowsException()
        {
            var member = new Member();
            Assert.Throws<ArgumentException>(() => member.Count = 200);
        }

    }
}
