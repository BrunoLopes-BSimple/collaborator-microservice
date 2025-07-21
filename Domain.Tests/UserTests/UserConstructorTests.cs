using Domain.Models;

namespace Domain.Tests.UserTests;

public class UserConstructorTests
{
    [Fact]
    public void User_Constructor_ShouldCreateUser()
    {
        new User(Guid.NewGuid());
    }
}
