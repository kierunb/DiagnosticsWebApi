using DiagnosticsWebApi.Entities;

namespace DiagnosticsWebApi.Services;

public class UserProfileDataGeneratorService
{
    public UserProfile GenerateUserProfile() 
    {
        // retrive image from database and convert to base64
        var random = new Random(DateTime.Now.Millisecond);
        var buffer = new byte[5*1024];
        random.NextBytes(buffer);
        string base64EncodedAvatar = Convert.ToBase64String(buffer);

        return new UserProfile 
        { 
            Id = Guid.NewGuid(),
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            AvatarUri = Faker.Internet.Url(),
            AvatarBase64 = base64EncodedAvatar,
            Bio = Faker.Lorem.Sentence(minWordCount: 100)
        };
    }
}
