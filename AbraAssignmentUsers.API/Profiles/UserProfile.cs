namespace AbraAssignmentUsers.API.Profiles
{
    public class UserProfile : Profile
    {
        CreateMap<Models.Domain.User, Models.DTO.User>().ReverseMap();
    }
}
