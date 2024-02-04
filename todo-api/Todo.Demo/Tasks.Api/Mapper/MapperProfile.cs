using AutoMapper;
using Tasks.Api.Contracts;
using Tasks.Api.Entities;

namespace Tasks.Api.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateTodoItemRequest, TodoItem>();
        }
    }
}
